using System;

namespace ThemePack.Common.Helpers
{
    public static class StringHelper
    {
        public static string QuoteIfNecessary(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException(nameof(str));
            }

            if (str[0] != '"')
            {
                str = $"\"{str}";
            }

            if (str[str.Length - 1] != '"')
            {
                str = $"{str}\"";
            }

            return str;
        }
    }
}
