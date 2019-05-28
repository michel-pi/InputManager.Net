using System;
using System.Windows.Input;

namespace AsyncKeyState.Converter
{
    /// <summary>
    /// Provides methods for converting KeyStates to strings and vice versa.
    /// </summary>
    public static class KeyStatesConverter
    {
        /// <summary>
        /// Converts a KeyStates enum value to a signed int.
        /// </summary>
        /// <param name="state">A KeyStates enum value.</param>
        /// <returns>The signed int this method returns.</returns>
        public static int ToInt(KeyStates state)
        {
            if (ValidationHelper.IsKeyStatesOutOfRange(state)) throw ThrowHelper.InvalidEnumArgumentException<KeyStates>(nameof(state), state);

            return (int)state;
        }

        /// <summary>
        /// Converts a KeyStates enum value to a human readable string.
        /// </summary>
        /// <param name="state">A KeyStates enum value.</param>
        /// <returns>The string this method generates.</returns>
        public static string ToString(KeyStates state)
        {
            if (ValidationHelper.IsKeyStatesOutOfRange(state)) throw ThrowHelper.InvalidEnumArgumentException<KeyStates>(nameof(state), state);

            return state.ToString();
        }

        /// <summary>
        /// Converts the string representation of a KeyStates enum value to a KeyStates enum value.
        /// </summary>
        /// <param name="value">The string representation of a KeyStates enum value.</param>
        /// <returns>A KeyStates enum value.</returns>
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
