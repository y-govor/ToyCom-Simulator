using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ToyCom.DesktopApp
{
    /// <summary>
    /// Interaction logic for TextEditorView.xaml
    /// </summary>
    public partial class TextEditorView : UserControl
    {
        public TextEditorView()
        {
            InitializeComponent();

			// Add line numbers
			LinesRTBControl.Document.Blocks.Clear();
			LinesRTBControl.Document.Blocks.Add(new Paragraph(new Run("0")));

			MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
			if(mw.Settings.ShowLines)
            {
				// Calculate line column width
				LineNumColumn.Width = new GridLength(mw.Settings.FontSize);
			}
		}

		#region TextEditorControl

		// Scroll both controls
		private void TextEditorControl_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			LinesRTBControl.ScrollToVerticalOffset(TextEditorControl.VerticalOffset);
		}

		// Colorize comments in text editor
		private void TextEditorControl_TextChanged(object sender, TextChangedEventArgs e)
        {
			TextEditorControl.TextChanged -= this.TextEditorControl_TextChanged;

			// Add line numbers
			LinesRTBControl.Document.Blocks.Clear();
			for(int i = 0; i < TextEditorControl.Document.Blocks.Count; i++)
            {
				LinesRTBControl.Document.Blocks.Add(new Paragraph(new Run(i.ToString())));
			}

			MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
			if(mw.Settings.ShowLines)
            {
				// Calculate line column width
				int digits = 0;
				int num = TextEditorControl.Document.Blocks.Count;
				num = (num > 1) ? (num - 1) : num;

				while (num != 0)
				{
					num /= 10;
					digits++;
				}

				LineNumColumn.Width = new GridLength(mw.Settings.FontSize * digits);
			}

			// Select all text
			TextRange range = new TextRange(TextEditorControl.Document.ContentStart,
											TextEditorControl.Document.ContentEnd);
			// Make it default color
			range.ClearAllProperties();

			// Break if HighlightComments setting is false
			if(!(mw.Settings.HighlightComments))
            {
				TextEditorControl.TextChanged += this.TextEditorControl_TextChanged;
				return;
			}

			string text;
			List<Tag> tags = new List<Tag>();

			// Find all comments
			TextPointer navigator = TextEditorControl.Document.ContentStart;
			while(navigator.CompareTo(TextEditorControl.Document.ContentEnd) < 0)
			{
				TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
				if(context == TextPointerContext.ElementStart && navigator.Parent is Run run)
				{
					text = run.Text;
					if(text != "")
					{
						Tag tag = CheckRun(text, run);
						if(!tag.IsEmpty)
						{
							tags.Add(tag);
						}
					}
				}
				navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
			}

			// Get comment color
			ResourceDictionary res = Application.Current.Resources.MergedDictionaries[1];
			SolidColorBrush brush = (SolidColorBrush)res["TextEditorCommentBrush"];

			// Colorize each comment
			foreach(Tag tag in tags)
			{
				TextRange tr = new TextRange(tag.StartPosition, tag.EndPosition);
				tr.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
			}

			TextEditorControl.TextChanged += this.TextEditorControl_TextChanged;
		}

		// Check Run for comment
		public Tag CheckRun(string text, Run run)
		{
			Tag tag = new Tag();
			tag.IsEmpty = true;

			for(int i = 0; i < text.Length; i++)
			{
				if(text[i] == ';')
				{
					tag.StartPosition = run.ContentStart.GetPositionAtOffset(i, LogicalDirection.Forward);
					tag.EndPosition = run.ContentEnd;
					tag.IsEmpty = false;
					break;
				}
			}

			return tag;
		}

        #endregion
    }
}