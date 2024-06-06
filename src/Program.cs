// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er;

/// <summary>
/// アプリケーションのエントリポイントを提供します。
/// </summary>
internal static class Program
{
    /// <summary>
    /// アプリケーションのメインエントリポイントです。
    /// </summary>
    [STAThread]
    internal static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var mainForm = new MainForm();
        Application.Run(mainForm);
    }
}
