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

        public RB_Square(float height, float width, float angle = 0)
        {
            _height = height;
            _width = width;
            _angle = angle;
            this.Id = _idCount++;
        }

        public override Box getBoundingBox()
        {
            Vector2 bottomleft = new Vector2(leftMostPoint().X, downMostPoint().Y);
            Vector2 topright = new Vector2(rightMostPoint().X, upMostPoint().Y);
            return new Box(bottomleft, topright);
        }

        public override ContactResult isContacting(RigidBody r) 
        {
            Vector2 diff = Vector2.Subtract(this.Position, r.Position);
            double separationDist = Vector2.Distance(this.Position, r.Position);
            double minSeparationDist = double.PositiveInfinity;

            //#########################################
            // TBA
            //#########################################
            if (r.GetType() == this.GetType())
            {
                RB_Square c = (RB_Square)r;
                minSeparationDist = 0;
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
            return new Vector2(this.Position.X, (this.Position.X + _width) * (float)Math.Cos(_angle % 90) + (this.Position.Y + _height) * (float)Math.Sin(_angle % 90));
        }
        public override Vector2 downMostPoint() 
        {
            return new Vector2(this.Position.Y, (this.Position.X - _width) * (float)Math.Cos(_angle % 90) + (this.Position.Y - _height) * (float)Math.Sin(_angle % 90));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pointTexture == null)
            {
                pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Draw(pointTexture, new Rectangle((int)(64 * Position.X), (int)(64 * Position.Y), (int)_width, (int)_height), Color.Chocolate);
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
