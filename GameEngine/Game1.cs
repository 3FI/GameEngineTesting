using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class Game1 : Game
    {
        static public bool debug = true;
        static public int pxPerUnit = 64;
        static public bool isMouseVisible = true;

        private ContentManager baseContent;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        static public int screenWidth;
        static public int screenHeight;
        private static GameStates _gameState = GameStates.Playing;
        static public SpriteFont BaseFont;

        static KeyboardState _previous_kState;
        static KeyboardState _kState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            baseContent = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
            IsMouseVisible = isMouseVisible;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            //Makes the screen resolution public to the rest of the program
            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;

            //Makes the content pipeline public to the rest of the program
            Scene.SceneManager.content = Content;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            BaseFont = baseContent.Load<SpriteFont>("Ubuntu32");

            //Load the Default Scene
            Scene.Scene1.Play();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            _kState = Keyboard.GetState();
            
            IsMouseVisible = isMouseVisible;

            //Handle activation of the debug menu
            if ((_kState.IsKeyDown(Keys.F3) && !_previous_kState.IsKeyDown(Keys.F3))) debug = !debug;

            //Updates the current scene
            Scene.SceneManager.Update(gameTime);

            //Play all the sounds
            Sound.SoundManager.Update();

            _previous_kState = _kState;

            //Update gametime
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Set White Background
            GraphicsDevice.Clear(Color.GhostWhite);

            _spriteBatch.Begin();

            //If debug menu active, draw it
            if (debug)
            {
                //Draw the coordinate system
                float zoom;
                Vector2 cameraPosition;
                float cameraWidth;
                float cameraHeight;
                if (Scene.SceneManager.scene.Camera != null)
                {
                    zoom = Scene.SceneManager.scene.Camera.zoom;
                    cameraPosition = Scene.SceneManager.scene.Camera.position;
                    cameraWidth = Scene.SceneManager.scene.Camera.Width;
                    cameraHeight = Scene.SceneManager.scene.Camera.Height;
                }
                else
                {
                    zoom = 1f;
                    cameraPosition = new Vector2(Game1.screenWidth / 2 / Game1.pxPerUnit, Game1.screenHeight / 2 / Game1.pxPerUnit);
                    cameraWidth = Game1.screenWidth / Game1.pxPerUnit;
                    cameraHeight = Game1.screenHeight / Game1.pxPerUnit;
                }
                Texture2D pointTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });
                for (int i=0; i<Scene.SceneManager.scene.Width; i++) _spriteBatch.Draw(pointTexture, new Rectangle((int)(64 * zoom * (i - cameraPosition.X + cameraWidth / 2)), 0, 2, _graphics.PreferredBackBufferHeight + 2), Color.Blue);
                for (int i=0; i< Scene.SceneManager.scene.Height; i++) _spriteBatch.Draw(pointTexture, new Rectangle(0, (int)(64 * zoom * (i - cameraPosition.Y + cameraHeight / 2)), _graphics.PreferredBackBufferWidth + 2, 2), Color.Blue);
               
                //Draw the BVH
                if (Collision.Collision.Bvh != null)
                    Collision.Collision.Bvh.Draw(_spriteBatch);

                //Draw the current state of the game
                _spriteBatch.DrawString(BaseFont, _gameState.ToString(), new Vector2(0, _graphics.PreferredBackBufferHeight - 64), Color.Black);
            }

            //Turns the background grey if paused
            if (_gameState == GameStates.Paused)
            {
                GraphicsDevice.Clear(Color.Gray);
            }

            //Draw the scene
            Scene.SceneManager.Draw(_spriteBatch,gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public static bool pauseHandling()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (_kState.IsKeyDown(Keys.Escape) && !_previous_kState.IsKeyDown(Keys.Escape)))
                if (_gameState == GameStates.Playing) { _gameState = GameStates.Paused; isMouseVisible = true; }
                else if (_gameState == GameStates.Paused) { _gameState = GameStates.Playing; isMouseVisible = false; }
            return (_gameState == GameStates.Paused);
        }
    }
    public enum GameStates
    {
        Menu = 0,
        Playing = 1,
        Paused = 2
    }
}
