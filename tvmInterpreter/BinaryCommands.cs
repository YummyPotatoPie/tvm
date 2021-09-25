using System;

namespace tvmInterpreter
{
    /// <summary>
    /// Class contains handlers for binary Tiny Byte Code commands
    /// </summary>
    public static class BinaryCommands
    {
        /// <summary>
        /// Add values
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns></returns>
        public static int Add(int a, int b) => a + b;

        /// <summary>
        /// Substract values
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns></returns>
        public static int Sub(int a, int b) => a - b;

        /// <summary>
        /// Multiply values
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Multiplication result</returns>
        public static int Mul(int a, int b) => a * b;

        /// <summary>
        /// Divide values
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Division result</returns>
        public static int Div(int a, int b) => b == 0 ? throw new ArgumentException("Division by zero") : a / b;
    }
}
