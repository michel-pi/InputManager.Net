using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace InputManager.PInvoke
{
	internal delegate short GetAsyncKeyStateDelegate(Keys key);

	[return: MarshalAs(UnmanagedType.Bool)]
	internal delegate bool GetKeyboardStateDelegate([MarshalAs(UnmanagedType.LPArray)] byte[] buffer);

	internal delegate short GetKeyStateDelegate(int virtualKeyCode);

	internal delegate uint SendInputDelegate(uint inputs, ref NativeInputEvent inputEvent, int size);

	internal delegate void KeybdEventDelegate(byte key, byte scancode, NativeKeyboardEventFlags flags, IntPtr extraInfo);

	internal delegate void MouseEventDelegate(NativeMouseEventFlags flags, uint x, uint y, uint data, IntPtr extraInfo);

	[return: MarshalAs(UnmanagedType.Bool)]
	internal delegate bool PostMessageDelegate(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

	internal delegate IntPtr SendMessageDelegate(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);
}
