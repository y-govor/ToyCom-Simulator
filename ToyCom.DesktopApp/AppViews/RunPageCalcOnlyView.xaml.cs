using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToyCom.Utilities;
using ToyCom.VirtualMachine;

namespace ToyCom.DesktopApp
{
    /// <summary>
    /// Interaction logic for RunPageView.xaml
    /// </summary>
    public partial class RunPageCalcOnlyView : UserControl
    {
        int value = 0;
        bool temp = false;

        public RunPageCalcOnlyView()
        {
            InitializeComponent();

            MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

            if(mw.CurrentViewModel is RunPageCalcOnlyViewModel run)
            {
                // Initialize virtual machine
                VM vm = new VM(Validate.TrimCode(Global.TextEditorLastText).ToArray());

                ThreadStart tr = new ThreadStart(() =>
                {
                    while(!vm.IsFinished)
                    {
                        if(vm.RAM[(int)vm.CPU.PC] / 100 == 7)
                        {
                            temp = true;
                            run.OutputLog += "AWAITING INPUT..." + Environment.NewLine;

                            SendInputButton.Click += SendInputButton_Click;

                            ThreadStart start = new ThreadStart(() =>
                            {
                                while(temp) { }
                            });

                            Thread thread = new Thread(start);
                            thread.Start();
                            thread.Join();

                            Operations.Input(ref vm.RAM, (int)vm.RAM[(int)vm.CPU.PC] % 100, value);
                        }

                        if(vm.RAM[(int)vm.CPU.PC] / 100 == 8)
                        {
                            Operations.Output(ref vm.RAM, (int)vm.RAM[(int)vm.CPU.PC] % 100, out value);
                            run.OutputLog += $"OUTPUT[{vm.Addr}]: {value}{Environment.NewLine}";
                        }

                        try
                        {
                            vm.Execute();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show($"Run Time Error: {ex.Message}", "Exception",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                });

                Thread bgThread = new Thread(tr);
                bgThread.Start();
                bgThread.Join();
                run.OutputLog += "PROGRAM FINISHED EXECUTING";
            }
        }

        private void SendInputButton_Click(object sender, EventArgs e)
        {
            Int32.TryParse(InputField.Text, out value);
            temp = false;

            SendInputButton.Click -= SendInputButton_Click;
        }
    }
}