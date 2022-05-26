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
    public sealed class MainMenu : Scene
    {        
        private MainMenu()
        {
            /////////////////////////////////////////////////////////////////////////////////
            //                                  PROPERTIES                                 //
            /////////////////////////////////////////////////////////////////////////////////
            Width = 40;
            Height = 20;
            Ui = new LinkedList<UI.Component>();
            static void playButton(GameTime gameTime) { Scene1.Play(); }
            Ui.AddLast(new UI.Button(5, 10, new Vector2(6, 11), Game1.BaseFont, "TEST", Color.White, null) { OnClick = new Action<GameTime>(playButton) } );
            Ui.AddLast(new UI.Component(2, 2, new Vector2(2, 2)));
            Content = new LinkedList<GameObject>();
            Content.AddLast(new Obstacle(new Vector2(8, 8), new Vector2(0, 0), new Vector2(0, 0), "Texture2D/Test/SwordV1", new Collision.RB_Square(1, 1)));
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private static MainMenu instance = null;
        public static MainMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainMenu();
                }
                return instance;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public static void Play()
        {
            Game1.isMouseVisible = true;
            SceneManager.Load(MainMenu.Instance);
        }

        public override void Kill()
        {
            instance = null;
        }
        public override void Update(GameTime gameTime)
        {
            if (Ui != null)
                foreach (UI.Component component in Ui)
                {
                    component.Update(gameTime);
                }            // throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            // throw new NotImplementedException();
        }
    }
}

