using FlaxEngine;

namespace Game.NPC;

/// <summary>Class containing npc data. Should be used as a json asset. READ ONLY!!</summary>
public class NpcData
{
    [Header("Metadata")]
    public string id;
    public string name;
    public Texture headTexture;
    public Texture dialogueTexture;
    public Texture worldTexture;

    [Header("Friendly stats")]
    public int friendHealth;
    public int friendMaxHealth;
    public float friendActionRegen;
    public string friendAction1Name;
    public string friendAction2Name;
    public int friendAction1Id;
    public int friendAction2Id;

    [Header("Enemy stats")]
    public int enemyHealth;
    public int enemyMaxHealth;
    public float enemyActionRegen;
    public string enemyAction1Name;
    public string enemyAction2Name;
    public int enemyAction1Id;
    public int enemyAction2Id;
}
