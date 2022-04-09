using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GameEngine.Collision
{
    public class Circle : RigidBody
    {
        public float _radius;

        public Circle(float radius)
        {
            _radius = radius;
            this.Id = _idCount++;
        }

        public override Box getBoundingBox()
        {
            Vector2 bottomleft = Vector2.Subtract(this.Position, new Vector2(_radius));
            Vector2 topright = Vector2.Add(this.Position, new Vector2(_radius));
            return new Box(bottomleft, topright);
        }

        public override ContactResult isContacting(RigidBody r) 
        {
            Vector2 diff = Vector2.Subtract(this.Position, r.Position);
            double separationDist = Vector2.Distance(this.Position, r.Position);
            double minSeparationDist = double.PositiveInfinity;
            if (r.GetType() == this.GetType())
            {
                Circle c = (Circle)r;
                minSeparationDist = this._radius + c._radius;
            }
                

            if (separationDist < minSeparationDist)
            {
                return new ContactResult(this, r, separationDist - minSeparationDist, Vector2.Normalize(diff));
            }
            return null;
        }


        public override Vector2 leftMostPoint() 
        {
            return new Vector2(this.Position.X-this._radius, this.Position.Y);
        }
        public override Vector2 rightMostPoint() 
        {
            return new Vector2(this.Position.X + this._radius, this.Position.Y);
        }
        public override Vector2 upMostPoint() 
        {
            return new Vector2(this.Position.X, this.Position.Y-this._radius);
        }
        public override Vector2 downMostPoint() 
        {
            return new Vector2(this.Position.X, this.Position.Y + this._radius);
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X - (int)_radius, (int)Position.Y - (int)_radius, (int)_radius*2, (int)_radius*2), Color.Chocolate);
        }

        public bool isInside(Vector2 point)
        {
            return Vector2.Distance(point, this.Position) < this._radius;
        }

        public override String ToString()
        {
            return String.Format("Circle(id: {0}, position: {0}, radius: {0})", this.Id, this.Position.ToString(), this._radius);
        }
    }
}
