namespace tvm
{
    /// <summary>
    /// Class contains tvm settings
    /// </summary>
    internal sealed class TvmConfiguration
    {
        public readonly string[] SourceFilePaths;

        public readonly bool Interpretation;

        public readonly bool Compilation;

        public TvmConfiguration(string[] sourceFilePaths, bool interpretation, bool compilation)
        {
            SourceFilePaths = sourceFilePaths;
            Interpretation = interpretation;
            Compilation = compilation;
        }
    }
}
