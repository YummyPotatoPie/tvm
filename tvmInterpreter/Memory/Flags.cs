namespace tvmInterpreter.Memory
{
    public enum CompareFlags
    {
        Default,
        Lower,
        Equal,
        Bigger
    }

    public class Flags
    {
        public CompareFlags CompareFlag  { get; set; } = CompareFlags.Default;
    }
}
