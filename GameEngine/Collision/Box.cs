using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Collision
{
    public class Box
    {
        public Vector2 bottomLeft;
        public Vector2 topRight;

        public Box(Vector2 bottomLeft, Vector2 topRight)
        {
            this.bottomLeft = bottomLeft;
            this.topRight = topRight;
        }

        public String toString()
        {
            return "Box(bottomLeft: " + bottomLeft + ", topRight: " + topRight + ")";
        }

        // returns true if this and b are overlapping
        public bool intersectBox(Box b)
        {
            return Math.Max(this.bottomLeft.X, b.bottomLeft.X) < Math.Min(this.topRight.X, b.topRight.X) && Math.Min(this.topRight.Y, b.topRight.Y) > Math.Max(this.bottomLeft.Y, b.bottomLeft.Y);
        }

        // returns true if this is fully inside of b
        public bool isFullyInside(Box b)
        {
            return this.bottomLeft.X >= b.bottomLeft.X && this.topRight.X <= b.topRight.X && this.bottomLeft.Y >= b.bottomLeft.Y && this.topRight.Y <= b.topRight.Y;
        }

        public Box getBoundingBox()
        {
            return this;
        }

        public double getWidth()
        {
            return this.topRight.X - this.bottomLeft.X;
        }

        public double getHeight()
        {
            return this.topRight.Y - this.bottomLeft.Y;
        }

        public double getMidX()
        {
            return this.bottomLeft.X + this.getWidth() / 2f;
        }

        public double getMidY()
        {
            return this.bottomLeft.Y + this.getHeight() / 2f;
        }

        public double getArea()
        {
            return this.getWidth() * this.getHeight();
        }

        public Vector2 getPosition()
        {
            return Vector2.Divide(Vector2.Add(this.bottomLeft, this.topRight), 2);
        }
    }
}
