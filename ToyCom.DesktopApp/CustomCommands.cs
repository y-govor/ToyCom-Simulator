using System.Windows.Input;

namespace ToyCom.DesktopApp
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Load = new RoutedUICommand(
            "Load", "Load", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Save = new RoutedUICommand(
            "Save", "Save", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ZoomIn = new RoutedUICommand(
            "ZoomIn", "ZoomIn", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Add, ModifierKeys.Control),
                new KeyGesture(Key.OemPlus, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ZoomOut = new RoutedUICommand(
            "ZoomOut", "ZoomOut", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Subtract, ModifierKeys.Control),
                new KeyGesture(Key.OemMinus, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Exit = new RoutedUICommand(
            "Exit", "Exit", typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );
    }
}