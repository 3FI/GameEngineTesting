using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Scene
{
    public class Camera
    {
        public Vector2 position = new Vector2(0, 0);
        public float zoom;
        private float screenHeight = Game1.screenHeight;
        private float screenWidth = Game1.screenWidth;

        public float Height
        {
            get { return screenHeight / 64 / zoom; }
        }
        public float Width
        {
            get { return screenWidth / 64 / zoom; }
        }
        public Camera(Vector2 Position, float Zoom = 1f)
        {
            position = Position;
            zoom = Zoom;
        }
    }
}
