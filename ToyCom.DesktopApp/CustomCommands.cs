using System.Windows.Input;

namespace ToyCom.DesktopApp
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand(
            "Exit", "Exit", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand Load = new RoutedUICommand(
            "Load", "Load", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );
    }
}