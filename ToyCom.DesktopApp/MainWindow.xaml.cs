using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        #region Fields

        private const int WM_GETMINMAXINFO = 0x0024;
		private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);
		[DllImport("user32.dll")]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MONITORINFO
		{
			public int cbSize;
			public RECT rcMonitor;
			public RECT rcWork;
			public uint dwFlags;
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			public POINT ptReserved;
			public POINT ptMaxSize;
			public POINT ptMaxPosition;
			public POINT ptMinTrackSize;
			public POINT ptMaxTrackSize;
		}

		#endregion

		public MainWindow()
		{
			InitializeComponent();
		}

        #region Methods

        protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
		}

		public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(msg == WM_GETMINMAXINFO)
			{
				// Tell the system what the size should be when maximized.
				MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

				// Adjust the maximized size and position to fit the work area of the correct monitor
				IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

				if(monitor != IntPtr.Zero)
				{
					MONITORINFO monitorInfo = new MONITORINFO();
					monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
					GetMonitorInfo(monitor, ref monitorInfo);
					RECT rcWorkArea = monitorInfo.rcWork;
					RECT rcMonitorArea = monitorInfo.rcMonitor;
					mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
					mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
					mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
					mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
				}

				Marshal.StructureToPtr(mmi, lParam, true);
			}

			return IntPtr.Zero;
		}

        #endregion

        #region Custom Commands

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// Quit application
			Application.Current.Shutdown();
		}

		private void LoadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void LoadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

			// Read the text from file and load it into the text editor
			if(ofd.ShowDialog() == true)
            {
				Global.TextEditorLastText = File.ReadAllText(ofd.FileName);

				MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
				if(mw.CurrentViewModel is TextEditorViewModel vm)
                {
					vm.TextEditorText = Global.TextEditorLastText;
				}
			}
		}

		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

			// Write the text to file
			if(sfd.ShowDialog() == true)
			{
				MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
				if(mw.CurrentViewModel is TextEditorViewModel vm)
				{
					File.WriteAllText(sfd.FileName, vm.TextEditorText);
				}
				else
                {
					File.WriteAllText(sfd.FileName, Global.TextEditorLastText);
				}				
			}
		}

		#endregion
	}
}