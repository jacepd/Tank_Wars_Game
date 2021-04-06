using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using TankWars;

namespace View
{
    public class DrawingPanel : Panel
    {
        private World theWorld;
        public DrawingPanel(World w)
        {
            DoubleBuffered = true;
            theWorld = w;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX, (int)worldY);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            // Center the view on the middle of the world,
            // since the image and world use different coordinate systems
            int viewSize = Size.Width; // view is square, so we can just use width
            e.Graphics.TranslateTransform(viewSize / 2, viewSize / 2);

            lock (theWorld)
            {
                // Draw the background
                //Image background = Image.FromFile("../Resources/Images/Background.png");
                //e.Graphics.DrawImage(background, new Point(0, 0));

                // Draw the walls
                foreach (Wall wall in theWorld.getWalls())
                {
                    DrawObjectWithTransform(e, wall, wall.getFirstEndpoint().GetX(), wall.getFirstEndpoint().GetY(), 0, WallDrawer);
                }

                // Draw the tanks
                foreach (Tank tank in theWorld.getTanks())
                {
                    DrawObjectWithTransform(e, tank, tank.getLocation().GetX(), tank.getLocation().GetY(), tank.getOrientation().ToAngle(), TankDrawer);
                }

                // Draw the projectiles
                foreach (Projectile projectile in theWorld.getProjectiles())
                {
                    DrawObjectWithTransform(e, projectile, projectile.getLocation().GetX(), projectile.getLocation().GetY(), projectile.getDirection().ToAngle(), ProjectileDrawer);
                }

                // Draw the powerups
                foreach (Powerup powerup in theWorld.getPowerups())
                {
                    DrawObjectWithTransform(e, powerup, powerup.getLocation().GetX(), powerup.getLocation().GetY(), 0, PowerupDrawer);
                }

                // Draw the beams
                foreach (Beam beam in theWorld.getWBeams())
                {
                    DrawObjectWithTransform(e, beam, beam.getOrigin().GetX(), beam.getOrigin().GetY(), beam.getDirection().ToAngle(), BeamDrawer);
                }

            }
        }

        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            int tankWidth = 60;
            int tankHeight = 60;

            int turretWidth = 50;
            int turretHeight = 50;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle tankRectangle = new Rectangle(-(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
            Rectangle turretRectangle = new Rectangle(-(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);

            // Since there are 8 different colors to choose from was getting remainder of the ID to choose the color for tank
            // May be a better way to do this
            if (t.getID() % 8 == 0)
            {
                // Use a color of tank and turret
            }
            else if (t.getID() % 8 == 1)
            {
                // use different color of tank and turret
            }
            else if (t.getID() % 8 == 2)
            {
                // use different color of tank and turret
            }
            else if (t.getID() % 8 == 3)
            {
                // use different color of tank and turret
            }
            else if (t.getID() % 8 == 4)
            {
                // use different color of tank and turret
            }
            else if (t.getID() % 8 == 5)
            {
                // use different color of tank and turret
            }
            else if (t.getID() % 8 == 6)
            {
                // use different color of tank and turret
            }
            else if (t.getID() % 8 == 7)
            {
                // use different color of tank and turret
            }




            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //using (System.Drawing.SolidBrush blueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
            //using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            //{
            //    if (t.getID() == 1) // team 1 is blue
            //        e.Graphics.FillRectangle(blueBrush, r);
            //    else                  // team 2 is green
            //        e.Graphics.FillRectangle(greenBrush, r);
            //}
        }

        private void WallDrawer(object o, PaintEventArgs e)
        {
            Wall w = o as Wall;

            int width = 50;
            int height = 50;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);


            // Draw Image



            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //using (System.Drawing.SolidBrush grayBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray))
            //{
            //    e.Graphics.FillRectangle(grayBrush, r);
            //}
        }

        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;

            int width = 30;
            int height = 30;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);


            // Draw Image
        }

        private void PowerupDrawer(object o, PaintEventArgs e)
        {
            Powerup p = o as Powerup;

            // Draw Image
        }

        private void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam b = o as Beam;

            // Draw Image
        }
    }
}
