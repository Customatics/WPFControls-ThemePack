using System;
using ThemePack.Common.Helpers;

namespace ThemePack.Common.Win
{
    public sealed class NativeException : Exception
    {
        public Utils.LT_STATUS Error { get; }

        public NativeException(Utils.LT_STATUS error, string message)
            : base(message)
        {
            Error = error;
        }

        public override string Message
        {
            get
            {
                return $"error {Error}. message: ${base.Message ?? "(-- no message --)"}";
            }
        }
    }
}
