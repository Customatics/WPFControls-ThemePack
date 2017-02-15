using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemePack.Common.Base
{
    public sealed class DPI
    {
        public double X { get; }
        public double Y { get; }

        public DPI(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
