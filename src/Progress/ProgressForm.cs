// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressForm.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace ExcelToA5er.Progress;

/// <summary>
/// 進捗画面のユーザーインターフェイスを構成するウィンドウを表します。
/// </summary>
public partial class ProgressForm : Form
{
    /// <summary>進行状況の報告者を表します。</summary>
    private readonly ProgressReporter _progressReporter;

    /// <summary>
    /// <see cref="ProgressForm"/> クラスの新しいインスタンスを生成します。
    /// </summary>
    public ProgressForm()
    {
        this.InitializeComponent();
        this._progressReporter = new ProgressReporter(progressInfo => this.UpdateBy(progressInfo));
    }

    /// <summary>
    /// タイトルを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>タイトル。既定値は null です。</para>
    /// </value>
    public string? Title { get => this.Text; set => this.Text = value; }

    /// <summary>
    /// メッセージを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>メッセージ。既定値は null です。</para>
    /// </value>
    public string? Message { get => this.labelMessage.Text; set => this.labelMessage.Text = value; }

    /// <summary>
    /// 非同期操作として、モーダル表示する範囲を使用します。
    /// </summary>
    /// <param name="owner">フォームを所有するトップレベルウィンドウ。</param>
    /// <returns>進捗表示する範囲。</returns>
    public IProgressScoap UseProgressFormScoap(IWin32Window owner) => new ProgressFormScoap(this, this.ShowDialogAsync(owner));

    /// <inheritdoc/>
    protected override void OnLoad(EventArgs e)
    {
        Debug.Print($"{nameof(ProgressForm)}.{nameof(this.OnLoad)}");
        base.OnLoad(e);
    }

    /// <inheritdoc/>
    protected override void OnShown(EventArgs e)
    {
        Debug.Print($"{nameof(ProgressForm)}.{nameof(this.OnShown)}");
        base.OnShown(e);
    }

    /// <summary>
    /// 進捗情報で表示更新思案す。
    /// </summary>
    /// <param name="progressInfo">進捗情報。</param>
    private void UpdateBy(ProgressInfo progressInfo) => this.Message = progressInfo.Message;

    /// <summary>
    /// フォームが読み込まれたときに発生するイベントのイベントハンドラです。
    /// </summary>
    /// <param name="sender">イベントソース。</param>
    /// <param name="e">イベントデータ。</param>
    private void ProgressForm_Load(object sender, EventArgs e)
    {
        this.StartPosition = FormStartPosition.Manual;
        this.Location = new Point(
            this.Owner.Location.X + ((this.Owner.Width - this.Width) / 2),
            this.Owner.Location.Y + ((this.Owner.Height - this.Height) / 2));
    }

    /// <summary>
    /// 進捗表示する範囲を表します。
    /// </summary>
    private sealed class ProgressFormScoap : IProgressScoap
    {
        /// <summary><see cref="ProgressForm"/> を表します。</summary>
        private readonly ProgressForm _progressForm;

        /// <summary>表示完了を表すタスクを表します。</summary>
        private readonly Task<DialogResult> _progressFormTask;

        /// <summary>
        /// <see cref="ProgressFormScoap"/> クラスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="progressForm"><see cref="ProgressForm"/>。</param>
        /// <param name="progressFormTask">表示完了を表すタスク。</param>
        public ProgressFormScoap(ProgressForm progressForm, Task<DialogResult> progressFormTask)
        {
            this._progressForm = progressForm ?? throw new ArgumentNullException(nameof(progressForm));
            this._progressFormTask = progressFormTask ?? throw new ArgumentNullException(nameof(progressFormTask));
            this.Reporter = progressForm._progressReporter;
        }

        /// <inheritdoc/>
        public IProgressReporter Reporter { get; }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            // NOTE: https://stackoverflow.com/a/33411037
            this._progressForm?.Close();
            if (this._progressFormTask != null)
            {
                await this._progressFormTask.ConfigureAwait(true);
            }
        }
    }
}
