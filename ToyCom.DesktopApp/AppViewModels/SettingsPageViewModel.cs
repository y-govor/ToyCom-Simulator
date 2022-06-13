using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ToyCom.Utilities;
using Tomlet;

namespace ToyCom.DesktopApp
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public ICommand LoadTextEditorCommand { get; private set; }

        public SettingsPageModel Model { get; private set; }

        #region Settings

        public List<string> Languages
        {
            get
            {
                return this.Model.Languages;
            }

            set
            {
                this.Model.Languages = value;
                this.OnPropertyChanged("Languages");
            }
        }

        public List<string> Themes
        {
            get
            {
                return this.Model.Themes;
            }

            set
            {
                this.Model.Themes = value;
                this.OnPropertyChanged("Themes");
            }
        }

        public List<string> Fonts
        {
            get
            {
                return this.Model.Fonts;
            }

            set
            {
                this.Model.Fonts = value;
                this.OnPropertyChanged("Fonts");
            }
        }

        public int SelectedLanguageIndex
        {
            get
            {
                return this.Model.SelectedLanguageIndex;
            }

            set
            {
                this.Model.SelectedLanguageIndex = value;
                this.OnPropertyChanged("SelectedLanguageIndex");
            }
        }

        public int SelectedThemeIndex
        {
            get
            {
                return this.Model.SelectedThemeIndex;
            }

            set
            {
                this.Model.SelectedThemeIndex = value;
                this.OnPropertyChanged("SelectedThemeIndex");
            }
        }

        public int SelectedFontIndex
        {
            get
            {
                return this.Model.SelectedFontIndex;
            }

            set
            {
                this.Model.SelectedFontIndex = value;
                this.OnPropertyChanged("SelectedFontIndex");
            }
        }

        public uint FontSize
        {
            get
            {
                return this.Model.FontSize;
            }

            set
            {
                this.Model.FontSize = value;
                this.OnPropertyChanged("FontSize");
            }
        }

        public bool IsToolbarVisible
        {
            get
            {
                return this.Model.IsToolbarVisible;
            }

            set
            {
                this.Model.IsToolbarVisible = value;
                this.Model.Settings.ShowToolbar = this.IsToolbarVisible;
                this.OnPropertyChanged("IsToolbarVisible");

                ((MainWindowViewModel)Application.Current.MainWindow.DataContext).ToolbarHeight =
                    (this.Model.IsToolbarVisible) ? 28 : 0;
            }
        }

        public bool IsLineNumbersVisible
        {
            get
            {
                return this.Model.IsLineNumbersVisible;
            }

            set
            {
                this.Model.IsLineNumbersVisible = value;
                this.OnPropertyChanged("IsLineNumbersVisible");
            }
        }

        public bool IsCommentHighlightEnabled
        {
            get
            {
                return this.Model.IsCommentHighlightEnabled;
            }

            set
            {
                this.Model.IsCommentHighlightEnabled = value;
                this.OnPropertyChanged("IsCommentHighlightEnabled");
            }
        }

        #endregion

        public SettingsPageViewModel(SettingsPageModel model)
        {
            this.Model = model;
            this.LoadTextEditorCommand = new DelegateCommand(o => this.LoadTextEditor());

            // Add all available languages to the list
            Languages languages;
            try
            {
                languages = new Languages(
                    new Dictionary<string, Language>()
                    {
                        {
                            Application.Current.FindResource("l_Settings_General_Lang_en") as string,
                            Language.English
                        },
                        {
                            Application.Current.FindResource("l_Settings_General_Lang_uk") as string,
                            Language.Ukrainian
                        }
                    }
                );
            }
            catch(Exception)
            {
                languages = new Languages();
            }

            this.Languages = new List<string>();
            foreach(string language in languages)
            {
                this.Languages.Add(language);
            }

            // Selected language
            this.SelectedLanguageIndex = languages.Contains(this.Model.Settings.Language) ?
                (int)languages[this.Model.Settings.Language] : (int)languages[DefaultSettings.Language];

            // Add all available themes to the list
            this.Themes = new List<string>() { "Light", "Dark" };
            // Selected theme
            this.SelectedThemeIndex = (this.Model.Settings.Theme.ToLower() == "dark") ? 1 : 0;

            // Add all installed fonts to the list
            FontFamily[] fonts = (new InstalledFontCollection()).Families;
            this.Fonts = new List<string>();
            for(int i = 0; i < fonts.Length; i++)
            {
                this.Fonts.Add(fonts[i].Name);

                // Selected font
                if(fonts[i].Name == this.Model.Settings.FontFamily)
                {
                    this.SelectedFontIndex = i;
                }
            }

            // Font size
            this.FontSize = this.Model.Settings.FontSize;
            // Toolbar visibility
            this.IsToolbarVisible = this.Model.Settings.ShowToolbar;
            // Line numbers visibility
            this.IsLineNumbersVisible = this.Model.Settings.ShowLines;
            // Comment highlight
            this.IsCommentHighlightEnabled = this.Model.Settings.HighlightComments;
        }

        private void LoadTextEditor()
        {
            UpdateSettings();

            // Switch ViewModel to TextEditorViewModel
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).CurrentViewModel =
                new TextEditorViewModel(new TextEditorModel(this.Model.Settings));
        }

        // Update the settings
        private void UpdateSettings()
        {
            this.Model.Settings.Language = this.Languages[this.SelectedLanguageIndex];
            this.Model.Settings.Theme = this.Themes[this.SelectedThemeIndex];
            this.Model.Settings.ShowToolbar = this.IsToolbarVisible;
            this.Model.Settings.FontFamily = this.Fonts[this.SelectedFontIndex];
            this.Model.Settings.FontSize = this.FontSize;
            this.Model.Settings.ShowLines = this.IsLineNumbersVisible;
            this.Model.Settings.HighlightComments = this.IsCommentHighlightEnabled;

            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).Settings =
                this.Model.Settings;

            // Save settings to config file
            File.WriteAllText("settings.cfg", TomletMain.TomlStringFrom(this.Model.Settings));
        }
    }
}