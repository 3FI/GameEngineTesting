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

            //#########################################
            // TODO : COLLISIONS
            //#########################################
            if (r.GetType() == this.GetType())
            {
                RB_Square c = (RB_Square)r;
                minSeparationDist = 0;
                double penetration = 0;
                if (this._angle == 0)
                {
                    if (this.downMostPoint().Y >= c.upMostPoint().Y && this.downMostPoint().Y <= c.downMostPoint().Y)
                    {
                        if (this.rightMostPoint().X >= c.leftMostPoint().X && this.rightMostPoint().X <= c.rightMostPoint().X)
                        {
                            //Bottom Left Collisison 
                            penetration = Math.Min (c.upMostPoint().Y - this.downMostPoint().Y, this.leftMostPoint().X - c.rightMostPoint().X);
                            //Math.Sqrt(Math.Pow(this.downMostPoint().Y - c.upMostPoint().Y, 2) + Math.Pow(this.rightMostPoint().X - c.leftMostPoint().X, 2));
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                        
                        else if (this.leftMostPoint().X < c.rightMostPoint().X && this.leftMostPoint().X >= c.leftMostPoint().X)
                        {
                            //Bottom Right Collision
                            penetration = Math.Min(c.upMostPoint().Y - this.downMostPoint().Y, c.leftMostPoint().X - this.rightMostPoint().X);
                            //Math.Sqrt(Math.Pow(this.downMostPoint().Y - c.upMostPoint().Y, 2) + Math.Pow(c.rightMostPoint().X - this.leftMostPoint().X, 2));
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                    }
                    else if (this.upMostPoint().Y <= c.downMostPoint().Y && this.downMostPoint().Y >= c.downMostPoint().Y)
                    {
                        if (this.rightMostPoint().X >= c.leftMostPoint().X && this.rightMostPoint().X <= c.rightMostPoint().X)
                        {
                            //Top Left Collisison
                            penetration = Math.Min (this.upMostPoint().Y - c.downMostPoint().Y, this.leftMostPoint().X - c.rightMostPoint().X);
                            //Math.Sqrt(Math.Pow(c.downMostPoint().Y - this.upMostPoint().Y, 2) + Math.Pow(c.rightMostPoint().X - this.leftMostPoint().X, 2));
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }

                        else if (this.leftMostPoint().X < c.rightMostPoint().X && this.leftMostPoint().X >= c.leftMostPoint().X)
                        {
                            //Top Right Collision
                            penetration = Math.Min(this.upMostPoint().Y - c.downMostPoint().Y, c.leftMostPoint().X - this.rightMostPoint().X);
                            //Math.Sqrt(Math.Pow(c.downMostPoint().Y - this.upMostPoint().Y, 2) + Math.Pow(this.rightMostPoint().X - c.leftMostPoint().X, 2));
                            return new ContactResult(this, r, penetration, Vector2.Normalize(diff));
                        }
                    }
                }
            }

            //#########################################
            // TBA
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
            float zoom = Scene.SceneManager.scene.Camera.zoom;

            spriteBatch.Draw(pointTexture, new Rectangle((int)(64 * zoom * (Position.X - Scene.SceneManager.scene.Camera.position.X + Scene.SceneManager.scene.Camera.Width / 2)) - (int)(64 * zoom * _width), (int)(64 * zoom * (Position.Y - Scene.SceneManager.scene.Camera.position.Y + Scene.SceneManager.scene.Camera.Height / 2)) - (int)(64 * zoom * _height), (int)(64 * zoom * _width * 2), (int)(64 * zoom * _height * 2)), Color.Chocolate);
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
