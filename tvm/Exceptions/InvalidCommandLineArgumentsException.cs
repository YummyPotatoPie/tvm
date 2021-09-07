using System;

namespace tvm.Exceptions
{
    public class InvalidCommandLineArgumentsException : Exception
    {
        public InvalidCommandLineArgumentsException(string message) : base(message) { }
    }
}
