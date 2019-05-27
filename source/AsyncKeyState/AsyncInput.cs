using System;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState
{
    public static class AsyncInput
    {
        public const int MaxKeyValue = User32.MaxKeyCode;
        public const int MinKeyValue = User32.MinKeyCode;

        public static KeyStates GetKeyState(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(key));

            var result = User32.GetAsyncKeyState(key);

            bool isToggled = (result & 0x0001) != 0;
            bool isPressed = (result & 0x8000) != 0;

            return isToggled
                ? isPressed ? KeyStates.Down | KeyStates.Toggled : KeyStates.Toggled
                : isPressed ? KeyStates.Down : KeyStates.None;
        }

        public static bool IsPressed(Key key)
        {
            return (GetKeyState(key) & KeyStates.Down) != 0;
        }

        public static bool WasPressed(Key key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) != 0
                && (state & KeyStates.Down) == 0;
        }

        public static bool IsPressedFirstTime(Key key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) != 0
                && (state & KeyStates.Down) != 0;
        }

        public static bool IsToggled(Key key)
        {
            return (GetKeyState(key) & KeyStates.Toggled) != 0;
        }

        public static bool IsUp(Key key)
        {
            return GetKeyState(key) == KeyStates.None;
        }

        internal static class ValidationHelper
        {
            public static bool IsKeyOutOfRange(Key key)
            {
                return (int)key < MinKeyValue || (int)key > MaxKeyValue;
            }
        }
    }
}
