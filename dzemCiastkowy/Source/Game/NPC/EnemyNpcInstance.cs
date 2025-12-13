namespace Game.NPC;

/// <summary>Instance of an enemy npc.</summary>
/// <param name="data">Data of the npc.</param>
public class EnemyNpcInstance(NpcData data)
{
    public NpcData Data { get; } = data;

    public int health = data.enemyHealth;
    public int maxHealth = data.enemyMaxHealth;
    public float actionRegen = data.enemyActionRegen;
    public float actionPoints = 0;
}
