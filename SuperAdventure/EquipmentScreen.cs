using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;

namespace SuperAdventure
{
    public partial class EquipmentScreen : Form
    {
        private GameState _state;
        private Player _player;

        public EquipmentScreen(GameState state)
        {
            _state = state;
            _player = state.Player;
            InitializeComponent();

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 120,
                DataPropertyName = "Name"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Lvl",
                Width = 30,
                DataPropertyName = "LevelRequirement"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Type",
                Width = 65,
                DataPropertyName = "TypeName"
            });
            dgvInventory.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Equip",
                UseColumnTextForButtonValue = true,
                Width = 45,
                DataPropertyName = "ID"
            });

            dgvInventory.DataSource = _player.Equippable;
            dgvInventory.CellClick += dgvInventory_CellClick;

            dgvEquipment.RowHeadersVisible = false;
            dgvEquipment.AutoGenerateColumns = false;

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 120,
                DataPropertyName = "Name"
            });
            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Lvl",
                Width = 30,
                DataPropertyName = "LevelRequirement"
            });
            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Type",
                Width = 65,
                DataPropertyName = "TypeName"
            });
            dgvEquipment.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Remove",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ID"
            });

            dgvEquipment.DataSource = _player.Equipment;
            dgvEquipment.CellClick += dgvEquipment_CellClick;
        }

        private void dgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // The "Equip" button
            if (e.ColumnIndex == 4)
            {
                var itemID = dgvInventory.Rows[e.RowIndex].Cells[0].Value;
                if (_player.EquipItem(Convert.ToInt32(itemID)))
                    dgvInventory.DataSource = _player.Equippable;
            }

        }

        private void dgvEquipment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // The "Remove" button
            if (e.ColumnIndex == 4)
            {
                var itemID = dgvEquipment.Rows[e.RowIndex].Cells[0].Value;
                if (_player.UnequipItem(Convert.ToInt32(itemID)))
                    dgvInventory.DataSource = _player.Equippable;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
