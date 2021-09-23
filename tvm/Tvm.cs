using System;
using System.IO;

using CommandLine;

using tvmByteCodeCompiler;
using tvmInterpreter;

namespace tvm
{
    /// <summary>
    /// Main program class 
    /// </summary>
    public sealed class Tvm
    {
        private enum ExecuteMode
        {
            Default, // Interpretation
            Interpretation, 
            Compilation
        }

        private static ExecuteMode _executeMode = ExecuteMode.Default;

        private static bool _compiledRun = false;

        private static string _filePath = "";

        public static void Main(string[] args)
        {
            try
            {
                ConfigureTvm(args);
                Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        private static void ConfigureTvm(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(option =>
            {
                if (!File.Exists(option.ByteCodeFile))
                {
                    Console.WriteLine("File does not exist");
                    Environment.Exit(1);
                }

                if (option.Interpretation)
                {
                    _executeMode = ExecuteMode.Interpretation;
                }
                if (option.Compilation)
                {
                    if (_executeMode == ExecuteMode.Interpretation)
                    {
                        Console.WriteLine("Cannot set interpretation and compilation mode at the same time");
                        Environment.Exit(1);
                    }
                    _executeMode = ExecuteMode.Compilation;
                }

                if (option.CompiledRun)
                {
                    CheckFileCorrectness(option.ByteCodeFile, ".tbcc");
                    _compiledRun = !_compiledRun;
                }
                else CheckFileCorrectness(option.ByteCodeFile, ".tbc");

                _filePath = option.ByteCodeFile;
            });
        }

        private static void Execute()
        {
            byte[] commands;

            if (_compiledRun) commands = File.ReadAllBytes(_filePath);
            else commands = new ByteCodeCompiler(new StreamReader(_filePath).ReadToEnd()).Compile();

            if (_executeMode == ExecuteMode.Interpretation || _executeMode == ExecuteMode.Default) new Interpreter(commands).Interpret();
            else Console.WriteLine("Compilation mode does not support yet");
        }

        private static void CheckFileCorrectness(string filePath, string extention)
        {
            if (!(Path.GetExtension(filePath) == extention))
            {
                Console.WriteLine("Invalid extention of input file");
                Environment.Exit(1);
            }
        }
    }
}
