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
    private Random rand = new Random();

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
                    if (NPC.NPC_Action1[i].Get<Button>().IsPressed || NPC.NPC_Action2[i].Get<Button>().IsPressed)
                    {
                        int actionId = 0;
                        if (NPC.NPC_Action1[i].Get<Button>().IsPressed)
                            actionId = NPC.Npcs[i].Data.friendAction1Id;
                        if (NPC.NPC_Action2[i].Get<Button>().IsPressed)
                            actionId = NPC.Npcs[i].Data.friendAction2Id;

                        int target = rand.Next(0, NPC.Enemies.Count + 1);
                        switch (actionId)
                        {
                            case 0:
                                foreach(EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.health -= 10;
                                    if(enemy.health <= 0)
                                        enemy.health = 0;
                                }
                                break;
                            case 1:
                                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                                {
                                    ally.health += 20;
                                    if (ally.health >= ally.maxHealth)
                                        ally.health = ally.maxHealth;
                                }
                                break;
                            case 2:
                                foreach(EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.health -= 5;
                                    if (enemy.health <= 0)
                                        enemy.health = 0;
                                }
                                break;
                            case 3:
                                NPC.Enemies[target].health -= 15;
                                if (NPC.Enemies[target].health  <= 0)
                                    NPC.Enemies[target].health = 0;
                                break;
                            case 4:
                                foreach (EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.health -= 8;
                                    if (enemy.health <= 0)
                                        enemy.health = 0;
                                }
                                break;
                            case 5:
                                NPC.Enemies[target].health -= 20;
                                if (NPC.Enemies[target].health <= 0)
                                    NPC.Enemies[target].health = 0;
                                break;


                        }
                        NPC.Npcs[i].actionPoints = 0.0f;
                    }
                }
            }
            for (int i = 0; i < NPC.Enemies.Count;  i++)
            {
                if (NPC.Enemies[i].actionPoints >= 1.0f)
                {
                    int actionId = 0;
                    int choice = rand.Next(0, 2);
                    if (choice == 0)
                        actionId = NPC.Enemies[i].Data.enemyAction1Id;
                    if (choice == 1)
                        actionId = NPC.Enemies[i].Data.enemyAction2Id;
                    switch (NPC.Enemies[i].Data.enemyAction1Id)
                    {
                        case 0:
                            int hitAlly = rand.Next(0, NPC.Npcs.Count);
                            NPC.Npcs[hitAlly].health -= 5;
                            if (NPC.Npcs[hitAlly].health <= 0)
                                NPC.Npcs[hitAlly].health = 0;
                            break;
                        case 1:
                            foreach (FriendlyNpcInstance ally in NPC.Npcs)
                            { 
                                ally.actionPoints -= 0.4f;
                                if (ally.actionPoints <= 0)
                                    ally.actionPoints = 0;
                            }
                                
                            break;
                            
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
