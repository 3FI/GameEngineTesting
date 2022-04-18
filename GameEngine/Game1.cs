using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class Game1 : Game
    {
        private bool debug = false;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public LinkedList<GameObject> _gameObjectList = new LinkedList<GameObject>();
        private Player _ball;
        private GameStates _gameState = GameStates.Playing;
        SpriteFont Ubuntu32;

        //#########################################
        //REMOVE EVENTUALLY. IS USED TO DRAW HITBOX
        //#########################################
        private Texture2D whiteRectangle;

        KeyboardState _previous_kState;
        KeyboardState _kState;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            //Initialize the player
            _ball = new Player(
                new Vector2(_graphics.PreferredBackBufferWidth / 2 / 64, _graphics.PreferredBackBufferHeight / 2 / 64),
                new Vector2(0, 0), new Vector2(0, 12),
                new Dictionary<string, Animation>() 
                {
                    { "Default", new Animation("ball", 2, true) },
                },
                new GameEngine.Collision.RB_Circle(0.5f));

            _gameObjectList.AddLast(_ball);

            //Initalize an obstacle
            Obstacle obstacle = new Obstacle(new Vector2(_graphics.PreferredBackBufferWidth / 1.5f / 64, _graphics.PreferredBackBufferHeight / 1.5f / 64), new Vector2(0, 0), new Vector2(0, 0), "SwordV1", new GameEngine.Collision.RB_Circle(0.5f));
            
            _gameObjectList.AddLast(obstacle);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all texture and assign them to their objects
            foreach (GameObject gameobject in _gameObjectList)
            {
                if (gameobject.animationManager != null) foreach (Animation animation in gameobject.animationDict.Values) animation.Texture = Content.Load<Texture2D>(animation.TextureAdress);
                else if (gameobject.Sprite != null) gameobject.Sprite.Texture = Content.Load<Texture2D>(gameobject.Sprite.TextureAdress);
            }

            //#########################################
            //REMOVE EVENTUALLY. IS USED TO DRAW HITBOX
            //#########################################
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            Ubuntu32 = Content.Load<SpriteFont>("Ubuntu32");
        }

        protected override void Update(GameTime gameTime)
        {
            _kState = Keyboard.GetState();
            
            //Handle Pause and unpause
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (_kState.IsKeyDown(Keys.Escape) && !_previous_kState.IsKeyDown(Keys.Escape)))
                if (_gameState == GameStates.Playing) { _gameState = GameStates.Paused; IsMouseVisible = true; }
                else if (_gameState == GameStates.Paused) { _gameState = GameStates.Playing; IsMouseVisible = false; }

            if (_gameState == GameStates.Playing)
            {
                //#########################################
                // CHANGE THE PLAYER METHOD TO A STATIC ONE
                //#########################################
                _ball.Movement(gameTime);

                //#########################################
                // RUN IT FOR ALL ANIMATION MANAGER
                //#########################################
                _ball.animationManager.Update(gameTime);
                        
                //Handle collisions
                Collision.Collision.simulate(_gameObjectList,_graphics);

                //Update position and velocity 
                foreach (GameObject gameobject in _gameObjectList)
                {
                    gameobject.Position += gameobject.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + 0.5f * gameobject.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    gameobject.Velocity += gameobject.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }

            //Handle debug menu
            if ((_kState.IsKeyDown(Keys.F3) && !_previous_kState.IsKeyDown(Keys.F3))) debug = !debug;

            _previous_kState = _kState;

            //Update gametime
            if (_gameState == GameStates.Playing) base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            //Set White Background
            GraphicsDevice.Clear(Color.GhostWhite);

            _spriteBatch.Begin();
            foreach (GameObject gameobject in _gameObjectList)
            {
                //If there's an animationManager, draw the current frame
                if (gameobject.animationManager != null)
                    gameobject.animationManager.Draw(_spriteBatch);

                //If it's a sprite instead, draws it
                else if (gameobject.Sprite != null)
                    gameobject.Sprite.Draw(gameTime,_spriteBatch);

                //If debug menu active, draw the rigid body hitbox to the screen.
                if (debug && gameobject.Rigidbody != null) gameobject.Rigidbody.Draw(_spriteBatch, whiteRectangle);
            }

            //If debug menu active, draw the player position to the screen
            if (debug)
            {
                _spriteBatch.DrawString(Ubuntu32, "xPosition:" + _ball.Position.X.ToString(), new Vector2(0, 0), Color.Black);
                _spriteBatch.DrawString(Ubuntu32, "yPosition:" + _ball.Position.Y.ToString(), new Vector2(0, 32), Color.Black);
                _spriteBatch.DrawString(Ubuntu32, "xVelocity:" + _ball.Velocity.X.ToString(), new Vector2(0, 64), Color.Black);
                _spriteBatch.DrawString(Ubuntu32, "yVelocity:" + _ball.Velocity.Y.ToString(), new Vector2(0, 96), Color.Black);
                _spriteBatch.DrawString(Ubuntu32, _gameState.ToString(), new Vector2(0, _graphics.PreferredBackBufferHeight - 64), Color.Black);
            }

            //Turns the background grey if paused
            if (_gameState == GameStates.Paused)
            {
                GraphicsDevice.Clear(Color.Gray);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
    public enum GameStates
    {
        Menu = 0,
        Playing = 1,
        Paused = 2
    }
}
