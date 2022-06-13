using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
    /// <summary>
    /// RichTextBoxHelper class
    /// </summary>
    public class RichTextBoxHelper : DependencyObject
    {
        /// <summary>
        /// Get DocumentTextProperty
        /// </summary>
        public static string GetDocumentText(DependencyObject obj)
        {
            MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

            if(mw.CurrentViewModel is TextEditorViewModel)
            {
                return (string)obj.GetValue(DocumentTextProperty);
            }

            return Global.TextEditorLastText;
        }

        /// <summary>
        /// Set DocumentTextProperty
        /// </summary>
        public static void SetDocumentText(DependencyObject obj, string value)
        {
            obj.SetValue(DocumentTextProperty, value);
        }

        /// <summary>
        /// DocumentText Property
        /// </summary>
        public static DependencyProperty DocumentTextProperty = DependencyProperty.RegisterAttached(
            "DocumentText", typeof(string), typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
                PropertyChangedCallback = (obj, e) =>
                {
                    RichTextBox rtb = (RichTextBox)obj;
                    FlowDocument fd = new FlowDocument();
                    TextRange range = new TextRange(fd.ContentStart, fd.ContentEnd);
                    string text = GetDocumentText(rtb);

                    range.Text = text;
                    rtb.Document = fd;

                    // Update
                    range.Changed += (obj2, e2) =>
                    {
                        if(rtb.Document == fd)
                        {
                            SetDocumentText(rtb, range.Text);
                        }
                    };
                }
            }
        );
    }
}