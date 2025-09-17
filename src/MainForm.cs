// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using ExcelToA5er.Generators;
using ExcelToA5er.Metadata;
using ExcelToA5er.Progress;
using ExcelToA5er.Templates;

namespace ExcelToA5er;

/// <summary>
/// メイン画面のユーザーインターフェイスを構成するウィンドウを表します。
/// </summary>
public partial class MainForm : Form
{
    /// <summary>非同期操作の実行時間としての最低時間を表します。</summary>
    private static readonly TimeSpan _delayTimeSpan = TimeSpan.FromSeconds(2);

    /// <summary>現在のテーブル定義書情報を表します。</summary>
    private XlsxInformation? _currentXlsxInfo;

    /// <summary>
    /// <see cref="MainForm"/> クラスの新しいインスタンスを生成します。
    /// </summary>
    public MainForm()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// リストボックス項目へ変換します。
    /// </summary>
    /// <param name="xlsxInfo">テーブル定義情報。</param>
    /// <returns>リストボックス項目のコレクション。</returns>
    private static DisplayItem[] ToDisplayItems(XlsxInformation xlsxInfo)
    {
        var items = xlsxInfo.TableDefinitions.Select(definition => new DisplayItem(definition)).ToArray();
        return items ?? [];
    }

    /// <summary>
    /// 物理テーブル名リストを読み込み、物理テーブル名の配列を取得します。
    /// </summary>
    /// <param name="tableNamesFilePath">物理テーブル名リストのファイルパス。</param>
    /// <returns>物理テーブル名の配列。</returns>
    private static string[]? LoadTargetTableNamesFromJson(string tableNamesFilePath)
    {
        if (!File.Exists(tableNamesFilePath))
        {
            return null;
        }

        try
        {
            // 物理テーブル名リストを読み込みます。
            var json = File.ReadAllText(tableNamesFilePath);
            return JsonSerializer.Deserialize<string[]>(json);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// 非同期操作として、XLSX ファイルを読み込みます。
    /// </summary>
    /// <param name="xlsxFilePath">XLSX ファイルパス。</param>
    /// <returns>テーブル定義情報。</returns>
    private async Task<XlsxInformation> LoadXlsxAsync(string xlsxFilePath)
    {
        using var progressForm = new ProgressForm();
        progressForm.Title = "読み込み";

        var progressScope = progressForm.UseProgressFormScope(this);
        await using (progressScope.ConfigureAwait(true))
        {
            progressScope.Reporter.ReportStarting("読み込み中です。しばらくお待ちください。");

            var loadTask = XlsxInformation.LoadAsync(xlsxFilePath);
            var delayTask = Task.Delay(MainForm._delayTimeSpan);
            await Task.WhenAll(loadTask, delayTask).ConfigureAwait(true);

            var xlsxInfo = await loadTask.ConfigureAwait(true);
            await progressScope.Reporter.ReportCompletedAsync("読み込みが完了しました。").ConfigureAwait(true);
            return xlsxInfo;
        }
    }

    /// <summary>
    /// 非同期操作として、A5ER ファイルを生成します。
    /// </summary>
    /// <param name="outputPath">出力先ファイルパス。</param>
    /// <param name="tableDefinitions">テーブル情報のコレクション。</param>
    /// <returns>完了を表すタスク。</returns>
    private async Task SaveA5ErAsync(string outputPath, ICollection<TableDefinition> tableDefinitions)
    {
        using var progressForm = new ProgressForm();
        progressForm.Title = "変換";

        var progressScope = progressForm.UseProgressFormScope(this);
        await using (progressScope.ConfigureAwait(true))
        {
            progressScope.Reporter.ReportStarting("変換中です。しばらくお待ちください。");

            var generateTask = this.GenerateAsync(outputPath, tableDefinitions);
            var delayTask = Task.Delay(MainForm._delayTimeSpan);
            await Task.WhenAll(generateTask, delayTask).ConfigureAwait(true);

            await generateTask.ConfigureAwait(true);
            await progressScope.Reporter.ReportCompletedAsync("変換完了しました。").ConfigureAwait(true);
        }
    }

    /// <summary>
    /// 非同期操作として、A5ER ファイルを生成します。
    /// </summary>
    /// <param name="outputPath">出力先ファイルパス。</param>
    /// <param name="tableDefinitions">テーブル情報のコレクション。</param>
    /// <returns>完了を表すタスク。</returns>
    private async Task GenerateAsync(string outputPath, ICollection<TableDefinition> tableDefinitions)
    {
        if (!this.checkBoxToSplitOutputFile.Checked)
        {
            await new A5ErRuntimeTextTemplate(tableDefinitions).GenerateAsync(outputPath).ConfigureAwait(false);
            return;
        }

        var tasks = tableDefinitions
            .Select(tableDefinition =>
            {
                var outputDirectory = Path.GetDirectoryName(outputPath) ?? string.Empty;
                var fileName = Path.GetFileNameWithoutExtension(outputPath);
                var fileExtension = Path.GetExtension(outputPath);
                var partialOutputFileName = $"{fileName}_{tableDefinition.PhysicalName}{fileExtension}";
                var partialOutputPath = Path.Combine(outputDirectory, partialOutputFileName);
                return new A5ErRuntimeTextTemplate([tableDefinition]).GenerateAsync(partialOutputPath);
            });
        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    /// <summary>
    /// テーブル情報のコレクションを取得します。
    /// </summary>
    /// <returns>テーブル情報のコレクション。</returns>
    private TableDefinition[] GetTargetTableDefinitions()
    {
        if (this.checkBoxToUseTableNames.Checked)
        {
            // 物理テーブル名リストを使用する場合、JSONファイルに従ってテーブル情報のコレクションを抽出します。
            var jsonFilePath = this.textBoxTableNamesFilePath.Text;
            var targetTableNames = MainForm.LoadTargetTableNamesFromJson(jsonFilePath);
            var loadedAllTableDefinitions = this.listBoxTableInfo.Items.OfType<DisplayItem>().Select(item => item.TableInfo).ToArray();
            var filteredTableDefinitions = loadedAllTableDefinitions
                .Join(
                    targetTableNames ?? [],
                    outer => outer.PhysicalName,
                    inner => inner,
                    (outer, inner) => outer)
                .ToArray();
            return filteredTableDefinitions;
        }

        var selectedItems = this.listBoxTableInfo.SelectedItems.OfType<DisplayItem>().ToArray();
        var tableDefinitions = selectedItems.Select(item => item.TableInfo).ToArray();
        return tableDefinitions;
    }

    /// <summary>
    /// フォームが読み込まれたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private void MainForm_Load(object sender, EventArgs e)
    {
        this.listBoxTableInfo.Items.Clear();
        this.buttonToConvert.Enabled = false;

        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var versionText = version?.ToString(3);
        this.Text = versionText is not null ? $"ExcelToA5er ({versionText})" : "ExcelToA5er";
    }

    /// <summary>
    /// リストボックスの選択項目が変更されたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private void ListBoxTableInfo_SelectedIndexChanged(object sender, EventArgs e) =>
        this.buttonToConvert.Enabled = this.listBoxTableInfo.SelectedItems.Count > 0 && this._currentXlsxInfo?.A5ErFilePath != null;

    /// <summary>
    /// [参照] ボタンがクリックされたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private async void ButtonToBrowseXlsx_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog();
        openFileDialog.Title = @"テーブル定義書を選択してください。";
        openFileDialog.Filter = @"テーブル定義書(*.xlsx)|*.xlsx";
        openFileDialog.RestoreDirectory = true;
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.ReadOnlyChecked = true;
        openFileDialog.ShowReadOnly = true;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            var xlsxInfo = await this.LoadXlsxAsync(openFileDialog.FileName).ConfigureAwait(true);
            var items = MainForm.ToDisplayItems(xlsxInfo).Cast<object>().ToArray();
            this._currentXlsxInfo = xlsxInfo;
            this.listBoxTableInfo.Items.Clear();
            this.listBoxTableInfo.Items.AddRange(items);
            this.textBoxXlsxFilePath.Text = Path.GetFileName(xlsxInfo.XlsxFilePath);
            this.textBoxOutputFilePath.Text = Path.GetFileName(xlsxInfo.A5ErFilePath);
        }
    }

    /// <summary>
    /// [参照] ボタンがクリックされたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private void ButtonToBrowseTableNames_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog();
        openFileDialog.Title = @"物理テーブル名リストを選択してください。";
        openFileDialog.Filter = @"物理テーブル名リスト(*.json)|*.json";
        openFileDialog.RestoreDirectory = true;
        openFileDialog.CheckFileExists = true;
        openFileDialog.CheckPathExists = true;
        openFileDialog.ReadOnlyChecked = true;
        openFileDialog.ShowReadOnly = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            this.textBoxTableNamesFilePath.Text = openFileDialog.FileName;
        }
    }

    /// <summary>
    /// [変換] ボタンがクリックされたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private async void ButtonToConvert_Click(object sender, EventArgs e)
    {
        var a5ErFilePath = this._currentXlsxInfo?.A5ErFilePath;
        if (a5ErFilePath == null)
        {
            return;
        }

        var tableDefinitions = this.GetTargetTableDefinitions();
        await this.SaveA5ErAsync(a5ErFilePath, tableDefinitions).ConfigureAwait(true);
    }

    /// <summary>
    /// リストボックス項目を表します。
    /// </summary>
    private sealed class DisplayItem
    {
        /// <summary>
        /// <see cref="DisplayItem"/> クラスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tableDefinition">テーブル情報。</param>
        public DisplayItem(TableDefinition tableDefinition)
        {
            this.TableInfo = tableDefinition;
        }

        /// <summary>
        /// テーブル情報を取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="TableDefinition" /> 型。
        /// <para>テーブル情報。既定値は null です。</para>
        /// </value>
        public TableDefinition TableInfo { get; }

        /// <inheritdoc />
        public override string ToString() => $"{this.TableInfo.PhysicalName}: {this.TableInfo.LogicalName}";
    }
}
