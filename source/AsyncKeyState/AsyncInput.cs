using System;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState
{
    public static class AsyncInput
    {
        public const int MaxKeyValue = User32.MaxKeyCode;
        public const int MinKeyValue = User32.MinKeyCode;

        public static KeyStates GetKeyState(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            var result = User32.GetAsyncKeyState(key);

            bool isToggled = (result & 0x0001) != 0;
            bool isPressed = (result & 0x8000) != 0;

            var state = KeyStates.None;

            if (isToggled)
            {
                state |= KeyStates.Toggled;
            }

            if (isPressed)
            {
                state |= KeyStates.Down;
            }

            return state;
        }

        public static bool IsPressed(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Down) == KeyStates.Down;
        }

        public static bool WasPressed(Keys key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) == KeyStates.Toggled
                && (state & KeyStates.Down) != KeyStates.Down;
        }

        public static bool IsFirstTimePressed(Keys key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) == KeyStates.Toggled
                && (state & KeyStates.Down) == KeyStates.Down;
        }

        public static bool IsToggled(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Toggled) == KeyStates.Toggled;
        }

        public static bool IsUp(Keys key)
        {
            return GetKeyState(key) == KeyStates.None;
        }
    }
}
