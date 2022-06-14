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
            if(mode == ProgramExecutionMode.CalculationsOnly)
            {
                this.CurrentViewModel = new RunPageCalcOnlyViewModel(new RunPageCalcOnlyModel());
            }
        }
    }
}