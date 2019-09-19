using System;
using System.Windows.Forms;
using AsyncKeyState;

namespace AsyncKeyState.Test
{
	internal static class Program
	{
		private static void Main()
		{
			InputGenerator.Keyboard.SendInput(Keys.Space);
		}
	}
}
