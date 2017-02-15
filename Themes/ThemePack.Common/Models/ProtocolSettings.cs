using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemePack.Common.Models
{
    public static class ProtocolSettings
    {
        public static readonly Version Version = typeof(ProtocolSettings).Assembly.GetName().Version;
        public static readonly int MarkDisplayTimeMs = 5000; // 5000 ms is a lifetime of marking display app
        public static readonly int PixelSize = sizeof(int);
    }
}
