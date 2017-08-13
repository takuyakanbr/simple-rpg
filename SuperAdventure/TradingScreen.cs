using System;
using System.Windows.Forms;

using Engine;
using Engine.Data;

namespace SuperAdventure
{
    public partial class TradingScreen : Form
    {
        private Player _player;
        private Vendor _vendor;

        public TradingScreen(GameState state)
        {
            _player = state.Player;
            _vendor = state.CurrentVendor;
            InitializeComponent();

            // Style, to display numeric column values
            DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle();
            rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Populate the datagrid for the player's inventory
            dgvMyItems.RowHeadersVisible = false;
            dgvMyItems.AutoGenerateColumns = false;

            // This hidden column holds the item ID, so we know which item to sell
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Name"
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });
            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });
            dgvMyItems.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Sell 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ID"
            });

            // Bind the player's inventory to the datagridview 
            dgvMyItems.DataSource = _player.Inventory;

            // When the user clicks on a row, call this function
            dgvMyItems.CellClick += dgvMyItems_CellClick;


            // Populate the datagrid for the vendor's inventory
            dgvVendorItems.RowHeadersVisible = false;
            dgvVendorItems.AutoGenerateColumns = false;

            // This hidden column holds the item ID, so we know which item to sell
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Visible = false
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Name"
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });
            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "BuyPrice"
            });
            dgvVendorItems.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "Buy 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ID"
            });

            // Bind the vendor's inventory to the datagridview 
            dgvVendorItems.DataSource = state.CurrentVendor.Inventory;

            // When the user clicks on a row, call this function
            dgvVendorItems.CellClick += dgvVendorItems_CellClick;
        }

        private void dgvMyItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // The "Sell 1" button
            if (e.ColumnIndex == 4)
            {
                var itemID = dgvMyItems.Rows[e.RowIndex].Cells[0].Value;
                Item itemBeingSold = World.GetItem(Convert.ToInt32(itemID));

                if (itemBeingSold.Untradable)
                {
                    MessageBox.Show("You cannot sell the " + itemBeingSold.Name);
                }
                else
                {
                    _player.RemoveItemFromInventory(itemBeingSold.ID);
                    _player.Gold += itemBeingSold.Price;
                    _vendor.AddItemToInventory(itemBeingSold);
                }
            }
        }

        private void dgvVendorItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // The "Buy 1" button
            if (e.ColumnIndex == 4)
            {
                int itemID = Convert.ToInt32(dgvVendorItems.Rows[e.RowIndex].Cells[0].Value);
                Item itemBeingBought = World.GetItem(itemID);
                
                if (_player.Gold >= itemBeingBought.Price)
                {
                    _player.AddItemToInventory(itemBeingBought);
                    _player.Gold -= itemBeingBought.Price;
                    _vendor.RemoveItemFromInventory(itemID);
                }
                else
                {
                    MessageBox.Show("You do not have enough gold to buy the " + itemBeingBought.Name);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
