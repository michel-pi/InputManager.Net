using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState
{
    /// <summary>
    /// Provides access to the asynchronous status of all virtual keys.
    /// </summary>
    public class KeyboardState
    {
        /// <summary>
        /// The upper bound of a virtual key.
        /// </summary>
        public const int MaxKeyValue = User32.MaxKeyCode;

        /// <summary>
        /// The lower bound of a virtual key.
        /// </summary>
        public const int MinKeyValue = User32.MinKeyCode;

        [ThreadStatic] private static KeyboardState _threadKeyboardState;

        private readonly int _managedThreadId;

        private readonly byte[] _buffer;

        /// <summary>
        /// Gets the bit field representing the status of a key.
        /// </summary>
        /// <param name="index">A virtual key.</param>
        /// <returns>A 1 byte large bit field holding the status of a virtual key.</returns>
        public byte this[int index]
        {
            get
            {
                if (ValidationHelper.IsIndexOutOfRange(index)) throw ThrowHelper.ArgumentOutOfRangeException(nameof(index));

                return _buffer[index];
            }
        }

        /// <summary>
        /// Gets the key state of a specified key.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns>The KeyStates of the specified key.</returns>
        public KeyStates this[Keys key]
        {
            get
            {
                return GetKeyState(key);
            }
        }

        /// <summary>
        /// Initializes a new KeyboardState object and updates the underlying buffer.
        /// </summary>
        public KeyboardState()
        {
            _managedThreadId = Thread.CurrentThread.ManagedThreadId;

            _buffer = new byte[MaxKeyValue];

            Update();
        }

        /// <summary>
        /// Returns the asynchronous state of a virtual key.
        /// </summary>
        /// <param name="key">A virtual key code.</param>
        /// <returns>The state of a virtual key code.</returns>
        public KeyStates GetKeyState(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            var bitField = _buffer[(int)key];

            // a key is toggled when it's most significant bit is set
            bool isToggled = (bitField & (1 << 0)) != 0;
            // a key is pressed when it's least significant bit is set
            bool isPressed = (bitField & (1 << 7)) != 0;

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
        public bool IsPressed(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// Determines whether a key was pressed.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key was pressed; otherwise, <see langword="false"/>.</returns>
        public bool WasPressed(Keys key)
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
        public bool IsFirstTimePressed(Keys key)
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
        public bool IsToggled(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Toggled) == KeyStates.Toggled;
        }

        /// <summary>
        /// Determines whether a key is not pressed or toggled.
        /// </summary>
        /// <param name="key">A virtual key.</param>
        /// <returns><see langword="true"/> if the virtual key is not pressed or toggled; otherwise, <see langword="false"/>.</returns>
        public bool IsUp(Keys key)
        {
            return GetKeyState(key) == KeyStates.None;
        }

        /// <summary>
        /// Updates the buffer holding the status of all virtual keys.
        /// </summary>
        public void Update()
        {
            // invalidates the keyboard state for the current thread
            // removes the need for a message loop
            User32.GetKeyState(0);
            User32.GetKeyboardState(_buffer);
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="T:System.Object" /> represent the same type and were created on the same thread.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true" /> if <paramref name="obj" /> is a KeyboardState and was created on the same thread; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (obj is KeyboardState keyboard)
            {
                return keyboard._managedThreadId == _managedThreadId;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a value indicating whether two specified instances of KeyboardState were created on the same thread.
        /// </summary>
        /// <param name="value">An object to compare to this instance.</param>
        /// <returns><see langword="true" /> if <paramref name="value" /> was created on the same thread; otherwise, <see langword="false" />.</returns>
        public bool Equals(KeyboardState value)
        {
            return value != null
                && value._managedThreadId == _managedThreadId;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _managedThreadId.GetHashCode();
        }

        /// <summary>
        /// Converts this KeyboardState structure to a human-readable string.
        /// </summary>
        /// <returns>A string representation of this KeyboardState.</returns>
        public override string ToString()
        {
            return $"{ nameof(KeyboardState) } of thread #{ _managedThreadId.ToString() }";
        }

        /// <summary>
        /// Instantiates a new KeyboardState object.
        /// </summary>
        /// <returns>A KeyboardState object.</returns>
        public static KeyboardState Create()
        {
            return new KeyboardState();
        }

        /// <summary>
        /// Gets a cached and thread-static KeyboardState object which is unique for the calling thread and updates it.
        /// </summary>
        /// <returns>A KeyboardState object.</returns>
        public static KeyboardState GetThreadStatic()
        {
            if (_threadKeyboardState == null)
            {
                _threadKeyboardState = new KeyboardState();

                return _threadKeyboardState;
            }
            else
            {
                _threadKeyboardState.Update();

                return _threadKeyboardState;
            }
        }

        /// <summary>
        /// Gets a cached and thread-static KeyboardState object which is unique for the calling thread.
        /// </summary>
        /// <param name="update">Indicates whether the thread-static KeyboardState object should be updated before returning it.</param>
        /// <returns>A KeyboardState object.</returns>
        public static KeyboardState GetThreadStatic(bool update)
        {
            if (update || _threadKeyboardState == null)
            {
                return GetThreadStatic();
            }
            else
            {
                return _threadKeyboardState;
            }
        }
    }
}
