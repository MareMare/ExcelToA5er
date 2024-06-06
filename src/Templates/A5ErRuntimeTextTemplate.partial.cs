// --------------------------------------------------------------------------------------------------------------------
// <copyright file="A5ErRuntimeTextTemplate.partial.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using ExcelToA5er.Generators;
using ExcelToA5er.Metadata;

namespace ExcelToA5er.Templates;

/// <summary>
/// A5ER ファイルを生成する実行時テンプレートを表します。
/// </summary>
public partial class A5ErRuntimeTextTemplate : IGenerator
{
    /// <summary>
    /// <see cref="A5ErRuntimeTextTemplate"/> クラスの新しいインスタンスを生成します。
    /// </summary>
    /// <param name="tableDefinitions">テーブル情報のコレクション。</param>
    internal A5ErRuntimeTextTemplate(ICollection<TableDefinition> tableDefinitions)
    {
        ArgumentNullException.ThrowIfNull(tableDefinitions);
        this.TableDefinitions = tableDefinitions.ToList();
    }

    /// <summary>
    /// テーブル情報のコレクションを取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="TableDefinition" /> 型。
    /// <para>テーブル情報のコレクション。既定値は要素数 0 です。</para>
    /// </value>
    internal IList<TableDefinition> TableDefinitions { get; }

    /// <summary>
    /// XY 座標へ変換します。
    /// </summary>
    /// <param name="definition">テーブル情報。</param>
    /// <returns>XY 座標。</returns>
    internal Point ToPoint(TableDefinition definition)
    {
        // 表示位置は要素順で単純に左上から右下にかけて順番に重ねて表示しています。
        var index = this.TableDefinitions.IndexOf(definition);
        return index == -1
            ? Point.Empty
            : new Point(x: (index * 100) + 50, y: (index * 100) + 50);
    }
}
