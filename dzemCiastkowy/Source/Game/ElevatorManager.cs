using System;
using System.Collections.Generic;
using dzemCiastkowy;
using FlaxEngine;
using FlaxInk; // Required for JsonAssetReference<InkStory>
using Game.NPC;
using Game.Dialogue; // Required for DialogueController
using System.Linq; // Added for safety if you need LINQ

namespace Game.Managers;

// UPDATED FloorData Class
[Serializable]
public class FloorData
{
    // All fields inside a [Serializable] class that you want to save must be public.
    public bool safeFloor = false;
    public string encounterName;
    public string encounterName2;
    public string encounterName3;

    //public AudioClip music;

    public int weight = 0;
    public int maxWeight = 0;

    public bool talkBeforeFight = false;

    // The asset reference itself is public, which is correct.
    public JsonAssetReference<InkStory> storyAsset;

    // It's also best practice to include a parameterless constructor when 
    // using custom classes in Lists.
    public FloorData() { }

    // Your existing custom constructor is fine to keep, but the parameterless 
    // one helps the Inspector create new instances.
    public FloorData(bool safeFloor, string encounterName)
    {
        this.safeFloor = safeFloor;
        this.encounterName = encounterName;
    }
}

/// <summary>
/// Manages the floor progression and encounter spawning for the elevator game loop.
/// </summary>
public class ElevatorManager : Script
{
    // ... existing Configuration fields ...

    [Header("Configuration")]
    [Tooltip("The time (in seconds) the elevator stays open for the player to react.")]
    public float OpenDoorDuration = 5.0f;

    public ClickButton cb;

    public List<FloorData> possibleFriendlyEncounters = new List<FloorData>();
    public List<FloorData> possibleEnemyEncounters = new List<FloorData>();

    [Tooltip("List of NPC IDs, one for each floor encounter.")]
    public List<FloorData> EncounterSequence = new List<FloorData>
    {
        // NOTE: You must select the ElevatorManager in the Editor, find this list, 
        // and assign the InkStory asset (if friendly) for each element.
        new FloorData(true, "tutorial_npc"),
        new FloorData(false, "basic_enemy"),
        new FloorData(true, "recruit_friend"),
        new FloorData(false, "basic_enemy")
    };

    [Header("Runtime State")]
    [Tooltip("The current floor number (1-based).")]
    [ReadOnly]
    public int CurrentFloor = 0;

    [ReadOnly]
    public bool IsEncounterActive = false;

    // References to your existing managers
    private NPC_System _npcSystem;
    private ExternalNPCManager _npcManager;
    private DialogueController _dialogueController;

    private float timer1;
    private bool traveling = false;

    private bool waitForKill = false;

    /// <inheritdoc/>
    public override void OnStart()
    {
        // Find necessary components
        _npcSystem = Actor.Scene.FindScript<NPC_System>();
        _npcManager = Actor.Scene.FindScript<ExternalNPCManager>();
        cb = Actor.Scene.FindScript<ClickButton>();
        _dialogueController = Actor.Scene.FindScript<DialogueController>();

        if (_npcSystem == null)
        {
            Debug.LogError("ElevatorManager requires an NPC_System component.");
            return;
        }
        if (_dialogueController == null)
        {
            Debug.LogError("ElevatorManager requires a DialogueController component.");
            return;
        }

        // Link the dialogue finished event to end the encounter
        _dialogueController.OnStoryFinished += EndCurrentEncounter;
    }

    /// <summary>
    /// Initiates the next floor encounter. Call this when the current encounter is finished.
    /// </summary>
    public void GoToNextFloor()
    {
        if (IsEncounterActive) return;

        CurrentFloor++;
        traveling = true;

        Actor.Scene.FindScript<FloorRenderer>()?.OnFloorUpdate(1);

        Debug.Log($"--- Elevator arriving at Floor {CurrentFloor} ---");

        if (CurrentFloor > EncounterSequence.Count)
        {
            Debug.Log("Game Over: All encounters completed!");
            return;
        }

        var floorDisplay = Actor.Scene.FindScript<FloorDisplay>();
        if (floorDisplay != null)
        {
            for (int i = CurrentFloor; i < 5 + CurrentFloor; i++)
            {
                if (i <= EncounterSequence.Count - 1)
                {
                    floorDisplay.Floors[i - CurrentFloor].IsSafe = EncounterSequence[i].safeFloor ? 0 : 1;
                }
                else
                {
                    floorDisplay.Floors[i - CurrentFloor].IsSafe = 2;
                }
            }
        }
    }

    private void OpenDoor()
    {
        Actor.Scene.FindScript<ElevatorDoor>()?.OnOpenClose();
        IsEncounterActive = true;

        StartEncounter(CurrentFloor - 1);
    }

    private void StartEncounter(int sequenceIndex)
    {
        if (sequenceIndex < 0 || sequenceIndex >= EncounterSequence.Count) return;

        var currentEncounter = EncounterSequence[sequenceIndex];
        string npcIdToSpawn = currentEncounter.encounterName;

        // 1. Spawn the NPC/Enemy
        //_npcManager?.SpawnNpc(npcIdToSpawn);

        Debug.Log($"Encounter started: Spawned NPC ID: {npcIdToSpawn}");

        // 2. CONDITIONAL DIALOGUE START
        if (currentEncounter.safeFloor)
        {
            // NEW LOGIC: Use the directly assigned storyAsset
            var storyAsset = currentEncounter.storyAsset;

            if (storyAsset.Instance != null)
            {
                _dialogueController.StartStory(storyAsset);
                // DialogueController.OnStoryFinished will call EndCurrentEncounter
            }
            else
            {
                Debug.LogError($"Friendly encounter ({npcIdToSpawn}) is missing an assigned InkStory asset! Ending encounter immediately.");
                EndCurrentEncounter();
            }
        }
        else
        {
            waitForKill = true;
            _npcManager.SpawnEnemyNpc(npcIdToSpawn);
            Actor.Scene.FindScript<CombatSystem>().inCombat = true;
            Actor.Scene.FindScript<MusicController>().StartStopCombatMusic();
        }
    }

    /// <summary>
    /// Called when the player has finished their interaction (dialogue ended or enemy defeated).
    /// </summary>
    [DebugCommand]
    public void EndCurrentEncounter()
    {
        if (!IsEncounterActive)
        {
            Debug.LogWarning("Cannot end encounter, no encounter is currently active.");
            return;
        }

        if( Actor.Scene.FindScript<CombatSystem>().inCombat)
        {
            Actor.Scene.FindScript<MusicController>().StartStopCombatMusic();
        }

        IsEncounterActive = false;
        Actor.Scene.FindScript<CombatSystem>().inCombat = false;

        if (cb != null)
        {
            cb.unlocked = true;
        }
    }

    public override void OnUpdate()
    {
        if (traveling)
        {
            timer1 += Time.DeltaTime;

            if (timer1 > 1.5f)
            {
                timer1 = 0;
                traveling = false;
                OpenDoor();
            }
        }

        if(waitForKill && NPC_System.Instance.Enemies.Count == 0)
        {
            waitForKill = false;
            EndCurrentEncounter();
        }
    }
}