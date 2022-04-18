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

        public bool intersectBox(Box b)
        {
            return CustomMath.Max(this.bottomLeft.X, b.bottomLeft.X) < CustomMath.Min(this.topRight.X, b.topRight.X) && CustomMath.Min(this.topRight.Y, b.topRight.Y) > CustomMath.Max(this.bottomLeft.Y, b.bottomLeft.Y);
        }

        public bool isFullyInside(Box b)
        {
            return this.bottomLeft.X >= b.bottomLeft.X && this.topRight.X <= b.topRight.X && this.bottomLeft.Y >= b.bottomLeft.Y && this.topRight.Y <= b.topRight.Y;
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
            return this.bottomLeft.X + this.getWidth()/2f;
        }

        public double getMidY()
        {
            return this.bottomLeft.Y + this.getHeight()/2f;
        }

        public double getArea()
        {
            return this.getWidth() * this.getHeight();
        }

        public Vector2 getPosition()
        {
            return Vector2.Divide(Vector2.Add(this.bottomLeft, this.topRight), 2);
        }

        public override String ToString()
        {
            return "Box(bottomLeft: " + bottomLeft + ", topRight: " + topRight + ")";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (bottomLeft == ((Box)obj).bottomLeft) && (topRight == ((Box)obj).topRight);
            }
        }
    }
}
