using System.Collections.Generic;

namespace Uwp.ThemePack.Models.Models
{
    /// <summary>
    /// Represents wpf theme resources <see cref="ColorSchemeM"/> and <see cref="ControlStyleM"/>
    /// </summary>
    public class ThemeM
    {
        public ThemeM(string name, IList<ColorSchemeM> schemes, IList<ControlStyleM> styles)
        {
            Name = name;
            ColorSchemeModels = schemes;
            ControlStyleModels = styles;
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

    }
}
