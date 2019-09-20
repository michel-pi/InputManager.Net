using System;
using System.Windows.Forms;
using System.Windows.Input;

using AsyncKeyState.PInvoke;

namespace AsyncKeyState.InputHelper
{
	internal static class MouseInputHelper
	{
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

				case Keys.XButton1:
				case Keys.XButton2:
					return down ? NativeMouseEventFlags.XDown : NativeMouseEventFlags.XUp;

				default:
					throw new ArgumentOutOfRangeException(nameof(key), "Only Keys.LButton, Keys.RButton and Keys.MButton are supported by this method.");
			}
		}

		public static bool SendMouseEvent(Keys key, KeyStates state)
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

			return true;
		}

		public static bool SendMouseInput(Keys key)
			=> SendMouseInput(key, KeyStates.Toggled);

		public static bool SendMouseInput(Keys key, KeyStates state)
		{
			var inputEvent = new NativeInputEvent()
			{
				Type = NativeInputType.Mouse
			};

			if ((state & KeyStates.Toggled) == KeyStates.Toggled)
			{
				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, true);

				if (User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) == 0u)
					return false;

				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, false);

				return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
			}
			else if ((state & KeyStates.Down) == KeyStates.Down)
			{
				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, true);

				return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
			}
			else
			{
				inputEvent.MouseInput.Flags = GetMouseEventFlags(key, false);

				return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
			}
		}

		public static bool SendMouseMoveEvent(int x, int y, bool relative)
		{
			if (!relative)
			{
				x *= ushort.MaxValue / Screen.PrimaryScreen.Bounds.Width;
				y *= ushort.MaxValue / Screen.PrimaryScreen.Bounds.Height;
			}

			User32.MouseEvent(
				relative ? NativeMouseEventFlags.Move : NativeMouseEventFlags.Absolute | NativeMouseEventFlags.Move,
				(uint)x,
				(uint)y,
				0u,
				IntPtr.Zero);

			return true;
		}

		public static bool SendMouseMoveInput(int x, int y, bool relative)
		{
			if (!relative)
			{
				x *= ushort.MaxValue / Screen.PrimaryScreen.Bounds.Width;
				y *= ushort.MaxValue / Screen.PrimaryScreen.Bounds.Height;
			}

			var inputEvent = new NativeInputEvent()
			{
				Type = NativeInputType.Mouse
			};

			inputEvent.MouseInput.Flags = relative
				? NativeMouseEventFlags.Move
				: NativeMouseEventFlags.Absolute | NativeMouseEventFlags.Move;

			inputEvent.MouseInput.X = x;
			inputEvent.MouseInput.Y = y;

			return User32.SendInput(1u, ref inputEvent, NativeInputEvent.Size) != 0u;
		}
	}
}
