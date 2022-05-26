using System.Windows.Controls;
using System.Windows.Media;

namespace ToyCom.DesktopApp
{
    public class TextEditorViewModel : ViewModelBase
    {
        public TextEditorModel Model { get; private set; }

        #region Settings

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

        #endregion

        public TextEditorViewModel(TextEditorModel model)
        {
            this.Model = model;
            this.FontFamily = new FontFamily(this.Model.Settings.FontFamily);
            this.FontSize = this.Model.Settings.FontSize;
        }
    }
}