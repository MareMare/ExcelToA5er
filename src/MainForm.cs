// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using ExcelToA5er.Generators;
using ExcelToA5er.Metadatas;
using ExcelToA5er.Progress;
using ExcelToA5er.Templates;

namespace ExcelToA5er;

/// <summary>
/// メイン画面のユーザーインターフェイスを構成するウィンドウを表します。
/// </summary>
public partial class MainForm : Form
{
    /// <summary>非同期操作の実行時間としての最低時間を表します。</summary>
    private static readonly TimeSpan DelayTimeSpan = TimeSpan.FromSeconds(2);

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
        var items = xlsxInfo?.TableDefinitions?.Select(definition => new DisplayItem(definition)).ToArray();
        return items ?? Array.Empty<DisplayItem>();
    }

    /// <summary>
    /// 非同期操作として、XLSX ファイルを読み込みます。
    /// </summary>
    /// <param name="xlsxFilePath">XLSX ファイルパス。</param>
    /// <returns>テーブル定義情報。</returns>
    private async Task<XlsxInformation> LoadXlsxAsync(string xlsxFilePath)
    {
        using var progressForm = new ProgressForm { Title = "読み込み" };
        await using var progressScoap = progressForm.UseProgressFormScoap(this);
        progressScoap.Reporter.ReportStarting("読み込み中です。しばらくお待ちください。");

        var loadTask = XlsxInformation.LoadAsync(xlsxFilePath);
        var delayTask = Task.Delay(MainForm.DelayTimeSpan);
        await Task.WhenAll(loadTask, delayTask).ConfigureAwait(true);

        var xlsxInfo = await loadTask.ConfigureAwait(true);
        await progressScoap.Reporter.ReportCompletedAsync("読み込みが完了しました。").ConfigureAwait(true);
        return xlsxInfo;
    }

    /// <summary>
    /// 非同期操作として、A5ER ファイルを生成します。
    /// </summary>
    /// <param name="outputPath">出力先ファイルパス。</param>
    /// <param name="tableDefinitions">テーブル情報のコレクション。</param>
    /// <returns>完了を表すタスク。</returns>
    private async Task SaveA5erAsync(string outputPath, ICollection<TableDefinition> tableDefinitions)
    {
        using var progressForm = new ProgressForm { Title = "変換" };
        await using var progressScoap = progressForm.UseProgressFormScoap(this);
        progressScoap.Reporter.ReportStarting("変換中です。しばらくお待ちください。");

        var generateTask = new A5erRuntimeTextTemplate(tableDefinitions).GenerateAsync(outputPath);
        var delayTask = Task.Delay(MainForm.DelayTimeSpan);
        await Task.WhenAll(generateTask, delayTask).ConfigureAwait(true);

        await generateTask.ConfigureAwait(true);
        await progressScoap.Reporter.ReportCompletedAsync("変換完了しました。").ConfigureAwait(true);
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
    }

    /// <summary>
    /// リストボックスの選択項目が変更されたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private void ListBoxTableInfo_SelectedIndexChanged(object sender, EventArgs e) =>
        this.buttonToConvert.Enabled = this.listBoxTableInfo.SelectedItems.Count > 0 && this._currentXlsxInfo?.A5erFilePath != null;

    /// <summary>
    /// [参照] ボタンがクリックされたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private async void ButtonToBrowseXlsx_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "テーブル定義書を選択してください。",
            Filter = "テーブル定義書(*.xlsx)|*.xlsx",
            RestoreDirectory = true,
            CheckFileExists = true,
            CheckPathExists = true,
            ReadOnlyChecked = true,
            ShowReadOnly = true,
        };
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            var xlsxInfo = await this.LoadXlsxAsync(openFileDialog.FileName).ConfigureAwait(true);
            var items = MainForm.ToDisplayItems(xlsxInfo);
            this._currentXlsxInfo = xlsxInfo;
            this.listBoxTableInfo.Items.Clear();
            this.listBoxTableInfo.Items.AddRange(items);
            this.textBoxXlsxFilePath.Text = Path.GetFileName(xlsxInfo.XlsxFilePath);
            this.textBoxOutputFilePath.Text = Path.GetFileName(xlsxInfo.A5erFilePath);
        }
    }

    /// <summary>
    /// [変換] ボタンがクリックされたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private async void ButtonToConvert_Click(object sender, EventArgs e)
    {
        var a5erFilePath = this._currentXlsxInfo?.A5erFilePath;
        if (a5erFilePath == null)
        {
            return;
        }

        var selectedItems = this.listBoxTableInfo.SelectedItems.OfType<DisplayItem>().ToArray();
        var tableDefinitions = selectedItems.Select(item => item.TableInfo).ToArray();
        await this.SaveA5erAsync(a5erFilePath, tableDefinitions).ConfigureAwait(true);
    }

    /// <summary>
    /// リストボックス項目を表します。
    /// </summary>
    private class DisplayItem
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
        public override string ToString() => this.TableInfo != null ? $"{this.TableInfo.PhysicalName}: {this.TableInfo.LogicalName}" : "<未設定>";
    }
}
