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
        public float _angle;

        public RB_Square(float height, float width, bool isFix = false, float angle = 0)
        {
            _height = height/2;
            _width = width/2;
            _angle = angle;
            fix = isFix;
            this.Id = _idCount++;
        }

        public RB_Square(Vector2 topLeft, Vector2 bottomRight, Vector2 position, bool isFix = false, float angle = 0)
        {
            _height = (bottomRight.Y - topLeft.Y)/2;
            _width = (bottomRight.X - topLeft.X)/2;
            this.Position = position;
            _angle = angle;
            fix = isFix;
            this.Id = _idCount++;
        }

        public override Box getBoundingBox()
        {
            Vector2 topleft = new Vector2(leftMostPoint().X, upMostPoint().Y);
            Vector2 bottomright = new Vector2(rightMostPoint().X, downMostPoint().Y);
            return new Box(topleft, bottomright);
        }

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

                double thisDown = downMostPoint().Y;
                double thisUp = upMostPoint().Y;
                double thisRight = rightMostPoint().X;
                double thisLeft = leftMostPoint().X;

                double cDown = c.downMostPoint().Y;
                double cUp = c.upMostPoint().Y;
                double cRight = c.rightMostPoint().X;
                double cLeft = c.leftMostPoint().X;

                //TODO : FIX CORNER COLLISION

                if (this._angle == 0)
                {
                    if (thisDown >= cUp && thisDown <= cDown)
                    {
                        //Collision between bottom of this and top of c

                        if ((thisLeft >= cLeft && thisRight <= cRight) || (cLeft >= thisLeft && cRight <= thisRight))
                        {
                            //Pure Top Bottom collision

                            penetration = thisDown - cUp;
                            return new ContactResult(this, r, penetration, new Vector2(0, -1));
                        }

                        else if (thisRight >= cLeft && thisRight <= cRight)
                        {
                            //Collision between bottomRight of this and topLeft of c

                            penetration = Math.Min ( thisDown - cUp, thisRight - cLeft );
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                        
                        else if (thisLeft < cRight && thisLeft >= cLeft)
                        {
                            //Collision between bottomLeft of this and topRight of c

                            penetration = Math.Min( thisDown - cUp, cRight - thisLeft );
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                    }

                    else if (thisUp >= cUp && thisUp <= cDown)
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

                            penetration = Math.Min ( cDown - thisUp, thisRight - cLeft );
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                        
                        else if (thisLeft < cRight && thisLeft >= cLeft)
                        {
                            //Collision between topLeft of this and bottomRight of c

                            penetration = Math.Min ( cDown - thisUp, cRight - thisLeft);
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                    }
                }
            }

            //#########################################
            // TODO : COLLISION CIRCLE-SQUARE
            //#########################################
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
            return new Vector2((this.Position.X - _width) * (float)Math.Cos(_angle % 90) - (this.Position.Y + _height) * (float)Math.Sin(_angle % 90),this.Position.Y);
        }
        public override Vector2 rightMostPoint() 
        {
            return new Vector2((this.Position.X + _width) * (float)Math.Cos(_angle % 90) - (this.Position.Y - _height) * (float)Math.Sin(_angle % 90), this.Position.Y);
        }
        public override Vector2 upMostPoint() 
        {
            return new Vector2(this.Position.X, (this.Position.Y - _height) * (float)Math.Cos(_angle % 90) + (this.Position.Y + _width) * (float)Math.Sin(_angle % 90));
        }
        public override Vector2 downMostPoint() 
        {
            return new Vector2(this.Position.X, (this.Position.Y + _height) * (float)Math.Cos(_angle % 90) + (this.Position.Y - _width) * (float)Math.Sin(_angle % 90));
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
                zoom = Scene.SceneManager.scene.Camera.zoom;
                cameraPosition = Scene.SceneManager.scene.Camera.position;
                cameraWidth = Scene.SceneManager.scene.Camera.Width;
                cameraHeight = Scene.SceneManager.scene.Camera.Height;
            }
            else
            {
                zoom = 1f;
                cameraPosition = new Vector2(Game1.screenWidth / 2 / Game1.pxPerUnit, Game1.screenHeight / 2 / Game1.pxPerUnit);
                cameraWidth = Game1.screenWidth / Game1.pxPerUnit;
                cameraHeight = Game1.screenHeight / Game1.pxPerUnit;
            }
            spriteBatch.Draw(
                pointTexture, 
                new Rectangle(
                    (int)(64 * zoom * (Position.X - cameraPosition.X + cameraWidth / 2)) - (int)(64 * zoom * _width), 
                    (int)(64 * zoom * (Position.Y - cameraPosition.Y + cameraHeight / 2)) - (int)(64 * zoom * _height), 
                    (int)(64 * zoom * _width * 2), 
                    (int)(64 * zoom * _height * 2)), 
                    Color.Chocolate);
        }
        public override String ToString()
        {
            return "RB_Square(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tHeight: " + _height + ", \n\tWidth: " + _width + ", \n\tAngle: " + _angle + ", \n\tId: " + Id + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Id == ((RB_Circle)obj).Id);
            }
        }
    }
}
