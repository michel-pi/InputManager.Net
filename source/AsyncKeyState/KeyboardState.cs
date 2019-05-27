using System;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState
{
    public class KeyboardState
    {
        public const int MaxKeyValue = User32.MaxKeyCode;
        public const int MinKeyValue = User32.MinKeyCode;

        private readonly byte[] _buffer;

        public byte this[int index]
        {
            get
            {
                if (ValidationHelper.IsIndexOutOfRange(index)) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));

                return _buffer[index];
            }
        }

        public KeyStates this[Key key]
        {
            get
            {
                return GetKeyState(key);
            }
        }

        public KeyboardState()
        {
            _buffer = new byte[MaxKeyValue];

            Update();
        }

        public KeyStates GetKeyState(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));

            var result = _buffer[(int)key];

            // a key is toggled when it's most significant bit is set
            bool isToggled = (result & (1 << 0)) != 0;
            // a key is pressed when it's least significant bit is set
            bool isPressed = (result & (1 << 7)) != 0;

            return isToggled
                ? isPressed ? KeyStates.Down | KeyStates.Toggled : KeyStates.Toggled
                : isPressed ? KeyStates.Down : KeyStates.None;
        }

        public bool IsPressed(Key key)
        {
            return (GetKeyState(key) & KeyStates.Down) != 0;
        }

        public bool WasPressed(Key key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) != 0
                && (state & KeyStates.Down) == 0;
        }

        public bool IsPressedFirstTime(Key key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) != 0
                && (state & KeyStates.Down) != 0;
        }

        public bool IsToggled(Key key)
        {
            return (GetKeyState(key) & KeyStates.Toggled) != 0;
        }

        public bool IsUp(Key key)
        {
            return GetKeyState(key) == KeyStates.None;
        }

        public void Update()
        {
            // invalidates the keyboard state for the current thread
            // removes the need for a message loop
            User32.GetKeyState(0);
            User32.GetKeyboardState(_buffer);
        }

        public static KeyboardState Create()
        {
            return new KeyboardState();
        }

        internal static class ValidationHelper
        {
            public static bool IsIndexOutOfRange(int index)
            {
                return index < MinKeyValue || index > MaxKeyValue;
            }

            public static bool IsKeyOutOfRange(Key key)
            {
                return (int)key < MinKeyValue || (int)key > MaxKeyValue;
            }
        }
    }
}
