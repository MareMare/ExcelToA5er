// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressReporter.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er.Progress;

/// <summary>
/// 進行状況の報告を提供します。
/// </summary>
internal class ProgressReporter : Progress<ProgressInfo>, IProgressReporter
{
    /// <summary>完了または失敗字のレポートに対するデフォルトのタイムアウト時間を表します。</summary>
    private readonly TimeSpan _defaultTimeout;

    /// <summary>
    /// <see cref="ProgressReporter" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="handler">進行状況の値が報告される各を呼び出すメソッドのデリゲート。</param>
    public ProgressReporter(Action<ProgressInfo> handler)
        : base(handler)
    {
        this._defaultTimeout = TimeSpan.FromSeconds(1.0d);
    }

    /// <summary>
    /// 進行状況の更新を報告します。
    /// </summary>
    /// <param name="info">進捗情報。</param>
    public void Report(ProgressInfo info) => this.ReportCore(info);

    /// <summary>
    /// 進行状況のクリアを報告します。
    /// </summary>
    public void ReportClear() => this.ReportCore(ProgressInfo.New(string.Empty, 0d));

    /// <summary>
    /// 進行状況の開始を報告します。
    /// </summary>
    /// <param name="message">進捗メッセージ。</param>
    public void ReportStarting(string message) => this.ReportCore(ProgressInfo.New(message, 0d));

    /// <summary>
    /// 非同期操作として、進行状況の完了を報告します。
    /// </summary>
    /// <param name="message">進捗メッセージ。</param>
    /// <param name="waitingTimeSpan">表示待機時間。</param>
    /// <returns>完了を表す <see cref="Task" />。</returns>
    public Task ReportCompletedAsync(string message, TimeSpan? waitingTimeSpan = null) => this.ReportCoreAsync(ProgressInfo.New(message, 100d), waitingTimeSpan ?? this._defaultTimeout);

    /// <summary>
    /// 非同期操作として、進行状況の失敗を報告します。
    /// </summary>
    /// <param name="message">進捗メッセージ。</param>
    /// <param name="waitingTimeSpan">表示待機時間。</param>
    /// <returns>完了を表す <see cref="Task" />。</returns>
    public Task ReportFailedAsync(string message, TimeSpan? waitingTimeSpan = null) => this.ReportCoreAsync(ProgressInfo.New(message, 100d), waitingTimeSpan ?? this._defaultTimeout);

    /// <summary>
    /// 進行状況の更新を報告します。
    /// </summary>
    /// <param name="info">進捗情報。</param>
    private void ReportCore(ProgressInfo info) => ((IProgress<ProgressInfo>)this).Report(info);

    /// <summary>
    /// 非同期操作として、進行状況の更新を報告します。
    /// </summary>
    /// <param name="info">進捗情報。</param>
    /// <param name="waitingTimeSpan">表示待機時間。</param>
    /// <returns>完了を表す <see cref="Task" />。</returns>
    private async Task ReportCoreAsync(ProgressInfo info, TimeSpan? waitingTimeSpan)
    {
        ((IProgress<ProgressInfo>)this).Report(info);

        var timeout = waitingTimeSpan ?? TimeSpan.Zero;
        if (timeout != TimeSpan.Zero)
        {
            await Task.Delay(timeout).ConfigureAwait(false);
        }
    }
}
