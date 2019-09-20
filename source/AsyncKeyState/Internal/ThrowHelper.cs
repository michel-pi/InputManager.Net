using System;
using System.ComponentModel;

namespace AsyncKeyState.Internal
{
    internal static class ThrowHelper
    {
        public static InvalidEnumArgumentException InvalidEnumArgumentException<T>(string paramName, T value)
        {
            return new InvalidEnumArgumentException(paramName, (int)(object)value, typeof(T));
        }

        public static ArgumentNullException ArgumentNullException(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        public static ArgumentOutOfRangeException ArgumentOutOfRangeException(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }

        public static ArgumentException EnumParseException<T>(string value)
        {
            return new ArgumentException($"Requested value '{ value }' in '{ typeof(T).Name }' was not found.");
        }
    }
}
