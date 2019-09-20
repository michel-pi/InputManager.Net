using System;
using System.Windows.Forms;
using System.Windows.Input;

using InputManager.PInvoke;

namespace InputManager.Internal
{
    internal static class ValidationHelper
    {
        public static bool IsIndexOutOfRange(int index)
        {
            return index < User32.MinKeyCode || index > User32.MaxKeyCode;
        }

        public static bool IsKeyOutOfRange(Key key)
        {
            return key < User32.MinKeyCode || (int)key > User32.MaxKeyCode;
        }

        public static bool IsKeyOutOfRange(Keys key)
        {
            return key < User32.MinKeyCode || (int)key > User32.MaxKeyCode;
        }

        public static bool IsKeyStatesOutOfRange(KeyStates state)
        {
            return state < KeyStates.None || state > (KeyStates.Down | KeyStates.Toggled);
        }
    }
}
