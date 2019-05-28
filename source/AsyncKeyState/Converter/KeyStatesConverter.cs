using System;
using System.Windows.Input;

namespace AsyncKeyState.Converter
{
    public static class KeyStatesConverter
    {
        public static int ToInt(KeyStates state)
        {
            if (ValidationHelper.IsKeyStatesOutOfRange(state)) throw ThrowHelper.InvalidEnumArgumentException<KeyStates>(nameof(state), state);

            return (int)state;
        }

        public static string ToString(KeyStates state)
        {
            if (ValidationHelper.IsKeyStatesOutOfRange(state)) throw ThrowHelper.InvalidEnumArgumentException<KeyStates>(nameof(state), state);

            return state.ToString();
        }

        public static KeyStates FromString(string value)
        {
            if (value == null) throw ThrowHelper.ArgumentNullException(nameof(value));
            if (value.Length == 0) throw ThrowHelper.ArgumentOutOfRangeException(nameof(value));

            if (Enum.TryParse<KeyStates>(value, true, out var result))
            {
                return result;
            }
            else
            {
                throw ThrowHelper.EnumParseException<KeyStates>(value);
            }
        }
    }
}
