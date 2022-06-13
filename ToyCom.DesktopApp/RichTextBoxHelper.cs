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
        public static string GetDocumentText(DependencyObject obj)
        {
            MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            if(mw.CurrentViewModel is TextEditorViewModel)
            {
                return (string)obj.GetValue(DocumentTextProperty);
            }
            return Global.TextEditorLastText;
        }

        public static void SetDocumentText(DependencyObject obj, string value)
        {
            obj.SetValue(DocumentTextProperty, value);
        }

        public static readonly DependencyProperty DocumentTextProperty =
            DependencyProperty.RegisterAttached(
                "DocumentText",
                typeof(string),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = (obj, e) =>
                    {
                        var richTextBox = (RichTextBox)obj;

                        // Parse the XAML to a document (or use XamlReader.Parse())
                        var xaml = GetDocumentText(richTextBox);
                        var doc = new FlowDocument();
                        var range = new TextRange(doc.ContentStart, doc.ContentEnd);

                        range.Text = xaml;

                        // Set the document
                        richTextBox.Document = doc;

                        // When the document changes update the source
                        range.Changed += (obj2, e2) =>
                        {
                            if(richTextBox.Document == doc)
                            {
                                SetDocumentText(richTextBox, range.Text);
                            }
                        };
                    }
                }
        );
    }
}