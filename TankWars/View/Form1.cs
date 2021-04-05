// Edited by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankWars;

namespace View
{
    public partial class Form1 : Form
    {
        private Controller control;

        public Form1(Controller ctrl)
        {
            InitializeComponent();

            ServerNameTextbox.Text = "localhost";

            PlayerNameTextbox.MaxLength = 16;
            PlayerNameTextbox.Text = "playername";

            ServerNameTextbox.Focus();
            ServerNameTextbox.SelectAll();

            control = ctrl;
        }

        private void ServerConnectButton_Click(object sender, EventArgs e)
        {
            ServerConnectButton.Enabled = false;
            ServerNameTextbox.Enabled = false;
            PlayerNameTextbox.Enabled = false;
            control.ConnectToServer(ServerNameTextbox.Text, PlayerNameTextbox.Text);
        }
    }
}
