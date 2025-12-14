using System;
using System.Collections.Generic;
using dzemCiastkowy;
using FlaxEngine;
using Game.NPC;

namespace Game.Managers;

public class FloorData
{
    public bool safeFloor = false;
    public string encounterName;

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
    [Header("Configuration")]
    [Tooltip("The time (in seconds) the elevator stays open for the player to react.")]
    public float OpenDoorDuration = 5.0f;

    public ClickButton cb;

    [Tooltip("List of NPC IDs, one for each floor encounter.")]
    public List<FloorData> EncounterSequence = new List<FloorData>
    {
        new FloorData(true, "tutorial_npc"), 
    
        new FloorData(false, "basic_enemy"), 
    
        new FloorData(true, "recruit_friend"), 
    
        new FloorData(false, "basic_enemy")
    };

    [Header("Runtime State")]
    [Tooltip("The current floor number.")]
    [ReadOnly]
    public int CurrentFloor = 0;

    [ReadOnly]
    public bool IsEncounterActive = false;

    // References to your existing managers
    private NPC_System _npcSystem;
    private ExternalNPCManager _npcManager;

    // TODO: Add reference to a Door/Visuals script if you create one
    // public DoorController DoorScript; 

    private float timer1;
    private bool traveling = false;


    /// <inheritdoc/>
    public override void OnStart()
    {
        // Find necessary components
        _npcSystem = Actor.Scene.FindScript<NPC_System>();
        _npcManager = Actor.Scene.FindScript<ExternalNPCManager>();
        cb = Actor.Scene.FindScript<ClickButton>();

        if (_npcSystem == null)
        {
            Debug.LogError("ElevatorManager requires an NPC_System component on the same Actor or must find it elsewhere.");
            return;
        }

    }

    /// <summary>
    /// Initiates the next floor encounter. Call this when the current encounter is finished.
    /// </summary>
    public void GoToNextFloor()
    {
        CurrentFloor++;
        traveling = true;

        Actor.Scene.FindScript<FloorRenderer>().OnFloorUpdate(1);

        Debug.Log($"--- Elevator arriving at Floor {CurrentFloor} ---");

        // Check if we have completed all planned encounters
        if (CurrentFloor > EncounterSequence.Count)
        {
            Debug.Log("Game Over: All encounters completed!");
            // TODO: Trigger end-game sequence
            return;
        }
        var floorDisplay = Actor.Scene.FindScript<FloorDisplay>();

        Debug.Log(CurrentFloor);
        for(int i = CurrentFloor; i < 5 + CurrentFloor; i++)
        {
            if (i <= EncounterSequence.Count - 1)
            {
                if (EncounterSequence[i].safeFloor)
                {
                    floorDisplay.Floors[i - CurrentFloor].IsSafe = 0;
                }
                else
                {
                    floorDisplay.Floors[i - CurrentFloor].IsSafe = 1;
                }
            }
            else
            {
                floorDisplay.Floors[i - CurrentFloor].IsSafe = 2;
            }
        }

        // 1. Move the elevator visual/environment
        // DoorScript?.StartTravelAnimation(); 

        // Use a timer to simulate the travel time and door opening
        // In a real game, you would call this after the travel animation finishes.
    }

    private void OpenDoor()
    {
        Actor.Scene.FindScript<ElevatorDoor>().OnOpenClose();
        IsEncounterActive = true;

        // Start the encounter for the current floor
        StartEncounter(CurrentFloor - 1); // Sequence index is 0-based
    }

    private void StartEncounter(int sequenceIndex)
    {
        // Get the ID for the current floor
        string npcIdToSpawn = EncounterSequence[sequenceIndex].encounterName;

        // 2. Spawn the NPC/Enemy
        if (_npcManager != null)
        {
            _npcManager.SpawnNpc(npcIdToSpawn);
        }
        else
        {
            Debug.LogError("ExternalNPCManager is missing. Cannot spawn NPC.");
        }

        Debug.Log($"Encounter started: Spawned NPC ID: {npcIdToSpawn}");

        // 3. Start the timer for interaction/reaction
        EndCurrentEncounter();
        // NOTE: You would typically cancel this timer if the player starts dialogue/combat!
    }

    /// <summary>
    /// Called when the player has finished their interaction (e.g., pressed a "Next Floor" button, 
    /// defeated the enemy, or added the NPC to the team).
    /// </summary>
    [DebugCommand]
    public void EndCurrentEncounter()
    {
        if (!IsEncounterActive)
        {
            Debug.LogWarning("Cannot end encounter, no encounter is currently active.");
            return;
        }

        IsEncounterActive = false;

        //// Clear out the current NPC for the next floor
        //if (_npcSystem.Npcs.Count > 0)
        //{
        //    // You only need to remove the newly spawned NPC, which should be the last one added.
        //    // If the player successfully recruited, they remain.
        //    // For now, let's assume we remove the target NPC from the encounter list
        //    _npcManager.RemoveNpc(_npcSystem.Npcs.Count - 1);
        //}

        // Close the door visuals
        // DoorScript?.CloseDoors();

        cb.unlocked = true;

        // Move on to the next floor
    }

    public override void OnUpdate()
    {
        if(traveling)
        {
            timer1 += Time.DeltaTime;

            if(timer1 > 6.5f)
            {
                timer1 = 0;
                traveling = false;
                OpenDoor();
            }
        }
    }
}