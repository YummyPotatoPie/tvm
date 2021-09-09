using System;
using System.Collections.Generic;
using Xunit;

using tvm.Exceptions;

namespace tvm.Tests
{
    public class ArgumentParserTests
    {
        private static bool ParsedArgsEqual(Dictionary<ArgumentValue, ArgumentType> args1, Dictionary<ArgumentValue, ArgumentType> args2)
        {
            if (args1.Count != args2.Count) return false;
            else
            {
                foreach (KeyValuePair<ArgumentValue, ArgumentType> arg in args1)
                {
                    try
                    {
                        ArgumentType argType = args2[arg.Key];
                        if (argType != arg.Value) return false;
                    }
                    catch (KeyNotFoundException)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [Fact]
        public void SimpleCommandLineArgsTest()
        {
            // Arrange
            string args = "Filename1.tbc -C -D -asdas -sample";

            // Act
            Dictionary<ArgumentValue, ArgumentType> parsedArgs = new ArgumentParser<ArgumentValue>(args).ParseArgs();

            // Assert
            Assert.True(ParsedArgsEqual(new Dictionary<ArgumentValue, ArgumentType>
            {
                { new ArgumentValue("Filename1.tbc"), ArgumentType.Filename },
                { new ArgumentValue("C"), ArgumentType.Special },
                { new ArgumentValue("D"), ArgumentType.Special },
                { new ArgumentValue("asdas"), ArgumentType.Special },
                { new ArgumentValue("sample"), ArgumentType.Special }
            },
            parsedArgs));
        }

        [Fact]
        public void NotSeparatedArgumentsTest()
        {
            // Arrange
            string args = "HelloWorld.tbc File2.tbc-C-G-OP1-OP3--compile=true";

            // Act
            Dictionary<ArgumentValue, ArgumentType> parsedArgs = new ArgumentParser<ArgumentValue>(args).ParseArgs();

            // Assert
            Assert.True(ParsedArgsEqual(new Dictionary<ArgumentValue, ArgumentType>
            {
                { new ArgumentValue("HelloWorld.tbc"), ArgumentType.Filename },
                { new ArgumentValue("File2.tbc"), ArgumentType.Filename },
                { new ArgumentValue("C"), ArgumentType.Special },
                { new ArgumentValue("G"), ArgumentType.Special },
                { new ArgumentValue("OP1"), ArgumentType.Special },
                { new ArgumentValue("OP3"), ArgumentType.Special },
                { new NameArgumentValue("compile", "true"), ArgumentType.NameSpecial }
            },
            parsedArgs));
        }

        [Fact]
        public void InvalidExtentionOfFileTest() => Assert.Throws<CommandLineArgumentsParseException>(new Action(ParseInvalidArgs));

        private void ParseInvalidArgs() => new ArgumentParser<ArgumentValue>("Hello.wrong Bruh.no").ParseArgs();

    }
}
