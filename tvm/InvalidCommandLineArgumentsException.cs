using System;

namespace tvm
{
    public class InvalidCommandLineArgumentsException : Exception
    {
        public InvalidCommandLineArgumentsException(string message) : base(message) { }
    }
}
