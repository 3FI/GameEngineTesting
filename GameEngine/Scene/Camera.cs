using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Scene
{
    public class Camera
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private static void _defaultBehavior()
        {
            if(Player.Instance != null)
            {
                if (Player.Instance.Position.X - SceneManager.scene.Camera.Width / 2 > 0 && Player.Instance.Position.X + SceneManager.scene.Camera.Width / 2 < SceneManager.scene.Width)
                    SceneManager.scene.Camera.position.X = Player.Instance.Position.X;
                if (Player.Instance.Position.Y - SceneManager.scene.Camera.Height / 2 > 0 && Player.Instance.Position.Y + SceneManager.scene.Camera.Height / 2 < SceneManager.scene.Height)
                    SceneManager.scene.Camera.position.Y = Player.Instance.Position.Y;
            }
        }
        public Action Behaviour = new Action(_defaultBehavior);
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

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Camera(Vector2 Position, float Zoom = 1f)
        {
            position = Position;
            zoom = Zoom;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public void Update(GameTime gameTime)
        {
            Behaviour?.Invoke();
        }
        public override String ToString()
        {
            string result = "Camera(\n\tPosition: " + position + ", \n\tZoom: " + zoom + "\n)";
            return result;
        }
    }
}
