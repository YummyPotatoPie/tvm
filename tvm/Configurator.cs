using System.Collections.Generic;

namespace tvm
{
    /// <summary>
    /// Class implementing builder pattern needed for configuring virtual machine
    /// </summary>
    internal sealed class Configurator
    {
        private readonly List<string> SourceFilePaths;

        private bool Interpretation = false;

        private bool Compilation = false;

        public Configurator() => SourceFilePaths = new();

        public Configurator AddSourceFilePath(string sourceFilePath)
        {
            SourceFilePaths.Add(sourceFilePath);
            return this;
        }

        public Configurator InterpretingMode()
        {
            Interpretation = true;
            return this;
        }

        public Configurator CompilationMode()
        {
            Compilation = true;
            return this;
        }

        public TvmConfiguration Build() => new(SourceFilePaths.ToArray(), Interpretation, Compilation);
    }
}
