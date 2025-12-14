using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxInk;
using Game;
using Game.Dialogue;

namespace dzemCiastkowy;

/// <summary>
/// TutorialController Script.
/// </summary>
public class TutorialController : Script
{
    public JsonAssetReference<InkStory> story;
    public JsonAssetReference<InkStory> story2;
    public DialogueController controller;

    public ElevatorDoor doorcont;

    public Camera cum1;
    public Camera cum2;
    public Camera cum3;

    private bool _started = false;
    private bool _started2 = false;
    private float timer = 0;

    private bool lol = false;

    /// <inheritdoc/>
    public override void OnStart()
    {
        doorcont.OnOpenClose();
        cum1.IsActive = true;
        cum2.IsActive = false;
        cum3.IsActive = false;
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
            Debug.Log("Start tutorial");
            controller.StartStory(story);
            _started = true;
        }

        if(_started && !controller.StoryActive && !_started2)
        {
            if(!lol)
            {
                lol = true;
                cum1.IsActive = false;
                cum2.IsActive = true;
            }
            timer += Time.DeltaTime;
            if(timer > 0.5f)
            {
                Debug.Log("Start tutorial 2");
                _started2 = true;
                controller.StartStory(story2);
            }
        }

        if(_started2 && !controller.StoryActive)
        {
            cum2.IsActive = false;
            cum3.IsActive = true;
            Actor.Scene.FindScript<ClickButton>().unlocked = true;
            Destroy(Actor);
        }

        //Debug.Log("started: " + _started + " controller: " + controller.StoryActive + " startedd2: " + _started2);

        // Here you can add code that needs to be called every frame
    }
}
