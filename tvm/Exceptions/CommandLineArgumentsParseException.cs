using System;

namespace tvm.Exceptions
{
    public class CommandLineArgumentsParseException : Exception
    {
        public CommandLineArgumentsParseException(string message) : base(message) { }
    }
}
