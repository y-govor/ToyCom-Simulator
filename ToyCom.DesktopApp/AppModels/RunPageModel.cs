using ToyCom.Utilities;

namespace ToyCom.DesktopApp
{
    public class RunPageModel
    {
        public Settings Settings { get; set; }
        public ProgramExecutionMode ExecutionMode { get; set; }

        public RunPageModel(Settings settings, ProgramExecutionMode mode)
        {
            this.Settings = settings;
            this.ExecutionMode = mode;
        }
    }
}