using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    /// <summary>
    /// A tile of the map
    /// </summary>
    public class Tile
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Sprite of this tile (nullable : if so, AnimationManager isn't)
        /// </summary>
        public Graphics.Sprite Sprite;

        /// <summary>
        /// AnimationManager of this tile (nullable : if so, Sprite isn't)
        /// </summary>
        public Graphics.AnimationManager AnimationManager;
        /// <summary>
        /// Animation Dictionnary of this tile (nullable : if so, Sprite isn't)
        /// </summary>
        public Dictionary<String, Graphics.Animation> AnimationDict;
        
        /// <summary>
        /// Dictionnary of sounds that could be played. (nullable) NOT YET USED
        /// </summary>
        public Dictionary<String, Sound.Sound> Sounds;

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (AnimationManager != null)
                AnimationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (Sprite != null)
                Sprite.Draw(gameTime, spriteBatch);

            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);
        }

        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            //If there's an animationManager, draw the current frame
            if (AnimationDict != null)
            {
                foreach (Graphics.Animation animation in AnimationDict.Values)
                    if (!animation.Load(content)) result = false;
            }
            if (AnimationManager != null)
            {
                if (!AnimationManager.Load(content)) result = false;
            }

            //If it's a sprite instead, draws it
            else if (Sprite != null)
            {
                if (!Sprite.Load(content)) result = false;
            }
            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);

            if (!result) System.Diagnostics.Debug.WriteLine("Unable to load" + this);
            return result;
        }

        public void Dispose()
        {
            //If there's an animationManager, draw the current frame
            if (AnimationDict != null)
                foreach (Graphics.Animation animation in AnimationDict.Values)
                    animation.Texture.Dispose();
            if (AnimationManager != null)
                AnimationManager.DefaultAnimation.Texture.Dispose();

            //If it's a sprite instead, draws it
            else if (Sprite != null)
                Sprite.Texture.Dispose();
        }
    }
}
