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
    static class SceneManager
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        static public Scene scene;
        static public ContentManager content;

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Load this scene as the new current scene
        /// </summary>
        /// <param name="Scene"></param>
        public static void Load(Scene Scene)
        {
            Unload();
            scene = Scene;
            foreach (GameObject gameobject in scene.Content)
            {
                //ANIMATIONMANAGER LOADING
                if (gameobject.animationManager != null)
                {
                    foreach (Animation animation in gameobject.animationDict.Values)
                    {
                        try { animation.Texture = content.Load<Texture2D>(animation.TextureAdress); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load texture " + animation.TextureAdress + " in animation " + animation + " in gameobject " + gameobject);
                            animation.Texture = content.Load<Texture2D>("SwordV2"); //TODO : Implement Correct PlaceHolder Texture
                        }
                    }
                    if (gameobject.animationManager.DefaultAnimation.Texture == null)
                    {
                        try { gameobject.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>(gameobject.animationManager.DefaultAnimation.TextureAdress); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load texture " + gameobject.animationManager.DefaultAnimation.TextureAdress + " as the default error handling animation of gameobject " + gameobject);
                            gameobject.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>("SwordV2"); //TODO : Implement Correct PlaceHolder Texture
                        }
                    }
                }

                //SPRITE LOADING
                else if (gameobject.Sprite != null)
                {
                    try { gameobject.Sprite.Texture = content.Load<Texture2D>(gameobject.Sprite.TextureAdress); }
                    catch (ContentLoadException)
                    {
                        System.Diagnostics.Debug.WriteLine("Unable to load texture " + gameobject.Sprite.TextureAdress + " in gameobject " + gameobject);
                        gameobject.Sprite.Texture = content.Load<Texture2D>("SwordV2"); //TODO : Implement Correct PlaceHolder Texture
                    }
                }

                //SOUND LOADING
                if (gameobject.Sounds != null)
                {
                    foreach (Sound.Sound sound in gameobject.Sounds.Values)
                    {
                        try { sound.SoundEffect = content.Load<SoundEffect>(sound.SoundEffectId); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load sound " + sound.SoundEffectId + " in gameobject " + gameobject);
                            sound.SoundEffect = content.Load<SoundEffect>("test2"); //TODO : Implement Correct PlaceHolder Sound
                        }
                    }
                }
            }

            //MAP SPRITES LOADING
            if (scene.map != null)
            {
                foreach (LinkedList<Sprite> layer in scene.map.sprites) foreach (Sprite sprite in layer)
                    {
                        try { sprite.Texture = content.Load<Texture2D>(sprite.TextureAdress); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load texture " + sprite.TextureAdress + " in map " + scene.map);
                            sprite.Texture = content.Load<Texture2D>("SwordV2"); //TODO : Implement Correct PlaceHolder Texture
                        }
                    }
            }
            //TODO : Implement AnimationManagers for map tiles
        }
        
        /// <summary>
        /// Dispose of the current scene
        /// </summary>
        public static void Unload()
        {
            if (scene != null) scene.Kill();
            //TODO : DISPOSE RESOURCES
        }

        /// <summary>
        /// Update all the components of the current scene
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            if (scene != null)
            {
                scene.Update(gameTime);
                foreach (GameObject gameobject in scene.Content)
                {
                    gameobject.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draw the current scene to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (scene != null)
            {
                //TODO ADD LAYER HANDLING
                if (scene.map != null) scene.map.Draw(spriteBatch,gameTime);
                scene.Draw(spriteBatch, gameTime);
                foreach (GameObject gameobject in scene.Content)
                {
                    gameobject.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}
