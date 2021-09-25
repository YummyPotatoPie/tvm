using CommandLine;

namespace tvm
{
    /// <summary>
    /// Settings for configurate TVM
    /// </summary>
    internal sealed class CommandLineOptions
    {
        /// <summary>
        /// Input file 
        /// </summary>
        [Value(index: 0, Required = true, HelpText = "Byte code source files")]
        public string ByteCodeFile { get; private set; }

        /// <summary>
        /// Sets interpretation mode
        /// </summary>
        [Option(shortName: 'i', longName: "interpret", Required = false, HelpText = "Sets interpretation mode", Default = false)]
        public bool Interpretation { get; private set; }

        /// <summary>
        /// Sets compilation mode 
        /// </summary>
        [Option(shortName: 'c', longName: "compile", Required = false, HelpText = "Sets compilation mode", Default = false)]
        public bool Compilation { get; private set; }

        /// <summary>
        /// Sets mode for run .tbcc files 
        /// </summary>
        [Option(longName: "compiled-run", Required = false, HelpText = "Run .tbcc source files", Default = false)]
        public bool CompiledRun { get; private set; }

        /// <summary>
        /// Sets strict mode for Tiny Byte Code compiler
        /// </summary>
        [Option(shortName: 's', longName: "strict", Required = false, HelpText = "Uses for strict compile mode", Default = false)]
        public bool StrictMode { get; private set; }
    }
}
