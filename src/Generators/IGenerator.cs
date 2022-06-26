// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenerator.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er.Generators;

/// <summary>
/// 実行時テンプレートを示すインターフェイスを表します。
/// </summary>
internal interface IGenerator
{
    /// <summary>
    /// テンプレートを実行します。
    /// </summary>
    /// <returns>テンプレートの実行結果の文字列。</returns>
    string TransformText();
}
