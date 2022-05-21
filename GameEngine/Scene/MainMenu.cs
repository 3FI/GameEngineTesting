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
            Width = 40;
            Height = 20;

            /////////////////////////////////////////////////////////////////////////////////
            //                                  PROPERTIES                                 //
            /////////////////////////////////////////////////////////////////////////////////
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
            // throw new NotImplementedException();
        }
    }
}

