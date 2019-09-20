using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.InputHelper;
using AsyncKeyState.Internal;

namespace AsyncKeyState
{
	/// <summary>
	/// Synthesizes custom Mouse, Keyboard and Window input.
	/// </summary>
	public static class InputGenerator
	{
		/// <summary>
		/// Synthesizes Keyboard events.
		/// </summary>
		public static class Keyboard
		{
			/// <summary>
			/// Presses a key on the Keyboard.
			/// </summary>
			/// <param name="key">The key to press.</param>
			/// <param name="useKeyboardEvent">Determines whether to use the keyboard_event api or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Press(Keys key, bool useKeyboardEvent = false)
				=> Press(key, KeyStates.Toggled, useKeyboardEvent);

			/// <summary>
			/// Changes the state of a key on the Keyboard.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="state">The new state of the given key.</param>
			/// <param name="useKeyboardEvent">Determines whether to use the keyboard_event api or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Press(Keys key, KeyStates state, bool useKeyboardEvent = false)
			{
				if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ArgumentOutOfRangeException(nameof(key));
				if (ValidationHelper.IsKeyStatesOutOfRange(state)) ThrowHelper.ArgumentOutOfRangeException(nameof(state));

				return useKeyboardEvent
					? KeyboardInputHelper.SendKeyboardEvent(key, state)
					: KeyboardInputHelper.SendKeyboardInput(key, state);
			}

			public static Task<bool> PressAsync(Keys key)
				=> Task.Factory.StartNew(() => Press(key));

			public static Task<bool> PressAsync(Keys key, KeyStates state)
				=> Task.Factory.StartNew(() => Press(key, state));
		}

		public static class Mouse
		{
			public static bool Move(int x, int y, bool relative = false, bool useMouseEvent = false)
			{
				return useMouseEvent
					? MouseInputHelper.SendMouseMoveEvent(x, y, relative)
					: MouseInputHelper.SendMouseMoveInput(x, y, relative);
			}

			public static Task<bool> MoveAsync(int x, int y, bool relative = false, bool useMouseEvent = false)
				=> Task.Factory.StartNew(() => Move(x, y, relative, useMouseEvent));

			public static bool Press(Keys key, bool useMouseEvent = false)
										=> Press(key, KeyStates.Toggled, useMouseEvent);

			public static bool Press(Keys key, KeyStates state, bool useMouseEvent = false)
			{
				if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ArgumentOutOfRangeException(nameof(key));
				if (ValidationHelper.IsKeyStatesOutOfRange(state)) ThrowHelper.ArgumentOutOfRangeException(nameof(state));

				return useMouseEvent
					? MouseInputHelper.SendMouseEvent(key, state)
					: MouseInputHelper.SendMouseInput(key, state);
			}

			public static Task<bool> PressAsync(Keys key)
				=> Task.Factory.StartNew(() => Press(key));

			public static Task<bool> PressAsync(Keys key, KeyStates state)
				=> Task.Factory.StartNew(() => Press(key, state));
		}

		public static class Window
		{
			public static bool MoveMouse(IntPtr windowHandle, int x, int y, bool usePostMessage = false)
			{
				return usePostMessage
					? WindowInputHelper.PostMouseMoveMessage(windowHandle, x, y)
					: WindowInputHelper.SendMouseMoveMessage(windowHandle, x, y);
			}

			public static Task<bool> MoveMouseAsync(IntPtr windowHandle, int x, int y, bool usePostMessage = false)
				=> Task.Factory.StartNew(() => MoveMouse(windowHandle, x, y, usePostMessage));

			public static bool PressKey(IntPtr windowHandle, Keys key, bool usePostMessage = false)
										=> PressKey(windowHandle, key, KeyStates.Toggled, usePostMessage);

			public static bool PressKey(IntPtr windowHandle, Keys key, KeyStates state, bool usePostMessage = false)
			{
				return usePostMessage
					? WindowInputHelper.PostMessage(windowHandle, key, state)
					: WindowInputHelper.SendMessage(windowHandle, key, state);
			}

			public static Task<bool> PressKeyAsync(IntPtr windowHandle, Keys key, bool usePostMessage = false)
				=> Task.Factory.StartNew(() => PressKey(windowHandle, key, usePostMessage));

			public static Task<bool> PressKeyAsync(IntPtr windowHandle, Keys key, KeyStates state, bool usePostMessage = false)
				=> Task.Factory.StartNew(() => PressKey(windowHandle, key, state, usePostMessage));
		}
	}
}
