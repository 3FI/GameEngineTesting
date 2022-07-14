using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.Scene
{
    /// <summary>
    /// The main menu scene
    /// </summary>
    public sealed class MainMenu : Scene        //sealed means impossible to inherit
    {        
        /// <summary>
        /// The private constructor used since this is a singleton
        /// </summary>
        private MainMenu()
        {
            /////////////////////////////////////////////////////////////////////////////////
            //                                    CONTENT                                  //
            /////////////////////////////////////////////////////////////////////////////////
            
            Width = 40;
            Height = 20;

            Ui = new LinkedList<UI.Component>();
            static void playButton(GameTime gameTime) { Scene1.Play(); }
            Ui.AddLast(new UI.Button(2, 5, new Vector2(15, 8), Game1.BaseFont, "Play Plateformer", Color.White, null) { OnClick = new Action<GameTime>(playButton) } );

            static void playCardButton(GameTime gameTime) { CardGame.Play(); }
            Ui.AddLast(new UI.Button(2, 5, new Vector2(15, 12), Game1.BaseFont, "Play Card Game", Color.White, null) { OnClick = new Action<GameTime>(playCardButton) });

            Ui.AddLast(new UI.Component(2, 2, new Vector2(2, 2)));

            Content = new LinkedList<GameObject>();
            Content.AddLast(new Obstacle(new Vector2(8, 8), new Vector2(0, 0), new Vector2(0, 0), "Texture2D/Test/SwordV1", new Collision.RB_Square(1, 1)));
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The MainMenu instance
        /// </summary>
        private static MainMenu _instance = null;
        /// <summary>
        /// Access to the MainMenu instance. Creates it if null
        /// </summary>
        public static MainMenu Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainMenu();
                }
                return _instance;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Plays the scene
        /// </summary>
        public static void Play()
        {
            Game1.isMouseVisible = true;
            SceneManager.Load(MainMenu.Instance);
        }

        public override void Kill()
        {
            _instance = null;
        }
        public override void Update(GameTime gameTime)
        {
            if (Ui != null)
                foreach (UI.Component component in Ui)
                {
                    component.Update(gameTime);
                }                                                   // throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);                       // throw new NotImplementedException();
        }
    }
}

