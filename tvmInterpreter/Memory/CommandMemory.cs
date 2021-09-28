using System;

namespace tvmInterpreter.Memory
{
    /// <summary>
    /// Memory for TVM programs
    /// </summary>
    public sealed class CommandMemory
    {
        private readonly byte[] _byteCode;

        private int _position = 0;

        public CommandMemory(byte[] byteCode) => _byteCode = byteCode;

        /// <summary>
        /// Set new pointer position
        /// </summary>
        /// <param name="pointer">New pointer position</param>
        public void SetPointer(int pointer) => _position = pointer;

        /// <summary>
        /// Reads number from command stack
        /// </summary>
        /// <returns>Current number at the command stream</returns>
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

        /// <summary>
        /// Return current command from command stack
        /// </summary>
        /// <returns>Current command</returns>
        public byte GetCurrentCommand() => _byteCode[_position];

        /// <summary>
        /// Checks if command stream reached end
        /// </summary>
        /// <returns>True if reached end else false</returns>
        public bool EndOfProgram() => _position >= _byteCode.Length;

        /// <summary>
        /// Sets command pointer to the next command
        /// </summary>
        public void Next() => _position++;
    }
}
