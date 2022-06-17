using System;
using System.Windows;
using System.Windows.Input;
using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand LoadTextEditorCommand { get; private set; }
        public ICommand LoadSettingsPageCommand { get; private set; }

        public ICommand LoadRunPageAutomaticCommand { get; private set; }
        public ICommand LoadRunPageStepOverCommand { get; private set; }
        public ICommand LoadRunPageCalcOnlyCommand { get; private set; }

        public ICommand ShowHelpCommand { get; private set; }

        #region Properties

        // ViewModel that is currently bound to the ContentControl
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return this._currentViewModel;
            }

            set
            {
                this._currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        // Object that stores app settings
        private Settings _settings;

        public Settings Settings
        {
            get
            {
                return this._settings;
            }

            set
            {
                this._settings = value;
                this.OnPropertyChanged("Settings");
            }
        }

        // Toolbar height
        private int _toolbarHeight;

        public int ToolbarHeight
        {
            get
            {
                return this._toolbarHeight;
            }

            set
            {
                this._toolbarHeight = value;
                this.OnPropertyChanged("ToolbarHeight");
            }
        }

        #endregion

        public MainWindowViewModel(Settings settings)
        {
            this.Settings = settings;
            this.ToolbarHeight = 28;

            // Load Text Editor as default viewmodel
            this.LoadTextEditor();

            // Hook up Commands to associated methods
            this.LoadTextEditorCommand = new DelegateCommand(o => this.LoadTextEditor());
            this.LoadSettingsPageCommand = new DelegateCommand(o => this.LoadSettingsPage());

            this.LoadRunPageAutomaticCommand = new DelegateCommand(o =>
                this.LoadRunPage(ProgramExecutionMode.Automatic));

            this.LoadRunPageStepOverCommand = new DelegateCommand(o =>
                this.LoadRunPage(ProgramExecutionMode.StepOver));

            this.LoadRunPageCalcOnlyCommand = new DelegateCommand(o =>
                this.LoadRunPage(ProgramExecutionMode.CalculationsOnly));

            this.ShowHelpCommand = new DelegateCommand(o =>
            {
                MessageBox.Show(
                    "0000 – Finish program execution" + Environment.NewLine +
                    "01xx – Load number into Accumulator" + Environment.NewLine +
                    "02xx – Store number in specified address" + Environment.NewLine +
                    "03xx – Add specified number to Accumulator" + Environment.NewLine +
                    "04xx – Subtract specified number from Accumulator" + Environment.NewLine +
                    "05xx – Multiply Accumulator by specified number" + Environment.NewLine +
                    "06xx – Divide Accumulator by specified number" + Environment.NewLine +
                    "07xx – Input value into the specified address" + Environment.NewLine +
                    "08xx – Output value stored in the specified address" + Environment.NewLine +
                    "09xx – Jump to specified address" + Environment.NewLine +
                    "10xx – Jump to specified address if ZF = 1" + Environment.NewLine +
                    "11xx – Jump to specified address if SF = 1",
                    "ToyCom Commands", MessageBoxButton.OK);
            });
        }

        private void LoadTextEditor()
        {
            this.CurrentViewModel = new TextEditorViewModel(new TextEditorModel(this.Settings));
        }

        private void LoadSettingsPage()
        {
            this.CurrentViewModel = new SettingsPageViewModel(new SettingsPageModel(this.Settings));
        }

        private void LoadRunPage(ProgramExecutionMode mode)
        {
            int line = -1;
            ToyComException e = ToyComException.None;
            
            // Validate code
            if(!Validate.ValidateCode(ref line, ref e, Global.TextEditorLastText))
            {
                MessageBox.Show($"{e}Exception on line {line}", "Compilation error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Load Run Page
            if(mode == ProgramExecutionMode.CalculationsOnly)
            {
                this.CurrentViewModel = new RunPageCalcOnlyViewModel(new RunPageCalcOnlyModel(this.Settings));
            }
            else
            {
                this.CurrentViewModel = new RunPageViewModel(new RunPageModel(this.Settings, mode));
            }
        }
    }
}