using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.NPC
{
    /// <summary>
    /// Manages the data layer for NPCs, allowing external scripts to add/modify NPCs
    /// which the NPC_System then visualizes.
    /// </summary>
    public class ExternalNPCManager : Script
    {
        [Tooltip("Reference to the NPC_System script that handles the UI.")]
        public NPC_System NpcSystem;

        /// <summary>
        /// Adds a new NPC to the first available slot in the system.
        /// </summary>
        /// <param name="name">Name of the NPC.</param>
        /// <param name="hp">Current Health.</param>
        /// <param name="maxHp">Max Health.</param>
        /// <param name="ap">Action Points.</param>
        /// <param name="portrait">Character Texture.</param>
        /// <returns>True if added successfully, False if the party is full.</returns>
        public bool AddNpc(string name, int hp, int maxHp, float ap, Texture portrait)
        {
            if (NpcSystem == null)
            {
                Debug.LogError("ExternalNPCManager: NpcSystem reference is missing!");
                return false;
            }

            // Find the first empty slot in the NPC_System array
            for (int i = 0; i < NpcSystem.NPCs.Length; i++)
            {
                if (NpcSystem.NPCs[i] == null)
                {
                    // Create the new data object
                    NPC_Class newNpc = new NPC_Class
                    {
                        charName = name,
                        healthPoints = hp,
                        maxHealth = maxHp,
                        actionPoints = ap,
                        charTexture = portrait,
                        charID = new Random().Next(1000, 9999) // Simple random ID
                    };

                    // Assign to system
                    NpcSystem.NPCs[i] = newNpc;
                    Debug.Log($"Added NPC '{name}' to slot {i}");
                    return true;
                }
            }

            Debug.LogWarning("Cannot add NPC: Party is full (Max 3).");
            return false;
        }

        /// <summary>
        /// Modifies the Health of a specific NPC slot.
        /// </summary>
        public void ModifyHealth(int slotIndex, int amount)
        {
            if (IsValidSlot(slotIndex))
            {
                var npc = NpcSystem.NPCs[slotIndex];

                // Modify and Clamp
                npc.healthPoints += amount;
                npc.healthPoints = Mathf.Clamp(npc.healthPoints, 0, npc.maxHealth);
            }
        }

        /// <summary>
        /// Sets the Action Points for a specific NPC slot.
        /// </summary>
        public void SetActionPoints(int slotIndex, float newAp)
        {
            if (IsValidSlot(slotIndex))
            {
                // You might want to clamp this based on a MaxAP if you have one
                NpcSystem.NPCs[slotIndex].actionPoints = newAp;
            }
        }

        /// <summary>
        /// Removes an NPC from the system completely.
        /// </summary>
        public void RemoveNpc(int slotIndex)
        {
            if (IsValidSlot(slotIndex))
            {
                Debug.Log($"Removing NPC '{NpcSystem.NPCs[slotIndex].charName}' from slot {slotIndex}");
                NpcSystem.NPCs[slotIndex] = null;
            }
        }

        // Helper to check if a slot has a valid NPC
        private bool IsValidSlot(int index)
        {
            if (NpcSystem == null) return false;
            if (index < 0 || index >= NpcSystem.NPCs.Length) return false;
            if (NpcSystem.NPCs[index] == null) return false;

            return true;
        }

        // --- TESTING AREA (Optional) ---
        // You can remove this OnUpdate later, it's just for testing keys
        public override void OnUpdate()
        {
            // Press '1' to add a dummy NPC
            if (Input.GetKeyDown(KeyboardKeys.Alpha1))
            {
                // Passing 'null' for texture for now, drag a texture here if you want to test visuals
                AddNpc("Warrior", 100, 100, 5.0f, null);
            }

            // Press 'Space' to damage the first NPC
            if (Input.GetKeyDown(KeyboardKeys.Spacebar))
            {
                ModifyHealth(0, -10); // Deal 10 damage to slot 0
            }
        }
    }
}