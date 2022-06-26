// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Generator.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace ExcelToA5er.Generators;

/// <summary>
/// 実行時テンプレートの実行を提供します。
/// </summary>
internal static class Generator
{
    /// <summary>
    /// 非同期操作として、実行時テンプレートを実行しファイルとして保存します。
    /// </summary>
    /// <param name="generator"><see cref="IGenerator"/>。</param>
    /// <param name="outputFilePath">出力先ファイルパス。</param>
    /// <returns>完了を表すタスク。</returns>
    public static async Task GenerateAsync(this IGenerator generator, string outputFilePath)
    {
        var generatedText = generator.TransformText();

        var outputPath = outputFilePath;
        var outputPathFolder = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputPathFolder) && !Directory.Exists(outputPathFolder))
        {
            Directory.CreateDirectory(outputPathFolder);
        }

        await File.WriteAllTextAsync(outputPath, generatedText, Encoding.UTF8).ConfigureAwait(false);
    }
}
