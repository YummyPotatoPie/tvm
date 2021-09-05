using System;
using System.Collections.Generic;
using Xunit;

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
            Dictionary<ArgumentType, string> parsedArgs = ArgumentParser.ParseArgs(args);

            // Assert
            Assert.Equal(new Dictionary<ArgumentType, string>
            {
                { ArgumentType.Filename, "Filename1.tbc" },
                { ArgumentType.Special, "C" },
                { ArgumentType.Special, "D" },
                { ArgumentType.Special, "asdas" },
                { ArgumentType.Special, "sample" }
            }, 
            parsedArgs);
        }

        [Fact]
        public void NotSeparatedArgumentsTest()
        {
            // Arrange
            string args = "HelloWorld.tbc File2.tbc-C-G-OP1-OP3";

            // Act
            Dictionary<ArgumentType, string> parsedArgs = ArgumentParser.ParseArgs(args);

            // Assert
            Assert.Equal(new Dictionary<ArgumentType, string>
            {
                { ArgumentType.Filename, "HelloWorld.tbc" },
                { ArgumentType.Filename, "File2.tbc" },
                { ArgumentType.Special, "C" },
                { ArgumentType.Special, "G" },
                { ArgumentType.Special, "OP1" },
                { ArgumentType.Special, "OP3" }
            },
            parsedArgs);
        }

        [Fact]
        public void InvalidExtentionOfFileTest() => Assert.Throws(typeof (CommandLineArgumentsParseException), new Action(ParseInvalidArgs));

        private void ParseInvalidArgs() => ArgumentParser.ParseArgs("Hello.wrong File.no");

    }
}
