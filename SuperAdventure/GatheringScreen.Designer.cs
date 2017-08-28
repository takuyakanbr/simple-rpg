namespace SuperAdventure
{
    partial class GatheringScreen
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
            this.components = new System.ComponentModel.Container();
            this.lblAction = new System.Windows.Forms.Label();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(40, 23);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(85, 13);
            this.lblAction.TabIndex = 0;
            this.lblAction.Text = "You are sailing...";
            // 
            // rtbResults
            // 
            this.rtbResults.Location = new System.Drawing.Point(12, 55);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.ReadOnly = true;
            this.rtbResults.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbResults.Size = new System.Drawing.Size(353, 119);
            this.rtbResults.TabIndex = 1;
            this.rtbResults.Text = "";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(300, 18);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(65, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GatheringScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 186);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.lblAction);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GatheringScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GatheringScreen_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.RichTextBox rtbResults;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timer1;
    }
}