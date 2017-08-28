using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Engine;
using Engine.Data;

namespace SuperAdventure
{
    public partial class GatheringScreen : Form
    {
        private GameState _state;
        private Player _player;
        private GatheringNode _node;
        private string _skillName;
        private int _count = 0;

        public GatheringScreen(GameState state)
        {
            _state = state;
            _player = state.Player;
            _node = (GatheringNode)state.CurrentEntity;
            InitializeComponent();

            // update UI
            _skillName = PlayerSkill.SKILL_NAMES[(int)_node.Skill].ToLower();
            lblAction.Text = "You are " + _skillName + "...";
            AddMessage("You start " + _skillName + " at the " + _node.Name + ".");

            // start timer
            timer1.Start();
        }

        private void AddMessage(string message)
        {
            rtbResults.Text += message + Environment.NewLine;
            rtbResults.SelectionStart = rtbResults.Text.Length;
            rtbResults.ScrollToCaret();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_count >= _node.GatherCounts)
            {
                timer1.Stop();
                AddMessage(Environment.NewLine + "The gathering spot has been depleted.");
                return;
            }

            _count++;
            _player.AddExperience(_node.Skill, _node.Experience);
            AddMessage("You gain " + _node.Experience + " " + _skillName + " experience");

            foreach (var lootItem in _node.LootTable)
            {
                if (RandomGenerator.NextDouble() <= lootItem.DropChance)
                {
                    int quantity = RandomGenerator.Next(lootItem.MinQuantity, lootItem.MaxQuantity);
                    _player.AddItemToInventory(lootItem.Data, quantity);
                    AddMessage("You acquire " + quantity + " " + lootItem.Data.Name);
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GatheringScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
            _node = null;
            _state = null;
        }
    }
}
