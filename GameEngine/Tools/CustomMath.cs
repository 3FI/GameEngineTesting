using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Tools
{
    public static class CustomMath
    {
        /// <summary>
        /// [Insert Summary]
        /// </summary>
        /// <param name="value"> [Insert Parameter Description] </param>
        /// <returns> [Insert Return Description] </returns>
        public static double Abs(double value)
        {
            if (value < 0) value *= -1;
            return value;
        }
        /// <summary>
        /// [Insert Summary]
        /// </summary>
        /// <param name="value1"> [Insert Parameter Description] </param>
        /// <param name="value2"> [Insert Parameter Description] </param>
        /// <returns> [Insert Return Description] </returns>
        public static float Max(float value1, float value2)
        {
            if (value1 <= value2) return value2;
            else return value1;
        }
        /// <summary>
        /// [Insert Summary]
        /// </summary>
        /// <param name="value1"> [Insert Parameter Description] </param>
        /// <param name="value2"> [Insert Parameter Description] </param>
        /// <returns> [Insert Return Description] </returns>
        public static float Min(float value1, float value2)
        {
            if (value2 <= value1) return value2;
            else return value1;
        }
        /// <summary>
        /// [Insert Summary]
        /// </summary>
        /// <param name="firstFloat"> [Insert Parameter Description] </param>
        /// <param name="secondFloat"> [Insert Parameter Description] </param>
        /// <param name="by"> [Insert Parameter Description] </param>
        /// <returns> [Insert Return Description] </returns>
        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        /// <summary>
        /// Project a 2D point onto an axis
        /// </summary>
        /// <param name="point"> 2D point to be projected on the axis </param>
        /// <param name="axis"> The axis the point will be projected onto </param>
        /// <returns> The coordinates of the projection point on the axis as a vector2 </returns>
        public static Vector2 Project(Vector2 point, Vector2 axis)
        {
            float x = (point.X*axis.X+point.Y*axis.Y)/((float)Math.Pow(axis.X,2)+ (float)Math.Pow(axis.Y, 2)) * axis.X;
            float y = (point.X * axis.X + point.Y * axis.Y) / ((float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2)) * axis.Y;
            return new Vector2(x,y);
        }
        /// <summary>
        /// Rotate a point along a new origin
        /// </summary>
        /// <param name="point"> The point we want to rotate </param>
        /// <param name="origin"> The point along which we want to rotate </param>
        /// <param name="angle"> The angle by which we rotate the point </param>
        /// <returns> The coordinate of the rotated point as a vector2 </returns>
        public static Vector2 RotateAround(Vector2 point, Vector2 origin, float angle)
        {
            float x = (point.X - origin.X) * (float)Math.Cos(angle / 360 * 2 * Math.PI) - (point.Y - origin.Y) * (float)Math.Sin(angle / 360 * 2 * Math.PI) + origin.X;
            float y = (point.X - origin.X) * (float)Math.Sin(angle / 360 * 2 * Math.PI) + (point.Y - origin.Y) * (float)Math.Cos(angle / 360 * 2 * Math.PI) + origin.Y;
            return new Vector2(x, y);
        }
    }
}