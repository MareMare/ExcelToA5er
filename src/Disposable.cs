// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Disposable.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er;

/// <summary>
/// Dispose 可能なインスタンスを提供します。
/// </summary>
public static class Disposable
{
    /// <summary>
    /// 何もしない既定の Dispose 可能なインスタンスを取得します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="IDisposable" /> 型。
    /// <para>既定値は <seealso cref="NopDisposable" /> です。</para>
    /// </value>
    public static IDisposable Empty { get; } = new NopDisposable();

    /// <summary>
    /// <see cref="IDisposable.Dispose" /> が呼ばれた時の処理を行うメソッドのデリゲートを指定して <see cref="IDisposable" /> を実装したインスタンスを生成します。
    /// </summary>
    /// <param name="dispose"><see cref="IDisposable.Dispose" /> が呼ばれた時の処理を行うメソッドのデリゲート。</param>
    /// <returns>生成された <see cref="IDisposable" /> を実装したインスタンス。</returns>
    public static IDisposable Create(Action dispose) => new AnonymousDisposable(dispose ?? throw new ArgumentNullException(nameof(dispose)));

    /// <summary>
    /// 何もしない既定の Dispose 可能なインスタンスを表します。
    /// </summary>
    private class NopDisposable : IDisposable
    {
        /// <inheritdoc />
        public void Dispose()
        {
            // no op
        }
    }

    /// <summary>
    /// 匿名の <see cref="IDisposable" /> インターフェイスの実装を表します。
    /// </summary>
    private class AnonymousDisposable : IDisposable
    {
        /// <summary><see cref="IDisposable.Dispose" /> が呼ばれた時の処理を行うメソッドのデリゲートを表します。</summary>
        private volatile Action? _dispose;

        /// <summary>
        /// <see cref="IDisposable.Dispose" /> が呼ばれた時の処理を行うメソッドのデリゲートを指定して <see cref="IDisposable" /> を実装したインスタンスを生成します。
        /// </summary>
        /// <param name="dispose"><see cref="IDisposable.Dispose" /> が呼ばれた時の処理を行うメソッドのデリゲート。</param>
        internal AnonymousDisposable(Action dispose)
        {
            this._dispose = dispose;
        }

        /// <inheritdoc />
        public void Dispose() => Interlocked.Exchange(ref this._dispose, null)?.Invoke();
    }
}
