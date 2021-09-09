namespace tvm
{
    /// <summary>
    /// Class represents argument like: -c -v
    /// </summary>
    public class ArgumentValue
    {
        public string Argument { get; private set; }

        public ArgumentValue(string argument) => Argument = argument;

        public override bool Equals(object obj) => obj is ArgumentValue value && Argument == value.Argument;

        public override int GetHashCode() => Argument.GetHashCode();

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
