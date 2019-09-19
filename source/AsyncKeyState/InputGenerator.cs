using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.InputHelper;

namespace AsyncKeyState
{
	public static class InputGenerator
	{
		public static class Keyboard
		{
			public static bool Press(Keys key, bool useKeyboardEvent = false)
				=> Press(key, KeyStates.Toggled, useKeyboardEvent);

			public static bool Press(Keys key, KeyStates state, bool useKeyboardEvent = false)
			{
				if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ArgumentOutOfRangeException(nameof(key));
				if (ValidationHelper.IsKeyStatesOutOfRange(state)) ThrowHelper.ArgumentOutOfRangeException(nameof(state));

				if (useKeyboardEvent)
				{
					KeyboardInputHelper.SendKeyboardEvent(key, state);

					return true;
				}
				else
				{
					return KeyboardInputHelper.SendInput(key, state);
				}
			}

			public static Task<bool> PressAsync(Keys key)
				=> Task.Factory.StartNew(() => Press(key));

			public static Task<bool> PressAsync(Keys key, KeyStates state)
				=> Task.Factory.StartNew(() => Press(key, state));
		}

		public static class Mouse
		{
			public static bool Press(Keys key, bool useMouseEvent = false)
				=> Press(key, KeyStates.Toggled, useMouseEvent);

			public static bool Press(Keys key, KeyStates state, bool useMouseEvent = false)
			{
				if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ArgumentOutOfRangeException(nameof(key));
				if (ValidationHelper.IsKeyStatesOutOfRange(state)) ThrowHelper.ArgumentOutOfRangeException(nameof(state));

				if (useMouseEvent)
				{
					MouseInputHelper.SendMouseEvent(key, state);

					return true;
				}
				else
				{
					return MouseInputHelper.SendInput(key, state);
				}
			}

			public static Task<bool> PressAsync(Keys key)
				=> Task.Factory.StartNew(() => Press(key));

			public static Task<bool> PressAsync(Keys key, KeyStates state)
				=> Task.Factory.StartNew(() => Press(key, state));

			public static bool Move(int x, int y, bool relative = false, bool useMouseEvent = false)
			{
				if (useMouseEvent)
				{
					MouseInputHelper.SendMouseEventMove(x, y, relative);

					return true;
				}
				else
				{
					return MouseInputHelper.SendInputMove(x, y, relative);
				}
			}

			public static Task<bool> MoveAsync(int x, int y, bool relative = false, bool useMouseEvent = false)
				=> Task.Factory.StartNew(() => Move(x, y, relative, useMouseEvent));
		}

		public static class Window
		{
			public static bool PressKey(IntPtr windowHandle, Keys key, bool usePostMessage = false)
				=> PressKey(windowHandle, key, KeyStates.Toggled, usePostMessage);

			public static bool PressKey(IntPtr windowHandle, Keys key, KeyStates state, bool usePostMessage = false)
			{
				if (usePostMessage)
				{
					return KeyboardInputHelper.PostMessage(windowHandle, key, state);
				}
				else
				{
					KeyboardInputHelper.SendMessage(windowHandle, key, state);

					return true;
				}
			}

			public static Task<bool> PressKeyAsync(IntPtr windowHandle, Keys key, bool usePostMessage = false)
				=> Task.Factory.StartNew(() => PressKey(windowHandle, key, usePostMessage));

			public static Task<bool> PressKeyAsync(IntPtr windowHandle, Keys key, KeyStates state, bool usePostMessage = false)
				=> Task.Factory.StartNew(() => PressKey(windowHandle, key, state, usePostMessage));

			public static bool MoveMouse(IntPtr windowHandle, int x, int y, bool usePostMessage = false)
			{
				if (usePostMessage)
				{
					return MouseInputHelper.PostMessageMove(windowHandle, x, y);
				}
				else
				{
					MouseInputHelper.SendMessageMove(windowHandle, x, y);

					return true;
				}
			}

			public static Task<bool> MoveMouseAsync(IntPtr windowHandle, int x, int y, bool usePostMessage = false)
				=> Task.Factory.StartNew(() => MoveMouse(windowHandle, x, y, usePostMessage));
		}
	}
}
