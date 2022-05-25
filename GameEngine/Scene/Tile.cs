using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    public class Tile
    {
        public Graphics.Sprite sprites;

        public Graphics.AnimationManager animationManager;
        public Dictionary<String, Graphics.Animation> animationDict;

        public Dictionary<String, Sound.Sound> sounds;

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (animationManager != null)
                animationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (sprites != null)
                sprites.Draw(gameTime, spriteBatch);

            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);
        }

        public void Load(ContentManager content)
        {
            //If there's an animationManager, draw the current frame
            if (animationDict != null)
                foreach (Graphics.Animation animation in animationDict.Values)
                    animation.Load(content);
            if (animationManager != null)
                animationManager.Load(content);

            //If it's a sprite instead, draws it
            else if (sprites != null)
                sprites.Load(content);

            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);
        }

        public void Dispose()
        {
            //If there's an animationManager, draw the current frame
            if (animationDict != null)
                foreach (Graphics.Animation animation in animationDict.Values)
                    animation.Texture.Dispose();
            if (animationManager != null)
                animationManager.DefaultAnimation.Texture.Dispose();

            //If it's a sprite instead, draws it
            else if (sprites != null)
                sprites.Texture.Dispose();
        }
    }
}
