using System;
using System.Windows.Forms;
using System.Windows.Input;

using InputManager.PInvoke;

namespace InputManager.InputHelper
{
	internal static class KeyboardInputHelper
	{
		public static bool SendKeyboardEvent(Keys key)
			=> SendKeyboardEvent(key, KeyStates.Toggled);

		public static bool SendKeyboardEvent(Keys key, KeyStates state)
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

			return true;
		}

		public static bool SendKeyboardInput(Keys key)
			=> SendKeyboardInput(key, KeyStates.Toggled);

		public static bool SendKeyboardInput(Keys key, KeyStates state)
		{
			var inputEvent = new NativeInputEvent()
			{
				Type = NativeInputType.Keyboard
			};
			inputEvent.KeyboardInput.Key = (ushort)key;

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Down;

				if (User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) == 0u)
					return false;

				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Up;

				return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Down;

				return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
			}
			else
			{
				inputEvent.KeyboardInput.Flags = NativeKeyboardEventFlags.Up;

				return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
			}
		}
	}
}
