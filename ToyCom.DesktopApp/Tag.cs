using System.Windows.Documents;

namespace ToyCom.DesktopApp
{
	/// <summary>
	/// Tag contains text fragment to colorize
	/// </summary>
	public class Tag
	{
		public TextPointer StartPosition { get; set; }
		public TextPointer EndPosition { get; set; }
		public bool IsEmpty { get; set; }
	}
}