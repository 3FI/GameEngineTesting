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
        private Texture2D whiteRectangle;
        SpriteFont Ubuntu32;

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
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            _ball = new Player(new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), new Vector2(0, 0), new Vector2(0, 800),"ball", new GameEngine.Collision.Circle(32f));
            _gameObjectList.AddLast(_ball);
            Obstacle obstacle = new Obstacle(new Vector2(_graphics.PreferredBackBufferWidth / 1.5f, _graphics.PreferredBackBufferHeight / 1.5f), new Vector2(0, 0), new Vector2(0, 0), "ball", new GameEngine.Collision.Circle(32f));
            _gameObjectList.AddLast(obstacle);

            //movmentVelocity = 2000f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            foreach (GameObject gameobject in _gameObjectList)
            {
                gameobject.Texture = Content.Load<Texture2D>(gameobject.TextureAdress);
            }

            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            Ubuntu32 = Content.Load<SpriteFont>("Ubuntu32");
        }

        protected override void Update(GameTime gameTime)
        {
            _kState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (_kState.IsKeyDown(Keys.Escape) && !_previous_kState.IsKeyDown(Keys.Escape)))
                if (_gameState == GameStates.Playing) { _gameState = GameStates.Paused; IsMouseVisible = true; }
                else if (_gameState == GameStates.Paused) { _gameState = GameStates.Playing; IsMouseVisible = false; }

                    if (_gameState == GameStates.Playing)
                    {
                        // TODO: Add your update logic here
                        _ball.Movement(gameTime);
                        Collision.Collision.simulate(_gameObjectList,_graphics);
                        foreach (GameObject gameobject in _gameObjectList)
                        {
                            //gameobject.acceleration.X *= MathHelper.Clamp(1 - Math.Abs(gameobject.Velocity.X / 200f), 0, 1);

                            gameobject.Position += gameobject.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + 0.5f * gameobject.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            gameobject.Velocity += gameobject.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                    }

                    _previous_kState = _kState;
                    if (_gameState == GameStates.Playing) base.Update(gameTime);
                }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.GhostWhite);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            foreach (GameObject gameobject in _gameObjectList)
            {
                _spriteBatch.Draw
                    (
                         /*texture*/                 gameobject.Texture,
                         /*destinationRectangle*/    gameobject.Position,
                         /*sourceRectangle*/         null,
                         /*color*/                   Color.White,
                         /*rotation*/                0f,
                         /*origin*/                  new Vector2(gameobject.Texture.Width / 2, gameobject.Texture.Height / 2),
                         /*effects*/                 Vector2.One,
                         /*layerDepth*/              SpriteEffects.None,
                         0f
                   );
                if (debug && gameobject.Rigidbody != null) gameobject.Rigidbody.Draw(_spriteBatch, whiteRectangle);
            }

            _spriteBatch.DrawString(Ubuntu32, "xVelocity:"+_ball.Velocity.X.ToString(), new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(Ubuntu32, _gameState.ToString(), new Vector2(0,_graphics.PreferredBackBufferHeight-100), Color.Black);

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
