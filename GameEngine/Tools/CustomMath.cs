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
        public static float Abs(float value)
        {
            if (value < 0) value *= -1;
            return value;
        }
        public static float Max(float value1, float value2)
        {
            if (value1 <= value2) return value2;
            else return value1;
        }
        public static float Min(float value1, float value2)
        {
            if (value2 <= value1) return value2;
            else return value1;
        }
        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        public static Vector2 Project(Vector2 point, Vector2 axis)
        {
            float x = (point.X*axis.X+point.Y*axis.Y)/((float)Math.Pow(axis.X,2)+ (float)Math.Pow(axis.Y, 2)) * axis.X;
            float y = (point.X * axis.X + point.Y * axis.Y) / ((float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2)) * axis.Y;
            return new Vector2(x,y);
        }
        public static Vector2 RotateAround(Vector2 point, Vector2 origin, float angle)
        {
            float x = (point.X - origin.X) * (float)Math.Cos(angle / 360 * 2 * Math.PI) - (point.Y - origin.Y) * (float)Math.Sin(angle / 360 * 2 * Math.PI) + origin.X;
            float y = (point.X - origin.X) * (float)Math.Sin(angle / 360 * 2 * Math.PI) + (point.Y - origin.Y) * (float)Math.Cos(angle / 360 * 2 * Math.PI) + origin.Y;
            return new Vector2(x, y);
        }
    }
}