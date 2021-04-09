// Created by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWars;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    /// <summary>
    /// Class for representing tank death animations
    /// </summary>
    public class DeathAnimation
    {
        private Vector2D origin; // The location from which the animation originates
        private int frameNumber; // How many frames have passed sicnce the start of the animation

        /// <summary>
        /// Creates a new tank death animation
        /// </summary>
        /// <param name="origin"></param>
        public DeathAnimation(Vector2D origin)
        {
            this.origin = origin;
            frameNumber = 0;
        }

        /// <summary>
        /// Returns the origin of the death animation
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrigin()
        {
            return origin;
        }

        /// <summary>
        /// Returns the frame number of the death animation
        /// </summary>
        /// <returns></returns>
        public int getFrameNumber()
        {
            return frameNumber;
        }

        /// <summary>
        /// Draws the death animation
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void DeathDrawer(object o, PaintEventArgs e)
        {
            using (SolidBrush blueBrush = new SolidBrush(Color.BlueViolet))
            {
                e.Graphics.FillEllipse(blueBrush,frameNumber, 0 , 5, 5);
                e.Graphics.FillEllipse(blueBrush, -frameNumber, 0, 5, 5);
                e.Graphics.FillEllipse(blueBrush, 0, frameNumber, 5, 5);
                e.Graphics.FillEllipse(blueBrush, 0, -frameNumber, 5, 5);

                e.Graphics.FillEllipse(blueBrush, frameNumber, -frameNumber, 5, 5);
                e.Graphics.FillEllipse(blueBrush, -frameNumber, frameNumber, 5, 5);
                e.Graphics.FillEllipse(blueBrush, -frameNumber, -frameNumber, 5, 5);
                e.Graphics.FillEllipse(blueBrush, frameNumber, frameNumber, 5, 5);
            }

            frameNumber++;
        }
    }
}
