using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
    public class RunPageViewModel : ViewModelBase
    {
        public RunPageModel Model { get; private set; }

        #region Properties

        private ProgramExecutionMode _executionMode;

        public ProgramExecutionMode ExecutionMode
        {
            get
            {
                return this._executionMode;
            }

            set
            {
                this._executionMode = value;
                this.OnPropertyChanged("ExecutionMode");
            }
        }

        private int _animationSpeed;

        public int AnimationSpeed
        {
            get
            {
                return this._animationSpeed;
            }

            set
            {
                if(value >= 0 && value <= 9)
                {
                    this._animationSpeed = value;
                    this.OnPropertyChanged("AnimationSpeed");
                }
            }
        }

        #endregion

        public RunPageViewModel(RunPageModel model)
        {
            this.Model = model;
        }
    }
}