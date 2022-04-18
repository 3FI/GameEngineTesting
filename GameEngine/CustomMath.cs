using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
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
    }
}