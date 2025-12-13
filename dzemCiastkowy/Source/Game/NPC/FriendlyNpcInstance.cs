namespace Game.NPC;

/// <summary>Instance of a friendly npc.</summary>
/// <param name="data">Data of the npc.</param>
public class FriendlyNpcInstance(NpcData data)
{
    public NpcData Data { get; } = data;

    public int maxHealth = data.friendMaxHealth;
    public int health = data.friendHealth;
    public int actionPoints = 0;
}
