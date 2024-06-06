// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XlsxInformation.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er.Metadata;

/// <summary>
/// テーブル定義書情報を表します。
/// </summary>
internal sealed partial class XlsxInformation
{
    /// <summary>
    /// XLSX ファイルパスを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>XLSX ファイルパス。既定値は null です。</para>
    /// </value>
    public string XlsxFilePath { get; init; } = null!;

    /// <summary>
    /// A5ER ファイルパスを取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>A5ER ファイルパス。既定値は null です。</para>
    /// </value>
    public string A5ErFilePath => Path.ChangeExtension(this.XlsxFilePath, ".a5er");

    /// <summary>
    /// テーブル情報のコレクションを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="TableDefinition" /> 型。
    /// <para>テーブル情報のコレクション。既定値は null です。</para>
    /// </value>
    public ICollection<TableDefinition> TableDefinitions { get; init; } = null!;
}
