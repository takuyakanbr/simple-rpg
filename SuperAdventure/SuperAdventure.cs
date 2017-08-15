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

        public DialogScreen DialogScreen;

        public SuperAdventure()
        {
            InitializeComponent();

            _state = new GameState();
            _state.LoadProfile();
            _player = _state.Player;

            // Data-bindings for player stats
            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");

            // Data-bindings for player skills
            dgvSkills.RowHeadersVisible = false;
            dgvSkills.AutoGenerateColumns = false;
            dgvSkills.DataSource = _player.Skills;
            dgvSkills.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvSkills.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 190,
                DataPropertyName = "Name"
            });
            dgvSkills.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Lvl",
                Width = 40,
                DataPropertyName = "Level"
            });
            dgvSkills.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "XP",
                Width = 50,
                DataPropertyName = "Experience"
            });

            // Data-bindings for player inventory
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.DataSource = _player.Inventory;
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 200,
                DataPropertyName = "Name"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                Width = 80,
                DataPropertyName = "Quantity"
            });
            dgvInventory.CellClick += dgvInventory_CellClick;

            // Data-bindings for player quests
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.DataSource = _player.Quests;
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 200,
                DataPropertyName = "Name"
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                Width = 80,
                DataPropertyName = "IsComplete"
            });
            dgvQuests.CellClick += dgvQuests_CellClick;

            // Data-bindings for comboboxes
            cboConsumable.DataSource = _player.Consumables;
            cboConsumable.DisplayMember = "Name";
            cboConsumable.ValueMember = "ID";
            cboEntities.SelectedValueChanged += cboEntities_ValueChanged;
            cboEntities.DataSource = _state.EntitiesOnTile;
            cboEntities.DisplayMember = "Name";
            cboEntities.ValueMember = "ID";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _state.PropertyChanged += GameStateOnPropertyChanged;
            _state.OnMessage += DisplayMessage;
            _state.OnDialogEvent += UpdateDialog;

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

        private void UpdateDialog(object sender, DialogEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case DialogEventType.Update:
                    if (DialogScreen == null)
                    {
                        DialogScreen = new DialogScreen(this, _state);
                        DialogScreen.StartPosition = FormStartPosition.CenterParent;

                        // show dialog in a non-blocking way
                        BeginInvoke(new Action(() => DialogScreen.ShowDialog(this)));
                    }
                    DialogScreen.UpdateDialog(eventArgs.Dialog, eventArgs.Options);
                    break;
                case DialogEventType.Close:
                    DialogScreen.Close();
                    DialogScreen = null;
                    break;
            }
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
                // Show/hide movement buttons
                btnNorth.Visible = (_player.CurrentTile.North != null);
                btnEast.Visible = (_player.CurrentTile.East != null);
                btnSouth.Visible = (_player.CurrentTile.South != null);
                btnWest.Visible = (_player.CurrentTile.West != null);
                
                // Show/hide interaction combobox + button
                bool hasEntities = _state.EntitiesOnTile.Count() > 0;
                cboEntities.Visible = hasEntities;
                btnInteract.Visible = hasEntities;

                // Display current location name and description
                rtbLocation.Text = _player.CurrentTile.Name + Environment.NewLine + Environment.NewLine;
                rtbLocation.Text += _player.CurrentTile.Description + Environment.NewLine;
            }
        }
        
        private void cboEntities_ValueChanged(object sender, EventArgs e)
        {
            var entity = (Entity)cboEntities.SelectedItem;
            if (entity != null)
            {
                rtbDescription.Text = entity.Name + ": " + entity.Description;
            }
        }

        private void dgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var itemID = dgvInventory.Rows[e.RowIndex].Cells[0].Value;
            var item = World.GetItem(Convert.ToInt32(itemID));
            rtbDescription.Text = item.Name + ": " + item.Description;
        }

        private void dgvQuests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var questID = dgvQuests.Rows[e.RowIndex].Cells[0].Value;
            var quest = World.GetQuest(Convert.ToInt32(questID));
            rtbDescription.Text = quest.Name + ": " + quest.Description;
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
            // open the equipment screen
            EquipmentScreen screen = new EquipmentScreen(_state);
            screen.ShowDialog(this);
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
            _state.BeginInteraction((Entity)cboEntities.SelectedItem);
        }
    }
}