// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClosedXmlExtensions.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using ClosedXML.Excel;

namespace ExcelToA5er.Metadata;

/// <summary>
/// Closed Xml の拡張メソッドを提供します。
/// </summary>
internal static class ClosedXmlExtensions
{
    /// <summary>対象シートを判断する識別子のキーワードを表します。</summary>
    private const string _targetSheetKeyword1 = "テーブル情報";

    /// <summary>対象シートを判断する識別子のキーワードを表します。</summary>
    private const string _targetSheetKeyword2 = "エンティティ情報";

    /// <summary>対象シートのカラム情報を判断する識別子のキーワードを表します。</summary>
    private const string _targetSheetKeyword3 = "カラム情報";

    /// <summary>対象シートの識別子のセルアドレスを表します。</summary>
    private const string _cellOfTargetSheetKeyword = "A1";

    /// <summary>論理テーブル名のセルアドレスを表します。</summary>
    private const string _cellOfTableLogicalName = "C5";

    /// <summary>物理テーブル名のセルアドレスを表します。</summary>
    private const string _cellOfTablePhysicalName = "C6";

    /// <summary>No.列の先頭セルアドレスを表します。</summary>
    private const string _cellOfColumnNumberName = "A1";

    /// <summary>論理カラム名の先頭セルアドレスを表します。</summary>
    private const string _cellOfColumnLogicalName = "B1";

    /// <summary>物理カラム名の先頭セルアドレスを表します。</summary>
    private const string _cellOfColumnPhysicalName = "C1";

    /// <summary>カラムデータ型名の先頭セルアドレスを表します。</summary>
    private const string _cellOfColumnDataTypeName = "D1";

    /// <summary>必須およびPK列の先頭セルアドレスを表します。</summary>
    private const string _cellOfColumnIsPkOrNotNull = "E1";

    /// <summary>カラムコメントの先頭セルアドレスを表します。</summary>
    private const string _cellOfColumnCommentText = "G1";

    /// <summary>PK 列として判定するセル値を表します。</summary>
    private const string _cellValueOfPk = "PK";

    /// <summary>必須列として判定するセル値を表します。</summary>
    private const string _cellValueOfNotNull = "Yes";

    /// <summary>
    /// 指定されたワークシートが対象の使徒であるかを取得します。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <returns>対象のワークシートの場合は true。それ以外は false。</returns>
    public static bool IsTargetSheet(this IXLWorksheet worksheet)
    {
        ArgumentNullException.ThrowIfNull(worksheet);

        var keyword = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfTargetSheetKeyword).Trim().NullIfEmpty();
        return keyword is ClosedXmlExtensions._targetSheetKeyword1 or ClosedXmlExtensions._targetSheetKeyword2;
    }

    /// <summary>
    /// テーブル情報のコレクションを読み込みます。
    /// </summary>
    /// <param name="worksheets"><see cref="IXLWorksheet"/> のコレクション。</param>
    /// <returns>テーブル情報のコレクション。</returns>
    public static IEnumerable<TableDefinition> LoadTableDefinitions(this IEnumerable<IXLWorksheet> worksheets)
    {
        foreach (var worksheet in worksheets.EmptyIfNull())
        {
            var definition = worksheet.LoadTableDefinition();
            if (definition != null)
            {
                yield return definition;
            }
        }
    }

    /// <summary>
    /// テーブル情報を読み込みます。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <returns>テーブル情報。</returns>
    private static TableDefinition? LoadTableDefinition(this IXLWorksheet worksheet)
    {
        ArgumentNullException.ThrowIfNull(worksheet);

        var physicalName = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfTablePhysicalName).Trim().NullIfEmpty();
        var logicalName = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfTableLogicalName).Trim().NullIfEmpty();

        var definition = physicalName != null
            ? new TableDefinition
            {
                LogicalName = logicalName ?? physicalName,
                PhysicalName = physicalName,
                WorksheetName = worksheet.Name,
                ColumnDefinitions = worksheet.LoadColumnDefinitions().ToList(),
            }
            : null;
        return definition;
    }

    /// <summary>
    /// カラム情報のコレクションを読み込みます。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <returns>カラム情報のコレクション。</returns>
    private static IEnumerable<ColumnDefinition> LoadColumnDefinitions(this IXLWorksheet worksheet)
    {
        var foundFirstRowOffset = worksheet.FindColumnsRow();
        if (foundFirstRowOffset is null)
        {
            yield break;
        }

        var issuedPkNumber = 0;
        for (var rowOffset = foundFirstRowOffset.Value; ; rowOffset++)
        {
            var canContinue = worksheet.TryLoadColumnDefinition(rowOffset, ref issuedPkNumber, out var definition);
            if (!canContinue)
            {
                yield break;
            }

            if (definition != null)
            {
                yield return definition;
            }
        }
    }

    /// <summary>
    /// A1 セルから 50 行の範囲で "カラム情報" の行を検索し、見つかった行 +2 行を返却します。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <returns>A1セルからの行のオフセット。</returns>
    private static int? FindColumnsRow(this IXLWorksheet worksheet)
    {
        // "カラム情報" の行を 50 行の範囲で検索し、見つかった行 +2 行を返却します。
        for (var rowOffset = 0; rowOffset < 50; rowOffset++)
        {
            var keyword = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfTargetSheetKeyword, rowOffset).Trim().NullIfEmpty();
            if (keyword == _targetSheetKeyword3)
            {
                return rowOffset + 2; // 2: カラム情報の行＋No.行
            }
        }

        return null;
    }

    /// <summary>
    /// カラム情報を読み込みます。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <param name="rowOffset">行のオフセット。(0～)</param>
    /// <param name="issuedPkNumber">これまでに発行した PK の順位。(0～)</param>
    /// <param name="definition">カラム情報。</param>
    /// <returns>継続可能な場合は true。それ以外は false。</returns>
    private static bool TryLoadColumnDefinition(this IXLWorksheet worksheet, int rowOffset, ref int issuedPkNumber, out ColumnDefinition? definition)
    {
        var numberName = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfColumnNumberName, rowOffset).Trim().NullIfEmpty();
        var physicalName = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfColumnPhysicalName, rowOffset).Trim().NullIfEmpty();
        var logicalName = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfColumnLogicalName, rowOffset).Trim().NullIfEmpty();
        var dataTypeName = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfColumnDataTypeName, rowOffset).Trim().NullIfEmpty();
        var commentText = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfColumnCommentText, rowOffset).Trim().NullIfEmpty();

        var isPkOrNotNullText = worksheet.GetCellStringOrEmpty(ClosedXmlExtensions._cellOfColumnIsPkOrNotNull, rowOffset).Trim();
        var isNotNull = isPkOrNotNullText.Contains(ClosedXmlExtensions._cellValueOfNotNull, StringComparison.OrdinalIgnoreCase);
        var isPk = isPkOrNotNullText.Contains(ClosedXmlExtensions._cellValueOfPk, StringComparison.OrdinalIgnoreCase);

        // No.列と物理カラム名ともに有効(空欄以外)な場合に限り、カラム情報として抽出します。
        definition = numberName != null && physicalName != null
            ? new ColumnDefinition
            {
                LogicalName = logicalName ?? physicalName,
                PhysicalName = physicalName,
                SqlDataTypeName = dataTypeName ?? string.Empty,
                Comment = commentText ?? string.Empty,
                IsNotNull = isNotNull,
                PkNumber = isPk ? issuedPkNumber++ : null,
            }
            : null;

        // 物理カラム名が有効(空欄以外)な場合に限り継続可能とします。
        return physicalName != null;
    }

    /// <summary>
    /// 列挙子が null であれば空の列挙子にします。
    /// </summary>
    /// <typeparam name="T">要素の型。</typeparam>
    /// <param name="self">要素の列挙子。</param>
    /// <returns>null でない要素の列挙子。</returns>
    private static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? self) => self ?? [];

    /// <summary>
    /// 空文字列であれば null にします。
    /// </summary>
    /// <param name="self">対象の文字列。</param>
    /// <returns>対象の文字列か null。</returns>
    private static string? NullIfEmpty(this string self) => string.IsNullOrEmpty(self) ? null : self;

    /// <summary>
    /// セルアドレスを求めます。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <param name="baseCellAddress">基準となるセルアドレス。</param>
    /// <param name="rowOffset">行のオフセット。(0～)</param>
    /// <param name="columnOffset">列のオフセット。(0～)</param>
    /// <returns>セルアドレス。</returns>
    private static string? ResolveCellAddress(this IXLWorksheet worksheet, string baseCellAddress, int rowOffset, int columnOffset)
    {
        var address = worksheet.Cell(baseCellAddress).Address;
        var rowNumber = address.RowNumber + rowOffset;
        var columnNumber = address.ColumnNumber + columnOffset;
        return worksheet.Cell(rowNumber, columnNumber).Address.ToString();
    }

    /// <summary>
    /// セルに設定された値(文字列)を取得します。
    /// </summary>
    /// <param name="worksheet"><see cref="IXLWorksheet"/>。</param>
    /// <param name="baseCellAddress">基準となるセルアドレス。</param>
    /// <param name="rowOffset">行のオフセット。(0～)</param>
    /// <param name="columnOffset">列のオフセット。(0～)</param>
    /// <returns>セルに設定された値(文字列)。</returns>
    private static string GetCellStringOrEmpty(this IXLWorksheet worksheet, string baseCellAddress, int rowOffset = 0, int columnOffset = 0)
    {
        var cellAddress = worksheet.ResolveCellAddress(baseCellAddress, rowOffset, columnOffset);
        var cellString = cellAddress is not null ? worksheet.Cell(cellAddress).GetFormattedString() : null;
        return cellString ?? string.Empty;
    }
}
