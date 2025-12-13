using System.Collections.Generic;

namespace Game.NPC;

public class NpcDatabase
{
    public List<NPC_Class> npcs = [];

    public NPC_Class GetNpcById(string id) {
        return npcs[0];
    }
}
