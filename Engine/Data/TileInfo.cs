using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    // contains data about a tile
    public class TileInfo
    {
        public int ID;
        public string Name;
        public string GeoName;
        public string Description;
        public List<Entity> Entities;
        public List<MonsterSpawn> MonsterSpawns;

        // run when player attempts to enter this tile;
        // player will not be allowed into this tile if this returns false
        public Func<GameState, Player, bool> OnEnter;
        
        public TileInfo(int id, string name, string geoName, string description)
        {
            ID = id;
            Name = name;
            GeoName = geoName;
            Description = description;
            Entities = new List<Entity>();
            MonsterSpawns = new List<MonsterSpawn>();
        }

        public TileInfo AddEntity(int entityID)
        {
            Entities.Add(World.GetEntity(entityID));
            return this;
        }

        public TileInfo AddMonsterSpawn(int monsterID, double spawnChance, bool canAvoid = true, bool initiative = false)
        {
            MonsterSpawns.Add(new MonsterSpawn(World.GetMonster(monsterID), spawnChance, canAvoid, initiative));
            return this;
        }
    }
}
