using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// QuitGame Script.
/// </summary>
public class QuitGame : Script
{
    /// <inheritdoc/>
    public override void OnStart()
    {
        
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        // Here you can add code that needs to be called when script is enabled (eg. register for events)
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        var uiControl = Actor as UIControl;
        if (uiControl != null)
        {
            if (uiControl.Get<Button>().IsPressed)
                        Engine.RequestExit();
        }
        
        // Here you can add code that needs to be called every frame
    }
}
