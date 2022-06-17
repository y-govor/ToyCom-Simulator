using System.Collections.Generic;
using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
    public class SettingsPageModel
    {
        public Settings Settings { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Themes { get; set; }
        public List<string> Fonts { get; set; }
        public int SelectedLanguageIndex { get; set; }
        public int SelectedThemeIndex { get; set; }
        public int SelectedFontIndex { get; set; }
        public uint FontSize { get; set; }
        public bool IsToolbarVisible { get; set; }
        public bool IsLineNumbersVisible { get; set; }
        public bool IsCommentHighlightEnabled { get; set; }
        public int ExecutionDelay { get; set; }

        public SettingsPageModel(Settings settings)
        {
            this.Settings = settings;
        }
    }
}