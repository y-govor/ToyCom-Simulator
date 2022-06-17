using System;

namespace ToyCom.VirtualMachine
{
    public static class Operations
    {
        // 01: Load
        // Load Acc from memory
        public static void Load(ref CPU cpu, ref RAM ram, int operand)
        {
            int value = (ram[operand] == null) ? 0 : ram[operand].Value;
            cpu.Acc = value;
        }

        // 02: Store
        // Store the Acc to memory
        public static void Store(ref CPU cpu, ref RAM ram, int operand)
        {
            int value = (cpu.Acc == null) ? 0 : cpu.Acc.Value;
            ram[operand] = value;
        }

        // 03: Add
        // Add value to the Acc
        public static void Add(ref CPU cpu, ref RAM ram, int operand)
        {
            int value = (ram[operand] == null) ? 0 : ram[operand].Value;

            if(((cpu.Acc + value) > 9999) || ((cpu.Acc + value) < -9999))
            {
                throw new Exception("Overflow");
            }

            cpu.Acc += value;

            cpu.SF = (cpu.Acc == 0) ? true : false;
            cpu.ZF = (cpu.Acc < 0) ? true : false;
        }

        // 04: Sub
        // Subtract value from the Acc
        public static void Sub(ref CPU cpu, ref RAM ram, int operand)
        {
            int value = (ram[operand] == null) ? 0 : ram[operand].Value;

            if(((cpu.Acc - value) > 9999) || ((cpu.Acc - value) < -9999))
            {
                throw new Exception("Overflow");
            }

            cpu.Acc -= value;

            cpu.SF = (cpu.Acc == 0) ? true : false;
            cpu.ZF = (cpu.Acc < 0) ? true : false;
        }

        // 05: Mul
        // Multiply the Acc by value
        public static void Mul(ref CPU cpu, ref RAM ram, int operand)
        {
            int value = (ram[operand] == null) ? 0 : ram[operand].Value;

            if(((cpu.Acc * value) > 9999) || ((cpu.Acc * value) < -9999))
            {
                throw new Exception("Overflow");
            }

            cpu.Acc *= value;

            cpu.SF = (cpu.Acc == 0) ? true : false;
            cpu.ZF = (cpu.Acc < 0) ? true : false;
        }

        // 06: Div
        // Divide the Acc by value
        public static void Div(ref CPU cpu, ref RAM ram, int operand)
        {
            if(ram[operand] == null || ram[operand] == 0)
            {
                throw new Exception("Division By Zero");
            }

            cpu.Acc /= ram[operand];

            cpu.SF = (cpu.Acc == 0) ? true : false;
            cpu.ZF = (cpu.Acc < 0) ? true : false;
        }

        // 07: Input
        // Wait for user input and place value to the memory
        public static void Input(ref RAM ram, int operand, int value)
        {
            ram[operand] = value;
        }

        // 08: Output
        // Output value from the memory
        public static void Output(ref RAM ram, int operand, out int value)
        {
            value = (ram[operand] == null) ? 0 : ram[operand].Value;
        }

        // 09: Jump
        // Change program counter to the specified address
        public static void Jump(ref CPU cpu, int operand)
        {
            cpu.PC = operand;
        }

        // 10: JumpIfZero
        // Change program counter to the specified address if ZF = 1
        public static void JumpIfZero(ref CPU cpu, int operand)
        {
            if(cpu.ZF)
            {
                cpu.PC = operand;
            }
        }

        // 11: JumpIfNegative
        // Change program counter to the specified address if SF = 1
        public static void JumpIfNegative(ref CPU cpu, int operand)
        {
            if(cpu.SF)
            {
                cpu.PC = operand;
            }
        }
    }
}