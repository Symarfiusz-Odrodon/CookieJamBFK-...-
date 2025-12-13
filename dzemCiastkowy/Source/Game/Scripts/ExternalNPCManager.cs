using System;
using FlaxEngine;

namespace Game.NPC;

/// <summary>
/// script to externally manage NPC states and spawning.
/// </summary>
public class ExternalNPCManager : Script
{
    // ========================================================================
    // EDITOR DEBUGGING / TESTING SECTION
    // ========================================================================
    [Header("Debug Controls")]
    [Tooltip("The ID of the NPC to spawn when triggering 'SpawnNpc'.")]
    public string DebugNpcId = "warrior";

    [Tooltip("The index in the Npcs list (0, 1, or 2) to target for stat changes.")]
    public int DebugTargetIndex = 0;

    [Tooltip("Amount to change Health or AP by when triggering the bools below.")]
    public float DebugAmount = 0.1f;
    public float DebugAmount2 = 10f;

    // --- Triggers ---

    private bool _triggerSpawn;
    [ShowInEditor, Tooltip("Click to spawn an NPC with the ID specified above.")]
    public bool TriggerSpawn
    {
        get => _triggerSpawn;
        set { _triggerSpawn = false; if (value) SpawnNpc(DebugNpcId); }
    }

    private bool _triggerDamage;
    [ShowInEditor, Tooltip("Click to damage the target NPC.")]
    public bool TriggerDamage
    {
        get => _triggerDamage;
        set { _triggerDamage = false; if (value) ModifyNpcHealth(DebugTargetIndex, -DebugAmount2    ); }
    }

    private bool _triggerHeal;
    [ShowInEditor, Tooltip("Click to heal the target NPC.")]
    public bool TriggerHeal
    {
        get => _triggerHeal;
        set { _triggerHeal = false; if (value) ModifyNpcHealth(DebugTargetIndex, DebugAmount2); }
    }

    private bool _triggerAddAP;
    [ShowInEditor, Tooltip("Click to add Action Points to the target NPC.")]
    public bool TriggerAddAP
    {
        get => _triggerAddAP;
        set { _triggerAddAP = false; if (value) ModifyNpcAP(DebugTargetIndex, DebugAmount); }
    }

    // ========================================================================
    // PUBLIC API (Call these from other scripts)
    // ========================================================================

    /// <summary>
    /// Spawns a new NPC by ID using the main system.
    /// </summary>
    public void SpawnNpc(string npcId)
    {
        if (NPC_System.Instance == null)
        {
            Debug.LogError("NPC_System Instance is missing!");
            return;
        }

        Debug.Log($"External Manager: Requesting Spawn of '{npcId}'");
        NPC_System.Instance.AddFriendNpc(npcId);
    }

    /// <summary>
    /// Modifies the health of a specific NPC. Handles clamping between 0 and MaxHealth.
    /// </summary>
    /// <param name="index">Index in the NPC list (0-2).</param>
    /// <param name="amount">Positive to heal, negative to damage.</param>
    public void ModifyNpcHealth(int index, float amount)
    {
        var npc = GetNpcSafe(index);
        if (npc == null) return;

        // Modify value
        npc.health += (int)amount;

        // Clamp to valid range
        npc.health = Mathf.Clamp(npc.health, 0, npc.maxHealth);

        Debug.Log($"NPC {index} Health is now: {npc.health}/{npc.maxHealth}");
    }

    /// <summary>
    /// Modifies the Action Points of a specific NPC.
    /// </summary>
    /// <param name="index">Index in the NPC list (0-2).</param>
    /// <param name="amount">Amount to add or subtract.</param>
    public void ModifyNpcAP(int index, float amount)
    {
        var npc = GetNpcSafe(index);
        if (npc == null) return;

        npc.actionPoints += amount;

        // Optional: Prevent negative AP
        if (npc.actionPoints < 0) npc.actionPoints = 0;

        Debug.Log($"NPC {index} AP is now: {npc.actionPoints}");
    }

    /// <summary>
    /// Removes an NPC from the system.
    /// </summary>
    public void RemoveNpc(int index)
    {
        if (NPC_System.Instance == null) return;

        if (index >= 0 && index < NPC_System.Instance.Npcs.Count)
        {
            NPC_System.Instance.Npcs.RemoveAt(index);
            Debug.Log($"Removed NPC at index {index}");
        }
    }

    // ========================================================================
    // HELPERS
    // ========================================================================

    private FriendlyNpcInstance GetNpcSafe(int index)
    {
        if (NPC_System.Instance == null)
        {
            Debug.LogError("NPC_System Instance is null.");
            return null;
        }

        if (index < 0 || index >= NPC_System.Instance.Npcs.Count)
        {
            Debug.LogWarning($"Invalid NPC Index: {index}. Current Count: {NPC_System.Instance.Npcs.Count}");
            return null;
        }

        return NPC_System.Instance.Npcs[index];
    }
}