using System;

namespace tvmInterpreter.Memory
{
    /// <summary>
    /// Registers of TVM
    /// </summary>
    public sealed class RegisterMemory
    {
        private readonly int[] _registers = { 0, 0 };

        public RegisterMemory() { }

        /// <summary>
        /// Returns register value
        /// </summary>
        /// <param name="register">Register number</param>
        /// <returns>Register value</returns>
        public int GetRegisterValue(int register) =>
            IsValidRegisterNumber(register) ? _registers[register - 1] : throw new InvalidOperationException("Invalid register number");

        /// <summary>
        /// Sets value to the register
        /// </summary>
        /// <param name="register">Register number</param>
        /// <param name="value">Number lol</param>
        public void SetRegisterValue(int register, int value)
        {
            if (IsValidRegisterNumber(register)) _registers[register - 1] = value;
            else throw new InvalidOperationException("Invalid register number");
        }

        /// <summary>
        /// Checks validness of input register number
        /// </summary>
        /// <param name="registerNumber">Regiser number</param>
        /// <returns>True if valid register number (1-4) else false</returns>
        private bool IsValidRegisterNumber(int registerNumber) => registerNumber >= 1 && registerNumber <= _registers.Length + 1;
    }
}
