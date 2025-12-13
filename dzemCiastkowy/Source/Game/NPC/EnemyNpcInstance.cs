namespace Game.NPC;

/// <summary>Instance of an enemy npc.</summary>
/// <param name="data">Data of the npc.</param>
public class EnemeyNpcInstance(NpcData data)
{
    public NpcData Data { get; } = data;

    public int health = data.enemyHealth;
}
