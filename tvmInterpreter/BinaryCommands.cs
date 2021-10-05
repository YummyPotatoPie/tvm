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

        /// <summary>
        /// Right binary shift command
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Right binary shift result</returns>
        public static int ShiftRight(int a, int b) => a >> b;

        /// <summary>
        /// Left binary shift command
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Left binary shift result</returns>
        public static int ShiftLeft(int a, int b) => a << b;

        /// <summary>
        /// Binary xor operation
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Binary xor result</returns>
        public static int Xor(int a, int b) => a ^ b;

        /// <summary>
        /// Binary or operation
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Binary or result</returns>
        public static int Or(int a, int b) => a | b;

        /// <summary>
        /// Binary and operation
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Binary and result</returns>
        public static int And(int a, int b) => a & b;


        /// <summary>
        /// Calculate remainder
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>Remainder</returns>
        public static int Mod(int a, int b) => a % b;
    }
}
