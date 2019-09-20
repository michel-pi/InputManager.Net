using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.InputHelper;
using AsyncKeyState.Internal;

namespace AsyncKeyState
{
	/// <summary>
	/// Synthesizes custom mouse, keyboard and window input.
	/// </summary>
	public static class InputGenerator
	{
		/// <summary>
		/// Synthesizes keyboard events.
		/// </summary>
		public static class Keyboard
		{
			/// <summary>
			/// Presses a key on the Keyboard.
			/// </summary>
			/// <param name="key">The key to press.</param>
			/// <param name="useKeyboardEvent">Determines whether to use the keyboard_event API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Press(Keys key, bool useKeyboardEvent = false)
				=> Press(key, KeyStates.Toggled, useKeyboardEvent);

			/// <summary>
			/// Changes the state of a key on the Keyboard.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="state">The new state of the given key.</param>
			/// <param name="useKeyboardEvent">Determines whether to use the keyboard_event API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Press(Keys key, KeyStates state, bool useKeyboardEvent = false)
			{
				if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ArgumentOutOfRangeException(nameof(key));
				if (ValidationHelper.IsKeyStatesOutOfRange(state)) ThrowHelper.ArgumentOutOfRangeException(nameof(state));

				return useKeyboardEvent
					? KeyboardInputHelper.SendKeyboardEvent(key, state)
					: KeyboardInputHelper.SendKeyboardInput(key, state);
			}

			/// <summary>
			/// Presses a key on the Keyboard and immediately returns.
			/// </summary>
			/// <param name="key">The key to press.</param>
			/// <param name="useKeyboardEvent">Determines whether to use the keyboard_event API or not.</param>
			/// <returns>Returns a Task<bool></bool>.</returns>
			public static Task<bool> PressAsync(Keys key, bool useKeyboardEvent = false)
				=> Task.Factory.StartNew(() => Press(key, useKeyboardEvent));

			/// <summary>
			/// Changes the state of a key on the Keyboard and immediately returns.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="state">The new state of the given key.</param>
			/// <param name="useKeyboardEvent">Determines whether to use the keyboard_event API or not.</param>
			/// <returns>Returns a Task<bool></bool>.</returns>
			public static Task<bool> PressAsync(Keys key, KeyStates state, bool useKeyboardEvent = false)
				=> Task.Factory.StartNew(() => Press(key, state, useKeyboardEvent));
		}

		/// <summary>
		/// Synthesizes mouse events.
		/// </summary>
		public static class Mouse
		{
			/// <summary>
			/// Moves the mouse to a specific coordinate on the screen or relative by an amount of pixels.
			/// </summary>
			/// <param name="x">The absolute x-coordinate of the mouse or a relative amount of pixels.</param>
			/// <param name="y">The absolute y-coordinate of the mouse or a relative amount of pixels.</param>
			/// <param name="relative">Determines whether the coordinates are absolute.</param>
			/// <param name="useMouseEvent">Determines whether to use the mouse_event API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Move(int x, int y, bool relative = false, bool useMouseEvent = false)
			{
				return useMouseEvent
					? MouseInputHelper.SendMouseMoveEvent(x, y, relative)
					: MouseInputHelper.SendMouseMoveInput(x, y, relative);
			}

			/// <summary>
			/// Moves the mouse to a specific coordinate on the screen or relative by an amount of pixels and returns immediately.
			/// </summary>
			/// <param name="x">The absolute x-coordinate of the mouse or a relative amount of pixels.</param>
			/// <param name="y">The absolute y-coordinate of the mouse or a relative amount of pixels.</param>
			/// <param name="relative">Determines whether the coordinates are absolute.</param>
			/// <param name="useMouseEvent">Determines whether to use the mouse_event API or not.</param>
			/// <returns>Returns a Task<bool></bool>.</returns>
			public static Task<bool> MoveAsync(int x, int y, bool relative = false, bool useMouseEvent = false)
				=> Task.Factory.StartNew(() => Move(x, y, relative, useMouseEvent));

			/// <summary>
			/// Presses a button on the mouse.
			/// </summary>
			/// <param name="key">The mouse button to press.</param>
			/// <param name="useMouseEvent">Determines whether to use the mouse_event API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Press(Keys key, bool useMouseEvent = false)
										=> Press(key, KeyStates.Toggled, useMouseEvent);

			/// <summary>
			/// Presses a button on the mouse.
			/// </summary>
			/// <param name="key">The mouse button to press.</param>
			/// <param name="state">The new state of the mouse button.</param>
			/// <param name="useMouseEvent">Determines whether to use the mouse_event API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool Press(Keys key, KeyStates state, bool useMouseEvent = false)
			{
				if (ValidationHelper.IsKeyOutOfRange(key)) ThrowHelper.ArgumentOutOfRangeException(nameof(key));
				if (ValidationHelper.IsKeyStatesOutOfRange(state)) ThrowHelper.ArgumentOutOfRangeException(nameof(state));

				return useMouseEvent
					? MouseInputHelper.SendMouseEvent(key, state)
					: MouseInputHelper.SendMouseInput(key, state);
			}

			/// <summary>
			/// Presses a button on the mouse and returns immediately.
			/// </summary>
			/// <param name="key">The mouse button to press.</param>
			/// <param name="useMouseEvent">Determines whether to use the mouse_event API or not.</param>
			/// <returns>Returns a Task<bool></bool>.</returns>
			public static Task<bool> PressAsync(Keys key, bool useMouseEvent = false)
				=> Task.Factory.StartNew(() => Press(key, useMouseEvent));

			/// <summary>
			/// Presses a button on the mouse and returns immediately.
			/// </summary>
			/// <param name="key">The mouse button to press.</param>
			/// <param name="state">The new state of the mouse button.</param>
			/// <param name="useMouseEvent">Determines whether to use the mouse_event API or not.</param>
			/// <returns>Returns a Task<bool></bool>.</returns>
			public static Task<bool> PressAsync(Keys key, KeyStates state, bool useMouseEvent = false)
				=> Task.Factory.StartNew(() => Press(key, state, useMouseEvent));
		}

		/// <summary>
		/// Synthesizes mouse and keyboard events for a window or control.
		/// </summary>
		public static class Window
		{
			/// <summary>
			/// Moves the mouse relative to the window. PostMessage alows async execution and returns immediately.
			/// </summary>
			/// <param name="windowHandle">The handle of a Window or Control.</param>
			/// <param name="x">The new x-coordinate of the mouse.</param>
			/// <param name="y">The new y-coordinate of the mouse.</param>
			/// <param name="usePostMessage">Determines whether to use the PostMessage API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool MoveMouse(IntPtr windowHandle, int x, int y, bool usePostMessage = false)
			{
				return usePostMessage
					? WindowInputHelper.PostMouseMoveMessage(windowHandle, x, y)
					: WindowInputHelper.SendMouseMoveMessage(windowHandle, x, y);
			}

			/// <summary>
			/// Presses a key (mouse or keyboard) inside a window or control. PostMessage alows async execution and returns immediately.
			/// </summary>
			/// <param name="windowHandle">The handle of a Window or Control.</param>
			/// <param name="key">The key to press.</param>
			/// <param name="usePostMessage">Determines whether to use the PostMessage API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool PressKey(IntPtr windowHandle, Keys key, bool usePostMessage = false)
										=> PressKey(windowHandle, key, KeyStates.Toggled, usePostMessage);

			/// <summary>
			/// Presses a key (mouse or keyboard) inside a window or control. PostMessage alows async execution and returns immediately.
			/// </summary>
			/// <param name="windowHandle">The handle of a Window or Control.</param>
			/// <param name="key">The key.</param>
			/// <param name="state">The state of the given key.</param>
			/// <param name="usePostMessage">Determines whether to use the PostMessage API or not.</param>
			/// <returns>Returns true on success.</returns>
			public static bool PressKey(IntPtr windowHandle, Keys key, KeyStates state, bool usePostMessage = false)
			{
				return usePostMessage
					? WindowInputHelper.PostMessage(windowHandle, key, state)
					: WindowInputHelper.SendMessage(windowHandle, key, state);
			}
		}
	}
}
