// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncDisposable.cs" company="MareMare">
// Copyright © 2021 MareMare All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExcelToA5er
{
    /// <summary>
    /// Dispose 可能なインスタンスを提供します。
    /// </summary>
    internal static class AsyncDisposable
    {
        /// <summary>
        /// 何もしない既定の Dispose 可能なインスタンスを取得します。
        /// </summary>
        /// <value>
        /// 値を表す <see cref="IAsyncDisposable" /> 型。
        /// <para>既定値は <seealso cref="NopAsyncDisposable" /> です。</para>
        /// </value>
        public static IAsyncDisposable Empty { get; } = new NopAsyncDisposable();

        /// <summary>
        /// <see cref="IAsyncDisposable.DisposeAsync" /> が呼ばれた時の処理を行うメソッドのデリゲートを指定して <see cref="IDisposable" /> を実装したインスタンスを生成します。
        /// </summary>
        /// <param name="dispose"><see cref="IAsyncDisposable.DisposeAsync" /> が呼ばれた時の処理を行うメソッドのデリゲート。</param>
        /// <returns>生成された <see cref="IAsyncDisposable" /> を実装したインスタンス。</returns>
        public static IAsyncDisposable Create(Func<ValueTask> dispose) => new AnonymousAsyncDisposable(dispose ?? throw new ArgumentNullException(nameof(dispose)));

        /// <summary>
        /// 何もしない既定の Dispose 可能なインスタンスを表します。
        /// </summary>
        private sealed class NopAsyncDisposable : IAsyncDisposable
        {
            /// <inheritdoc />
            public ValueTask DisposeAsync() => default;
        }

        /// <summary>
        /// 匿名の <see cref="IDisposable" /> インターフェイスの実装を表します。
        /// </summary>
        private sealed class AnonymousAsyncDisposable : IAsyncDisposable
        {
            /// <summary><see cref="IAsyncDisposable.DisposeAsync" /> が呼ばれた時の処理を行うメソッドのデリゲートを表します。</summary>
            private volatile Func<ValueTask>? _dispose;

            /// <summary>
            /// <see cref="IAsyncDisposable.DisposeAsync" /> が呼ばれた時の処理を行うメソッドのデリゲートを指定して <see cref="IDisposable" /> を実装したインスタンスを生成します。
            /// </summary>
            /// <param name="dispose"><see cref="IAsyncDisposable.DisposeAsync" /> が呼ばれた時の処理を行うメソッドのデリゲート。</param>
            internal AnonymousAsyncDisposable(Func<ValueTask> dispose)
            {
                this._dispose = dispose;
            }

            /// <inheritdoc />
            public ValueTask DisposeAsync() => Interlocked.Exchange(ref this._dispose, null)?.Invoke() ?? default;
        }
    }
}
