using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public static class Math
    {
        public static float Abs(float value)
        {
            if (value < 0) value *= -1;
            return value;
        }
    }
}