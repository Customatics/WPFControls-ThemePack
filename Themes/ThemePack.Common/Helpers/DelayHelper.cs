using System;
using System.Threading;

namespace ThemePack.Common.Helpers
{
    /// <summary>
    /// Helper with correct delay handling.
    /// </summary>
    /// <date>13:30 08/10/2016</date>
    /// <author>Anton Liakhovich</author>
    public static class DelayHelper
    {
        /// <summary>
        /// Make a delay.
        /// </summary>
        /// <param name="delay">delay seconds.</param>
        /// <exception cref="OverflowException"><paramref name="delay" /> is less than <see cref="TimeSpan.MinValue" /> or greater than <see cref="TimeSpan.MaxValue" />.
        /// -or-<paramref name="delay" /> is <see cref="Double.PositiveInfinity" />. -or- <paramref name="delay" /> is <see cref="Double.NegativeInfinity"/>. </exception>
        public static bool Delay(double delay)
        {
            return Delay(delay, CancellationToken.None);
        }

        /// <summary>
        /// Make a delay.
        /// </summary>
        /// <param name="delay">delay seconds.</param>
        /// <param name="token">delay <see cref="CancellationToken"/>.</param>
        /// <exception cref="OverflowException"><paramref name="delay" /> is less than <see cref="TimeSpan.MinValue" /> or greater than <see cref="TimeSpan.MaxValue" />.
        /// -or-<paramref name="delay" /> is <see cref="Double.PositiveInfinity" />. -or- <paramref name="delay" /> is <see cref="Double.NegativeInfinity"/>. </exception>
        public static bool Delay(double delay, CancellationToken token)
        {
            return Delay(TimeSpan.FromSeconds(delay), token);
        }

        /// <summary>
        /// Make a delay.
        /// </summary>
        /// <param name="delay">delay milliseconds.</param>
        /// <exception cref="OverflowException"><paramref name="delay" /> is less than <see cref="TimeSpan.MinValue" /> or greater than <see cref="TimeSpan.MaxValue" />.
        /// -or-<paramref name="delay" /> is <see cref="Double.PositiveInfinity" />. -or- <paramref name="delay" /> is <see cref="Double.NegativeInfinity"/>. </exception>
        public static bool DelayMilliseconds(double delay)
        {
            return DelayMilliseconds(delay, CancellationToken.None);
        }

        /// <summary>
        /// Make a delay.
        /// </summary>
        /// <param name="delay">delay milliseconds.</param>
        /// <param name="token">delay <see cref="CancellationToken"/>.</param>
        /// <exception cref="OverflowException"><paramref name="delay" /> is less than <see cref="TimeSpan.MinValue" /> or greater than <see cref="TimeSpan.MaxValue" />.
        /// -or-<paramref name="delay" /> is <see cref="Double.PositiveInfinity" />. -or- <paramref name="delay" /> is <see cref="Double.NegativeInfinity"/>. </exception>
        public static bool DelayMilliseconds(double delay, CancellationToken token)
        {
            return Delay(TimeSpan.FromMilliseconds(delay), token);
        }

        /// <summary>
        /// Make a delay.
        /// </summary>
        /// <param name="delay"><see cref="TimeSpan"/> to wait..</param>
        public static bool Delay(TimeSpan delay)
        {
            return Delay(delay, CancellationToken.None);
        }

        /// <summary>
        /// Make a delay.
        /// </summary>
        /// <param name="delay"><see cref="TimeSpan"/> to wait..</param>
        /// <param name="token">delay <see cref="CancellationToken"/>.</param>
        public static bool Delay(TimeSpan delay, CancellationToken token)
        {
            if (delay.TotalMilliseconds <= 0)
            {
                return false;
            }

            try
            {
                var waitEvent = new ManualResetEventSlim(false);
                waitEvent.Wait(delay, token);

                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }
    }
}