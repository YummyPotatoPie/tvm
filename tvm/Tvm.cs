using System;
using System.IO;

using CommandLine;

using tvmByteCodeCompiler;
using tvmInterpreter;

namespace tvm
{
    /// <summary>
    /// Tiny Virtual Machine main class 
    /// </summary>
    public sealed class Tvm
    {
        /// <summary>
        /// Execute mode flags 
        /// </summary>
        private enum ExecuteMode
        {
            Default, // Interpretation
            Interpretation, 
            Compilation
        }

        private static ExecuteMode _executeMode = ExecuteMode.Default;

        private static bool _compiledRun = false;

        private static bool _strictMode = false;

        private static string _filePath = "";

        /// <summary>
        /// Method sets configure of TVM and execute byte code 
        /// </summary>
        /// <param name="args">TVM settting</param>
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

        /// <summary>
        /// Sets TVM configuration
        /// </summary>
        /// <param name="args">TVM settings</param>
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
                _strictMode = option.StrictMode;
            });
        }

        /// <summary>
        /// Execute byte code
        /// </summary>
        private static void Execute()
        {
            byte[] commands;

            if (_compiledRun) commands = File.ReadAllBytes(_filePath);
            else
            {
                StreamReader reader = new(_filePath);
                string sourceCode = reader.ReadToEnd();
                reader.Close();

                commands = new ByteCodeCompiler(sourceCode, _strictMode).Compile();
            }

            if (_executeMode == ExecuteMode.Interpretation || _executeMode == ExecuteMode.Default) new Interpreter(commands).Interpret();
            else Console.WriteLine("Compilation mode does not support yet");
        }

        /// <summary>
        /// Checks correctness of input file 
        /// </summary>
        /// <param name="filePath">Input file with byte code</param>
        /// <param name="extention">Correct extention for TVM configutarion</param>
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
