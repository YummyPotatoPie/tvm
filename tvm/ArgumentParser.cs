using System;
using System.Collections.Generic;

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
    // [filename] -arg1 -arg2 -arg3 
    /// <summary>
    /// Command line arguments parser
    /// </summary>    
    public static class ArgumentParser<T> where T : ArgumentValue
    {
     
        /// <summary>
        /// Parse command line arguments
        /// </summary>
        /// <returns>Parsed arguments</returns>
        public static Dictionary<ArgumentType, ArgumentValue> ParseArgs(string args)
        {
            throw new NotImplementedException();
        }
        
    }
}
