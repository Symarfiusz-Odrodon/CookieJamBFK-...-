using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace Game.NPC;

/// <summary>Class containing data for all npcs. READ ONLY!!!</summary>
public class NpcDatabase
{
    public List<JsonAssetReference<NpcData>> npcs = [];

    public NpcData GetNpcById(string id) =>
        npcs.Where(x => x.Instance?.id == id)
            .Select(x => x.Instance)
            .FirstOrDefault();
}
