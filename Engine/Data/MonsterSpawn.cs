using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
    public class MonsterSpawn
    {
        public Monster Data;
        public double SpawnChance;
        public bool CanAvoid;
        public bool HasInitiative;

        public MonsterSpawn(Monster data, double spawnChance, bool canAvoid = true, bool initiative = false)
        {
            Data = data;
            SpawnChance = spawnChance;
            CanAvoid = canAvoid;
            HasInitiative = initiative;
        }
    }
}
