using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ToyCom.Utilities;

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

			MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

			// If ShowLines option is enabled
			if(mw.Settings.ShowLines)
			{
				// Add first line number
				LinesRTBControl.Document.Blocks.Clear();
				LinesRTBControl.Document.Blocks.Add(new Paragraph(new Run("0")));

				// Calculate line column width
				LineNumColumn.Width = new GridLength(mw.Settings.FontSize);
			}
		}

		#region TextEditorControl Events

		// Scroll both controls
		private void TextEditorControl_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			LinesRTBControl.ScrollToVerticalOffset(TextEditorControl.VerticalOffset);
		}

		// Update RTB parameters after the text has been changed
		private void TextEditorControl_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextEditorControl.TextChanged -= this.TextEditorControl_TextChanged;

			// Add line numbers
			AddLines();
			// Calculate line column width
			CalcWidth();
			// Colorize comments
			HighlightComments();

			TextEditorControl.TextChanged += this.TextEditorControl_TextChanged;
		}

		// Apply parameters to the TextEditorControl when loaded
		private void TextEditorControl_Loaded(object sender, RoutedEventArgs e)
		{
			/*
			TextRange tr = new TextRange(TextEditorControl.Document.ContentStart,
										 TextEditorControl.Document.ContentEnd);

			// Restore contents of the TextEditorControl
			if(String.IsNullOrWhiteSpace(tr.Text))
            {
				//TextEditorControl.TextChanged -= this.TextEditorControl_TextChanged;
				//TextEditorControl.TextChanged += this.TextEditorControl_TextChanged;

				MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
				((TextEditorViewModel)mw.CurrentViewModel).TextEditorText = Global.TextEditorLastText;
			}
			*/

			new TextRange(TextEditorControl.Document.ContentStart,
				TextEditorControl.Document.ContentEnd).Text = Global.TextEditorLastText;


			// Add line numbers
			AddLines();
			// Calculate line column width
			CalcWidth();
			// Colorize comments
			HighlightComments();
		}

		// Save TextEditorControl content
		private void TextEditorControl_Unloaded(object sender, RoutedEventArgs e)
		{
			Global.TextEditorLastText = new TextRange(TextEditorControl.Document.ContentStart,
				TextEditorControl.Document.ContentEnd).Text;
		}

		#endregion

		#region TextEditorControl Methods

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

		// Add line numbers
		private void AddLines()
		{
			MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

			// If ShowLines option is enabled
			if(mw.Settings.ShowLines)
			{
				LinesRTBControl.Document.Blocks.Clear();
				for(int i = 0; i < TextEditorControl.Document.Blocks.Count; i++)
				{
					LinesRTBControl.Document.Blocks.Add(new Paragraph(new Run(i.ToString())));
				}
			}
		}

		// Calculate line column width
		private void CalcWidth()
		{
			MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

			// If ShowLines option is enabled
			if(mw.Settings.ShowLines)
			{
				int digits = 0;
				int num = TextEditorControl.Document.Blocks.Count;
				num = (num > 1) ? (num - 1) : num;

				while(num != 0)
				{
					num /= 10;
					digits++;
				}

				// Change LineNumColumn width
				LineNumColumn.Width = new GridLength(mw.Settings.FontSize * digits);
			}
		}

		// Colorize comments in text editor
		private void HighlightComments()
		{
			// Select all text
			TextRange range = new TextRange(TextEditorControl.Document.ContentStart,
											TextEditorControl.Document.ContentEnd);
			// Make it default color
			range.ClearAllProperties();

			// Break if HighlightComments setting is false
			if(!((MainWindowViewModel)Application.Current.MainWindow.DataContext).Settings.HighlightComments)
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
		}

		#endregion
	}
}