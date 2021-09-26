using System;

namespace tvmInterpreter
{
    /// <summary>
    /// Stack memory of TVM
    /// </summary>
    public sealed class StackMemory
    {
        private int[] _stack;

        private int _position = 0;

        private int _stackTopPosition = 0;

        /// <summary>
        /// Create instance of stack memory with default size 1024
        /// </summary>
        public StackMemory() => _stack = new int[1024];

        /// <summary>
        /// Create instance of stack memory with the specified size
        /// </summary>
        /// <param name="size">Size of stack</param>
        public StackMemory(int size)
        {
            if (size == 0) throw new ArgumentException("Stack size cannot be equal zero");
            _stack = new int[size];
        }

        /// <summary>
        /// Increase stack size
        /// </summary>
        private void StackSizeIncrease()
        {
            int[] temp = new int[_stack.Length * 2];
            for (int i = 0; i < _stack.Length; i++) temp[i] = _stack[i];
            _stack = temp;
        }

        /// <summary>
        /// Push value to the top of the stack
        /// </summary>
        /// <param name="value">Number (another lol)</param>
        public void Push(int value)
        {
            if (_position == _stack.Length) StackSizeIncrease();
            _stack[_position] = value;
            _position++;
            _stackTopPosition++;
        }

        /// <summary>
        /// Delete value from top of the stack
        /// </summary>
        public void Pop()
        {
            if (_position == 0) throw new InvalidOperationException("Segmentation fault: attempt to pop value from empty stack");
            _stack[_position - 1] = default;
            _position--;
            _stackTopPosition--;
        }

        /// <summary>
        /// Peeks value from top of the stack
        /// </summary>
        /// <returns>Value from top of the stack</returns>
        public int Peek()
        {
            if (_position == 0) throw new InvalidOperationException("Segmentation fault: attempt to peek value, but stack is empty");
            return _stack[_position - 1];
        }

        public int Peek(int offset)
        {
            CheckOffset(offset); 
            return _stack[_position + offset];
        }

        /// <summary>
        /// Checks offset correctness
        /// </summary>
        /// <param name="offset">Offset value</param>
        /// <returns>True if offset correct else false</returns>
        private bool CheckOffset(int offset) => 
            _position + offset >= 0 && _position + offset < _stackTopPosition && _position != 0 
            ? true : throw new InvalidOperationException("Segmentation fault: attempt to use value from invalid address");
    }
}
