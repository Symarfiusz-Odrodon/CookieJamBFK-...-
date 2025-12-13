using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// CharacterDisplay Script.
/// </summary>
public class CharacterDisplay : Script
{
    private Texture charTexture;
    public Material charMaterial;
    /// <inheritdoc/>
    public override void OnStart()
    {
        charMaterial.SetParameterValue("TextureParam", charTexture);
        var model = Actor.As<StaticModel>();
        if (model != null)
        {
            model.SetMaterial(0, charMaterial);
        }
        // Here you can add code that needs to be called when script is created, just before the first game update
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
        // Here you can add code that needs to be called every frame
    }
}
