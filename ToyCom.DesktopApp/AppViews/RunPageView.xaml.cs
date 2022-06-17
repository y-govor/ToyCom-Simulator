using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ToyCom.Utilities;
using ToyCom.VirtualMachine;

namespace ToyCom.DesktopApp
{
    /// <summary>
    /// Interaction logic for RunPageView.xaml
    /// </summary>
    public partial class RunPageView : UserControl
    {
        int value = 0;
        bool temp = false;

        public RunPageView()
        {
            InitializeComponent();

            MainWindowViewModel mw = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

            if(mw.CurrentViewModel is RunPageViewModel run)
            {
                // Initialize virtual machine
                VM vm = new VM(Validate.TrimCode(Global.TextEditorLastText).ToArray());

                // Display program in DataGrid
                for(int i = 0; i < 100; i++)
                {
                    if(vm.RAM[i] == 0 || vm.RAM[i] == null)
                    {
                        break;
                    }

                    run.ToyComMemory[i][1] = (int)vm.RAM[i];
                }

                ThreadStart tr = new ThreadStart(() =>
                {
                    while(!vm.IsFinished)
                    {
                        // Display program in DataGrid
                        for(int i = 0; i < 100; i++)
                        {
                            run.ToyComMemory[i][1] = vm.RAM[i] ?? 0;
                        }

                        if(vm.RAM[(int)vm.CPU.PC] / 100 == 7)
                        {
                            temp = true;
                            run.OutputLog += "AWAITING INPUT..." + Environment.NewLine;

                            // Display registers values
                            run.AccValue = vm.CPU.Acc ?? 0;
                            run.IRValue = vm.RAM[vm.CPU.PC ?? 0 + 1] ?? 0;
                            run.PCValue = (vm.CPU.PC == 0 || vm.CPU.PC == null) ? 0 : (int)vm.CPU.PC;

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

                            // Display registers values
                            run.AccValue = vm.CPU.Acc ?? 0;
                            run.IRValue = vm.CPU.IR ?? 0;
                            run.PCValue = vm.CPU.PC ?? 0;
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

                        // Display registers values
                        run.AccValue = vm.CPU.Acc ?? 0;
                        run.IRValue = vm.CPU.IR ?? 0;
                        run.PCValue = vm.CPU.PC ?? 0;

                        if(run.Model.ExecutionMode == ProgramExecutionMode.Automatic)
                        {
                            // Delay between executing commands
                            Thread.Sleep(run.Model.Settings.ExecutionDelay);
                        }
                        else
                        {
                            temp = true;
                            run.OutputLog += "PRESS THE BUTTON TO CONTINUE" + Environment.NewLine;
                            run.ContinueButtonVisibility = Visibility.Visible;
                            ContinueButton.Click += ContinueButton_Click;

                            ThreadStart start = new ThreadStart(() =>
                            {
                                while(temp) { }
                            });

                            Thread thread = new Thread(start);
                            thread.Start();
                            thread.Join();

                            run.ContinueButtonVisibility = Visibility.Hidden;
                        }
                    }

                    run.OutputLog += "PROGRAM FINISHED EXECUTING";
                });

                Thread bgThread = new Thread(tr);
                bgThread.Start();
            }
        }

        private void SendInputButton_Click(object sender, EventArgs e)
        {
            Int32.TryParse(InputField.Text, out value);
            temp = false;

            SendInputButton.Click -= SendInputButton_Click;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            temp = false;
            ContinueButton.Click -= ContinueButton_Click;
        }
    }
}