using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game.NPC;

/// <summary>
/// MyScript Script.
/// </summary>
public class CombatSystem : Script
{
    public bool inBattle = false;
    private float timer1 = 0;
    private NPC_System NPC;
    /// <inheritdoc/>
    public override void OnUpdate()
    {
        NPC = NPC_System.Instance;

        if(NPC == null )
            return;

        if (inBattle)
        {
            NPC.uiVisible = true;
            timer1 += Time.DeltaTime;

            if (timer1 > 1.0f)
            {

                timer1 = 0;
                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                {
                    ally.actionPoints += ally.actionRegen;
                    if (ally.actionPoints > 1)
                        ally.actionPoints = 1;
                }
                foreach (EnemyNpcInstance enemy in NPC.Enemies)
                {
                    enemy.actionPoints += enemy.actionRegen;
                    if (enemy.actionPoints > 1)
                        enemy.actionPoints = 1;
                }
            }

            for (int i = 0; i < NPC.Npcs.Count; i++)
            {
                if (NPC.Npcs[i].actionPoints >= 1.0f)
                {
                    if (NPC.NPC_Action1[i].Get<Button>().IsPressed)
                    {
                        Debug.Log($"Used action ID: 0");
                        switch (NPC.Npcs[i].Data.friendAction1Id)
                        {
                            case 0:

                                NPC.Npcs[i].actionPoints = 0;
                                break;
                        }

                    }
                }

            }
        }
        else
        {
            NPC.uiVisible = false;
        }
    }
}
