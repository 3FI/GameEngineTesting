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

        private static void _defaultBehavior(GameTime gameTime)
        {
            float lerping = 10f; 
            if(Player.Instance != null)
            {
                if (Player.Instance.Position.X - SceneManager.scene.Camera.Width / 2 > 0 && Player.Instance.Position.X + SceneManager.scene.Camera.Width / 2 < SceneManager.scene.Width)
                    SceneManager.scene.Camera.position.X = Tools.CustomMath.Lerp(SceneManager.scene.Camera.position.X, Player.Instance.Position.X, lerping * (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (Player.Instance.Position.Y - SceneManager.scene.Camera.Height / 2 > 0 && Player.Instance.Position.Y + SceneManager.scene.Camera.Height / 2 < SceneManager.scene.Height)
                    SceneManager.scene.Camera.position.Y = Tools.CustomMath.Lerp(SceneManager.scene.Camera.position.Y, Player.Instance.Position.Y, lerping * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        public Action<GameTime> Behaviour = new Action<GameTime>(_defaultBehavior);
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
            Behaviour?.Invoke(gameTime);
        }
        public override String ToString()
        {
            string result = "Camera(\n\tPosition: " + position + ", \n\tZoom: " + zoom + "\n)";
            return result;
        }
    }
}
