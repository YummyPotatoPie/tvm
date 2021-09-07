using System;
using System.Collections.Generic;

namespace tvm
{
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
    public static class ArgumentParser
    {
     
        /// <summary>
        /// Parse command line arguments
        /// </summary>
        /// <returns>Parsed arguments</returns>
        public static Dictionary<ArgumentType, string> ParseArgs(string args)
        {
            throw new NotImplementedException();
        }
        
    }
}
