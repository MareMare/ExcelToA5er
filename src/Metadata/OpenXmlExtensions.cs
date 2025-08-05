// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenXmlExtensions.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelToA5er.Metadata;

/// <summary>
/// OpenXML の拡張メソッドを提供します。
/// </summary>
internal static class OpenXmlExtensions
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
    /// 対象のワークシートを取得します。
    /// </summary>
    /// <param name="workbook">Workbook。</param>
    /// <param name="workbookPart">WorkbookPart。</param>
    /// <returns>対象のワークシート情報のコレクション。</returns>
    public static IEnumerable<WorksheetInfo> GetTargetWorksheets(this Workbook workbook, WorkbookPart workbookPart)
    {
        ArgumentNullException.ThrowIfNull(workbook);
        ArgumentNullException.ThrowIfNull(workbookPart);

        var sheets = workbook.Sheets?.Elements<Sheet>() ?? [];

        foreach (var sheet in sheets)
        {
            if (sheet.Id?.Value == null)
            {
                continue;
            }

            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id.Value);
            var worksheet = worksheetPart.Worksheet;

            if (worksheet.IsTargetSheet(workbookPart))
            {
                yield return new WorksheetInfo { Sheet = sheet, WorksheetPart = worksheetPart, Worksheet = worksheet };
            }
        }
    }

    /// <summary>
    /// 指定されたワークシートが対象のシートであるかを取得します。
    /// </summary>
    /// <param name="worksheet">Worksheet。</param>
    /// <param name="workbookPart">WorkbookPart。</param>
    /// <returns>対象のワークシートの場合は true。それ以外は false。</returns>
    public static bool IsTargetSheet(this Worksheet worksheet, WorkbookPart workbookPart)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(workbookPart);

        var keyword = worksheet.GetCellStringOrEmpty(_cellOfTargetSheetKeyword, workbookPart).Trim().NullIfEmpty();
        return keyword is _targetSheetKeyword1 or _targetSheetKeyword2;
    }

    /// <summary>
    /// テーブル情報のコレクションを読み込みます。
    /// </summary>
    /// <param name="worksheetInfos">WorksheetInfo のコレクション。</param>
    /// <returns>テーブル情報のコレクション。</returns>
    public static IEnumerable<TableDefinition> LoadTableDefinitions(this IEnumerable<WorksheetInfo> worksheetInfos)
    {
        foreach (var worksheetInfo in worksheetInfos.EmptyIfNull())
        {
            var definition = worksheetInfo.LoadTableDefinition();
            if (definition != null)
            {
                yield return definition;
            }
        }
    }

    /// <summary>
    /// テーブル情報を読み込みます。
    /// </summary>
    /// <param name="worksheetInfo">WorksheetInfo。</param>
    /// <returns>テーブル情報。</returns>
    private static TableDefinition? LoadTableDefinition(this WorksheetInfo worksheetInfo)
    {
        ArgumentNullException.ThrowIfNull(worksheetInfo);

        var workbookPart = worksheetInfo.WorksheetPart.GetParentParts().OfType<WorkbookPart>().First();
        var worksheet = worksheetInfo.Worksheet;

        var physicalName = worksheet.GetCellStringOrEmpty(_cellOfTablePhysicalName, workbookPart).Trim().NullIfEmpty();
        var logicalName = worksheet.GetCellStringOrEmpty(_cellOfTableLogicalName, workbookPart).Trim().NullIfEmpty();

        var definition = physicalName != null
            ? new TableDefinition
            {
                LogicalName = logicalName ?? physicalName,
                PhysicalName = physicalName,
                WorksheetName = worksheetInfo.Sheet.Name?.Value ?? string.Empty,
                ColumnDefinitions = worksheetInfo.LoadColumnDefinitions().ToList(),
            }
            : null;
        return definition;
    }

    /// <summary>
    /// カラム情報のコレクションを読み込みます。
    /// </summary>
    /// <param name="worksheetInfo">WorksheetInfo。</param>
    /// <returns>カラム情報のコレクション。</returns>
    private static IEnumerable<ColumnDefinition> LoadColumnDefinitions(this WorksheetInfo worksheetInfo)
    {
        var foundFirstRowOffset = worksheetInfo.FindColumnsRow();
        if (foundFirstRowOffset is null)
        {
            yield break;
        }

        var issuedPkNumber = 0;
        for (var rowOffset = foundFirstRowOffset.Value; ; rowOffset++)
        {
            var canContinue = worksheetInfo.TryLoadColumnDefinition(rowOffset, ref issuedPkNumber, out var definition);
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
    /// <param name="worksheetInfo">WorksheetInfo。</param>
    /// <returns>A1セルからの行のオフセット。</returns>
    private static int? FindColumnsRow(this WorksheetInfo worksheetInfo)
    {
        var workbookPart = worksheetInfo.WorksheetPart.GetParentParts().OfType<WorkbookPart>().First();
        var worksheet = worksheetInfo.Worksheet;

        // "カラム情報" の行を 50 行の範囲で検索し、見つかった行 +2 行を返却します。
        for (var rowOffset = 0; rowOffset < 50; rowOffset++)
        {
            var keyword = worksheet.GetCellStringOrEmpty(_cellOfTargetSheetKeyword, workbookPart, rowOffset).Trim()
                .NullIfEmpty();
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
    /// <param name="worksheetInfo">WorksheetInfo。</param>
    /// <param name="rowOffset">行のオフセット。(0～)</param>
    /// <param name="issuedPkNumber">これまでに発行した PK の順位。(0～)</param>
    /// <param name="definition">カラム情報。</param>
    /// <returns>継続可能な場合は true。それ以外は false。</returns>
    private static bool TryLoadColumnDefinition(
        this WorksheetInfo worksheetInfo,
        int rowOffset,
        ref int issuedPkNumber,
        out ColumnDefinition? definition)
    {
        var workbookPart = worksheetInfo.WorksheetPart.GetParentParts().OfType<WorkbookPart>().First();
        var worksheet = worksheetInfo.Worksheet;

        var numberName = worksheet.GetCellStringOrEmpty(_cellOfColumnNumberName, workbookPart, rowOffset).Trim()
            .NullIfEmpty();
        var physicalName = worksheet.GetCellStringOrEmpty(_cellOfColumnPhysicalName, workbookPart, rowOffset).Trim()
            .NullIfEmpty();
        var logicalName = worksheet.GetCellStringOrEmpty(_cellOfColumnLogicalName, workbookPart, rowOffset).Trim()
            .NullIfEmpty();
        var dataTypeName = worksheet.GetCellStringOrEmpty(_cellOfColumnDataTypeName, workbookPart, rowOffset).Trim()
            .NullIfEmpty();
        var commentText = worksheet.GetCellStringOrEmpty(_cellOfColumnCommentText, workbookPart, rowOffset).Trim()
            .NullIfEmpty()?.ReplaceNewLine();

        var isPkOrNotNullText =
            worksheet.GetCellStringOrEmpty(_cellOfColumnIsPkOrNotNull, workbookPart, rowOffset).Trim();
        var isNotNull = isPkOrNotNullText.Contains(_cellValueOfNotNull, StringComparison.OrdinalIgnoreCase);
        var isPk = isPkOrNotNullText.Contains(_cellValueOfPk, StringComparison.OrdinalIgnoreCase);

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
    /// セルに設定された値(文字列)を取得します。
    /// </summary>
    /// <param name="worksheet">Worksheet。</param>
    /// <param name="baseCellAddress">基準となるセルアドレス。</param>
    /// <param name="workbookPart">WorkbookPart。</param>
    /// <param name="rowOffset">行のオフセット。(0～)</param>
    /// <param name="columnOffset">列のオフセット。(0～)</param>
    /// <returns>セルに設定された値(文字列)。</returns>
    private static string GetCellStringOrEmpty(
        this Worksheet worksheet,
        string baseCellAddress,
        WorkbookPart workbookPart,
        int rowOffset = 0,
        int columnOffset = 0)
    {
        var targetCellAddress = ResolveCellAddress(baseCellAddress, rowOffset, columnOffset);
        if (targetCellAddress == null)
        {
            return string.Empty;
        }

        var cell = worksheet.GetCell(targetCellAddress);
        if (cell == null)
        {
            return string.Empty;
        }

        return GetCellValue(cell, workbookPart);
    }

    /// <summary>
    /// セルアドレスを求めます。
    /// </summary>
    /// <param name="baseCellAddress">基準となるセルアドレス。</param>
    /// <param name="rowOffset">行のオフセット。(0～)</param>
    /// <param name="columnOffset">列のオフセット。(0～)</param>
    /// <returns>セルアドレス。</returns>
    private static string? ResolveCellAddress(string baseCellAddress, int rowOffset, int columnOffset)
    {
        try
        {
            var columnLetters = new string(baseCellAddress.TakeWhile(char.IsLetter).ToArray());
            var rowNumberStr = new string(baseCellAddress.SkipWhile(char.IsLetter).ToArray());

            if (!int.TryParse(rowNumberStr, out var rowNumber))
            {
                return null;
            }

            var newColumnNumber = ColumnLettersToNumber(columnLetters) + columnOffset;
            var newRowNumber = rowNumber + rowOffset;

            if (newColumnNumber < 1 || newRowNumber < 1)
            {
                return null;
            }

            return NumberToColumnLetters(newColumnNumber) + newRowNumber;
        }
#pragma warning disable CA1031
        catch
#pragma warning restore CA1031
        {
            // 例外を握りつぶします。
            return null;
        }
    }

    /// <summary>
    /// 列番号を列文字に変換します。
    /// </summary>
    /// <param name="columnNumber">列番号（1始まり）。</param>
    /// <returns>列文字。</returns>
    private static string NumberToColumnLetters(int columnNumber)
    {
        var result = string.Empty;
        while (columnNumber > 0)
        {
            columnNumber--;
            result = (char)('A' + (columnNumber % 26)) + result;
            columnNumber /= 26;
        }

        return result;
    }

    /// <summary>
    /// 列文字を列番号に変換します。
    /// </summary>
    /// <param name="columnLetters">列文字。</param>
    /// <returns>列番号（1始まり）。</returns>
    private static int ColumnLettersToNumber(string columnLetters)
    {
        var result = 0;
        for (var i = 0; i < columnLetters.Length; i++)
        {
            result = (result * 26) + (columnLetters[i] - 'A' + 1);
        }

        return result;
    }

    /// <summary>
    /// 指定されたセルアドレスのセルを取得します。
    /// </summary>
    /// <param name="worksheet">Worksheet。</param>
    /// <param name="cellAddress">セルアドレス。</param>
    /// <returns>セル。</returns>
    private static Cell? GetCell(this Worksheet worksheet, string cellAddress) =>
        worksheet.Descendants<Cell>()
            .FirstOrDefault(c =>
                string.Equals(c.CellReference?.Value, cellAddress, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// セルの値を取得します。
    /// </summary>
    /// <param name="cell">セル。</param>
    /// <param name="workbookPart">WorkbookPart。</param>
    /// <returns>セルの値。</returns>
    private static string GetCellValue(Cell cell, WorkbookPart workbookPart)
    {
        var value = cell.CellValue?.InnerText ?? string.Empty;

        if (cell.DataType?.Value == CellValues.SharedString)
        {
            var sharedStringTablePart = workbookPart.SharedStringTablePart;
            if (sharedStringTablePart?.SharedStringTable != null)
            {
                var sharedStringTable = sharedStringTablePart.SharedStringTable;
                if (int.TryParse(value, out var index))
                {
                    var sharedStringItems = sharedStringTable.Elements<SharedStringItem>().ToList();
                    if (index >= 0 && index < sharedStringItems.Count)
                    {
                        var sharedStringItem = sharedStringItems[index];

                        // ふりがなを除去してテキストのみを取得
                        return GetTextWithoutPhonetic(sharedStringItem);
                    }
                }
            }
        }
        else if (cell.DataType?.Value == CellValues.InlineString)
        {
            // InlineStringの場合もふりがなが含まれる可能性があるため処理
            var inlineString = cell.InlineString;
            if (inlineString != null)
            {
                return GetInlineStringText(inlineString);
            }
        }

        return value;
    }

    /// <summary>
    /// ふりがなを除去してテキストのみを取得します。
    /// </summary>
    /// <param name="sharedStringItem">SharedStringItem。</param>
    /// <returns>ふりがなを除去したテキスト。</returns>
    private static string GetTextWithoutPhonetic(SharedStringItem sharedStringItem)
    {
        // シンプルなTextの場合
        var text = sharedStringItem.Text?.Text;
        if (!string.IsNullOrEmpty(text))
        {
            return text;
        }

        // Rich Textの場合、ふりがな（PhoneticRun）を除去してテキストのみを結合
        var textBuilder = new System.Text.StringBuilder();

        foreach (var element in sharedStringItem.Elements())
        {
            switch (element)
            {
                case Run run:
                    // Runからテキストを取得（ふりがなはPhoneticRunとして別要素になる）
                    textBuilder.Append(run.Text?.Text ?? string.Empty);
                    break;
                case Text directText:
                    textBuilder.Append(directText.Text);
                    break;
                case PhoneticRun:
                    // PhoneticRunは無視（ふりがな部分）
                    break;
            }
        }

        return textBuilder.ToString();
    }

    /// <summary>
    /// InlineStringからふりがなを除去してテキストのみを取得します。
    /// </summary>
    /// <param name="inlineString">InlineString。</param>
    /// <returns>ふりがなを除去したテキスト。</returns>
    private static string GetInlineStringText(InlineString inlineString)
    {
        // シンプルなTextの場合
        var text = inlineString.Text?.Text;
        if (!string.IsNullOrEmpty(text))
        {
            return text;
        }

        // Rich Textの場合、ふりがな（PhoneticRun）を除去してテキストのみを結合
        var textBuilder = new System.Text.StringBuilder();

        foreach (var element in inlineString.Elements())
        {
            switch (element)
            {
                case Run run:
                    // Runからテキストを取得（ふりがなはPhoneticRunとして別要素になる）
                    textBuilder.Append(run.Text?.Text ?? string.Empty);
                    break;
                case Text directText:
                    textBuilder.Append(directText.Text);
                    break;
                case PhoneticRun:
                    // PhoneticRunは無視（ふりがな部分）
                    break;
            }
        }

        return textBuilder.ToString();
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
    /// 改行コードが含まれる場合はスペースに置換します。
    /// </summary>
    /// <param name="self">対象の文字列。</param>
    /// <returns>改行コードがスペースに置換された文字列。</returns>
    private static string? ReplaceNewLine(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }

        return self.Replace("\r\n", " ", StringComparison.Ordinal)
            .Replace("\r", " ", StringComparison.Ordinal)
            .Replace("\n", " ", StringComparison.Ordinal);
    }
}

/// <summary>
/// ワークシート情報を表します。
/// </summary>
#pragma warning disable SA1402
internal class WorksheetInfo
#pragma warning restore SA1402
{
    /// <summary>
    /// <see cref="DocumentFormat.OpenXml.Spreadsheet.Sheet" />を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="DocumentFormat.OpenXml.Spreadsheet.Sheet" /> 型。
    /// <para><see cref="DocumentFormat.OpenXml.Spreadsheet.Sheet" />。既定値は null です。</para>
    /// </value>
    public Sheet Sheet { get; set; } = null!;

    /// <summary>
    /// <see cref="DocumentFormat.OpenXml.Packaging.WorksheetPart" /> を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="DocumentFormat.OpenXml.Packaging.WorksheetPart" /> 型。
    /// <para><see cref="DocumentFormat.OpenXml.Packaging.WorksheetPart" />。既定値は null です。</para>
    /// </value>
    public WorksheetPart WorksheetPart { get; set; } = null!;

    /// <summary>
    /// <see cref="DocumentFormat.OpenXml.Spreadsheet.Worksheet" /> を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="DocumentFormat.OpenXml.Spreadsheet.Worksheet" /> 型。
    /// <para><see cref="DocumentFormat.OpenXml.Spreadsheet.Worksheet" />。既定値は null です。</para>
    /// </value>
    public Worksheet Worksheet { get; set; } = null!;
}
