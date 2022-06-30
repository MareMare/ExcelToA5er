// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressInfo.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er.Progress;

/// <summary>
/// 進捗情報を表します。
/// </summary>
public class ProgressInfo
{
    /// <summary>
    /// <see cref="ProgressInfo" /> クラスの新しいインスタンスを生成します。
    /// </summary>
    private ProgressInfo()
    {
    }

    /// <summary>
    /// 進捗メッセージを取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>進捗メッセージ。既定値は null です。</para>
    /// </value>
    public string? Message { get; private init; }

    /// <summary>
    /// 進捗率 (0～100%) を取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="double" /> 型。
    /// <para>進捗率 (0～100%)。既定値は 0d です。</para>
    /// </value>
    public double Percentage { get; private init; }

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="message">進捗メッセージ。</param>
    /// <returns>進捗情報。</returns>
    public static ProgressInfo New(string message) => ProgressInfo.New(message, 0);

    /// <summary>
    /// 新しいインスタンスを生成します。
    /// </summary>
    /// <param name="message">進捗メッセージ。</param>
    /// <param name="percent">進捗率 (0～100%)。</param>
    /// <returns>進捗情報。</returns>
    public static ProgressInfo New(string message, double percent) => new () { Message = message, Percentage = percent };
}
