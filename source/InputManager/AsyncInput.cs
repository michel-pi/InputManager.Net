using System;
using System.Windows.Forms;
using System.Windows.Input;

using InputManager.Internal;
using InputManager.PInvoke;

namespace InputManager
{
    /// <summary>
    /// Provides methods to get the asynchronous state of a virtual key.
    /// </summary>
    public static class AsyncInput
    {
        /// <summary>
        /// The upper bound of a virtual key.
        /// </summary>
        public const int MaxKeyValue = User32.MaxKeyCode;

        /// <summary>
        /// The lower bound of a virtual key.
        /// </summary>
        public const int MinKeyValue = User32.MinKeyCode;

        /// <summary>
        /// Returns the asynchronous state of a virtual key.
        /// </summary>
        /// <param name="key">A virtual key code.</param>
        /// <returns>The state of a virtual key code.</returns>
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

        /// <summary>
        /// Determines whether a key is pressed.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key is pressed; otherwise, <see langword="false"/>.</returns>
        public static bool IsPressed(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// Determines whether a key was pressed.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key was pressed; otherwise, <see langword="false"/>.</returns>
        public static bool WasPressed(Keys key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) == KeyStates.Toggled
                && (state & KeyStates.Down) != KeyStates.Down;
        }

        /// <summary>
        /// Determines whether a key is pressed for the first time and not held down.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key is pressed for the first time and not held down; otherwise, <see langword="false"/>.</returns>
        public static bool IsFirstTimePressed(Keys key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) == KeyStates.Toggled
                && (state & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// Determines whether a key is toggled.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key is toggled; otherwise, <see langword="false"/>.</returns>
        public static bool IsToggled(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Toggled) == KeyStates.Toggled;
        }

        /// <summary>
        /// Determines whether a key is not pressed or toggled.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key is not pressed or toggled; otherwise, <see langword="false"/>.</returns>
        public static bool IsUp(Keys key)
        {
            return GetKeyState(key) == KeyStates.None;
        }
    }
}
