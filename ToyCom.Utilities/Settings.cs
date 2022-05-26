namespace ToyCom.Utilities
{
    /// <summary>
    /// Class for serializing/deserializing settings from config file
    /// </summary>
    public class Settings
    {
        // General Settings
        public string Language { get; set; }
        public string Theme { get; set; }
        public bool ShowToolbar { get; set; }

        // Text Editor Settings
        public string FontFamily { get; set; }
        public uint FontSize { get; set; }
        public bool ShowLines { get; set; }
        public bool HighlightComments { get; set; }

        public Settings()
        {
            Language = DefaultSettings.Language;
            Theme = DefaultSettings.Theme;
            ShowToolbar = DefaultSettings.ShowToolbar;
            FontFamily = DefaultSettings.FontFamily;
            FontSize = (uint)DefaultSettings.FontSize;
            ShowLines = DefaultSettings.ShowLines;
            HighlightComments = DefaultSettings.HighlightComments;
        }
    }
}