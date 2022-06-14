namespace ToyCom.DesktopApp
{
    public class RunPageCalcOnlyViewModel : ViewModelBase
    {
        public RunPageCalcOnlyModel Model { get; private set; }

        #region Properties

        //

        #endregion

        public RunPageCalcOnlyViewModel(RunPageCalcOnlyModel model)
        {
            this.Model = model;
        }
    }
}