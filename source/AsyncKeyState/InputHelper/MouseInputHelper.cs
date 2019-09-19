using System;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState.InputHelper
{
	internal static class MouseInputHelper
	{
		private static readonly int InputEventSize = NativeMouseInput.Size + 4;

		private static NativeMouseEventFlags GetMouseEventFlags(Keys key, bool down)
		{
			switch (key)
			{
				case Keys.LButton:
					return down ? NativeMouseEventFlags.LeftDown : NativeMouseEventFlags.LeftUp;
				case Keys.RButton:
					return down ? NativeMouseEventFlags.RightDown : NativeMouseEventFlags.RightUp;
				case Keys.MButton:
					return down ? NativeMouseEventFlags.MiddleDown : NativeMouseEventFlags.MiddleUp;
				default:
					throw new ArgumentOutOfRangeException(nameof(key), "Only Keys.LButton, Keys.RButton and Keys.MButton are supported by this method.");
			}
		}

		public static bool SendInput(Keys key)
			=> SendInput(key, KeyStates.Toggled);

		public static bool SendInput(Keys key, KeyStates state)
		{
			var inputEvent = new NativeInputEvent()
			{
				Type = NativeInputType.Mouse
			};

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, true);

				if (User32.SendInput(1u, ref inputEvent, InputEventSize) == 0u)
					return false;

				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, false);

				return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, true);

				return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
			}
			else
			{
				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, false);

				return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
			}
		}

		public static bool SendInputMove(int x, int y, bool relative)
		{
			var inputEvent = new NativeInputEvent()
			{
				Type = NativeInputType.Mouse
			};

			inputEvent.MouseInput.Flags = relative
				? NativeMouseEventFlags.Move
				: NativeMouseEventFlags.Absolute | NativeMouseEventFlags.Move;

			inputEvent.MouseInput.X = x;
			inputEvent.MouseInput.Y = y;

			return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
		}

		public static void SendMouseEvent(Keys key, KeyStates state)
		{
			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				User32.MouseEvent(GetMouseEventFlags(key, true), 0u, 0u, 0u, IntPtr.Zero);
				User32.MouseEvent(GetMouseEventFlags(key, false), 0u, 0u, 0u, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				User32.MouseEvent(GetMouseEventFlags(key, true), 0u, 0u, 0u, IntPtr.Zero);
			}
			else
			{
				User32.MouseEvent(GetMouseEventFlags(key, false), 0u, 0u, 0u, IntPtr.Zero);
			}
		}

		public static void SendMouseEventMove(int x, int y, bool relative)
		{
			User32.MouseEvent(
				relative ? NativeMouseEventFlags.Move : NativeMouseEventFlags.Absolute | NativeMouseEventFlags.Move,
				(uint)x,
				(uint)y,
				0u,
				IntPtr.Zero);
		}

		public static bool PostMessage(IntPtr hwnd, Keys key, KeyStates state)
		{
			var wparam = (IntPtr)((int)key);

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				return User32.PostMessageW(hwnd, WindowMessage.Keydown, wparam, IntPtr.Zero)
					&& User32.PostMessageW(hwnd, WindowMessage.Keyup, wparam, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				return User32.PostMessageW(hwnd, WindowMessage.Keydown, wparam, IntPtr.Zero);
			}
			else
			{
				return User32.PostMessageW(hwnd, WindowMessage.Keyup, wparam, IntPtr.Zero);
			}
		}

		public static bool PostMessageMove(IntPtr hwnd, int x, int y)
		{
			// msdn: MAKELPARAM
			var lparam = (IntPtr)((y << 16) | (x & 0xFFFF));

			return User32.PostMessageW(hwnd, WindowMessage.Mousemove, IntPtr.Zero, lparam);
		}

		public static void SendMessage(IntPtr hwnd, Keys key, KeyStates state)
		{
			var wparam = (IntPtr)((int)key);

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				User32.SendMessage(hwnd, WindowMessage.Keydown, wparam, IntPtr.Zero);
				User32.SendMessage(hwnd, WindowMessage.Keyup, wparam, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				User32.SendMessage(hwnd, WindowMessage.Keydown, wparam, IntPtr.Zero);
			}
			else
			{
				User32.SendMessage(hwnd, WindowMessage.Keyup, wparam, IntPtr.Zero);
			}
		}

		public static void SendMessageMove(IntPtr hwnd, int x, int y)
		{
			// msdn: MAKELPARAM
			var lparam = (IntPtr)((y << 16) | (x & 0xFFFF));

			User32.SendMessage(hwnd, WindowMessage.Mousemove, IntPtr.Zero, lparam);
		}
	}
}
