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
        private World world;
        private DrawingPanel drawingPanel;

        private const int viewSize = 500;
        private const int menuSize = 40;

        public Form1(Controller ctrl)
        {
            InitializeComponent();
            control = ctrl;
            world = ctrl.getWorld();
            ctrl.UpdateArrived += OnFrame;

            ServerNameTextbox.Text = "localhost";

            PlayerNameTextbox.MaxLength = 16;
            PlayerNameTextbox.Text = "playername";

            ServerNameTextbox.Focus();
            ServerNameTextbox.SelectAll();

            // Place and add the drawing panel
            drawingPanel = new DrawingPanel(world);
            drawingPanel.Location = new Point(0, menuSize);
            drawingPanel.Size = new Size(viewSize, viewSize);
            this.Controls.Add(drawingPanel);
        }

        /// <summary>
        /// Handler for the controller's UpdateArrived event
        /// </summary>
        private void OnFrame()
        {
            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can

            MethodInvoker invoker = new MethodInvoker(() => this.Invalidate(true));
            this.Invoke(invoker);
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
