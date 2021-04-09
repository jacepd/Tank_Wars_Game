// Created by Joshua Hardy and Jace Duennebeil, Spring 2021
// Uses code written by Daniel Kopta

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

        // Background image
        private Image backgroundImage;

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

        private HashSet<BeamAnimation> beamAnimations = new HashSet<BeamAnimation>();
        private List<BeamAnimation> animsToRemove = new List<BeamAnimation>();

        public DrawingPanel(World w)
        {
            DoubleBuffered = true;
            theWorld = w;

            // Initialize the images
            // Bakcground image
            backgroundImage = Image.FromFile("..\\..\\..\\Resources\\Images\\Background.png");

            // Wall image
            wallImage = Image.FromFile("..\\..\\..\\Resources\\Images\\WallSprite.png");

            // Tank and turret images
            blueTank = Image.FromFile("..\\..\\..\\Resources\\Images\\BlueTank.png");
            blueTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\BlueTurret.png");
            darkTank = Image.FromFile("..\\..\\..\\Resources\\Images\\DarkTank.png");
            darkTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\DarkTurret.png");
            greenTank = Image.FromFile("..\\..\\..\\Resources\\Images\\GreenTank.png");
            greenTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\GreenTurret.png");
            lightGreenTank = Image.FromFile("..\\..\\..\\Resources\\Images\\LightGreenTank.png");
            lightGreenTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\LightGreenTurret.png");
            orangeTank = Image.FromFile("..\\..\\..\\Resources\\Images\\OrangeTank.png");
            orangeTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\OrangeTurret.png");
            purpleTank = Image.FromFile("..\\..\\..\\Resources\\Images\\PurpleTank.png");
            purpleTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\PurpleTurret.png");
            redTank = Image.FromFile("..\\..\\..\\Resources\\Images\\RedTank.png");
            redTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\RedTurret.png");
            yellowTank = Image.FromFile("..\\..\\..\\Resources\\Images\\YellowTank.png");
            yellowTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\YellowTurret.png");

            // Projectile images
            blueProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-blue.png");
            darkProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-grey.png");
            greenProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-green.png");
            lightGreenProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-green.png");
            orangeProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-brown.png");
            purpleProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-violet.png");
            redProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-red.png");
            yellowProjectile = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-yellow.png");
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
            lock (theWorld)
            {
                // Draw the background
                if (theWorld.containsTank())
                {
                    // Center the view on the player's tank
                    int viewSize = Size.Width;
                    double playerX = theWorld.getPlayerTank().getLocation().GetX();
                    double playerY = theWorld.getPlayerTank().getLocation().GetY();
                    e.Graphics.TranslateTransform((float)-playerX + (viewSize / 2), (float)-playerY + (viewSize / 2));

                    DrawObjectWithTransform(e, null, 0, 0, 0, BackgroundDrawer);

                    // Draw the walls
                    foreach (Wall wall in theWorld.getWalls())
                    {
                        double firstX = wall.getFirstEndpoint().GetX();
                        double secondX = wall.getSecondEndpoint().GetX();
                        double firstY = wall.getFirstEndpoint().GetY();
                        double secondY = wall.getSecondEndpoint().GetY();

                        if (firstX - secondX < 0.0001 && firstX - secondX > -0.0001)
                        {
                            if (firstY < secondY)
                            {
                                for (double i = firstY; i <= secondY; i += 50)
                                {
                                    DrawObjectWithTransform(e, wall, firstX, i, 0, WallDrawer);
                                }
                            }
                            else
                            {
                                for (double i = firstY; i >= secondY; i -= 50)
                                {
                                    DrawObjectWithTransform(e, wall, firstX, i, 0, WallDrawer);
                                }
                            }
                        }
                        else
                        {
                            if (firstX < secondX)
                            {
                                for (double i = firstX; i <= secondX; i += 50)
                                {
                                    DrawObjectWithTransform(e, wall, i, firstY, 0, WallDrawer);
                                }
                            }
                            else
                            {
                                for (double i = firstX; i >= secondX; i -= 50)
                                {
                                    DrawObjectWithTransform(e, wall, i, firstY, 0, WallDrawer);
                                }
                            }
                        }
                    }

                    // Draw the powerups
                    foreach (Powerup powerup in theWorld.getPowerups())
                    {
                        DrawObjectWithTransform(e, powerup, powerup.getLocation().GetX(), powerup.getLocation().GetY(), 0, PowerupDrawer);
                    }

                    // Draw the tanks
                    foreach (Tank tank in theWorld.getTanks())
                    {
                        DrawObjectWithTransform(e, tank, tank.getLocation().GetX(), tank.getLocation().GetY(), tank.getOrientation().ToAngle(), TankDrawer);
                        DrawObjectWithTransform(e, tank, tank.getLocation().GetX(), tank.getLocation().GetY(), tank.getTurretDirection().ToAngle(), TurretDrawer);
                        DrawObjectWithTransform(e, tank, tank.getLocation().GetX(), tank.getLocation().GetY(), 0, HealthNameDrawer);
                    }

                    // Draw the projectiles
                    foreach (Projectile projectile in theWorld.getProjectiles())
                    {
                        DrawObjectWithTransform(e, projectile, projectile.getLocation().GetX(), projectile.getLocation().GetY(), projectile.getDirection().ToAngle(), ProjectileDrawer);
                    }

                    // Draw the beams
                    foreach (BeamAnimation anim in beamAnimations)
                    {
                        DrawObjectWithTransform(e, anim, anim.getOrigin().GetX(), anim.getOrigin().GetY(), anim.getDirection().ToAngle(), anim.BeamDrawer);
                        if (anim.getFrameNumber() > 60)
                        {
                            animsToRemove.Add(anim);
                        }
                    }
                    RemoveBeamAnimations();
                }
            }
        }

        private void BackgroundDrawer(object o, PaintEventArgs e)
        {
            int worldSize = theWorld.getWorldSize();
            Rectangle background = new Rectangle(-(worldSize / 2), -(worldSize / 2), worldSize, worldSize);

            e.Graphics.DrawImage(backgroundImage, background);
        }

        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            int tankWidth = 60;
            int tankHeight = 60;

            // Rectangles are drawn starting from the top-left corner.
            // So if we want the rectangle centered on the player's location, we have to offset it
            // by half its size to the left (-width/2) and up (-height/2)
            Rectangle tankRectangle = new Rectangle(-(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);

            int num = t.getID() % 8;

            // Since there are 8 different colors to choose from was getting remainder of the ID to choose the color for tank
            switch (num)
            {
                case 0:
                    e.Graphics.DrawImage(blueTank, tankRectangle);
                    break;
                case 1:
                    e.Graphics.DrawImage(darkTank, tankRectangle);
                    break;
                case 2:
                    e.Graphics.DrawImage(greenTank, tankRectangle);
                    break;
                case 3:
                    e.Graphics.DrawImage(lightGreenTank, tankRectangle);
                    break;
                case 4:
                    e.Graphics.DrawImage(orangeTank, tankRectangle);
                    break;
                case 5:
                    e.Graphics.DrawImage(purpleTank, tankRectangle);
                    break;
                case 6:
                    e.Graphics.DrawImage(redTank, tankRectangle);
                    break;
                case 7:
                    e.Graphics.DrawImage(yellowTank, tankRectangle);
                    break;
            }

            
        }

        private void HealthNameDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            Font nameFont = new Font("Arial", 12);
            SolidBrush nameBrush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            e.Graphics.DrawString(t.getName() + ": " + t.getScore(), nameFont, nameBrush, 0, 30, format);

            e.Graphics.DrawString("Health: " + t.getHealth(), nameFont, nameBrush, 0, -50, format);
        }

        private void TurretDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            int turretWidth = 50;
            int turretHeight = 50;

            Rectangle turretRectangle = new Rectangle(-(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);

            int num = t.getID() % 8;

            // Since there are 8 different colors to choose from was getting remainder of the ID to choose the color for tank
            switch (num)
            {
                case 0:
                    e.Graphics.DrawImage(blueTurret, turretRectangle);
                    break;
                case 1:
                    e.Graphics.DrawImage(darkTurret, turretRectangle);
                    break;
                case 2:
                    e.Graphics.DrawImage(greenTurret, turretRectangle);
                    break;
                case 3:
                    e.Graphics.DrawImage(lightGreenTurret, turretRectangle);
                    break;
                case 4:
                    e.Graphics.DrawImage(orangeTurret, turretRectangle);
                    break;
                case 5:
                    e.Graphics.DrawImage(purpleTurret, turretRectangle);
                    break;
                case 6:
                    e.Graphics.DrawImage(redTurret, turretRectangle);
                    break;
                case 7:
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

            int num = p.getOwnerID() % 8;

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

            using (Pen redPen = new Pen(Color.Red))
                using (SolidBrush greenBrush = new SolidBrush(Color.Green))
            {
                Rectangle outer = new Rectangle(5, 5, 10, 10);
                Rectangle inner = new Rectangle(5, 5, 8, 8);
                e.Graphics.FillEllipse(greenBrush, inner);
                e.Graphics.DrawEllipse(redPen, outer);
            }
        }

        public void AddBeamAnimation(Beam beam)
        {
            BeamAnimation anim = new BeamAnimation(beam.getOrigin(), beam.getDirection());
            this.Invoke(new MethodInvoker(() => beamAnimations.Add(anim)));
        }

        public void RemoveBeamAnimations()
        {
            foreach (BeamAnimation anim in animsToRemove)
            {
                beamAnimations.Remove(anim);
            }
        }
       
    }
}
