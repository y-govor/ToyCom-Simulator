using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ToyCom.Utilities;
using ToyCom.VirtualMachine;

namespace ToyCom.Tests
{
    [TestClass]
    public class Tests
    {
        // Unformatted code
        string code = "; ToyCom program example   \n" +
                      "   ; Calculate sum of two numbers\n" +
                      "0730\n" +
                      "  0731\n" +
                      "0130\n" +
                      "0331  \n" +
                      "0232\n" +
                      " 0832\n" +
                      "0000 ";

        /// <summary>
        /// Validate.TrimCode() test
        /// </summary>
        [TestMethod]
        public void TrimCodeTest()
        {
            string[] expected = new string[] {
                "; ToyCom program example",
                "; Calculate sum of two numbers",
                "0730",
                "0731",
                "0130",
                "0331",
                "0232",
                "0832",
                "0000"
            };

            string[] actual = Validate.TrimCode(code).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validate.ValidateCode() test. Code is valid
        /// </summary>
        [TestMethod]
        public void ValidateCodeSuccessTest()
        {
            int line = -1;
            ToyComException e = ToyComException.None;

            bool expected = true;
            bool actual = Validate.ValidateCode(ref line, ref e, code);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validate.ValidateCode() test. Code is invalid
        /// </summary>
        [TestMethod]
        public void ValidateCodeFailureTest()
        {
            string code = "; ToyCom program example   \n" +
                          "; Calculate sum of two numbers\n" +
                          "0730\n" +
                          "0731\n" +
                          "0130\n" +
                          "0331\n" +
                          "0232\n" +
                          "0832\n" +
                          "1299";

            int line = -1;
            ToyComException e = ToyComException.None;

            bool actual = Validate.ValidateCode(ref line, ref e, code);

            // Code should contain WrongOpCode exception on line 8
            Assert.IsFalse(actual);
            Assert.AreEqual(line, 8);
            Assert.AreEqual(e, ToyComException.WrongOpCode);
        }

        /// <summary>
        /// ToyCom Virtual Machine test
        /// </summary>
        [TestMethod]
        public void VirtualMachineTest()
        {
            VM vm = new VM(Validate.TrimCode(code).ToArray());

            for(int i = 0; !vm.IsFinished; i++)
            {
                // Check values that are held by CPU registers
                // before stopping the program (0000 command)
                if(i == 6)
                {
                    Assert.AreEqual(vm.CPU.PC, 6);
                    Assert.AreEqual(vm.CPU.IR, 832);
                    Assert.AreEqual(vm.CPU.Acc, 20);
                }

                if(vm.RAM[(int)vm.CPU.PC] / 100 == 7)
                {
                    Operations.Input(ref vm.RAM, (int)vm.RAM[(int)vm.CPU.PC] % 100, 10);
                }

                if(vm.RAM[(int)vm.CPU.PC] / 100 == 8)
                {
                    int value;
                    Operations.Output(ref vm.RAM, (int)vm.RAM[(int)vm.CPU.PC] % 100, out value);
                }

                vm.Execute();
            }
        }
    }
}