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
    public class BeamAnimation
    {
        Vector2D origin; // The location from which the animation originates
        Vector2D direction; // The direction the beam is traveling
        int frameNumber; // How many frames have passed sicnce the start of the animation

        /// <summary>
        /// Creates a new beam animation
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dir"></param>
        public BeamAnimation(Vector2D origin, Vector2D dir)
        {
            this.origin = new Vector2D(origin);
            direction = new Vector2D(dir);
            frameNumber = 0;
        }

        /// <summary>
        /// Returns the origin of the beam animation
        /// </summary>
        /// <returns></returns>
        public Vector2D getOrigin()
        {
            return origin;
        }

        /// <summary>
        /// Returns the direction of the beam animation
        /// </summary>
        /// <returns></returns>
        public Vector2D getDirection()
        {
            return direction;
        }

        /// <summary>
        /// Returns the frame number of the beam animation
        /// </summary>
        /// <returns></returns>
        public int getFrameNumber()
        {
            return frameNumber;
        }

        /// <summary>
        /// Draws the beam animation
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam b = o as Beam;

            float beamSize = 5f - (float)(frameNumber / 12.0);

            using (Pen whiteBrush = new Pen(Color.White, beamSize))
            {
                e.Graphics.DrawLine(whiteBrush, new Point(0, 0), new Point(0, -2000));
            }

            frameNumber++;
        }
    }

}
