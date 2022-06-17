﻿using System;

namespace ToyCom.VirtualMachine
{
    /// <summary>
    /// ToyCom Virtual Machine class
    /// </summary>
    public class VM
    {
        public CPU CPU;
        public RAM RAM;
        public int OpCode;
        public int Addr;
        public bool IsFinished;

        public VM(string[] program)
        {
            // Initialize CPU and RAM modules
            this.CPU = new CPU();
            this.RAM = new RAM();

            // Set default parameters
            this.CPU.PC = 0;
            this.CPU.IR = null;
            this.CPU.Acc = null;
            this.CPU.ZF = false;
            this.CPU.SF = false;

            // Load program into memory
            for(int i = 0, j = 0; i < program.Length; i++)
            {
                if(String.IsNullOrEmpty(program[i]) || program[i][0] == ';')
                {
                    // Skip loop iteration if the line is a comment or empty
                    continue;
                }

                this.RAM[j] = Int32.Parse(program[i].Substring(0, 4));
                j++;
            }

            this.IsFinished = false;
        }

        /// <summary>
        /// Execute operation
        /// </summary>
        public void Execute()
        {
            // Extract instruction from memory
            this.CPU.IR = this.RAM[(int)this.CPU.PC];

            // Split instruction
            this.OpCode = (int)this.CPU.IR / 100;
            this.Addr = (int)this.CPU.IR % 100;

            // Execute instruction
            if(this.OpCode == 0) // Halt
            {
                IsFinished = true;
                return;
            }
            else if(this.OpCode == 1) // Load
            {
                Operations.Load(ref this.CPU, ref this.RAM, this.Addr);
            }
            else if(this.OpCode == 2) // Store
            {
                Operations.Store(ref this.CPU, ref this.RAM, this.Addr);
            }
            else if(this.OpCode == 3) // Add
            {
                Operations.Add(ref this.CPU, ref this.RAM, this.Addr);
            }
            else if(this.OpCode == 4) // Subtract
            {
                Operations.Sub(ref this.CPU, ref this.RAM, this.Addr);
            }
            else if(this.OpCode == 5) // Multiply
            {
                Operations.Mul(ref this.CPU, ref this.RAM, this.Addr);
            }
            else if(this.OpCode == 6) // Divide
            {
                Operations.Div(ref this.CPU, ref this.RAM, this.Addr);
            }
            else if(this.OpCode == 7) // Input
            {
                // Increment the program counter
                this.CPU.PC++;
                // Finish method execution
                return;
            }
            else if(this.OpCode == 8) // Output
            {
                // Increment the program counter
                this.CPU.PC++;
                // Finish method execution
                return;
            }
            else if(this.OpCode == 9) // Jump
            {
                Operations.Jump(ref this.CPU, this.Addr);
                // Finish method execution
                return;
            }
            else if(this.OpCode == 10) // Jump If Zero
            {
                Operations.JumpIfZero(ref this.CPU, this.Addr);
                // Finish method execution
                return;
            }
            else if(this.OpCode == 11) // Jump If Negative
            {
                Operations.JumpIfNegative(ref this.CPU, this.Addr);
                // Finish method execution
                return;
            }
            else
            {
                throw new Exception("Wrong Command");
            }

            // Increment the program counter
            this.CPU.PC++;
        }
    }
}