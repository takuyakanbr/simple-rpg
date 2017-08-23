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

        public Tile Connect(int northID, int eastID = -1, int southID = -1, int westID = -1)
        {
            if (northID != -1) ConnectNorth(northID);
            if (eastID != -1) ConnectEast(eastID);
            if (southID != -1) ConnectSouth(southID);
            if (westID != -1) ConnectWest(westID);
            return this;
        }

        public Tile ConnectNorth(int tileID)
        {
            Tile t = World.GetTile(tileID);
            North = t;
            t.South = this;
            return this;
        }

        public Tile ConnectEast(int tileID)
        {
            Tile t = World.GetTile(tileID);
            East = t;
            t.West = this;
            return this;
        }

        public Tile ConnectSouth(int tileID)
        {
            Tile t = World.GetTile(tileID);
            South = t;
            t.North = this;
            return this;
        }

        public Tile ConnectWest(int tileID)
        {
            Tile t = World.GetTile(tileID);
            West = t;
            t.East = this;
            return this;
        }
    }
}
