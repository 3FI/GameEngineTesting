using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    public abstract class Scene
    {
        public float Width;
        public float Height;
        public Camera Camera;
        internal Map map;

        public Dictionary<String, Sound.Music> Musics;
        public LinkedList<GameObject> Content;

        public abstract void Kill();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);


    }
}
