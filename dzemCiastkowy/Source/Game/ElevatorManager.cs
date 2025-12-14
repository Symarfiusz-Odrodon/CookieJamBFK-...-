using System;
using System.Collections.Generic;
using dzemCiastkowy;
using FlaxEngine;
using FlaxInk; // Required for JsonAssetReference<InkStory>
using Game.NPC;
using Game.Dialogue; // Required for DialogueController
using System.Linq;
using FlaxEngine.GUI; // Added for safety if you need LINQ

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

    public Texture idfkanymorethisisasprite;
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

    public SpriteRender thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh;

    // References to your existing managers
    private NPC_System _npcSystem;
    private ExternalNPCManager _npcManager;
    private DialogueController _dialogueController;

    private float timer1;
    private bool traveling = false;

    private bool waitForKill = false;
    private int completedTalk = 0;

    /// <inheritdoc/>
    public override void OnStart()
    {
        // Find necessary components
        _npcSystem = Actor.Scene.FindScript<NPC_System>();
        _npcManager = Actor.Scene.FindScript<ExternalNPCManager>();
        cb = Actor.Scene.FindScript<ClickButton>();
        _dialogueController = Actor.Scene.FindScript<DialogueController>();

        if (_npcSystem == null || _dialogueController == null || _npcManager == null)
        {
            Debug.LogError("ElevatorManager is missing critical components.");
            return;
        }

        // BUILD THE LIST
        BuildEncounterSequence();

        // Link the dialogue finished event to end the encounter
        _dialogueController.OnStoryFinished += EndCurrentEncounter;
    }

    private void BuildEncounterSequence()
    {
        EncounterSequence.Clear();

        int maxCount = Math.Max(possibleFriendlyEncounters.Count, possibleEnemyEncounters.Count);

        maxCount += 20;

        // Iterate through both lists by index and interleave (F[i] then E[i]).
        for (int i = 0; i < maxCount; i++)
        {
            // Add Friendly at current index i
            for( int j = 0; j < possibleFriendlyEncounters.Count; j++)
            {
                if (possibleFriendlyEncounters[j].weight == i)
                {
                    var f = possibleFriendlyEncounters[j];
                    f.safeFloor = true;
                    EncounterSequence.Add(f);
                }
            }

            for (int j = 0; j < possibleEnemyEncounters.Count; j++)
            {
                if (possibleEnemyEncounters[j].weight == i)
                {
                    var e = possibleEnemyEncounters[j];
                    e.safeFloor = false;
                    EncounterSequence.Add(e);
                }
            }
        }

        Debug.Log($"Sequence built with {EncounterSequence.Count} total floors.");
    }

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

        Debug.Log($"Encounter started: {npcIdToSpawn} (Safe: {currentEncounter.safeFloor})");

        // --- FRIENDLY FLOOR ---
        if (currentEncounter.safeFloor)
        {
            var storyAsset = currentEncounter.storyAsset;

            // Spawn Friendly NPC if name exists
            if (!string.IsNullOrEmpty(npcIdToSpawn))
            {
                //_npcManager.SpawnNpc(npcIdToSpawn);
            }

            if (storyAsset != null)
            {
                thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh.IsActive = true;

                // Assign the sprite using .Instance
                if (currentEncounter.idfkanymorethisisasprite != null)
                {
                    thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh.Image = currentEncounter.idfkanymorethisisasprite;
                }

                _dialogueController.StartStory(storyAsset);
            }
            else
            {
                Debug.LogError($"Friendly encounter ({npcIdToSpawn}) missing InkStory asset! Ending.");
                EndCurrentEncounter();
            }
        }
        // --- ENEMY FLOOR ---
        else
        {
            // Spawn multiple enemies
            var enemies = new List<string> { currentEncounter.encounterName, currentEncounter.encounterName2, currentEncounter.encounterName3 };
            if(!currentEncounter.talkBeforeFight)
            {
                foreach (var enName in enemies)
                {
                    if (!string.IsNullOrEmpty(enName))
                    {
                        _npcManager.SpawnEnemyNpc(enName);
                    }
                }
            } 

            // Check for Talk Before Fight
            if (currentEncounter.talkBeforeFight && currentEncounter.storyAsset != null)
            {
                thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh.IsActive = true;

                if (currentEncounter.idfkanymorethisisasprite != null)
                {
                    thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh.Image = currentEncounter.idfkanymorethisisasprite;
                }

                _dialogueController.StartStory(currentEncounter.storyAsset);
                // Note: Logic for starting combat AFTER talk needs to be handled in EndCurrentEncounter or Dialogue system
            }
            else
            {
                // Go straight to combat
                StartCombat();
            }
        }
    }

    private void StartCombat()
    {
        waitForKill = true;
        Actor.Scene.FindScript<CombatSystem>().inCombat = true;
        Actor.Scene.FindScript<MusicController>().StartStopCombatMusic();

        // Hide sprite during combat if preferred
        thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh.IsActive = false;
    }

    [DebugCommand]
    public void EndCurrentEncounter()
    {
        if (!IsEncounterActive && !waitForKill)
        {
            Debug.LogWarning("Cannot end encounter, no encounter is currently active.");
            return;
        }

        if (EncounterSequence[CurrentFloor - 1].talkBeforeFight && completedTalk < 1)
        {
            Debug.Log(completedTalk);
            if (NPC_System.Instance.Enemies.Count == 0)
            {
                _npcManager.SpawnEnemyNpc(EncounterSequence[CurrentFloor - 1].encounterName);
                _npcManager.SpawnEnemyNpc(EncounterSequence[CurrentFloor - 1].encounterName2);
                _npcManager.SpawnEnemyNpc(EncounterSequence[CurrentFloor - 1].encounterName3);
                StartCombat();
                IsEncounterActive = true;
                waitForKill = true;
            }
            return;
        }

        completedTalk = 0;

        // If we were in combat, stop music
        if (Actor.Scene.FindScript<CombatSystem>().inCombat)
        {
            Actor.Scene.FindScript<MusicController>().StartStopCombatMusic();
        }

        Actor.Scene.FindScript<ExternalNPCManager>().ModifyNpcHealth(0, 10000);
        Actor.Scene.FindScript<ExternalNPCManager>().ModifyNpcHealth(1, 10000);
        Actor.Scene.FindScript<ExternalNPCManager>().ModifyNpcHealth(2, 10000);

        IsEncounterActive = false;
        waitForKill = false;
        Actor.Scene.FindScript<CombatSystem>().inCombat = false;

        thebitchthatspawnsrnbecausefuckyouiamlosingmymindforgodssakeaaaaaaahhhhhhhh.IsActive = false;


        
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

        if (waitForKill && NPC_System.Instance.Enemies.Count == 0)
        {
            completedTalk++;
            waitForKill = false;
            EndCurrentEncounter();
        }
    }
}