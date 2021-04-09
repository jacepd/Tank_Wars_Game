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
    public class DeathAnimation
    {
        private Vector2D origin;
        private int frameNumber;

        public DeathAnimation(Vector2D origin)
        {
            this.origin = origin;
        }

        public Vector2D getOrigin()
        {
            return origin;
        }

        public int getFrameNumber()
        {
            return frameNumber;
        }

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
