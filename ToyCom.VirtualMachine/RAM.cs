namespace ToyCom.VirtualMachine
{
    /// <summary>
    /// ToyCom RAM class
    /// </summary>
    public class RAM
    {
        #region Fields

        // ToyCom memory
        private int?[] _memory;

        #endregion

        #region Properties

        // Class indexer
        public int? this[int index]
        {
            get { return _memory[index]; }
            set { _memory[index] = value; }
        }

        #endregion

        #region Methods

        public RAM()
        {
            // Initialize memory array with 100 elements
            _memory = new int?[100];
        }

        #endregion
    }
}