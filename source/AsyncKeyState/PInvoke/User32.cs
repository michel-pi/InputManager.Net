using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AsyncKeyState.PInvoke
{
    internal delegate short GetAsyncKeyStateDelegate(Keys key);

    [return: MarshalAs(UnmanagedType.Bool)]
    internal delegate bool GetKeyboardStateDelegate([MarshalAs(UnmanagedType.LPArray)] byte[] buffer);

    internal delegate short GetKeyStateDelegate(int virtualKeyCode);

    internal static class User32
    {
        public const int MaxKeyCode = 0x100;
        public const int MinKeyCode = 0x000;

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
