using System;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace AsyncKeyState.PInvoke
{
    public delegate short GetAsyncKeyStateDelegate(Key key);

    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetKeyboardStateDelegate([MarshalAs(UnmanagedType.LPArray)] byte[] buffer);

    public delegate short GetKeyStateDelegate(int virtualKeyCode);

    internal static class User32
    {
        public const int MaxKeyCode = 0xFF;
        public const int MinKeyCode = 0x00;

        public static readonly GetAsyncKeyStateDelegate GetAsyncKeyState;
        public static readonly GetKeyboardStateDelegate GetKeyboardState;
        public static readonly GetKeyStateDelegate GetKeyState;

        static User32()
        {
            var library = DynamicImport.ImportLibrary(nameof(User32));

            GetAsyncKeyState = DynamicImport.Import<GetAsyncKeyStateDelegate>(library, nameof(GetAsyncKeyState));
            GetKeyboardState = DynamicImport.Import<GetKeyboardStateDelegate>(library, nameof(GetKeyboardState));
            GetKeyState = DynamicImport.Import<GetKeyStateDelegate>(library, nameof(GetKeyState));
        }
    }
}
