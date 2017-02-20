using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ThemePack.Common.Services
{
    public static class CursorService
    {
        private static int waitCounter;
        private static Cursor activeCursor;

        public static void SetCursor(Cursor cursor)
        {
            if (cursor == Cursors.Wait)
            {
                SetWaitCursor();
            }
            else
            {
                SetActiveCursor(cursor);
            }
        }

        public static void SetWaitCursor()
        {
            UpdateCursor(activeCursor, Interlocked.Increment(ref waitCounter));
        }

        public static void SetNormalCursor()
        {
            Interlocked.Exchange(ref activeCursor, null);
            UpdateCursor(activeCursor, waitCounter);
        }

        private static void SetActiveCursor(Cursor cursor)
        {
            Interlocked.CompareExchange(ref activeCursor, cursor, null);
            UpdateCursor(activeCursor, waitCounter);
        }

        private static void UpdateCursor(Cursor active, int waits)
        {
            if (active != null)
            {
                OverrideCursor(active);
            }
            else
            {
                OverrideCursor(waits != 0
                    ? Cursors.Wait
                    : null);
            }
        }

        private static void OverrideCursor(Cursor cursor)
        {
            Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
            {
                if (Equals(cursor, Mouse.OverrideCursor) == false)
                {
                    Mouse.OverrideCursor = cursor;
                }
            }));
        }
    }
}