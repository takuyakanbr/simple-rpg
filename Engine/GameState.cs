using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine
{
    public class GameState
    {
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        private Player _player;

        public Player Player { get; private set; }

        public GameState()
        {

        }

        public void LoadPlayer()
        {
            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                Player = Player.CreatePlayerFromXmlString(
                    File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                Player = Player.CreateDefaultPlayer();
            }
        }

        public void SavePlayer()
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
        }
    }
}
