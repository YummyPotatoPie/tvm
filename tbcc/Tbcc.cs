using System;
using System.IO;
using System.Collections.Generic;

using CommandLine;
using tvmByteCodeCompiler;

namespace tbcc
{
    /// <summary>
    /// Console program for compile byte code source file into binary file 
    /// </summary>
    public sealed class Tbcc
    {
        /// <summary>
        /// Setting for TinyByteCodeCompiler 
        /// </summary>
        private class Settings
        {
            [Value(index: 0, Required = true, HelpText = "Byte code source file path")]
            public IEnumerable<string> ByteCodeFilePath { get; private set; }

            [Option(shortName: 's', longName: "strict", Required = false, HelpText = "Sets strict compilation mode", Default = false)]
            public bool StrictMode { get; private set; }
        }

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Setting for Tbcc</param>
        static void Main(string[] args)
        {
            bool strictMode = false;
            List<string> filePaths = new();

            Parser.Default.ParseArguments<Settings>(args).WithParsed(option =>
            {
                if (option.StrictMode) strictMode = !strictMode;

                foreach (string filePath in option.ByteCodeFilePath)
                {
                    CheckFileCorrectness(filePath);
                    filePaths.Add(filePath);
                }
            });


            foreach (string filePath in filePaths)
            {
                try
                {
                    Console.WriteLine($"Compilation of file {filePath}");
                    string sourceCode = new StreamReader(filePath).ReadToEnd();
                    ByteCodeCompiler compiler = new(sourceCode, strictMode);

                    BinaryWriter binaryWriter = new(File.Open(filePath + "c", FileMode.OpenOrCreate));
                    binaryWriter.Write(compiler.Compile());
                    binaryWriter.Close();

                    Console.WriteLine($"Compilation complete: created file {filePath + "c"}\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Compilation of file {filePath} interrupted\n");
                }
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// Checks file extention and existed of it
        /// </summary>
        private static void CheckFileCorrectness(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (Path.GetExtension(filePath) == ".tbc") return;
                else
                {
                    Console.WriteLine("Invalid extention of file");
                    Environment.Exit(2);
                }
            }
            else
            {
                Console.WriteLine("File does not exist");
                Environment.Exit(1);
            }
        }
    }
}
