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
                    SceneManager.scene.Camera.Position.X = Tools.CustomMath.Lerp(SceneManager.scene.Camera.Position.X, Player.Instance.Position.X, lerping * (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (Player.Instance.Position.Y - SceneManager.scene.Camera.Height / 2 > 0 && Player.Instance.Position.Y + SceneManager.scene.Camera.Height / 2 < SceneManager.scene.Height)
                    SceneManager.scene.Camera.Position.Y = Tools.CustomMath.Lerp(SceneManager.scene.Camera.Position.Y, Player.Instance.Position.Y, lerping * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        //TODO :ADD CUSTOM INTERPOLATION FOR ZOOM
        public Action<GameTime> Behaviour = new Action<GameTime>(_defaultBehavior);
        public Vector2 Position = new Vector2(0, 0);
        public float Zoom;
        private float _targetZoom;
        private float _zoomTime = 1;
        private float screenHeight = Game1.ScreenHeight;
        private float screenWidth = Game1.ScreenWidth;

        public float Height
        {
            get { return screenHeight / Game1.pxPerUnit / Zoom; }
        }
        public float Width
        {
            get { return screenWidth / Game1.pxPerUnit / Zoom; }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Camera(Vector2 position, float zoom = 1f)
        {
            this.Position = position;
            this.Zoom = zoom;
            _targetZoom = Zoom;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////
        public void SetZoom(float zoom)
        {
            _targetZoom = zoom;
        }
        public void SetZoom(float zoom, float time)
        {
            _targetZoom = zoom;
            _zoomTime = time;
        }
        private void _zoom(GameTime gameTime)
        {
            Zoom = Tools.CustomMath.Lerp(Zoom, _targetZoom, (float)gameTime.ElapsedGameTime.TotalSeconds / _zoomTime);
            //TODO : ADD MOVE CAMERA IF ZOOMING OUT OF MAP
        }
        public void Update(GameTime gameTime)
        {
            _zoom(gameTime);
            Behaviour?.Invoke(gameTime);
        }
        public override String ToString()
        {
            string result = "Camera(\n\tPosition: " + Position + ", \n\tZoom: " + Zoom + "\n)";
            return result;
        }
    }
}
