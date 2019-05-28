using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState
{
    public class KeyboardState
    {
        public const int MaxKeyValue = User32.MaxKeyCode;
        public const int MinKeyValue = User32.MinKeyCode;

        [ThreadStatic] private static KeyboardState _threadKeyboardState;

        private readonly int _managedThreadId;

        private readonly byte[] _buffer;

        public byte this[int index]
        {
            get
            {
                if (ValidationHelper.IsIndexOutOfRange(index)) throw ThrowHelper.ArgumentOutOfRangeException(nameof(index));

                return _buffer[index];
            }
        }

        public KeyStates this[Keys key]
        {
            get
            {
                return GetKeyState(key);
            }
        }

        public KeyboardState()
        {
            _managedThreadId = Thread.CurrentThread.ManagedThreadId;

            _buffer = new byte[MaxKeyValue];

            Update();
        }

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

        public bool IsPressed(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Down) == KeyStates.Down;
        }

        public bool WasPressed(Keys key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) == KeyStates.Toggled
                && (state & KeyStates.Down) != KeyStates.Down;
        }

        public bool IsFirstTimePressed(Keys key)
        {
            var state = GetKeyState(key);

            return (state & KeyStates.Toggled) == KeyStates.Toggled
                && (state & KeyStates.Down) == KeyStates.Down;
        }

        public bool IsToggled(Keys key)
        {
            return (GetKeyState(key) & KeyStates.Toggled) == KeyStates.Toggled;
        }

        public bool IsUp(Keys key)
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

        public override int GetHashCode()
        {
            return _managedThreadId.GetHashCode();
        }

        public override string ToString()
        {
            return $"{ nameof(KeyboardState) } of thread #{ _managedThreadId.ToString() }";
        }

        public static KeyboardState Create()
        {
            return new KeyboardState();
        }

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
