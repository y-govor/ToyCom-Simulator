using System.Windows;
using System.Windows.Input;

namespace ToyCom.DesktopApp
{
    public class RunPageViewModel : ViewModelBase
    {
        public ICommand LoadTextEditorCommand { get; private set; }

        public RunPageModel Model { get; private set; }

        #region Properties

        private string _outputLog;

        public string OutputLog
        {
            get
            {
                return this._outputLog;
            }

            set
            {
                this._outputLog = value;
                this.OnPropertyChanged("OutputLog");
            }
        }

        private int[][] _toycomMemory;

        public int[][] ToyComMemory
        {
            get
            {
                return this._toycomMemory;
            }

            set
            {
                this._toycomMemory = value;
                this.OnPropertyChanged("ToyComMemory");
            }
        }

        private int _accValue;

        public int AccValue
        {
            get
            {
                return this._accValue;
            }

            set
            {
                this._accValue = value;
                this.OnPropertyChanged("AccValue");
            }
        }

        private int _irValue;

        public int IRValue
        {
            get
            {
                return this._irValue;
            }

            set
            {
                this._irValue = value;
                this.OnPropertyChanged("IRValue");
            }
        }

        private int _pcValue;

        public int PCValue
        {
            get
            {
                return this._pcValue;
            }

            set
            {
                this._pcValue = value;
                this.OnPropertyChanged("PCValue");
            }
        }

        private Visibility _continueButtonVisibility;

        public Visibility ContinueButtonVisibility
        {
            get
            {
                return this._continueButtonVisibility;
            }

            set
            {
                this._continueButtonVisibility = value;
                this.OnPropertyChanged("ContinueButtonVisibility");
            }
        }

        #endregion

        public RunPageViewModel(RunPageModel model)
        {
            this.Model = model;
            this.OutputLog = string.Empty;
            this.ToyComMemory = new int[100][];
            this.AccValue = 0;
            this.IRValue = 0;
            this.PCValue = 0;
            this.ContinueButtonVisibility = Visibility.Hidden;

            for(int i = 0; i < 100; i++)
            {
                this.ToyComMemory[i] = new int[2] { i, 0 };
            }

            this.LoadTextEditorCommand = new DelegateCommand(o => this.LoadTextEditor());
        }

        private void LoadTextEditor()
        {
            // Switch ViewModel to TextEditorViewModel
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).CurrentViewModel =
                new TextEditorViewModel(new TextEditorModel(this.Model.Settings));
        }
    }
}