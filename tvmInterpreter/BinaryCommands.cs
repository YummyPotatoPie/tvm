using System;

namespace tvmInterpreter
{
    public static class BinaryCommands
    {
        public static int Add(int a, int b) => a + b;

        public static int Sub(int a, int b) => a - b;

        public static int Mul(int a, int b) => a * b;

        public static int Div(int a, int b) => b == 0 ? throw new ArgumentException("Division by zero") : a / b;
    }
}
