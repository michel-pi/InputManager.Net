using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace AsyncKeyState.Converter
{
    public static class KeyCodeConverter
    {
        public static int ToInt(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Key>(nameof(key), key);

            return (int)key;
        }

        public static int ToInt(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            return (int)key;
        }

        public static string ToString(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Key>(nameof(key), key);

            return key.ToString();
        }

        public static string ToString(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            return key.ToString();
        }

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

        public static Key FormsKeyToWPFKey(Keys key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Keys>(nameof(key), key);

            return KeyInterop.KeyFromVirtualKey((int)key);
        }

        public static Keys WPFKeyToFormsKey(Key key)
        {
            if (ValidationHelper.IsKeyOutOfRange(key)) throw ThrowHelper.InvalidEnumArgumentException<Key>(nameof(key), key);

            return (Keys)KeyInterop.VirtualKeyFromKey(key);
        }
    }
}
