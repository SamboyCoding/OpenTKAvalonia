using System.Diagnostics;
using Avalonia;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using Avalonia.Threading;
using OpenTK.Graphics.OpenGL;

namespace OpenTKAvalonia;

public abstract class BaseTkOpenGlControl : OpenGlControlBase, ICustomHitTest
{
    /// <summary>
    /// KeyboardState provides an easy-to-use, stateful wrapper around Avalonia's Keyboard events, as OpenTK keyboard states are not handled.
    /// You can access full keyboard state for both the current frame and the previous one through this object.
    /// </summary>
    public AvaloniaKeyboardState KeyboardState = new();

    private AvaloniaTkContext? _avaloniaTkContext;

    private bool _isInitialized;

    /// <summary>
    /// OpenTkRender is called once a frame to draw to the control.
    /// You can do anything you want here, but make sure you undo any configuration changes after, or you may get weirdness with other controls.
    /// </summary>
    protected virtual void OpenTkRender()
    {
        
    }

    /// <summary>
    /// OpenTkInit is called once when the control is first created.
    /// At this point, the GL bindings are initialized and you can invoke GL functions.
    /// You could use this function to load and compile shaders, load textures, allocate buffers, etc.
    /// </summary>
    protected virtual void OpenTkInit()
    {
        
    }

    /// <summary>
    /// OpenTkTeardown is called once when the control is destroyed.
    /// Though GL bindings are still valid, as OpenTK provides no way to clear them, you should not invoke GL functions after this function finishes executing.
    /// At best, they will do nothing, at worst, something could go wrong.
    /// You should use this function as a last chance to clean up any GL resources you have allocated - delete buffers, vertex arrays, programs, and textures.
    /// </summary>
    protected virtual void OpenTkTeardown()
    {
        
    }

    protected sealed override void OnOpenGlRender(GlInterface gl, int fb)
    {
        //Update last key states
        KeyboardState.OnFrame();
        
        //Set up the aspect ratio so shapes aren't stretched.
        //As avalonia is using this opengl instance internally to render the entire window, stuff gets messy, so we workaround that here
        //to provide a good experience to the user.
        // Not sure if this is needed anymore, seems to work fine without it
        // int[] oldViewport = new int[4];
        // GL.GetInteger(GetPName.Viewport, oldViewport);
        GL.Viewport(0, 0, (int) Bounds.Width, (int) Bounds.Height);

        //Tell our subclass to render
        OpenTkRender();
        
        //Reset viewport after our fix above
        // GL.Viewport(oldViewport[0], oldViewport[1], oldViewport[2], oldViewport[3]);
        
        //Workaround for avalonia issue #6488, set active texture back to 0
        // same thing i dont think this is needed anymore
        // GL.ActiveTexture(TextureUnit.Texture0);

        //Schedule next UI update with avalonia
        Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Background);
    }


    protected sealed override void OnOpenGlInit(GlInterface gl)
    {
        if (_isInitialized)
        {
            // workaround for avalonia issue #10371, so we dont initialize twice
            Debug.WriteLine("Calling OnOpenGlInit even tho its already initialized");
            return;
        }
        //Initialize the OpenTK<->Avalonia Bridge
        _avaloniaTkContext = new(gl);
        GL.LoadBindings(_avaloniaTkContext);

        //Invoke the subclass' init function
        OpenTkInit();
        _isInitialized = true;
    }

    //Simply call the subclass' teardown function
    protected sealed override void OnOpenGlDeinit(GlInterface gl)
    {
        OpenTkTeardown();
        _isInitialized = false;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (!IsEffectivelyVisible)
            return;
        
        KeyboardState.SetKey(e.Key, true);
    }
    
    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        if (!IsEffectivelyVisible)
            return;
        
        KeyboardState.SetKey(e.Key, false);
    }

    public bool HitTest(Point point) => Bounds.Contains(point);
}