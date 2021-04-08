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

            // Set the window size
            ClientSize = new Size(viewSize, viewSize + menuSize);

            ServerNameTextbox.Text = "localhost";
            ServerNameTextbox.Focus();
            ServerNameTextbox.SelectAll();

            PlayerNameTextbox.MaxLength = 16;
            PlayerNameTextbox.Text = "playername";                   

            // Place and add the drawing panel
            drawingPanel = new DrawingPanel(world);
            drawingPanel.Location = new Point(0, menuSize);
            drawingPanel.Size = new Size(viewSize, viewSize);
            this.Controls.Add(drawingPanel);

            this.KeyDown += HandleKeyDown;
            this.KeyUp += HandleKeyUp;
            drawingPanel.MouseDown += HandleMouseDown;
            drawingPanel.MouseUp += HandleMouseUp;
            drawingPanel.MouseMove += HandleMouseMove;
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
            KeyPreview = true;
            control.ConnectToServer(ServerNameTextbox.Text, PlayerNameTextbox.Text);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                control.HandleMoveRequest("up");
            }
            else if(e.KeyCode == Keys.A)
            {
                control.HandleMoveRequest("left");
            }
            else if (e.KeyCode == Keys.S)
            {
                control.HandleMoveRequest("down");
            }
            else if (e.KeyCode == Keys.D)
            {
                control.HandleMoveRequest("right");
            }

            // Prevent other key handlers from running
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                control.HandleMoveRequest("none");
            }          
        }

        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                control.HandleFireRequest("main");
            }
            else if(e.Button == MouseButtons.Right)
            {
                control.HandleFireRequest("alt");
            }
        }

        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                control.HandleFireRequest("none");
            }
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            int windowX = this.Location.X;
            int windowY = this.Location.Y;
            int centerX = windowX + drawingPanel.Location.X + (viewSize / 2);
            int centerY = windowY + drawingPanel.Location.Y + (viewSize / 2);

            int xLength = MousePosition.X - centerX;
            int yLength = MousePosition.Y - centerY;

            Vector2D vector = new Vector2D(xLength, yLength);
            control.HandleTurretDirection(vector);
        }
    }
}
