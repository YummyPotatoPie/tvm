namespace tvm
{
    /// <summary>
    /// Class represents argument like: -c -v
    /// </summary>
    public class ArgumentValue
    {
        public string Argument { get; private set; }

        public ArgumentValue(string argument) => Argument = argument;
    }

    /// <summary>
    /// Class represents arguments like: --compile=true
    /// </summary>
    public class NameArgumentValue : ArgumentValue
    {
        public string ArgumentValue { get; private set; }

        public NameArgumentValue(string argument, string argumentValue) : base(argument) => ArgumentValue = argumentValue;
    }
}
