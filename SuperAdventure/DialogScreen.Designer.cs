namespace SuperAdventure
{
    partial class DialogScreen
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
            this.rtbDialog = new System.Windows.Forms.RichTextBox();
            this.btnOption1 = new System.Windows.Forms.Button();
            this.btnOption2 = new System.Windows.Forms.Button();
            this.btnOption3 = new System.Windows.Forms.Button();
            this.btnOption4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbDialog
            // 
            this.rtbDialog.Location = new System.Drawing.Point(12, 12);
            this.rtbDialog.Name = "rtbDialog";
            this.rtbDialog.ReadOnly = true;
            this.rtbDialog.Size = new System.Drawing.Size(486, 104);
            this.rtbDialog.TabIndex = 0;
            this.rtbDialog.Text = "";
            // 
            // btnOption1
            // 
            this.btnOption1.Location = new System.Drawing.Point(12, 122);
            this.btnOption1.Name = "btnOption1";
            this.btnOption1.Size = new System.Drawing.Size(486, 23);
            this.btnOption1.TabIndex = 1;
            this.btnOption1.Text = "1. Nothing";
            this.btnOption1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOption1.UseVisualStyleBackColor = true;
            this.btnOption1.Click += new System.EventHandler(this.btnOption1_Click);
            // 
            // btnOption2
            // 
            this.btnOption2.Location = new System.Drawing.Point(12, 151);
            this.btnOption2.Name = "btnOption2";
            this.btnOption2.Size = new System.Drawing.Size(486, 23);
            this.btnOption2.TabIndex = 2;
            this.btnOption2.Text = "2. Nothing";
            this.btnOption2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOption2.UseVisualStyleBackColor = true;
            this.btnOption2.Click += new System.EventHandler(this.btnOption2_Click);
            // 
            // btnOption3
            // 
            this.btnOption3.Location = new System.Drawing.Point(12, 180);
            this.btnOption3.Name = "btnOption3";
            this.btnOption3.Size = new System.Drawing.Size(486, 23);
            this.btnOption3.TabIndex = 3;
            this.btnOption3.Text = "3. Nothing";
            this.btnOption3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOption3.UseVisualStyleBackColor = true;
            this.btnOption3.Click += new System.EventHandler(this.btnOption3_Click);
            // 
            // btnOption4
            // 
            this.btnOption4.Location = new System.Drawing.Point(12, 209);
            this.btnOption4.Name = "btnOption4";
            this.btnOption4.Size = new System.Drawing.Size(486, 23);
            this.btnOption4.TabIndex = 4;
            this.btnOption4.Text = "4. Nothing";
            this.btnOption4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOption4.UseVisualStyleBackColor = true;
            this.btnOption4.Click += new System.EventHandler(this.btnOption4_Click);
            // 
            // Interaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 244);
            this.Controls.Add(this.btnOption4);
            this.Controls.Add(this.btnOption3);
            this.Controls.Add(this.btnOption2);
            this.Controls.Add(this.btnOption1);
            this.Controls.Add(this.rtbDialog);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Interaction";
            this.Text = "Interaction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Interaction_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbDialog;
        private System.Windows.Forms.Button btnOption1;
        private System.Windows.Forms.Button btnOption2;
        private System.Windows.Forms.Button btnOption3;
        private System.Windows.Forms.Button btnOption4;
    }
}