using System;
using System.Collections.Generic;
using FlaxEngine;

namespace dzemCiastkowy;

/// <summary>
/// Controls the materials of floor objects based on a boolean state.
/// </summary>
public class FloorDisplay : Script
{
    // 1. Define the materials to swap between
    [Header("Settings")]
    [Tooltip("Material used when the floor is active/selected.")]
    public MaterialBase ActiveMaterial;

    [Tooltip("Material used when the floor is inactive.")]
    public MaterialBase InactiveMaterial;

    // 2. Define a simple container for our Floor data
    // We use Serializable so it shows up in the Flax Editor
    [Serializable]
    public struct FloorData
    {
        [Tooltip("The reference to the floor mesh object.")]
        public StaticModel FloorObject;

        [Tooltip("Check this to switch the material to Active.")]
        public bool IsActive;
    }

    // 3. Create the list to hold the 5 objects
    [Header("Floors")]
    [Tooltip("Add your 5 floor objects here.")]
    public List<FloorData> Floors = new List<FloorData>();

    /// <inheritdoc/>
    public override void OnStart()
    {
        // Initial visual update
        UpdateVisuals();
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        // We call this in Update so you can toggle the bools in the 
        // Editor Inspector and see the materials change in real-time.
        UpdateVisuals();
    }

    /// <summary>
    /// Iterates through all floors and updates their materials based on the IsActive bool.
    /// </summary>
    private void UpdateVisuals()
    {
        // Safety check to ensure materials are assigned
        if (ActiveMaterial == null || InactiveMaterial == null) return;

        for (int i = 0; i < Floors.Count; i++)
        {
            var floor = Floors[i];

            // Skip if the object reference is empty
            if (floor.FloorObject == null) continue;

            // Determine which material to use
            MaterialBase targetMaterial = floor.IsActive ? ActiveMaterial : InactiveMaterial;

            // Apply the material to slot 0 (the first material slot)
            // We check if it's already set to avoid unnecessary processing
            if (floor.FloorObject.GetMaterial(0) != targetMaterial)
            {
                floor.FloorObject.SetMaterial(0, targetMaterial);
            }
        }
    }

    // --- External Helper Methods ---

    /// <summary>
    /// External method to change a specific floor's state via code.
    /// Example usage: myFloorDisplay.SetFloorState(2, true);
    /// </summary>
    public void SetFloorState(int index, bool state)
    {
        if (index >= 0 && index < Floors.Count)
        {
            // We have to copy the struct, modify it, and put it back
            // because structs are value types in C#.
            var data = Floors[index];
            data.IsActive = state;
            Floors[index] = data;
        }
    }
}