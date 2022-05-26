using System.Drawing;

namespace ToyCom.Utilities
{
    /// <summary>
    /// Class containing default settings
    /// If parsing settings fails, the app will fall back to default settings
    /// </summary>
    public static class DefaultSettings
    {
        // General Settings
        public static string Language = "English";
        public static string Theme = "Light";
        public static bool ShowToolbar = true;

        // Text Editor Settings
        public static string FontFamily = "Consolas";
        public static double FontSize = 14;
        public static bool ShowLines = false;
        public static bool HighlightComments = false;
    }
}