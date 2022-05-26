using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
	public class TextEditorModel
    {
        public Settings Settings { get; set; }

        public TextEditorModel(Settings settings)
        {
            this.Settings = settings;
        }
	}
}