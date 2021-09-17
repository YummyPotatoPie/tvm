using System;

namespace tvmInterpreter
{
    public sealed class StackMemory
    {
        private int[] _stack;

        private int _position = 0;

        public StackMemory() => _stack = new int[1024];

        public StackMemory(int size)
        {
            if (size == 0) throw new Exception();
            _stack = new int[size];
        }

        private void StackSizeIncrease()
        {
            int[] temp = new int[_stack.Length * 2];
            for (int i = 0; i < _stack.Length; i++) temp[i] = _stack[i];
            _stack = temp;
        }

        public void Push(int value)
        {
            if (_position == _stack.Length) StackSizeIncrease();
            _stack[_position] = value;
            _position++;
        }

        public void Pop()
        {
            _stack[_position] = default;
            _position--;
        }

        public int Peek()
        {
            if (_position < 0) throw new Exception();
            return _stack[_position];
        }
    }
}
