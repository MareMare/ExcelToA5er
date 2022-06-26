// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er.Metadatas;

/// <summary>
/// カラム情報を表します。
/// </summary>
internal class ColumnDefinition
{
    /// <summary>
    /// 物理カラム名を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>物理カラム名。既定値は null です。</para>
    /// </value>
    public string PhysicalName { get; set; } = null!;

    /// <summary>
    /// 論理カラム名を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>論理カラム名。既定値は null です。</para>
    /// </value>
    public string LogicalName { get; set; } = null!;

    /// <summary>
    /// データ型名を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>データ型名。既定値は null です。</para>
    /// </value>
    public string SqlDataTypeName { get; set; } = null!;

    /// <summary>
    /// 必須 (NOT NULL) 列かどうかを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="bool" /> 型。
    /// <para>必須 (NOT NULL) 列かどうか。既定値は false です。</para>
    /// </value>
    public bool IsNotNull { get; set; }

    /// <summary>
    /// PK 列の順位 (1～) を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="int" /> 型。
    /// <para>PK 列の順位 (1～)。既定値は null です。</para>
    /// </value>
    public int? PkNumber { get; set; }

    /// <summary>
    /// コメントを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>コメント。既定値は null です。</para>
    /// </value>
    public string Comment { get; set; } = null!;

    /// <summary>
    /// 必須 (NOT NULL) 列の指定文字列を取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>必須 (NOT NULL) 列かどうか。既定値は null です。</para>
    /// </value>
    internal string IsNotNullText => this.IsNotNull ? "\"NOT NULL\"" : string.Empty;

    /// <summary>
    /// PK 列の順位 (0～) の文字列を取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>PK 列の順位 (0～) の文字列。既定値は null です。</para>
    /// </value>
    internal string PkNumberText => this.PkNumber.HasValue ? $"{this.PkNumber.Value}" : string.Empty;
}
