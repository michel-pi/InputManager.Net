<div align="center">

# InputManager.Net

[![Nuget](https://img.shields.io/nuget/v/AsyncKeyState.Net.svg)](https://www.nuget.org/packages/AsyncKeyState.Net/ "AsyncKeyState.Net on NuGet") [![Nuget](https://img.shields.io/nuget/dt/InputManager.Net.svg)](https://www.nuget.org/packages/AsyncKeyState.Net/ "Downloads on NuGet") [![Open issues](https://img.shields.io/github/issues-raw/michel-pi/InputManager.Net.svg)](https://github.com/michel-pi/InputManager.Net/issues "Open issues on Github") [![Closed issues](https://img.shields.io/github/issues-closed-raw/michel-pi/InputManager.Net.svg)](https://github.com/michel-pi/InputManager.Net/issues?q=is%3Aissue+is%3Aclosed "Closed issues on Github") [![MIT License](https://img.shields.io/github/license/michel-pi/InputManager.Net.svg)](https://github.com/michel-pi/InputManager.Net/blob/master/LICENSE "InputManager.Net license")

![Net Framework 4.7](https://img.shields.io/badge/.Net-4.7-informational.svg) ![Net Framework 4.8](https://img.shields.io/badge/.Net-4.8-informational.svg)
</div>

This library provides an easy to use interface for the native `GetAsyncKeyState`, `GetKeyState` and `GetKeyboardState` API and aswell synthesizes keyboard and mouse input.

To provide better compatibility to other implementations using `VirtualKeyCodes` i decided to not implement the enum myself and instead use the Windows Forms implementation which is a complete enum representing the native `VK_` constants.

You can freely convert between Windows Forms `Keys` and WPF `Key` enum values.

## [Documentation](https://michel-pi.github.io/InputManager.Net/ "InputManager.Net Documentation")

## NuGet

    Install-Package AsyncKeyState.Net

This library is also available in the [Github Package Registry](https://github.com/michel-pi/InputManager.Net/packages/ "InputManager.Net Github Package").

## Features

- Get the current `KeyStates` of any virtual key using the `AsyncInput` class
- Get and update all `KeyStates` at once using the `KeyboardState` class
- Automatically thread safe KeyboardState using a thread static cache
- Convert between WPF Key and Windows Forms Keys enum
- Documented and tested class library

## Examples

Using the `AsyncInput` class:

```cs
// returns a flags enum representing the state of the control key
AsyncInput.GetKeyState(Keys.Control);

// returns true if the ctrl key is currently pressed
AsyncInput.IsPressed(Keys.Control);

// and much more
```

Using the `KeyboardState` class:
(Which also includes mouse states ;))

```cs
var keyboard = new KeyboardState();

for (int i = KeyboardState.MinKeyValue; i < KeyboardState.MaxKeyValue; i++)
{
    Console.WriteLine(keyboard.IsPressed((Keys)i));
}

Console.ReadLine();
```

You can get a thread static instance of `KeyboardState` by using:

```cs
var keyboard = KeyboardState.GetThreadStatic();
```

The thread static instance is automatically thread safe by design.

## Contribute

The project file was generated using Visual Studio 2019.

Clone or download the repository and update/install the required NuGet packages.

You can help by reporting issues, adding new features, fixing bugs and by providing a better documentation.

## Donate

Do you like this project and want to help me to keep working on it?

Then maybe consider to donate any amount you like.

[![Donate via PayPal](https://media.wtf/assets/img/pp.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=YJDWMDUSM8KKQ "Donate via PayPal")

```
BTC     bc1qp6zc73vy8pmr6lfe4cxa6eqzvkuer9hrjwpzza
```

## License

- [InputManager.Net License](https://github.com/michel-pi/InputManager.Net/blob/master/LICENSE "InputManager.Net License")
