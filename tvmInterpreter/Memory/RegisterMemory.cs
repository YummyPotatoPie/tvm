using System;

namespace tvmInterpreter.Memory
{
    public sealed class RegisterMemory
    {
        private int[] _registers = { 0, 0, 0, 0 };

        public RegisterMemory() { }

        public int GetRegisterValue(int register) =>
            IsValidRegisterNumber(register) ? _registers[register - 1] : throw new InvalidOperationException("Invalid register number");

        public void SetRegisterValue(int register, int value)
        {
            if (IsValidRegisterNumber(register)) _registers[register - 1] = value;
            else throw new InvalidOperationException("Invalid register number");
        }

        private static bool IsValidRegisterNumber(int registerNumber) => registerNumber >= 1 && registerNumber <= 4;
    }
}
