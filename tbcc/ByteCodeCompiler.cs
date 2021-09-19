using System.Collections.Generic;
using System.IO;
using System;

using tvmByteCodeParser;
using tvmByteCodeCommands;

using CommandLine;

namespace tbcc
{
    /// <summary>
    /// Tiny byte code compiler
    /// </summary>
    public sealed class ByteCodeCompiler
    {
        private class ParserOptions
        {
            [Value(index: 0, Required = true, HelpText = "Byte code source files")]
            public IEnumerable<string> SourceFiles { get; private set; }

            [Option(shortName: 's', longName: "strict", Required = false, HelpText = "Set strict mode for compiler", Default = false)]
            public bool StrictMode { get; private set; }
        }

        private static bool StrictMode; 

        public static void Main(string[] args)
        {
            string[] sourceFiles = Array.Empty<string>();

            Parser.Default.ParseArguments<ParserOptions>(args).WithParsed(option =>
            {
                List<string> parsedFiles = new();
                foreach (string filename in option.SourceFiles)
                {
                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("File does not exist: " + filename);
                        Environment.Exit(1);
                    }
                    else if (!(Path.GetExtension(filename) == ".tbc"))
                    {
                        Console.WriteLine("Invalid extention of source file: " + filename);
                        Environment.Exit(2);
                    }
                    else parsedFiles.Add(filename);
                }
                sourceFiles = parsedFiles.ToArray();

                StrictMode = option.StrictMode;
            });

            CompileByteCodeFiles(sourceFiles);
            Console.WriteLine("Compilation complete");
        }

        private static void CompileByteCodeFiles(string[] sourceFiles)
        {
            foreach (string sourceFile in sourceFiles)
            {
                StreamReader reader = new(sourceFile);

                string[] commands = new ByteCodeParser(reader.ReadToEnd()).Parse();
                BinaryWriter compiledCodeWriter = new(File.Open(sourceFile + "c", FileMode.OpenOrCreate));

                compiledCodeWriter.Write(GetBytesFromByteCode(commands));
                compiledCodeWriter.Close();
            }
        }

        private static byte[] GetBytesFromByteCode(string[] commands)
        {
            Dictionary<string, byte> commandsOpCodes = ByteCodeCommands.Commands;
            List<byte> bytes = new();

            foreach (string command in commands)
            {
                if (commandsOpCodes.ContainsKey(command)) bytes.Add(commandsOpCodes[command]);
                else
                {
                    try
                    {
                        bool successParse = int.TryParse(command, out int value);
                        if (!successParse)
                        {
                            if (!StrictMode)
                            {
                                Console.WriteLine($"WARNING: {command} is not 32bit number, set to default value (0)");
                                value = 0;
                            }
                            else
                            {
                                Console.WriteLine($"ERROR: {command} is not 32bit number, compilation interrupted");
                                Environment.Exit(4);
                            }
                        }
                        byte[] valueBytes = BitConverter.GetBytes(value);

                        foreach (byte valueByte in valueBytes) bytes.Add(valueByte);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Invalid number value");
                        Environment.Exit(3);
                    }
                }
            }

            return bytes.ToArray();
        }
    }
}
