using System;
using System.IO;
using System.Windows;
using ToyCom.Utilities;
using Tomlet;

namespace ToyCom.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Settings settings = new Settings();

            // Get settings from file
            try
            {
                // Get file contents as a string
                string tomlString = File.ReadAllText("settings.cfg");
                // Convert string to an object
                settings = TomletMain.To<Settings>(tomlString);
            }
            catch(Exception)
            {
                // Convert an object to string
                string tomlString = TomletMain.TomlStringFrom(settings);
                // Create/overwrite config file with default settings
                File.WriteAllText("settings.cfg", tomlString);
            }

            // Set the language
            if(settings.Language == "Українська")
            {
                // Ukrainian
                this.Resources.MergedDictionaries[0].Source =
                    new Uri($"/Languages/lang.ua.xaml", UriKind.Relative);
            }
            else
            {
                // Default (English)
                this.Resources.MergedDictionaries[0].Source =
                    new Uri($"/Languages/lang.xaml", UriKind.Relative);
            }

            Application.Current.MainWindow = new MainWindow() { DataContext = new MainWindowViewModel(settings) };
            Application.Current.MainWindow.Show();
        }
    }
}