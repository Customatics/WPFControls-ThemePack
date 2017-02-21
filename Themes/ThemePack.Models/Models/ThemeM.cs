using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemePack.Models.Models
{
    /// <summary>
    /// Represents wpf theme resources <see cref="ColorSchemeM"/> and <see cref="ControlStyleM"/>
    /// </summary>
    public class ThemeM
    {
        public ThemeM(string name, IList<ColorSchemeM> schemes, IList<ControlStyleM> styles, IList<NumericValuesM> numerics)
        {
            Name = name;
            ColorSchemeModels = schemes;
            ControlStyleModels = styles;
            NumericValuesModels = numerics;
        }

        /// <summary>
        /// Theme name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of <see cref="ColorSchemeM"/>
        /// </summary>
        public IList<ColorSchemeM> ColorSchemeModels { get; set; }

        /// <summary>
        /// List of <see cref="ControlStyleM"/>
        /// </summary>
        public IList<ControlStyleM> ControlStyleModels { get; set; }

        /// <summary>
        /// List of <see cref="NumericValuesM"/>
        /// </summary>
        public IList<NumericValuesM> NumericValuesModels { get; set; }

    }
}
