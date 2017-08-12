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
    public partial class DialogScreen : Form
    {
        private SuperAdventure _parent;
        private GameState _state;

        public DialogScreen(SuperAdventure parent, GameState state)
        {
            _parent = parent;
            _state = state;
            InitializeComponent();
        }

        public void UpdateDialog(string dialog, string[] opts)
        {
            rtbDialog.Text = dialog;
            btnOption1.Visible = false;
            btnOption2.Visible = false;
            btnOption3.Visible = false;
            btnOption4.Visible = false;
            
            if (opts != null)
            {
                if (opts.Length >= 1)
                {
                    btnOption1.Text = "1. " + opts[0];
                    btnOption1.Visible = true;
                }
                if (opts.Length >= 2)
                {
                    btnOption2.Text = "2. " + opts[1];
                    btnOption2.Visible = true;
                }
                if (opts.Length >= 3)
                {
                    btnOption3.Text = "3. " + opts[2];
                    btnOption3.Visible = true;
                }
                if (opts.Length >= 4)
                {
                    btnOption4.Text = "4. " + opts[3];
                    btnOption4.Visible = true;
                }
            }
        }
        
        private void btnOption1_Click(object sender, EventArgs e)
        {
            _state.Interact(1);
        }

        private void btnOption2_Click(object sender, EventArgs e)
        {
            _state.Interact(2);
        }

        private void btnOption3_Click(object sender, EventArgs e)
        {
            _state.Interact(3);
        }

        private void btnOption4_Click(object sender, EventArgs e)
        {
            _state.Interact(4);
        }

        private void Interaction_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.DialogScreen = null;
        }
    }
}
