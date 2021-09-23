using System;

namespace tvmInterpreter.Memory
{
    public sealed class CommandMemory
    {
        private readonly byte[] _byteCode;

        private int _position = 0;

        public CommandMemory(byte[] byteCode) => _byteCode = byteCode;

        public int GetValue()
        {
            int value = 0;
            try
            {
                value = BitConverter.ToInt32(_byteCode, _position);
                _position += 4;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }
            return value;
        }

        public byte GetCurrentCommand() => _byteCode[_position];

        public bool EndOfProgram() => _position >= _byteCode.Length;

        public void Next() => _position++;
    }
}
