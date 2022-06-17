using System.Windows;
using System.Windows.Input;

namespace ToyCom.DesktopApp
{
    public class RunPageCalcOnlyViewModel : ViewModelBase
    {
        public ICommand LoadTextEditorCommand { get; private set; }

        public RunPageCalcOnlyModel Model { get; private set; }

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

        #endregion

        public RunPageCalcOnlyViewModel(RunPageCalcOnlyModel model)
        {
            this.Model = model;
            this.OutputLog = string.Empty;

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