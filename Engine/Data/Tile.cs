using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public class Tile
    {
        public int ID;
        public TileInfo Data;
        public Tile North;
        public Tile East;
        public Tile South;
        public Tile West;

        public string Name { get { return Data.Name; } }
        public string GeoName { get { return Data.GeoName; } }
        public string Description { get { return Data.Description; } }
        public List<Entity> Entities { get { return Data.Entities; } }
        public List<MonsterSpawn> MonsterSpawns { get { return Data.MonsterSpawns; } }
        public Func<GameState, Player, bool> OnEnter { get { return Data.OnEnter; } }

        public Tile(int id, TileInfo data)
        {
            ID = id;
            Data = data;
        }

        public Tile Connect(Tile northTile, Tile eastTile = null, Tile southTile = null, Tile westTile = null)
        {
            if (northTile != null) ConnectNorth(northTile);
            if (eastTile != null) ConnectEast(eastTile);
            if (southTile != null) ConnectSouth(southTile);
            if (westTile != null) ConnectWest(westTile);
            return this;
        }

        public Tile ConnectNorth(Tile northTile)
        {
            North = northTile;
            northTile.South = this;
            return this;
        }

        public Tile ConnectEast(Tile eastTile)
        {
            East = eastTile;
            eastTile.West = this;
            return this;
        }

        public Tile ConnectSouth(Tile southTile)
        {
            South = southTile;
            southTile.North = this;
            return this;
        }

        public Tile ConnectWest(Tile westTile)
        {
            West = westTile;
            westTile.East = this;
            return this;
        }
    }
}
