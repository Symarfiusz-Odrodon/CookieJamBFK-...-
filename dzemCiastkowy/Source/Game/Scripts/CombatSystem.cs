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
    public bool inCombat = false;
    private float timer1 = 0;
    private NPC_System NPC;
    /// <inheritdoc/>
    public override void OnUpdate()
    {
        NPC = NPC_System.Instance;

        if(NPC == null )
            return;

        for (int i = NPC.Npcs.Count - 1; i >= 0; i--)
        {
            if (NPC.Npcs[i] != null && NPC.Npcs[i].health == 0)
                removeAlly(i);
        }
        for (int i = NPC.Enemies.Count - 1; i >= 0; i--)
        {
            if (NPC.Enemies[i] != null && NPC.Enemies[i].health == 0)
                removeEnemy(i);
        }

        if (inCombat)
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
                        ally.actionPoints = 1.0f;
                }
                foreach (EnemyNpcInstance enemy in NPC.Enemies)
                {
                    enemy.actionPoints += enemy.actionRegen;
                    if (enemy.actionPoints > 1)
                        enemy.actionPoints = 1.0f;
                }
            }

            for (int i = 0; i < NPC.Npcs.Count; i++)
            {
                if (NPC.Npcs[i].actionPoints >= 1.0f)
                {
                    if (NPC.NPC_Action1[i].Get<Button>().IsPressed)
                    {
                        switch (NPC.Npcs[i].Data.friendAction1Id)
                        {
                            case 0:
                                foreach(EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.health -= 5;
                                    if(enemy.health <= 0)
                                        enemy.health = 0;
                                }
                                NPC.Npcs[i].actionPoints = 0.0f;
                                break;
                        }

                    }
                    if (NPC.NPC_Action2[i].Get<Button>().IsPressed)
                    {
                        switch (NPC.Npcs[i].Data.friendAction2Id)
                        {
                            case 1:
                                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                                {
                                    ally.health += 10;
                                    if (ally.health >= ally.maxHealth)
                                        ally.health = ally.maxHealth;
                                }
                                NPC.Npcs[i].actionPoints = 0.0f;
                                break;
                        }

                    }
                }
            }
            for (int i = 0; i < NPC.Enemies.Count;  i++)
            {
                if (NPC.Enemies[i].actionPoints >= 1.0f)
                {

                    Random rand = new Random();
                    int action = rand.Next(0, 2);
                    if(action == 1)
                    {
                        switch (NPC.Enemies[i].Data.enemyAction1Id)
                        {
                            case 0:
                                int hitAlly = rand.Next(0, NPC.Npcs.Count);
                                NPC.Npcs[hitAlly].health -= 5;
                                if (NPC.Npcs[hitAlly].health <= 0)
                                    NPC.Npcs[hitAlly].health = 0;
                                break;
                            
                        }
                    }
                    else
                    {
                        switch (NPC.Enemies[i].Data.enemyAction2Id)
                        {
                            case 1:
                                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                                { 
                                    ally.actionPoints -= 0.4f;
                                    if (ally.actionPoints <= 0)
                                        ally.actionPoints = 0;
                                }
                                
                                break;
                        }
                    }
                    NPC.Enemies[i].actionPoints = 0.0f;
                }
            }
        }
        else
        {
            NPC.uiVisible = false;
        }
    }

    void removeAlly(int index)
    {
        if (index >= 0 && index < NPC.Npcs.Count)
        {
            NPC.Npcs.RemoveAt(index);
        }
    }

    void removeEnemy(int index)
    {
        if (index >= 0 && index < NPC.Enemies.Count)
        {
            NPC.Enemies.RemoveAt(index);
        }
    }
}
