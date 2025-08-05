// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XlsxInformation.Loader.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DocumentFormat.OpenXml.Packaging;

namespace ExcelToA5er.Metadata;

/// <summary>
/// テーブル定義書 Excel ファイル情報を表します。
/// </summary>
internal partial class XlsxInformation
{
    /// <summary>
    /// 非同期操作として、指定された XLSX ファイルパスを読み込みます。
    /// </summary>
    /// <param name="xlsxFilePath">XLSX ファイルパス。</param>
    /// <returns>テーブル定義書情報。</returns>
    public static Task<XlsxInformation> LoadAsync(string xlsxFilePath)
    {
        static XlsxInformation Load(string xlsxFilePath)
        {
            using var stream = new FileStream(xlsxFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var document = SpreadsheetDocument.Open(stream, false);
            var workbookPart = document.WorkbookPart ?? throw new InvalidOperationException("WorkbookPart not found");
            var workbook = workbookPart.Workbook;

            var targetWorkSheets = workbook.GetTargetWorksheets(workbookPart).ToArray();
            var tableDefinitions = targetWorkSheets.LoadTableDefinitions().ToArray();

            var info = new XlsxInformation
            {
                XlsxFilePath = xlsxFilePath,
                TableDefinitions = tableDefinitions,
            };

            return info;
        }

        ArgumentNullException.ThrowIfNull(xlsxFilePath);
        return Task.Run(() => Load(xlsxFilePath));
    }
}
