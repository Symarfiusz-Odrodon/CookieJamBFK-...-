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

    [Header("Enemy stats")]
    public int enemyHealth;
}
