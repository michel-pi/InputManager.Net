using System;
using System.Security;

namespace AsyncKeyState.PInvoke
{
	[SuppressUnmanagedCodeSecurity]
    internal static class User32
    {
        public const int MaxKeyCode = 0x100;
        public const int MinKeyCode = 0x000;

        public static readonly GetAsyncKeyStateDelegate GetAsyncKeyState;
        public static readonly GetKeyboardStateDelegate GetKeyboardState;
        public static readonly GetKeyStateDelegate GetKeyState;

		public static readonly SendInputDelegate SendInput;
		public static readonly KeybdEventDelegate KeybdEvent;
		public static readonly MouseEventDelegate MouseEvent;
		public static readonly PostMessageDelegate PostMessageW;
		public static readonly SendMessageDelegate SendMessage;

		static User32()
        {
            var library = DynamicImport.ImportLibrary(nameof(User32));

            GetAsyncKeyState = DynamicImport.Import<GetAsyncKeyStateDelegate>(library, nameof(GetAsyncKeyState));
            GetKeyboardState = DynamicImport.Import<GetKeyboardStateDelegate>(library, nameof(GetKeyboardState));
            GetKeyState = DynamicImport.Import<GetKeyStateDelegate>(library, nameof(GetKeyState));
			SendInput = DynamicImport.Import<SendInputDelegate>(library, nameof(SendInput));
			KeybdEvent = DynamicImport.Import<KeybdEventDelegate>(library, "keybd_event");
			MouseEvent = DynamicImport.Import<MouseEventDelegate>(library, "mouse_event");
			PostMessageW = DynamicImport.Import<PostMessageDelegate>(library, nameof(PostMessageW));
			SendMessage = DynamicImport.Import<SendMessageDelegate>(library, nameof(SendMessage));
		}
    }
}
