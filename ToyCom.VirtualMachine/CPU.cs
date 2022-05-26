namespace ToyCom.VirtualMachine
{
    /// <summary>
    /// ToyCom CPU class
    /// </summary>
    public class CPU
    {
        #region Fields

        // Instruction Address Register (IR)
        //
        // The current instruction is loaded into
        // this register for execution
        private int? _instructionAddressRegister;

        // Process Counter Register (PC)
        //
        // Contains the address of the memory cell
        // with the instruction to be loaded into the IR.
        // After the instruction execution its value is incremented
        private int? _processCounterRegister;

        // Accumulator (Acc)
        //
        // Value should be loaded into Acc in order to
        // perform arithmetic operations with it
        private int? _AccumulatorRegister;

        // ZF (Zero Flag)
        //
        // ZF is set to 1 if the result of previous arithmetic
        // operation is zero, set to 0 otherwise
        private bool _zfFlag;

        // SF (Sign Flag)
        //
        // SF is set to 1 if the result of previous arithmetic
        // operation is negative, set to 0 otherwise
        private bool _sfFlag;

        #endregion

        #region Properties

        // Public property for Instruction Address Register (IR)
        public int? IR
        {
            get { return _instructionAddressRegister; }
            set { _instructionAddressRegister = value; }
        }

        // Public property for Process Counter Register (PC)
        public int? PC
        {
            get { return _processCounterRegister; }
            set { _processCounterRegister = value; }
        }

        // Public property for Accumulator (Acc)
        public int? Acc
        {
            get { return _AccumulatorRegister; }
            set { _AccumulatorRegister = value; }
        }

        // Public property for ZF (Zero Flag)
        public bool ZF
        {
            get { return _zfFlag; }
            set { _zfFlag = value; }
        }

        // Public property for SF (Sign Flag)
        public bool SF
        {
            get { return _sfFlag; }
            set { _sfFlag = value; }
        }

        #endregion
    }
}