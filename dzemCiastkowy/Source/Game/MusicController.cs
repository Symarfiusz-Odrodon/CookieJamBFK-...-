using System;
using System.Collections.Generic;
using FlaxEngine;

namespace dzemCiastkowy;

/// <summary>
/// MusicController Script.
/// </summary>
public class MusicController : Script
{
    public AudioSource elevatorMusic;
    public AudioSource combatMusic;
    public AudioSource jibberSpeech;
    public AudioSource jibberSpeech2;

    public bool jibber;

    private float timer = 0;
    private bool tim = false;
    private float maxTimer = 0.13f;
    private int lastId;
    private RandomStream _rng;

    public List<AudioClip> jibberSounds;

    public void StartStopElevatorMusic()
    {
        if(elevatorMusic.IsActuallyPlaying)
        {
            elevatorMusic.Pause();
        }
        else
        {
            elevatorMusic.Play();
        }
    }

    public void StartStopCombatMusic()
    {
        if(combatMusic.IsActuallyPlaying)
        {
            combatMusic.Pause();
        }
        else
        {
            combatMusic.Play();
        }
    }

    public void StartStopJibberSpeech()
    {
        if (jibber)
        {
            Debug.Log("JibberStop");
            jibber = false;
        }
        else
        {
            Debug.Log("JibberStart");
            jibber = true;
        }
    }


    /// <inheritdoc/>
    public override void OnStart()
    {
        _rng= new RandomStream(DateTime.Now.Millisecond);
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
        if(jibber)
        {
            timer += Time.DeltaTime;
            
            if(timer > 0.14f && jibberSounds.Count > 0)
            {
                Debug.Log(tim);
                tim = !tim;

                timer = 0;

                maxTimer = _rng.RandRange(0.15f, 0.2f);

                int randomIndex = _rng.RandRange(0, jibberSounds.Count - 1);

                if (randomIndex == lastId)
                {
                    randomIndex = (randomIndex + 1) % (jibberSounds.Count - 1);
                }

                lastId = randomIndex;

                AudioClip randomClip = jibberSounds[randomIndex];
                if(tim)
                {
                    jibberSpeech.Clip = randomClip;
                    Debug.Log("jibber1 " + randomIndex);

                    jibberSpeech.Play();
                }
                else
                {
                    jibberSpeech2.Clip = randomClip;
                    Debug.Log("jibber2 " + randomIndex);

                    jibberSpeech2.Play();
                }
            }
            if (!jibberSpeech.IsActuallyPlaying)
            {
            }
        }

        if (Input.GetKeyDown(KeyboardKeys.F))
        {
            StartStopJibberSpeech();
        }
        if (Input.GetKeyDown(KeyboardKeys.G))
        {
            StartStopElevatorMusic();
        }
    }
}
