using System;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState.InputHelper
{
	internal static class KeyboardInputHelper
	{
		private static readonly int InputEventSize = NativeKeyboardInput.Size + 4;

		public static bool SendInput(Keys key)
			=> SendInput(key, KeyStates.Toggled);

		public static bool SendInput(Keys key, KeyStates state)
		{
			var inputEvent = new NativeInputEvent()
			{
				Type = NativeInputType.Keyboard
			};
			inputEvent.KeyboardInput.Key = (ushort)key;

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Down;

				if (User32.SendInput(1u, ref inputEvent, InputEventSize) == 0u)
					return false;

				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Up;

				return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Down;

				return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
			}
			else
			{
				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Up;

				return User32.SendInput(1u, ref inputEvent, InputEventSize) != 0u;
			}
		}

		public static void SendKeyboardEvent(Keys key)
			=> SendKeyboardEvent(key, KeyStates.Toggled);

		public static void SendKeyboardEvent(Keys key, KeyStates state)
		{
			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				User32.KeybdEvent((byte)key, 0, NativeKeyboardEventFlags.Down, IntPtr.Zero);
				User32.KeybdEvent((byte)key, 0, NativeKeyboardEventFlags.Up, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				User32.KeybdEvent((byte)key, 0, NativeKeyboardEventFlags.Down, IntPtr.Zero);
			}
			else
			{
				User32.KeybdEvent((byte)key, 0, NativeKeyboardEventFlags.Up, IntPtr.Zero);
			}
		}

		public static bool PostMessage(IntPtr windowHandle, Keys key, KeyStates state)
		{
			var wparam = (IntPtr)key;

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				return User32.PostMessageW(windowHandle, GetWindowMessage(key, true), wparam, IntPtr.Zero)
					&& User32.PostMessageW(windowHandle, GetWindowMessage(key, false), wparam, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				return User32.PostMessageW(windowHandle, GetWindowMessage(key, true), wparam, IntPtr.Zero);
			}
			else
			{
				return User32.PostMessageW(windowHandle, GetWindowMessage(key, false), wparam, IntPtr.Zero);
			}
		}

		public static void SendMessage(IntPtr windowHandle, Keys key, KeyStates state)
		{
			var wparam = (IntPtr)key;

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				User32.SendMessage(windowHandle, GetWindowMessage(key, true), wparam, IntPtr.Zero);
				User32.SendMessage(windowHandle, GetWindowMessage(key, false), wparam, IntPtr.Zero);
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				User32.SendMessage(windowHandle, GetWindowMessage(key, true), wparam, IntPtr.Zero);
			}
			else
			{
				User32.SendMessage(windowHandle, GetWindowMessage(key, false), wparam, IntPtr.Zero);
			}
		}

		private static WindowMessage GetWindowMessage(Keys key, bool down)
		{
			if (down)
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
						return WindowMessage.Syskeydown;
					default:
						return WindowMessage.Keydown;
				}
			}
			else
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
						return WindowMessage.Syskeyup;
					default:
						return WindowMessage.Keyup;
				}
			}
		}
	}
}
