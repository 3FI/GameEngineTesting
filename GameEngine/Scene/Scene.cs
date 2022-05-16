using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    public abstract class Scene
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public int Width;
        public int Height;
        public Camera Camera;
        public Map map;
        public LinkedList<GameObject> Content;
        public Dictionary<String, Sound.Music> Musics;

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Dispose of this scene
        /// </summary>
        public abstract void Kill();

        /// <summary>
        /// Update the scene
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw the scene to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);


    }
}
