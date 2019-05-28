using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;

namespace AsyncKeyState
{
    /// <summary>
    /// Provides methods to convert between strings and enums and aswell Windows Forms (Keys) and WPF (Key) enums.
    /// </summary>
    public static class KeyCodeConverter
    {
        private static readonly List<Keys> _keys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();

        /// <summary>
        /// Gets a readonly enumeration of all Keys.
        /// </summary>
        public static IEnumerable<Keys> Keys
        {
            get
            {
                foreach (var key in _keys) yield return key;
            }
        }

        /// <summary>
        /// Converts a WPF Key to a signed integer.
        /// </summary>
        /// <param name="key">A WPF Key.</param>
        /// <returns>The signed int value representing a WPF Key.</returns>
        public static int ToInt(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Key>(nameof(key), key);

            return (int)key;
        }

        /// <summary>
        /// Converts a Windows Forms Keys to a signed integer.
        /// </summary>
        /// <param name="key">A Windows Forms Keys.</param>
        /// <returns>The signed int value representing a WPF Keys.</returns>
        public static int ToInt(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            return (int)key;
        }

        /// <summary>
        /// Converts a WPF Key to a human readable string.
        /// </summary>
        /// <param name="key">A WPF Key.</param>
        /// <returns>The string this method generates.</returns>
        public static string ToString(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Key>(nameof(key), key);

            return key.ToString();
        }

        /// <summary>
        /// Converts a Windows Forms Keys to a human readable string.
        /// </summary>
        /// <param name="key">A Windows Forms Keys enum value.</param>
        /// <returns>The string this method generates.</returns>
        public static string ToString(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            return key.ToString();
        }

        /// <summary>
        /// Turns the string representation of a WPF Key to a WPF Key enum value.
        /// </summary>
        /// <param name="value">The string representation of a WPF Key enum value.</param>
        /// <returns>The WPF Key enum value.</returns>
        public static Key WPFKeyFromString(string value)
        {
            if (value == null) throw ThrowHelper.ArgumentNullException(nameof(value));
            if (value.Length == 0) throw ThrowHelper.ArgumentOutOfRangeException(nameof(value));

            if (Enum.TryParse<Key>(value, true, out var result))
            {
                return result;
            }
            else
            {
                throw ThrowHelper.EnumParseException<Key>(value);
            }
        }

        /// <summary>
        /// Turns the string representation of a Windows Forms Keys to a Windows Forms Keys enum value.
        /// </summary>
        /// <param name="value">The string representation of a Windows Forms Keys enum value.</param>
        /// <returns>The Windows Forms Keys enum value.</returns>
        public static Keys FormsKeyFromString(string value)
        {
            if (value == null) throw ThrowHelper.ArgumentNullException(nameof(value));
            if (value.Length == 0) throw ThrowHelper.ArgumentOutOfRangeException(nameof(value));

            if (Enum.TryParse<Keys>(value, true, out var result))
            {
                return result;
            }
            else
            {
                throw ThrowHelper.EnumParseException<Keys>(value);
            }
        }

        /// <summary>
        /// Converts a Windows Forms Keys enum value to a WPF Key enum value.
        /// </summary>
        /// <param name="key">A Windows Forms Keys enum value.</param>
        /// <returns>The WPF Key enum value.</returns>
        public static Key FormsKeyToWPFKey(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            return KeyInterop.KeyFromVirtualKey((int)key);
        }

        /// <summary>
        /// Converts a WPF Key enum value to a Windows Forms Keys enum value.
        /// </summary>
        /// <param name="key">A WPF Key enum value.</param>
        /// <returns>The Windows Forms Keys enum value.</returns>
        public static Keys WPFKeyToFormsKey(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Key>(nameof(key), key);

            return (Keys)KeyInterop.VirtualKeyFromKey(key);
        }
    }
}
