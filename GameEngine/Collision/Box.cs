using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Collision
{
    public class Box
    {
        public Vector2 topLeft;
        public Vector2 bottomRight;

        public Box(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        public bool intersectBox(Box b)
        {
            return CustomMath.Max(this.topLeft.X, b.topLeft.X) < CustomMath.Min(this.bottomRight.X, b.bottomRight.X) && CustomMath.Min(this.bottomRight.Y, b.bottomRight.Y) > CustomMath.Max(this.topLeft.Y, b.topLeft.Y);
        }

        public bool isFullyInside(Box b)
        {
            return this.topLeft.X >= b.topLeft.X && this.bottomRight.X <= b.bottomRight.X && this.topLeft.Y >= b.topLeft.Y && this.bottomRight.Y <= b.bottomRight.Y;
        }

        public double getWidth()
        {
            return this.bottomRight.X - this.topLeft.X;
        }

        public double getHeight()
        {
            return this.bottomRight.Y - this.topLeft.Y;
        }

        public double getMidX()
        {
            return this.topLeft.X + this.getWidth()/2f;
        }

        public double getMidY()
        {
            return this.topLeft.Y + this.getHeight()/2f;
        }

        public double getArea()
        {
            return this.getWidth() * this.getHeight();
        }

        public Vector2 getPosition()
        {
            return Vector2.Divide(Vector2.Add(this.topLeft, this.bottomRight), 2);
        }

        static private Texture2D _pointTexture;
        public void Draw(SpriteBatch spriteBatch, Color color, int lineWidth)
        {
            if(_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle((int)(64 * this.topLeft.X), (int)(64 * this.topLeft.Y), lineWidth, (int)(64 * this.bottomRight.Y) - (int)(64 * this.topLeft.Y) + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(64 * this.topLeft.X), (int)(64 * this.topLeft.Y), (int)(64 * this.bottomRight.X) - (int)(64 * this.topLeft.X) + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(64 * this.bottomRight.X), (int)(64 * this.topLeft.Y), lineWidth, (int)(64 * this.bottomRight.Y) - (int)(64 * this.topLeft.Y) + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(64 * this.topLeft.X), (int)(64 * this.bottomRight.Y), (int)(64 * this.bottomRight.X) - (int)(64 * this.topLeft.X) + lineWidth, lineWidth), color);
        }

        public override String ToString()
        {
            return "Box(topLeft: " + topLeft + ", bottomRight: " + bottomRight + ")";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (topLeft == ((Box)obj).topLeft) && (bottomRight == ((Box)obj).bottomRight);
            }
        }
    }
}
