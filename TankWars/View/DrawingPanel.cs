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

        // Wall image
        private Image wallImage;

        // Tank and turret images
        private Image blueTank;
        private Image blueTurret;
        private Image darkTank;
        private Image darkTurret;
        private Image greenTank;
        private Image greenTurret;
        private Image lightGreenTank;
        private Image lightGreenTurret;
        private Image orangeTank;
        private Image orangeTurret;
        private Image purpleTank;
        private Image purpleTurret;
        private Image redTank;
        private Image redTurret;
        private Image yellowTank;
        private Image yellowTurret;

        // Projectile images
        private Image blueProjectile;
        private Image darkProjectile;
        private Image greenProjectile;
        private Image lightGreenProjectile;
        private Image orangeProjectile;
        private Image purpleProjectile;
        private Image redProjectile;
        private Image yellowProjectile;

        public DrawingPanel(World w)
        {
            DoubleBuffered = true;
            theWorld = w;

            // Initialize the images
            // Wall image
            //wallImage = Image.FromFile("../../Resources/Images/WallSprite.png");

            //// Tank and turret images
            //blueTank = Image.FromFile("../../Resources/Images/BlueTank.png");
            //blueTurret = Image.FromFile("../../Resources/Images/BlueTurret.png");
            //darkTank = Image.FromFile("../../Resources/Images/DarkTank.png");
            //darkTurret = Image.FromFile("../../Resources/Images/DarkTurret.png");
            //greenTank = Image.FromFile("../../Resources/Images/GreenTank.png");
            //greenTurret = Image.FromFile("../../Resources/Images/GreenTurret.png");
            //lightGreenTank = Image.FromFile("../../Resources/Images/LightGreenTank.png");
            //lightGreenTurret = Image.FromFile("../../Resources/Images/LightGreenTurret.png");
            //orangeTank = Image.FromFile("../../Resources/Images/OrangeTank.png");
            //orangeTurret = Image.FromFile("../../Resources/Images/OrangeTurret.png");
            //purpleTank = Image.FromFile("../../Resources/Images/PurpleTank.png");
            //purpleTurret = Image.FromFile("../../Resources/Images/PurpleTurret.png");
            //redTank = Image.FromFile("../../Resources/Images/RedTank.png");
            //redTurret = Image.FromFile("../../Resources/Images/RedTurret.png");
            //yellowTank = Image.FromFile("../../Resources/Images/YellowTank.png");
            //yellowTurret = Image.FromFile("../../Resources/Images/YellowTurret.png");

            //// Projectile images
            //blueProjectile = Image.FromFile("../../Resources/Images/shot-blue.png");
            //darkProjectile = Image.FromFile("../../Resources/Images/shot-grey.png");
            //greenProjectile = Image.FromFile("../../Resources/Images/shot-green.png");
            //lightGreenProjectile = Image.FromFile("../../Resources/Images/shot-green.png");
            //orangeProjectile = Image.FromFile("../../Resources/Images/shot-brown.png");
            //purpleProjectile = Image.FromFile("../../Resources/Images/shot-violet.png");
            //redProjectile = Image.FromFile("../../Resources/Images/shot-red.png");
            //yellowProjectile = Image.FromFile("../../Resources/Images/shot-yellow.png");
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

            int num = t.getID() % 8;

            // Since there are 8 different colors to choose from was getting remainder of the ID to choose the color for tank
            switch (num)
            {
                case 0:
                    e.Graphics.DrawImage(blueTank, tankRectangle);
                    e.Graphics.DrawImage(blueTurret, turretRectangle);
                    break;
                case 1:
                    e.Graphics.DrawImage(darkTank, tankRectangle);
                    e.Graphics.DrawImage(darkTurret, turretRectangle);
                    break;
                case 2:
                    e.Graphics.DrawImage(greenTank, tankRectangle);
                    e.Graphics.DrawImage(greenTurret, turretRectangle);
                    break;
                case 3:
                    e.Graphics.DrawImage(lightGreenTank, tankRectangle);
                    e.Graphics.DrawImage(lightGreenTurret, turretRectangle);
                    break;
                case 4:
                    e.Graphics.DrawImage(orangeTank, tankRectangle);
                    e.Graphics.DrawImage(orangeTurret, turretRectangle);
                    break;
                case 5:
                    e.Graphics.DrawImage(purpleTank, tankRectangle);
                    e.Graphics.DrawImage(purpleTurret, turretRectangle);
                    break;
                case 6:
                    e.Graphics.DrawImage(redTank, tankRectangle);
                    e.Graphics.DrawImage(redTurret, turretRectangle);
                    break;
                case 7:
                    e.Graphics.DrawImage(yellowTank, tankRectangle);
                    e.Graphics.DrawImage(yellowTurret, turretRectangle);
                    break;
            }

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

            e.Graphics.DrawImage(wallImage, r);
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

            int num = p.getID() % 8;

            // Since there are 8 different colors to choose from was getting remainder of the ID to choose the color for tank
            switch (num)
            {
                case 0:
                    e.Graphics.DrawImage(blueProjectile, r);
                    break;
                case 1:
                    e.Graphics.DrawImage(darkProjectile, r);
                    break;
                case 2:
                    e.Graphics.DrawImage(greenProjectile, r);
                    break;
                case 3:
                    e.Graphics.DrawImage(lightGreenProjectile, r);
                    break;
                case 4:
                    e.Graphics.DrawImage(orangeProjectile, r);
                    break;
                case 5:
                    e.Graphics.DrawImage(purpleProjectile, r);
                    break;
                case 6:
                    e.Graphics.DrawImage(redProjectile, r);
                    break;
                case 7:
                    e.Graphics.DrawImage(yellowProjectile, r);
                    break;
            }
        }

        private void PowerupDrawer(object o, PaintEventArgs e)
        {
            Powerup p = o as Powerup;

            using (Pen redBrush = new Pen(Color.Red))
            {
                Rectangle r = new Rectangle(5, 5, 10, 10);

                e.Graphics.DrawEllipse(redBrush, r);
            }
        }

        private void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam b = o as Beam;

            using(Pen whiteBrush = new Pen(Color.White, 20f))
            {
                e.Graphics.DrawLine(whiteBrush, new Point(0, 0), new Point(0, -2000));
            }
        }
    }
}
