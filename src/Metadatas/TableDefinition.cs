// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableDefinition.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er.Metadatas;

/// <summary>
/// テーブル情報を表します。
/// </summary>
internal partial class TableDefinition
{
    /// <summary>
    /// 物理テーブル名を取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>物理テーブル名。既定値は null です。</para>
    /// </value>
    public string PhysicalName { get; init; } = null!;

    /// <summary>
    /// 論理テーブル名を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>論理テーブル名。既定値は null です。</para>
    /// </value>
    public string LogicalName { get; init; } = null!;

    /// <summary>
    /// ワークシート名を取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>ワークシート名。既定値は null です。</para>
    /// </value>
    public string WorksheetName { get; init; } = null!;

    /// <summary>
    /// カラム情報のコレクションを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="ColumnDefinition" /> 型。
    /// <para>カラム情報のコレクション。既定値は null です。</para>
    /// </value>
    public ICollection<ColumnDefinition> ColumnDefinitions { get; init; } = null!;
}
