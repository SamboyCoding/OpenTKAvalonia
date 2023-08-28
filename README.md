# OpenTKAvalonia

OpenTKAvalonia is a simple wrapper to provide OpenTK OpenGL bindings for Avalonia, and a control which handles some of 
the heavy lifting for you.

This includes setting up the OpenGL bindings, setting up the aspect ratio so anything you draw looks as expected, 
~~working around avalonia bug 6488 (where failing to reset the active texture causes all controls to become invisible)~~(seems to be fixed),
working around avalonia issue [12680](https://github.com/AvaloniaUI/Avalonia/issues/12680) (where the init method is called twice), 
providing events for Initialization, Teardown, and Render, and providing a KeyboardState API for detecting keyboard input.

The library is somewhat work-in-progress but it currently functions for simple applications, and there is a sample included.

Note that, in order for events to be called on Windows, Avalonia must be able to load the OpenGL bindings, which requires 
switching the default rendering mode, like so:
```c#
public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new Win32PlatformOptions() {UseWgl = true}) //This line is the important one.
                .LogToTrace();
```
Without this, your control simply will never receive any of the OpenTK events (Initialization, Teardown, and Render).

The library itself is netstandard2.1, because that's what OpenTK requires, and the sample is net6.0.

## Demonstration

![Demonstration image](https://i.imgur.com/mAElUU9.png)