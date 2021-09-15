using System;
using System.Collections.Generic;
using CommandLine;

namespace tvm
{
    internal sealed class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Byte code source files")]
        public IEnumerable<string> ByteCodeFiles { get; private set; }

        [Option(shortName: 'i', longName: "interpret", Required = false, HelpText = "Sets interpretation mode", Default = true )]
        public bool Interpretation { get; private set; }
    }
}
