﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GameEngine.Collision
{
    public class RB_Circle : RigidBody
    {
        public float _radius;

        public RB_Circle(float radius)
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
                RB_Circle c = (RB_Circle)r;
                minSeparationDist = this._radius + c._radius;
            }

            else if (r is RB_Square)
            {
                RB_Square c = (RB_Square)r;
                minSeparationDist = 0;
            }

            else
            {
                minSeparationDist = 0;
                System.Diagnostics.Debug.WriteLine("Collision not implemented between types Circle and" + r.GetType());
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
            return new Vector2(this.Position.X, this.Position.Y - this._radius);
        }
        public override Vector2 downMostPoint() 
        {
            return new Vector2(this.Position.X, this.Position.Y + this._radius);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pointTexture == null)
            {
                pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Draw(pointTexture, new Rectangle((int)(64*Position.X) - (int)(64*_radius), (int)(64 * Position.Y) - (int)(64*_radius), (int)(64*_radius)*2, (int)(64*_radius)*2), Color.Red);
        }

        public override String ToString()
        {
            return "RB_Circle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tRadius: " + _radius + ", \n\tId: " + Id + "\n)";
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
