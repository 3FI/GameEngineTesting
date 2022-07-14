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
        /// <summary>
        /// Static boolean that determines wether or not the debug menu is currently active
        /// </summary>
        static public bool debug = true;
        /// <summary>
        /// The number of pixel per units the game uses
        /// </summary>
        static public int pxPerUnit = 64;
        /// <summary>
        /// Static boolean that determines wether or not the cursor is visible. The value is then actualized to the current game in the Update method of this.
        /// </summary>
        static public bool isMouseVisible = true;

        /// <summary>
        /// This is the content that will NOT be unloaded between the scenes you want to place in here everything that might not be loaded with the scenes such as fonts
        /// </summary>
        private ContentManager baseContent;

        /// <summary>
        /// The Graphic Device Manager of this Game
        /// </summary>
        private GraphicsDeviceManager _graphics;
        /// <summary>
        /// The Spritebatch of this game
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Width of the screen in pixel, accessed trough <paramref name="ScreenWidth"/>
        /// </summary>
        static private int _screenWidth;
        /// <summary>
        /// Height of the screen in pixel, accessed trough <paramref name="ScreenHeight"/>
        /// </summary>
        static private int _screenHeight;
        /// <summary>
        /// Width of the screen in pixel
        /// </summary>
        static public int ScreenWidth
        {
            get { return _screenWidth; }
        }
        /// <summary>
        /// Height of the screen in pixel
        /// </summary>
        static public int ScreenHeight
        {
            get { return _screenHeight; }
        }

        /// <summary>
        /// Current gamestate (Menu = 0, Playing = 1, Paused = 2)
        /// </summary>
        private static GameStates _gameState = GameStates.Playing;

        /// <summary>
        /// This is the base font that is used as a fallback if the font cannot be loaded
        /// </summary>
        static public SpriteFont BaseFont;

        //The following input handling is used to activate the debug menu

        /// <summary>
        /// Current state of the keyboard. Used to get which keys are pressed. Kept tracked in Update method.
        /// </summary>
        static private KeyboardState _kstate = new KeyboardState();
        /// <summary>
        /// Current state of the controller. Used to get which buttons are pressed. Kept tracked in Update method.
        /// </summary>
        static private GamePadState _gstate = new GamePadState();
        /// <summary>
        /// Previous state of the keyboard. Used to get which keys are released. Kept tracked in Update method.
        /// </summary>
        static private KeyboardState _previouskstate;
        /// <summary>
        /// Previous state of the controller. Used to get which buttons are released. Kept tracked in Update method.
        /// </summary>
        static private GamePadState _previousgstate;


        public Game1()
        {
            //Initialize the Graphic Devide Manager
            _graphics = new GraphicsDeviceManager(this);

            //Set the Content Directory
            Content.RootDirectory = "Content";

            //Create the BaseContent Content manager
            baseContent = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            //Initialize the status of the cursor
            IsMouseVisible = isMouseVisible;
        }

        protected override void Initialize()
        {
            //Sets the prograp to the right screen resolution and makes it public to the rest of the program
            _screenWidth = _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _screenHeight = _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            _graphics.IsFullScreen = false;

            _graphics.ApplyChanges();

            //Makes the main content pipeline public to the rest of the program
            Scene.SceneManager.Content = Content;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loads the default font
            BaseFont = baseContent.Load<SpriteFont>("Ubuntu32");

            //Initialize the SpriteBatch
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the Main Menu Scene
            Scene.MainMenu.Play();
        }

        protected override void Update(GameTime gameTime)
        {
            //Actualize the input
            _kstate = Keyboard.GetState();
            _gstate = GamePad.GetState(0);
            
            //Updated the status of the cursor
            IsMouseVisible = isMouseVisible;

            //Handle activation of the debug menu
            if ((_kstate.IsKeyDown(Keys.F3) && !_previouskstate.IsKeyDown(Keys.F3))) debug = !debug;

            //Updates the current scene
            Scene.SceneManager.Update(gameTime);

            //Play all the sounds
            Sound.SoundManager.Update();

            _previouskstate = _kstate;
            _previousgstate = _gstate;

            //Does all the scheduled interpolations
            Tools.InterpolationManager.Update(gameTime);

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

                //Gets the camera specificity
                float zoom;
                Vector2 cameraPosition;
                float cameraWidth;
                float cameraHeight;
                if (Scene.SceneManager.Scene.Camera != null)
                {
                    zoom = Scene.SceneManager.Scene.Camera.Zoom;
                    cameraPosition = Scene.SceneManager.Scene.Camera.Position;
                    cameraWidth = Scene.SceneManager.Scene.Camera.Width;
                    cameraHeight = Scene.SceneManager.Scene.Camera.Height;
                }
                else
                {
                    zoom = 1f;
                    cameraPosition = new Vector2(Game1.ScreenWidth / 2 / Game1.pxPerUnit, Game1.ScreenHeight / 2 / Game1.pxPerUnit);
                    cameraWidth = Game1.ScreenWidth / Game1.pxPerUnit;
                    cameraHeight = Game1.ScreenHeight / Game1.pxPerUnit;
                }

                //Initialize a single pixel texture
                Texture2D pointTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });

                //Draw the coordinates lines 
                for (int i=0; i<Scene.SceneManager.Scene.Width; i++) _spriteBatch.Draw(pointTexture, new Rectangle((int)(pxPerUnit * zoom * (i - (cameraPosition.X - cameraWidth / 2))), 0, 2, _graphics.PreferredBackBufferHeight + 2), Color.Blue);
                for (int i=0; i< Scene.SceneManager.Scene.Height; i++) _spriteBatch.Draw(pointTexture, new Rectangle(0, (int)(pxPerUnit * zoom * (i - (cameraPosition.Y - cameraHeight / 2))), _graphics.PreferredBackBufferWidth + 2, 2), Color.Blue);
               
                //Draw the BVH
                if (Collision.Collision.Bvh != null)
                    Collision.Collision.Bvh.Draw(_spriteBatch);

                //Draw the current state of the game
                _spriteBatch.DrawString(BaseFont, _gameState.ToString(), new Vector2(0, _graphics.PreferredBackBufferHeight - pxPerUnit), Color.Black);
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

        //Function that some scenes call to handle pause and unpause
        public static bool pauseHandling()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (_kstate.IsKeyDown(Keys.Escape) && !_previouskstate.IsKeyDown(Keys.Escape)))
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
