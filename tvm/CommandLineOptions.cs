using System;
using System.Collections.Generic;
using CommandLine;

namespace tvm
{
    internal sealed class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Byte code source files")]
        public string ByteCodeFile { get; private set; }

        [Option(shortName: 'i', longName: "interpret", Required = false, HelpText = "Sets interpretation mode", Default = true )]
        public bool Interpretation { get; private set; }

        [Option(shortName: 'c', longName: "compile", Required = false, HelpText = "Sets compilation mode", Default = false)]
        public bool Compilation { get; private set; }

        [Option(longName: "compiled-run", Required = false, HelpText = "Run .tbcc source files", Default = false)]
        public bool CompiledRun { get; private set; }
    }
}
