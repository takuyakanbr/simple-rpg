namespace SuperAdventure
{
    partial class EquipmentScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvEquipment = new System.Windows.Forms.DataGridView();
            this.dgvInventory = new System.Windows.Forms.DataGridView();
            this.lblVendorInventory = new System.Windows.Forms.Label();
            this.lblMyInventory = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(536, 274);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvEquipment
            // 
            this.dgvEquipment.AllowUserToAddRows = false;
            this.dgvEquipment.AllowUserToDeleteRows = false;
            this.dgvEquipment.AllowUserToResizeColumns = false;
            this.dgvEquipment.AllowUserToResizeRows = false;
            this.dgvEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipment.Location = new System.Drawing.Point(316, 44);
            this.dgvEquipment.Name = "dgvEquipment";
            this.dgvEquipment.ReadOnly = true;
            this.dgvEquipment.Size = new System.Drawing.Size(295, 216);
            this.dgvEquipment.TabIndex = 8;
            // 
            // dgvInventory
            // 
            this.dgvInventory.AllowUserToAddRows = false;
            this.dgvInventory.AllowUserToDeleteRows = false;
            this.dgvInventory.AllowUserToResizeColumns = false;
            this.dgvInventory.AllowUserToResizeRows = false;
            this.dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventory.Location = new System.Drawing.Point(12, 44);
            this.dgvInventory.Name = "dgvInventory";
            this.dgvInventory.ReadOnly = true;
            this.dgvInventory.Size = new System.Drawing.Size(290, 216);
            this.dgvInventory.TabIndex = 7;
            // 
            // lblVendorInventory
            // 
            this.lblVendorInventory.AutoSize = true;
            this.lblVendorInventory.Location = new System.Drawing.Point(444, 14);
            this.lblVendorInventory.Name = "lblVendorInventory";
            this.lblVendorInventory.Size = new System.Drawing.Size(52, 13);
            this.lblVendorInventory.TabIndex = 6;
            this.lblVendorInventory.Text = "Equipped";
            // 
            // lblMyInventory
            // 
            this.lblMyInventory.AutoSize = true;
            this.lblMyInventory.Location = new System.Drawing.Point(128, 14);
            this.lblMyInventory.Name = "lblMyInventory";
            this.lblMyInventory.Size = new System.Drawing.Size(51, 13);
            this.lblMyInventory.TabIndex = 5;
            this.lblMyInventory.Text = "Inventory";
            // 
            // EquipmentScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 309);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvEquipment);
            this.Controls.Add(this.dgvInventory);
            this.Controls.Add(this.lblVendorInventory);
            this.Controls.Add(this.lblMyInventory);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EquipmentScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Equipment";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvEquipment;
        private System.Windows.Forms.DataGridView dgvInventory;
        private System.Windows.Forms.Label lblVendorInventory;
        private System.Windows.Forms.Label lblMyInventory;
    }
}