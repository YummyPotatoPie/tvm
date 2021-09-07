using System;
using System.Collections.Generic;
using Xunit;

using tvm.Exceptions;

namespace tvm.Tests
{
    public class ArgumentParserTests
    {
        [Fact]
        public void SimpleCommandLineArgsTest()
        {
            // Arrange
            string args = "Filename1.tbc -C -D -asdas -sample";

            // Act
            Dictionary<ArgumentType, ArgumentValue> parsedArgs = ArgumentParser<ArgumentValue>.ParseArgs(args);

            // Assert
            Assert.Equal(new Dictionary<ArgumentType, ArgumentValue>
            {
                { ArgumentType.Filename, new ArgumentValue("Filename1.tbc") },
                { ArgumentType.Special, new ArgumentValue("C") },
                { ArgumentType.Special, new ArgumentValue("D") },
                { ArgumentType.Special, new ArgumentValue("asdas") },
                { ArgumentType.Special, new ArgumentValue("sample") }
            }, 
            parsedArgs);
        }

        [Fact]
        public void NotSeparatedArgumentsTest()
        {
            // Arrange
            string args = "HelloWorld.tbc File2.tbc-C-G-OP1-OP3";

            // Act
            Dictionary<ArgumentType, ArgumentValue> parsedArgs = ArgumentParser<ArgumentValue>.ParseArgs(args);

            // Assert
            Assert.Equal(new Dictionary<ArgumentType, ArgumentValue>
            {
                { ArgumentType.Filename, new ArgumentValue("HelloWorld.tbc") },
                { ArgumentType.Filename, new ArgumentValue("File2.tbc") },
                { ArgumentType.Special, new ArgumentValue("C") },
                { ArgumentType.Special, new ArgumentValue("G") },
                { ArgumentType.Special, new ArgumentValue("OP1") },
                { ArgumentType.Special, new ArgumentValue("OP3") }
            },
            parsedArgs);
        }

        [Fact]
        public void InvalidExtentionOfFileTest() => Assert.Throws<CommandLineArgumentsParseException>(new Action(ParseInvalidArgs));

        private void ParseInvalidArgs() => ArgumentParser<ArgumentValue>.ParseArgs("Hello.wrong File.no");

    }
}
