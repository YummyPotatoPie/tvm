using System;
using System.Text;
using System.Collections.Generic;

using tvm.Exceptions;

namespace tvm
{
    /// <summary>
    /// Main program class 
    /// </summary>
    public sealed class Tvm
    {
        public static void Main(string[] args)
        {
            try
            {
                var parsedArgs = ArgumentParser<ArgumentValue>.ParseArgs(string.Join("", args));
                var configuration = GetConfiguration(parsedArgs);

                //if (configuration.Interpretation) Interpreter.Interpret(configuration);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        /// <summary>
        /// Get virtual machine configuration by command line arguments
        /// </summary>
        /// <param name="parsedArgs">Parsed command line arguments</param>
        /// <returns>Virtual machine configuration</returns>
        private static TvmConfiguration GetConfiguration(Dictionary<ArgumentType, ArgumentValue> parsedArgs)
        {
            Configurator configurator = new(); 

            foreach (KeyValuePair<ArgumentType, ArgumentValue> arg in parsedArgs)
            {
                if (arg.Key == ArgumentType.Filename) configurator = configurator.AddSourceFilePath(arg.Value.Argument);
                else if (arg.Key == ArgumentType.Special)
                {
                    switch (arg.Value.Argument)
                    {
                        case "i":
                            configurator = configurator.InterpretingMode();
                            break;
                        case "c":
                            configurator = configurator.CompilationMode();
                            break;
                    }
                }
                //else statement for ArgumentType.NameSpecial
            }

            TvmConfiguration configuration = configurator.Build();
            CheckConfigurationCorrectness(configuration);
            return configuration;
        }

        /// <summary>
        /// Checks the consistency and correctness of arguments
        /// </summary>
        /// <param name="configuration">Virtual machine configuration</param>
        private static void CheckConfigurationCorrectness(TvmConfiguration configuration)
        {
            StringBuilder errorMessage = new();

            if (configuration.SourceFilePaths.Length == 0) errorMessage.Append("Must be at least one source file path\n");
            if (configuration.Interpretation == configuration.Compilation) errorMessage.Append("Cannot set interpretation mode and compilation mode at the same time\n");

            string message = errorMessage.ToString();
            if (message != "") throw new InvalidCommandLineArgumentsException(message);
        }
    }
}
