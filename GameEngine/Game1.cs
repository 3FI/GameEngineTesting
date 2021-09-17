using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class Game1 : Game
    {
        //float movmentSpeed;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<GameObject> gameobjectlist;
        private GameObject ball;
        SpriteFont Ubuntu32;


        private KeyboardState kstate = new KeyboardState();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            ball = new GameObject();
            gameobjectlist = new List<GameObject> { ball };
            ball.position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ball.speed = new Vector2 (0,0);
            ball.acceleration = new Vector2(0, 800);
            //movmentSpeed = 2000f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball.texture = Content.Load<Texture2D>("ball");
            Ubuntu32 = Content.Load<SpriteFont>("Ubuntu32");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var previouskstate = kstate;
            kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up) && !previouskstate.IsKeyDown(Keys.Up))
                ball.speed.Y = -30000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                ball.speed = Vector2.Lerp(ball.speed, new Vector2(-250, ball.speed.Y), 10f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //    ball.acceleration.X += -movmentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                ball.speed = Vector2.Lerp(ball.speed, new Vector2(250, ball.speed.Y),10f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //    ball.acceleration.X += movmentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right) || kstate.IsKeyDown(Keys.Left) && kstate.IsKeyDown(Keys.Right))
                ball.speed = Vector2.Lerp(ball.speed, new Vector2(0, ball.speed.Y), 1f * (float)gameTime.ElapsedGameTime.TotalSeconds);


            if (ball.position.X > _graphics.PreferredBackBufferWidth - ball.texture.Width / 2)
                ball.position.X = _graphics.PreferredBackBufferWidth - ball.texture.Width / 2;
            
            else if (ball.position.X < ball.texture.Width / 2)
                ball.position.X = ball.texture.Width / 2;

            if (ball.position.Y > _graphics.PreferredBackBufferHeight - ball.texture.Height / 2)
                ball.position.Y = _graphics.PreferredBackBufferHeight - ball.texture.Height / 2;
            
            else if (ball.position.Y < ball.texture.Height / 2)
                ball.position.Y = ball.texture.Height / 2;

            foreach (GameObject gameobject in gameobjectlist)
            {

                //gameobject.acceleration.X *= MathHelper.Clamp(1 - Math.Abs(gameobject.speed.X / 200f), 0, 1);

                gameobject.position += gameobject.speed * (float)gameTime.ElapsedGameTime.TotalSeconds + 0.5f * gameobject.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds;
                gameobject.speed += gameobject.acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            foreach (GameObject gameobject in gameobjectlist)
            {
                _spriteBatch.Draw
                    (
                         /*texture*/                 gameobject.texture,
                         /*destinationRectangle*/    gameobject.position,
                         /*sourceRectangle*/         null,
                         /*color*/                   Color.White,
                         /*rotation*/                0f,
                         /*origin*/                  new Vector2(gameobject.texture.Width / 2, gameobject.texture.Height / 2),
                         /*effects*/                 Vector2.One,
                         /*layerDepth*/              SpriteEffects.None,
                         0f
                   );
            }

            _spriteBatch.DrawString(Ubuntu32, ball.speed.X.ToString(), new Vector2(0, 0), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
