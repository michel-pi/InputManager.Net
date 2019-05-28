using System;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState
{
    internal static class ValidationHelper
    {
        public static bool IsIndexOutOfRange(int index)
        {
            return index < User32.MinKeyCode || index > User32.MaxKeyCode;
        }

        public static bool IsKeyOutOfRange(Key key)
        {
            return (int)key < User32.MinKeyCode || (int)key > User32.MaxKeyCode;
        }

        public static bool IsKeyOutOfRange(Keys key)
        {
            return (int)key < User32.MinKeyCode || (int)key > User32.MaxKeyCode;
        }

        public static bool IsKeyStatesOutOfRange(KeyStates state)
        {
            return state < KeyStates.None || state > (KeyStates.Down | KeyStates.Toggled);
        }
    }
}
