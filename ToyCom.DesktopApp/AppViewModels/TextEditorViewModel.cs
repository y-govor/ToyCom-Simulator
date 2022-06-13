using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
    public class TextEditorViewModel : ViewModelBase
    {
        public TextEditorModel Model { get; private set; }

        #region Properties

        private FontFamily _fontFamily;

        public FontFamily FontFamily
        {
            get
            {
                return this._fontFamily;
            }

            set
            {
                this._fontFamily = value;
                this.OnPropertyChanged("FontFamily");
            }
        }

        private double _fontSize;

        public double FontSize
        {
            get
            {
                return this._fontSize;
            }

            set
            {
                this._fontSize = value;
                this.OnPropertyChanged("FontSize");
            }
        }

        private string _textEditortext;

        public string TextEditorText
        {
            get
            {
                return this._textEditortext;
            }

            set
            {
                this._textEditortext = value;
                this.OnPropertyChanged("TextEditorText");
            }
        }

        #endregion

        public TextEditorViewModel(TextEditorModel model)
        {
            this.Model = model;
            this.FontFamily = new FontFamily(this.Model.Settings.FontFamily);
            this.FontSize = this.Model.Settings.FontSize;
            this.TextEditorText = Global.TextEditorLastText;
        }
    }
}