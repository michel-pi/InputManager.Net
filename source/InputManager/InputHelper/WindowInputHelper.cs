using System;
using System.Windows.Forms;
using System.Windows.Input;

using InputManager.PInvoke;

namespace InputManager.InputHelper
{
	internal static class WindowInputHelper
	{
		private static WindowMessage GetWindowMessageForKey(Keys key, bool down)
		{
			switch (key)
			{
				case Keys.Modifiers:
				case Keys.ControlKey:
				case Keys.Menu:
				case Keys.LControlKey:
				case Keys.RControlKey:
				case Keys.LMenu:
				case Keys.RMenu:
				case Keys.Control:
				case Keys.Alt:
					return down ? WindowMessage.Syskeydown : WindowMessage.Syskeyup;

				default:
					return down ? WindowMessage.Keydown : WindowMessage.Keyup;
			}
		}

		private static IntPtr MakeLParam(int x, int y)
		{
			return (IntPtr)((y << 16) | (x & 0xFFFF));
		}

		public static bool PostMessage(IntPtr windowHandle, Keys key, KeyStates state)
		{
			var wparam = (IntPtr)key;

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				return User32.PostMessageW(windowHandle, GetWindowMessageForKey(key, true), wparam, IntPtr.Zero)
					&& User32.PostMessageW(windowHandle, GetWindowMessageForKey(key, false), wparam, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				return User32.PostMessageW(windowHandle, GetWindowMessageForKey(key, true), wparam, IntPtr.Zero);
			}
			else
			{
				return User32.PostMessageW(windowHandle, GetWindowMessageForKey(key, false), wparam, IntPtr.Zero);
			}
		}

		public static bool PostMouseMoveMessage(IntPtr hwnd, int x, int y)
		{
			return User32.PostMessageW(hwnd, WindowMessage.Mousemove, IntPtr.Zero, MakeLParam(x, y));
		}

		public static bool SendMessage(IntPtr hwnd, Keys key, KeyStates state)
		{
			var wparam = (IntPtr)key;

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				User32.SendMessageW(hwnd, GetWindowMessageForKey(key, true), wparam, IntPtr.Zero);
				User32.SendMessageW(hwnd, GetWindowMessageForKey(key, false), wparam, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				User32.SendMessageW(hwnd, GetWindowMessageForKey(key, true), wparam, IntPtr.Zero);
			}
			else
			{
				User32.SendMessageW(hwnd, GetWindowMessageForKey(key, false), wparam, IntPtr.Zero);
			}

			return true;
		}

		public static bool SendMouseMoveMessage(IntPtr hwnd, int x, int y)
		{
			User32.SendMessageW(hwnd, WindowMessage.Mousemove, IntPtr.Zero, MakeLParam(x, y));

			return true;
		}
	}
}
