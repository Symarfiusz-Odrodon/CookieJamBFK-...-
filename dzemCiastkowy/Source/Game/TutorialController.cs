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
    public DialogueController controller;

    public Camera cum1;
    public Camera cum2;

    private bool _started = false;

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
        if (!_started && controller != null)
        {
            controller.StartStory(story);
            _started = true;
        }

        // Here you can add code that needs to be called every frame
    }
}
