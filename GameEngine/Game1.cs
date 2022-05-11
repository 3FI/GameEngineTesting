﻿using Microsoft.Xna.Framework;
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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public LinkedList<GameObject> _gameObjectList = new LinkedList<GameObject>();
        static public int screenWidth;
        static public int screenHeight;
        private GameStates _gameState = GameStates.Playing;
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
            _graphics.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;

            Scene.SceneManager.content = Content;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scene.SceneManager.Load(Scene.Scene1.Instance);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Ubuntu32 = Content.Load<SpriteFont>("Ubuntu32");
        }

        protected override void Update(GameTime gameTime)
        {
            _kState = Keyboard.GetState();
            
            //Handle Pause and unpause
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (_kState.IsKeyDown(Keys.Escape) && !_previous_kState.IsKeyDown(Keys.Escape)))
                if (_gameState == GameStates.Playing) { _gameState = GameStates.Paused; IsMouseVisible = true; }
                else if (_gameState == GameStates.Paused) { _gameState = GameStates.Playing; IsMouseVisible = false; }

            //Handle activation of the debug menu
            if ((_kState.IsKeyDown(Keys.F3) && !_previous_kState.IsKeyDown(Keys.F3))) debug = !debug;

            _previous_kState = _kState;

            Sound.SoundManager.Update();

            Scene.SceneManager.Update(gameTime);

            //Update gametime
            if (_gameState == GameStates.Playing) base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Set White Background
            GraphicsDevice.Clear(Color.GhostWhite);

            _spriteBatch.Begin();

            //If debug menu active, draw it
            if (debug)
            {                
                Texture2D pointTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                pointTexture.SetData<Color>(new Color[] { Color.White });
                for (int i=0; i<Scene.SceneManager.scene.Width; i++) _spriteBatch.Draw(pointTexture, new Rectangle((int)(64 * Scene.SceneManager.scene.Camera.zoom * (i - Scene.SceneManager.scene.Camera.position.X + Scene.SceneManager.scene.Camera.Width / 2)), 0, 2, _graphics.PreferredBackBufferHeight + 2), Color.Blue);
                for (int i=0; i< Scene.SceneManager.scene.Height; i++) _spriteBatch.Draw(pointTexture, new Rectangle(0, (int)(64 * Scene.SceneManager.scene.Camera.zoom * (i - Scene.SceneManager.scene.Camera.position.Y + Scene.SceneManager.scene.Camera.Height / 2)), _graphics.PreferredBackBufferWidth + 2, 2), Color.Blue);
               
                Collision.Collision.Bvh.Draw(_spriteBatch);
                _spriteBatch.DrawString(Ubuntu32, _gameState.ToString(), new Vector2(0, _graphics.PreferredBackBufferHeight - 64), Color.Black);
            }

            //Turns the background grey if paused
            if (_gameState == GameStates.Paused)
            {
                GraphicsDevice.Clear(Color.Gray);
            }

            Scene.SceneManager.Draw(_spriteBatch,gameTime);

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
