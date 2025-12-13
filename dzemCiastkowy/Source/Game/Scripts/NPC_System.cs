using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;



/// <summary>
/// NPC_System Script.
/// </summary>
namespace Game.NPC
{
    public class NPC_System : Script
    {
        public UIControl[] NPC_UIs = new UIControl[3];
        public NPC_Class[] NPCs = new NPC_Class[3];
        /// <inheritdoc/>
        public override void OnStart()
        {
            
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
            // Here you can add code that needs to be called every frame
        }
    }

};
