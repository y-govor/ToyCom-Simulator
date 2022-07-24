using System;
using System.Windows;
using System.Windows.Controls;

namespace ToyCom.DesktopApp
{
    /// <summary>
    /// Interaction logic for SettingsPageView.xaml
    /// </summary>
    public partial class SettingsPageView : UserControl
    {
        public SettingsPageView()
        {
            InitializeComponent();
        }

        // Change the language
        private void LanguageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LanguageSelection.SelectedIndex == -1) return;

            // Set the language
            if(LanguageSelection.SelectedIndex == 1)
            {
                // Ukrainian
                Application.Current.Resources.MergedDictionaries[0].Source =
                    new Uri($"/Languages/lang.ua.xaml", UriKind.Relative);
            }
            else
            {
                // Default (English)
                Application.Current.Resources.MergedDictionaries[0].Source =
                    new Uri($"/Languages/lang.xaml", UriKind.Relative);
            }
        }
    }
}