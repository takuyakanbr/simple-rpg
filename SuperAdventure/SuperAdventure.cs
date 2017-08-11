using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;
using Engine.Data;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private GameState _state;
        private Player _player;

        public SuperAdventure()
        {
            InitializeComponent();

            _state = new GameState();
            _state.LoadProfile();
            _player = _state.Player;

            // Data-bindings for player stats
            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");

            // Data-bindings for player inventory
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.DataSource = _player.Inventory;
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            // Data-bindings for player quests
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.DataSource = _player.Quests;
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "IsComplete"
            });

            // Data-bindings for comboboxes
            cboConsumable.DataSource = _player.Consumables;
            cboConsumable.DisplayMember = "Name";
            cboConsumable.ValueMember = "Id";
            cboEntities.DataSource = _state.EntitiesOnTile;
            cboEntities.DisplayMember = "Name";
            cboEntities.ValueMember = "Id";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _state.PropertyChanged += GameStateOnPropertyChanged;
            _state.OnMessage += DisplayMessage;

            _player.RecalculateStats();
            _state.MoveTo(_player.CurrentTile);
        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.Text += messageEventArgs.Message + Environment.NewLine;

            if (messageEventArgs.AddExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void GameStateOnPropertyChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CurrentMonster")
            {
                btnAttack.Visible = _state.CurrentMonster != null;
                if (_state.CurrentMonster == null)
                {
                    cboConsumable.Visible = false;
                    btnUseConsumable.Visible = false;
                }
                else
                {
                    cboConsumable.Visible = _player.Consumables.Any();
                    btnUseConsumable.Visible = _player.Consumables.Any();
                }
            }
        }

        private void PlayerOnPropertyChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Consumables")
            {
                cboConsumable.DataSource = _player.Consumables;
                if (!_player.Consumables.Any())
                {
                    cboConsumable.Visible = false;
                    btnUseConsumable.Visible = false;
                }
            }
            else if (propertyChangedEventArgs.PropertyName == "CurrentTile")
            {
                // Show/hide available movement buttons
                btnNorth.Visible = (_player.CurrentTile.North != null);
                btnEast.Visible = (_player.CurrentTile.East != null);
                btnSouth.Visible = (_player.CurrentTile.South != null);
                btnWest.Visible = (_player.CurrentTile.West != null);

                bool hasEntities = _state.EntitiesOnTile.Count() > 0;
                cboEntities.Visible = hasEntities;
                btnInteract.Visible = hasEntities;

                // Display current location name and description
                rtbLocation.Text = _player.CurrentTile.Name + Environment.NewLine + Environment.NewLine;
                rtbLocation.Text += _player.CurrentTile.Description + Environment.NewLine;
            }
        }
        
        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            _state.SaveProfile();
        }
        
        private void btnNorth_Click(object sender, EventArgs e)
        {
            _state.MoveNorth();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _state.MoveEast();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _state.MoveSouth();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            _state.MoveWest();
        }

        private void btnEquipment_Click(object sender, EventArgs e)
        {
            _state.OpenEquipment();
        }

        private void btnAttack_Click(object sender, EventArgs e)
        {
            _state.Attack();
        }

        private void btnUseConsumable_Click(object sender, EventArgs e)
        {
            _state.UseConsumable((ItemConsumable)cboConsumable.SelectedItem);
        }

        private void btnInteract_Click(object sender, EventArgs e)
        {
            _state.Interact((Entity)cboEntities.SelectedItem);
        }
    }
}