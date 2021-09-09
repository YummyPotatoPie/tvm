using System;
using System.Text;
using System.Collections.Generic;

using tvm.Exceptions;

namespace tvm
{
    // Test 
    /// <summary>
    /// Enum represents command line arguments type
    /// </summary>
    public enum ArgumentType
    {
        Filename,
        Special,
        NameSpecial
    }

    // Parsing arguments like:
    // [filename] -arg1 -arg2 -arg3 --compile=true
    /// <summary>
    /// Command line arguments parser
    /// </summary>    
    public class ArgumentParser<T> where T : ArgumentValue
    {

        private readonly string argumentsString;

        private int position = 0;

        public ArgumentParser(string args) => argumentsString = args;

        /// <summary>
        /// Parse command line arguments
        /// </summary>
        /// <exception cref="CommandLineArgumentsParseException"></exception>
        /// <returns>Parsed arguments</returns>
        public Dictionary<ArgumentValue, ArgumentType> ParseArgs()
        {
            Dictionary<ArgumentValue, ArgumentType> parsedArgs = new();
            KeyValuePair<ArgumentValue, ArgumentType> currentArg;

            while (!Eof())
            {
                SkipWhiteSpaces();
                if (char.IsLetterOrDigit(GetChar())) currentArg = ReadFilePath();
                else if (GetChar() == '-') currentArg = ReadArgument();
                else throw new CommandLineArgumentsParseException("Unexpected symbol: " + GetChar());

                try
                {
                    parsedArgs.Add(currentArg.Key, currentArg.Value);
                }
                catch (ArgumentException)
                {
                    throw new CommandLineArgumentsParseException("The argument has already been entered: " + currentArg.Key.Argument);
                }
            }

            return parsedArgs;

        }

        /// <summary>
        /// Parse input file path
        /// </summary>
        /// <exception cref="CommandLineArgumentsParseException"></exception>
        /// <returns></returns>
        private KeyValuePair<ArgumentValue, ArgumentType> ReadFilePath()
        {
            StringBuilder argument = new();

            while (!Eof() && char.IsLetterOrDigit(GetChar()))
            {
                argument.Append(GetChar());
                position++;
            }

            if (Eof() || GetChar() != '.') throw new CommandLineArgumentsParseException("Unexpected end of file path argument");
            else
            {
                StringBuilder extention = new();

                argument.Append(GetChar());
                position++;
                while (!Eof() && char.IsLetterOrDigit(GetChar()))
                {
                    extention.Append(GetChar());
                    position++;
                }

                if (extention.ToString() != "tbc") throw new CommandLineArgumentsParseException("Invalid extention of input file path: " + extention.ToString());
                else argument.Append(extention);
            }

            return new KeyValuePair<ArgumentValue, ArgumentType>(new ArgumentValue(argument.ToString()), ArgumentType.Filename);
        }

        /// <summary>
        /// Parse argument 
        /// </summary>
        /// <exception cref="CommandLineArgumentsParseException"></exception>
        /// <returns></returns>
        private KeyValuePair<ArgumentValue, ArgumentType> ReadArgument()
        {
            position++;
            if (Eof() || char.IsWhiteSpace(GetChar())) throw new CommandLineArgumentsParseException("Unexpected end of argument");
            else if (GetChar() == '-') return ReadNameArgument();
            else
            {
                StringBuilder argument = new();

                while (!Eof() && char.IsLetterOrDigit(GetChar()))
                {
                    argument.Append(GetChar());
                    position++;
                }

                return new KeyValuePair<ArgumentValue, ArgumentType>(new ArgumentValue(argument.ToString()), ArgumentType.Special);
            }
        }

        /// <summary>
        /// Parse named argument
        /// </summary>
        /// <exception cref="CommandLineArgumentsParseException"></exception>
        /// <returns></returns>
        private KeyValuePair<ArgumentValue, ArgumentType> ReadNameArgument()
        {
            position++;
            if (Eof() || char.IsWhiteSpace(GetChar())) throw new CommandLineArgumentsParseException("Unexpected end of argument");
            else if (!char.IsLetterOrDigit(GetChar())) throw new CommandLineArgumentsParseException("Unexcepted symbol:" + GetChar());
            else
            {
                StringBuilder nameArgument = new();

                while (!Eof() && char.IsLetterOrDigit(GetChar()))
                {
                    nameArgument.Append(GetChar());
                    position++;
                }

                if (Eof() || char.IsWhiteSpace(GetChar()) || GetChar() != '=') throw new CommandLineArgumentsParseException("Unexpected end of argument");
                else
                {
                    position++;
                    if (Eof() || char.IsWhiteSpace(GetChar())) throw new CommandLineArgumentsParseException("Unexpected end of argument");
                    else
                    {
                        StringBuilder nameArgumentValue = new();

                        while (!Eof() && char.IsLetterOrDigit(GetChar()))
                        {
                            nameArgumentValue.Append(GetChar());
                            position++;
                        }

                        return new KeyValuePair<ArgumentValue, ArgumentType>(new NameArgumentValue(nameArgument.ToString(), nameArgumentValue.ToString()), ArgumentType.NameSpecial);
                    }
                }
                
            }
        }

        /// <summary>
        /// Skips whitespaces
        /// </summary>
        private void SkipWhiteSpaces()
        {
            while (!Eof() && char.IsWhiteSpace(GetChar())) position++;
        }

        /// <summary>
        /// Reads current char at commant line arguments
        /// </summary>
        /// <returns>Current char</returns>
        private char GetChar() => argumentsString[position];

        /// <summary>
        /// Checks if stream reached end of arguments line
        /// </summary>
        /// <returns>True if reached end of argument line else false</returns>
        private bool Eof() => position == argumentsString.Length;
    }
}
