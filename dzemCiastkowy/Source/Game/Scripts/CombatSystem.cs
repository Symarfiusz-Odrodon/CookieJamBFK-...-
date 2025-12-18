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
    public SceneReference gameScene;
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
                            case 6:
                                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                                {
                                    ally.actionPoints += 0.2f;
                                    if (ally.actionPoints >= 1.0f)
                                        ally.actionPoints = 1.0f;
                                }
                                break;
                            case 7:
                                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                                {
                                    ally.health += 15;
                                    if (ally.health >= ally.maxHealth)
                                        ally.health = ally.maxHealth;
                                }
                                break;
                            case 8:
                                foreach (EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.actionPoints -= 0.35f;
                                    if (enemy.actionPoints <= 0)
                                        enemy.actionPoints = 0;
                                }
                                break;
                            case 9:
                                foreach (FriendlyNpcInstance ally in NPC.Npcs)
                                {
                                    ally.actionPoints += 0.5f;
                                    if (ally.actionPoints >= 1.0f)
                                        ally.actionPoints = 1.0f;
                                }
                                break;
                            case 10:
                                NPC.Enemies[target].health -= 50;
                                if (NPC.Enemies[target].health <= 0)
                                    NPC.Enemies[target].health = 0;
                                break;
                            case 11:
                                NPC.Npcs[i].health += 50;
                                if (NPC.Npcs[i].health >= NPC.Npcs[i].maxHealth)
                                    NPC.Npcs[i].health = NPC.Npcs[i].maxHealth;
                                break;
                            case 12:
                                NPC.Npcs[i].health -= 20;
                                foreach (EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.health -= 50;
                                    if (enemy.health <= 0)
                                        enemy.health = 0;
                                }
                                if (NPC.Npcs[i].health < 0)
                                    NPC.Npcs[i].health = 0;
                                break;
                            case 13:
                                NPC.Enemies[target].health -= 100;
                                if (NPC.Enemies[target].health <= 0)
                                    NPC.Enemies[target].health = 0;
                                break;
                            case 14:
                                foreach (EnemyNpcInstance enemy in NPC.Enemies)
                                {
                                    enemy.health -= 50;
                                    if (enemy.health <= 0)
                                        enemy.health = 0;
                                }
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
                    int hitAlly = rand.Next(0, NPC.Npcs.Count);
                    int actionId = 0;
                    int choice = rand.Next(0, 2);
                    int minHealth = 10000;
                    int maxHealth = 0;
                    int index = 0;
                    if (choice == 0)
                        actionId = NPC.Enemies[i].Data.enemyAction1Id;
                    if (choice == 1)
                        actionId = NPC.Enemies[i].Data.enemyAction2Id;
                    switch (NPC.Enemies[i].Data.enemyAction1Id)
                    {
                        case 0:
                            
                            NPC.Npcs[hitAlly].health -= 10;
                            if (NPC.Npcs[hitAlly].health <= 0)
                                NPC.Npcs[hitAlly].health = 0;
                            break;
                        case 1:
                            foreach (FriendlyNpcInstance ally in NPC.Npcs)
                            { 
                                ally.actionPoints -= 0.3f;
                                if (ally.actionPoints <= 0)
                                    ally.actionPoints = 0;
                            }
                            break;
                        case 2:
                            foreach (EnemyNpcInstance enemy in NPC.Enemies)
                            {
                                enemy.health += 5 * NPC.Npcs.Count;
                                if (enemy.health <= 0)
                                    enemy.health = 0;
                            }
                            break;
                        case 3:
                            foreach (FriendlyNpcInstance ally in NPC.Npcs)
                            {
                                ally.health -= 15;
                                if (ally.health <= 0)
                                    ally.health = 0;
                            }
                            break;
                        case 4:
                            for(int j = 0;  j < NPC.Npcs.Count; j++)
                            {
                                if(minHealth > NPC.Npcs[j].health)
                                {
                                    minHealth = NPC.Npcs[j].health;
                                    index = j;
                                }
                            }
                            if (NPC.Npcs[index] != null)
                            {
                                NPC.Npcs[index].health -= 10;
                                if(NPC.Npcs[index].health < 0)
                                    NPC.Npcs[index].health = 0;
                            }
                            
                            break;
                        case 5:
                            NPC.Npcs[hitAlly].health += 5;
                            NPC.Npcs[hitAlly].actionPoints = 0.0f;
                            if (NPC.Npcs[hitAlly].health >= NPC.Npcs[hitAlly].maxHealth)
                                NPC.Npcs[hitAlly].health = NPC.Npcs[hitAlly].maxHealth;
                            if (NPC.Npcs[hitAlly].actionPoints <= 0)
                                NPC.Npcs[hitAlly].actionPoints = 0;
                            break;
                        case 6:
                            for (int j = 0; j < NPC.Npcs.Count; j++)
                            {
                                if (maxHealth < NPC.Npcs[j].health)
                                {
                                    maxHealth = NPC.Npcs[j].health;
                                    index = j;
                                }
                            }
                            if (NPC.Npcs[index] != null)
                            {
                                NPC.Npcs[index].health -= 40;
                                if (NPC.Npcs[index].health < 0)
                                    NPC.Npcs[index].health = 0;
                            }
                            break;
                        case 7:
                            NPC.Npcs[hitAlly].actionPoints = 0.0f;
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
        if (index >= 0 && index < NPC.Npcs.Count && )
        {
            NPC.Npcs.RemoveAt(index);
            if(NPC.Npcs.Count == 0)
                Scripting.RunOnUpdate(() =>
                {
                    Level.UnloadAllScenes();
                    Level.LoadScene(gameScene);
                });

        }
    }

    void removeEnemy(int index)
    {
        if (index >= 0 && index < NPC.Enemies.Count)
        {
            NPC.Enemies.RemoveAt(index);
            if (NPC.Npcs.Count == 0)
                Scripting.RunOnUpdate(() =>
                {
                    Level.UnloadAllScenes();
                    Level.LoadScene(gameScene);
                });
        }
    }
}
