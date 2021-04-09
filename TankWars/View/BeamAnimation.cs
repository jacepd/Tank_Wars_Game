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
        Vector2D origin;
        Vector2D direction;
        int frameNumber;

        public BeamAnimation(Vector2D origin, Vector2D dir)
        {
            this.origin = new Vector2D(origin);
            direction = new Vector2D(dir);
        }

        public Vector2D getOrigin()
        {
            return origin;
        }

        public Vector2D getDirection()
        {
            return direction;
        }

        public int getFrameNumber()
        {
            return frameNumber;
        }

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
