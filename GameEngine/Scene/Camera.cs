using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Scene
{
    /// <summary>
    /// The camera object around which the scene is drawn when assigned to the said scene.
    /// By default follows the player around.
    /// </summary>
    public class Camera
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The default behavior of the camera. (Always follows the player)
        /// </summary>
        private static void _defaultBehavior(GameTime gameTime)
        {
            float lerping = 10f; 
            if(Player.Instance != null)
            {
                if (Player.Instance.Position.X - SceneManager.Scene.Camera.Width / 2 > 0 && Player.Instance.Position.X + SceneManager.Scene.Camera.Width / 2 < SceneManager.Scene.Width)
                    SceneManager.Scene.Camera.Position.X = Tools.CustomMath.Lerp(SceneManager.Scene.Camera.Position.X, Player.Instance.Position.X, lerping * (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (Player.Instance.Position.Y - SceneManager.Scene.Camera.Height / 2 > 0 && Player.Instance.Position.Y + SceneManager.Scene.Camera.Height / 2 < SceneManager.Scene.Height)
                    SceneManager.Scene.Camera.Position.Y = Tools.CustomMath.Lerp(SceneManager.Scene.Camera.Position.Y, Player.Instance.Position.Y, lerping * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        /// <summary>
        /// The behavior of the camera.
        /// </summary>
        public Action<GameTime> Behaviour = new Action<GameTime>(_defaultBehavior);
        /// <summary>
        /// Position of the camera.
        /// </summary>
        public Vector2 Position = new Vector2(0, 0);
        /// <summary>
        /// Zoom of the camera.
        /// </summary>
        public float Zoom;
        /// <summary>
        /// The zoom we want to interpolate to.
        /// </summary>
        private float _targetZoom;
        /// <summary>
        /// The time in seconds it takes for the zoom to rech _targetZoom
        /// </summary>
        private float _zoomTime = 1;
        /// <summary>
        /// The height of the screen in px. Used to stop the camera from going out of bounds
        /// </summary>
        private float _screenHeight = Game1.ScreenHeight;
        /// <summary>
        /// The width of the screen in px. Used to stop the camera from going out of bounds
        /// </summary>
        private float _screenWidth = Game1.ScreenWidth;
        /// <summary>
        /// The height of the screen in units.
        /// </summary>
        public float Height
        {
            get { return _screenHeight / Game1.pxPerUnit / Zoom; }
        }
        /// <summary>
        /// The width of the screen in units
        /// </summary>
        public float Width
        {
            get { return _screenWidth / Game1.pxPerUnit / Zoom; }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="position"> Initialization position of the camera /param>
        /// <param name="zoom"> Initialization zoom of the camera </param>
        public Camera(Vector2 position, float zoom = 1f)
        {
            this.Position = position;
            this.Zoom = zoom;
            _targetZoom = Zoom;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Zoom or dezoom by settinga new _targetZoom
        /// </summary>
        /// <param name="zoom">The new zoom</param>
        public void SetZoom(float zoom)
        {
            _targetZoom = zoom;
        }
        /// <summary>
        /// Zoom or dezoom at a certain speed by settinga new _targetZoom and _zoomTime
        /// </summary>
        /// <param name="zoom">The new zoom</param>
        /// <param name="time">The time it will take to reach this new zoom</param>
        public void SetZoom(float zoom, float time)
        {
            _targetZoom = zoom;
            _zoomTime = time;
        }
        /// <summary>
        /// Lerp the zoom towards _targetZoom. Called in the update method
        /// </summary>
        /// <param name="gameTime"></param>
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
