using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GameEngine.Collision
{
    public class RB_Square : RigidBody
    {
        public float _height;
        public float _width;

        public RB_Square(float height, float width, bool isFix = false, float angle = 0)
        {
            _height = height/2;
            _width = width/2;
            Angle = angle;
            fix = isFix;
            this.Id = _idCount++;
        }

        public RB_Square(Vector2 topLeft, Vector2 bottomRight, Vector2 position, bool isFix = false, float angle = 0)
        {
            _height = (bottomRight.Y - topLeft.Y)/2;
            _width = (bottomRight.X - topLeft.X)/2;
            this.Position = position;
            Angle = angle;
            fix = isFix;
            this.Id = _idCount++;
        }

        public override Box getBoundingBox()
        {
            Vector2 topleft = new Vector2(leftMostPoint().X, upMostPoint().Y);
            Vector2 bottomright = new Vector2(rightMostPoint().X, downMostPoint().Y);
            return new Box(topleft, bottomright);
        }
        //TODO : FIX BVH SLOWLY SHRINKING

        public override ContactResult isContacting(RigidBody r) 
        {
            Vector2 diff = Vector2.Subtract(this.Position, r.Position);
            double separationDist = Vector2.Distance(this.Position, r.Position);
            double minSeparationDist = double.PositiveInfinity;

            if (r.GetType() == this.GetType())
            {
                RB_Square c = (RB_Square)r;
                minSeparationDist = 0;
                double penetration = 0;
                Vector2 orientation = Vector2.Zero;

                float thisDown = downMostPoint().Y;
                float thisUp = upMostPoint().Y;
                float thisRight = rightMostPoint().X;
                float thisLeft = leftMostPoint().X;

                float cDown = c.downMostPoint().Y;
                float cUp = c.upMostPoint().Y;
                float cRight = c.rightMostPoint().X;
                float cLeft = c.leftMostPoint().X;

                bool collides = true;

                if (this.Angle%90 == 0 && c.Angle%90 == 0)
                {
                    if (thisDown >= cUp && thisDown <= cDown && thisUp < cUp)
                    {
                        //Collision between bottom of this and top of c

                        if ((thisLeft >= cLeft && thisRight <= cRight) || (cLeft >= thisLeft && cRight <= thisRight))
                        {
                            //Pure Bottom Top collision

                            penetration = thisDown - cUp;
                            GameObject?.DownContact?.Invoke();
                            return new ContactResult(this, r, penetration, new Vector2(0, -1));
                        }

                        else if (thisRight >= cLeft && thisRight <= cRight)
                        {
                            //Collision between bottomRight of this and topLeft of c

                            if (thisDown - cUp <= thisRight - cLeft)
                            {
                                //From Top
                                penetration = thisDown - cUp;
                                GameObject?.DownContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(0, -1));
                            }
                            else
                            {
                                //From Left
                                penetration = thisRight - cLeft;
                                GameObject?.RightContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(-1, 0));
                            }
                        }
                        
                        else if (thisLeft < cRight && thisLeft >= cLeft)
                        {
                            //Collision between bottomLeft of this and topRight of c

                            if (thisDown - cUp <= cRight - thisLeft)
                            {
                                //From Top
                                penetration = thisDown - cUp;
                                GameObject?.DownContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(0, -1));
                            }
                            else
                            {
                                //From Right
                                penetration = cRight - thisLeft;
                                GameObject?.LeftContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(1, 0));
                            }
                        }
                    }

                    else if (thisUp >= cUp && thisUp <= cDown && thisDown > cDown)
                    {
                        //Collision between top of this and bottom of c

                        if ((thisLeft >= cLeft && thisRight <= cRight) || (cLeft >= thisLeft && cRight <= thisRight))
                        {
                            //Pure Top Bottom collision

                            penetration = cDown - thisUp;
                            return new ContactResult(this, r, penetration, new Vector2(0,1));
                        }

                        if (thisRight >= cLeft && thisRight <= cRight)
                        {
                            //Collision between topRight of this and bottomLeft of c
                            
                            if (cDown - thisUp <= thisRight - cLeft)
                            {
                                //From Bottom
                                penetration = cDown - thisUp;
                                GameObject?.UpContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(0, 1));
                            }
                            else
                            {
                                //From Left
                                penetration = thisRight - cLeft;
                                GameObject?.RightContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(-1, 0));
                            }
                        }
                        
                        else if (thisLeft < cRight && thisLeft >= cLeft)
                        {
                            //Collision between topLeft of this and bottomRight of c

                            if (cDown - thisUp <= cRight - thisLeft)
                            {
                                //From Bottom
                                penetration = cDown - thisUp;
                                GameObject?.UpContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(0, 1));
                            }
                            else
                            {
                                //From Right
                                penetration = cRight - thisLeft;
                                GameObject?.LeftContact?.Invoke();
                                return new ContactResult(this, r, penetration, new Vector2(1, 0));
                            }
                        }
                    }

                    else if (thisRight >= cLeft && thisRight <= cRight)
                    {
                        //Pure Right Left Collision
                        penetration = thisRight - cLeft;
                        GameObject?.RightContact?.Invoke();
                        return new ContactResult(this, r, penetration, new Vector2(-1, 0));
                    }
                    else if (thisLeft < cRight && thisLeft >= cLeft)
                    {
                        //Pure Left Right Collision
                        penetration = cRight - thisLeft;
                        GameObject?.LeftContact?.Invoke();
                        return new ContactResult(this, r, penetration, new Vector2(1, 0));
                    }
                }

                else
                {
                    penetration = float.PositiveInfinity;
                    Vector2 LR = rightMostPoint();
                    Vector2 UL = leftMostPoint();
                    Vector2 UR = upMostPoint();
                    Vector2 LL = downMostPoint();

                    Vector2 cLR = c.rightMostPoint();
                    Vector2 cUL = c.leftMostPoint();
                    Vector2 cUR = c.upMostPoint();
                    Vector2 cLL = c.downMostPoint();

                    Vector2[] Points = new Vector2[] { LR, UL, UR, LL};
                    Vector2[] cPoints = new Vector2[] { cLR, cUL, cUR, cLL };

                    //TODO : CHANGE AXIS TO ALWAYS FACE THE OTHER OBJECT
                    Vector2 Axis1 = UR - UL;
                    Vector2 Axis2 = UR - LR;
                    Vector2 Axis3 = cUL - cUR;
                    Vector2 Axis4 = cUL - cLL;

                    Vector2[] Axis = new Vector2[] { Axis1, Axis2, Axis3, Axis4 }; 

                    foreach (Vector2 axis in Axis)
                    {
                        float min=float.PositiveInfinity;
                        float max=float.NegativeInfinity;
                        Vector2 minVector=Vector2.Zero;
                        Vector2 maxVector = Vector2.Zero;
                        foreach (Vector2 point in Points)
                        {
                            Vector2 p = Tools.CustomMath.Project(point, axis);
                            float scalar = Vector2.Dot(point, axis);
                            if (scalar > max) 
                            {
                                max = scalar;
                                maxVector = p;
                            }
                            if (scalar < min)
                            {
                                min = scalar;
                                minVector = p;
                            }
                        }
                        float cmin = float.PositiveInfinity;
                        float cmax = float.NegativeInfinity;
                        Vector2 cminVector = Vector2.Zero;
                        Vector2 cmaxVector =Vector2.Zero;
                        foreach (Vector2 point in cPoints)
                        {
                            Vector2 p = Tools.CustomMath.Project(point, axis);
                            float scalar = Vector2.Dot(point, axis);
                            if (scalar > cmax)
                            {
                                cmax = scalar;
                                cmaxVector = p;
                            }
                            if (scalar < cmin)
                            {
                                cmin = scalar;
                                cminVector = p;
                            }
                        }
                        if ((cmin <= max && cmax >= min))
                        {
                            if (Vector2.Distance(cmaxVector,minVector) < penetration)
                            {
                                penetration = Vector2.Distance(cmaxVector, minVector);
                                axis.Normalize();
                                orientation = axis;
                            }
                            continue;
                        }
                        else
                        {
                            collides = false;
                            break;
                        }
                    }

                    if (collides)
                        return new ContactResult(this, r, penetration, orientation);
                }
            }

            // TODO : COLLISION CIRCLE-SQUARE
            else if (r is RB_Circle)
            {
                RB_Circle c = (RB_Circle)r;
                minSeparationDist = 0;
            }

            else
            {
                minSeparationDist = 0;
                System.Diagnostics.Debug.WriteLine("Collision not implemented between types Square and" + r.GetType());
            }

            if (separationDist < minSeparationDist)
            {
                return new ContactResult(this, r, separationDist - minSeparationDist, Vector2.Normalize(diff));
            }
            return null;
        }


        public override Vector2 leftMostPoint() 
        {
            return new Vector2(
                (this.Position.X - _width - this.Position.X) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360)) - (this.Position.Y + _height - this.Position.Y) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360)) + this.Position.X,
                (this.Position.X - _width - this.Position.X) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360)) + (this.Position.Y + _height - this.Position.Y) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360)) + this.Position.Y);
        }
        public override Vector2 rightMostPoint() 
        {
            return new Vector2(
                (this.Position.X + _width - this.Position.X) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360)) - (this.Position.Y - _height - this.Position.Y) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360)) + this.Position.X,
                (this.Position.X + _width - this.Position.X) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360)) + (this.Position.Y - _height - this.Position.Y) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360)) + this.Position.Y);
        }
        public override Vector2 upMostPoint() 
        {
            return new Vector2(
                -(this.Position.Y - _height - this.Position.Y) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360) % 90) + (this.Position.X - _width - this.Position.X) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360)) + this.Position.X,
                (this.Position.Y - _height - this.Position.Y) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360) % 90) + (this.Position.X - _width - this.Position.X) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360)) + this.Position.Y);
        }
        public override Vector2 downMostPoint() 
        {
            return new Vector2(
                -(this.Position.Y + _height - this.Position.Y) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360) % 90) + (this.Position.X + _width - this.Position.X) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360)) + this.Position.X,
                (this.Position.Y + _height - this.Position.Y) * (float)Math.Cos((Angle % 90 * 2 * Math.PI / 360) % 90) + (this.Position.X + _width - this.Position.X) * (float)Math.Sin((Angle % 90 * 2 * Math.PI / 360)) + this.Position.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pointTexture == null)
            {
                pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            float zoom;
            Vector2 cameraPosition;
            float cameraWidth;
            float cameraHeight;
            if (Scene.SceneManager.scene.Camera != null)
            {
                zoom = Scene.SceneManager.scene.Camera.Zoom;
                cameraPosition = Scene.SceneManager.scene.Camera.Position;
                cameraWidth = Scene.SceneManager.scene.Camera.Width;
                cameraHeight = Scene.SceneManager.scene.Camera.Height;
            }
            else
            {
                zoom = 1f;
                cameraPosition = new Vector2(Game1.ScreenWidth / 2 / Game1.pxPerUnit, Game1.ScreenHeight / 2 / Game1.pxPerUnit);
                cameraWidth = Game1.ScreenWidth / Game1.pxPerUnit;
                cameraHeight = Game1.ScreenHeight / Game1.pxPerUnit;
            }
            spriteBatch.Draw(
                pointTexture,
                new Rectangle(
                    (int)(Game1.pxPerUnit * zoom * (Position.X - cameraPosition.X + cameraWidth / 2)),
                    (int)(Game1.pxPerUnit * zoom * (Position.Y - cameraPosition.Y + cameraHeight / 2)),
                    (int)(Game1.pxPerUnit * zoom * _width * 2),
                    (int)(Game1.pxPerUnit * zoom * _height * 2)),
                new Rectangle(1,1,1,1),
                Color.Chocolate,
                Angle / 360 * 2 * (float)Math.PI,
                new Vector2(0.5f,0.5f),
                SpriteEffects.None,
                0.5f);
            //TODO : Change to draw at angles

            /*
                Vector2 LR = rightMostPoint();
                Vector2 UL = leftMostPoint();
                Vector2 UR = upMostPoint();
                Vector2 LL = downMostPoint();
                spriteBatch.Draw(
                    pointTexture,
                    new Rectangle((int)(64 * zoom * (UR.X - cameraPosition.X + cameraWidth / 2)), (int)(64 * zoom * (UR.Y - cameraPosition.Y + cameraHeight / 2)), 4, 4),
                    Color.Blue);
                spriteBatch.Draw(
                    pointTexture,
                    new Rectangle((int)(64 * zoom * (LL.X - cameraPosition.X + cameraWidth / 2)), (int)(64 * zoom * (LL.Y - cameraPosition.Y + cameraHeight / 2)), 4, 4),
                    Color.Green);
                spriteBatch.Draw(
                    pointTexture,
                    new Rectangle((int)(64 * zoom * (UL.X - cameraPosition.X + cameraWidth / 2)), (int)(64 * zoom * (UL.Y - cameraPosition.Y + cameraHeight / 2)), 4, 4),
                    Color.Yellow);
                spriteBatch.Draw(
                    pointTexture,
                    new Rectangle((int)(64 * zoom * (LR.X - cameraPosition.X + cameraWidth / 2)), (int)(64 * zoom * (LR.Y - cameraPosition.Y + cameraHeight / 2)), 4, 4),
                    Color.Magenta);
            */
        }
        public override String ToString()
        {
            return "RB_Square(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tHeight: " + _height + ", \n\tWidth: " + _width + ", \n\tAngle: " + Angle + ", \n\tId: " + Id + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Id == ((RB_Square)obj).Id);
            }
        }
    }
}
