using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public class Tile
    {
        public TileInfo Data;
        public int ID;
        public Tile North;
        public Tile East;
        public Tile South;
        public Tile West;

        public string Name { get { return Data.Name; } }
        public string GeoName { get { return Data.GeoName; } }
        public string Description { get { return Data.Description; } }
        public List<Entity> Entities { get { return Data.Entities; } }
        public List<Monster> Monsters { get { return Data.Monsters; } }
        public Func<GameState, Player, bool> OnEnter { get { return Data.OnEnter; } }

        public Tile(TileInfo data, int id)
        {
            Data = data;
            ID = id;
        }
    }
}
