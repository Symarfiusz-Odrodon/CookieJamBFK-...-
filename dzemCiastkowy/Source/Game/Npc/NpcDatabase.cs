using System.Collections.Generic;

namespace Game.Npc;

public class NpcDatabase
{
    public List<NPCCLass> npcs = [];

    public NPCCLass GetNpcById(string id) {
        return npcs[0];
    }
}
