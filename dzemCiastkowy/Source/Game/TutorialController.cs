using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxInk;
using Game.Dialogue;

namespace dzemCiastkowy;

/// <summary>
/// TutorialController Script.
/// </summary>
public class TutorialController : Script
{
    public JsonAssetReference<InkStory> story;
    private DialogueController controller;

    /// <inheritdoc/>
    public override void OnStart()
    {
        Actor.Scene.FindScript<DialogueController>();
        controller.StartStory(story);
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
