using System.Collections.Generic;
using System.IO;
using System;
using CommandLine;

namespace tbcc
{
    /// <summary>
    /// Tiny byte code compiler
    /// </summary>
    public sealed class Tbcc
    {
        private class ParserOptions
        {
            [Value(index: 0, Required = true, HelpText = "Byte code source files")]
            public IEnumerable<string> SourceFiles { get; private set; }
        }

        public static void Main(string[] args)
        {
            string[] sourceFiles;

            Parser.Default.ParseArguments<ParserOptions>(args).WithParsed(option =>
            {
                List<string> parsedFiles = new();
                foreach (string filename in option.SourceFiles)
                {
                    if (File.Exists(filename) && Path.GetExtension(filename) == ".tbc") parsedFiles.Add(filename);
                    else
                    {
                        Console.WriteLine("Invalid extention of source file");
                        return;
                    }
                }
                sourceFiles = parsedFiles.ToArray();
            });


        }

        private static void CompileByteCodeFiles(string[] sourceFiles)
        {
            foreach (string sourceFile in sourceFiles)
            {
                StreamReader reader = new(sourceFile);
                BinaryWriter binaryWriter = new(File.Open(sourceFile + "c", FileMode.OpenOrCreate));

                binaryWriter.Write(new ByteCodeParser(reader.ReadToEnd()).Parse());
                binaryWriter.Close();
            }
        }
    }
}
