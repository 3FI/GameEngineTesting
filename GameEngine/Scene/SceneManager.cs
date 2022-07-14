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
    /// Static class that manages the current scene and loads and unloads scene when played.
    /// </summary>
    static class SceneManager
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The current scene (Initialized from Game1)
        /// </summary>
        static public Scene Scene;
        /// <summary>
        /// The main content manager (Set from Game1)
        /// </summary>
        static public ContentManager Content;

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Load this scene as the new current scene
        /// </summary>
        /// <param name="scene"></param>
        public static void Load(Scene scene)
        {
            //Unload Content and Kill Scene
            Unload();
            //Sets the current scene to the new scene
            Scene = scene;

            //GAMEOBJECT LOADING
            if (SceneManager.Scene.Content != null)
            {
                foreach (GameObject gameobject in Scene.Content)
                {
                    if (!gameobject.Load(Content)) System.Diagnostics.Debug.WriteLine("In scene :" + Scene);
                }
            }
            else System.Diagnostics.Debug.WriteLine("Content not initialized in " + Scene.GetType());

            //MAP TILES LOADING
            if (Scene.map != null)
            {
                foreach (LinkedList<Tile> layer in Scene.map.Tiles)
                {
                    foreach (Tile tile in layer)
                    {
                        if (!tile.Load(Content)) System.Diagnostics.Debug.WriteLine("In Layer: " + layer + " In Scene: " + Scene);
                    }
                }
            }
            else System.Diagnostics.Debug.WriteLine("Map not initialized in " + Scene.GetType());

            //UI LOADING
            if (Scene.Ui != null) 
            {
                foreach (UI.Component component in Scene.Ui)
                {
                    component.Load(Content);
                }
            }
            else System.Diagnostics.Debug.WriteLine("Ui not initialized in " + Scene.GetType());

            //MUSIC LOADING
            if (Scene.Musics != null)
            {
                foreach (Sound.Music music in Scene.Musics.Values) 
                    if (!music.Load(Content)) System.Diagnostics.Debug.WriteLine("In scene :" + Scene);
            }
            else System.Diagnostics.Debug.WriteLine("Musics not initialized in " + Scene.GetType());

            System.Diagnostics.Debug.WriteLine(scene);
        }
        





        /// <summary>
        /// Unload the current scene
        /// </summary>
        public static void Unload()
        {
            if (Scene != null)
            {
                Scene.Kill();
            }
            Content.Unload();
        }





        /// <summary>
        /// Update all the components of the current scene
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            if (Scene != null)
            {
                Scene.Update(gameTime);
            }
        }





        /// <summary>
        /// Draw the current scene to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Scene != null)
            {
                Scene.Draw(spriteBatch, gameTime);
            }
        }

        /*
        private static void Dispose()
        {
            //GAMEOBJECT DISPOSE
            if (Scene.Content != null)
            {
                foreach (GameObject gameobject in Scene.Content)
                {
                    //GAMEOBJECT ANIMATIONMANAGER DISPOSE
                    if (gameobject.AnimationManager != null)
                    {
                        foreach (Graphics.Animation animation in gameobject.AnimationDict.Values)
                        {
                            animation.Texture.Dispose();
                        }
                        if (gameobject.AnimationManager.DefaultAnimation.Texture != null)
                        {
                            gameobject.AnimationManager.DefaultAnimation.Texture.Dispose();
                        }
                    }

                    //GAMEOBJECT SPRITE DISPOSE
                    else if (gameobject.Sprite != null)
                    {
                        gameobject.Sprite.Texture.Dispose();
                    }

                    //GAMEOBJECT SOUND DISPOSE
                    if (gameobject.Sounds != null)
                    {
                        foreach (Sound.Sound sound in gameobject.Sounds.Values)
                        {
                            sound.SoundEffect.Dispose();
                        }
                    }
                }
            }

            //MAP SPRITES DISPOSE
            if (Scene.map != null)
            {
                foreach (LinkedList<Tile> layer in Scene.map.tiles) foreach (Tile tile in layer)
                    {
                        tile.Dispose();
                    }
            }

            //UI DISPOSE
            if (Scene.Ui != null)
            {
                foreach (UI.Component component in Scene.Ui)
                {
                    //UI ANIMATIONMANAGER DISPOSE
                    if (component.AnimationManager != null)
                    {
                        foreach (Graphics.Animation animation in component.AnimationDict.Values)
                        {
                            animation.Texture.Dispose();
                        }
                        if (component.AnimationManager.DefaultAnimation.Texture != null)
                        {
                            component.AnimationManager.DefaultAnimation.Texture.Dispose();
                        }
                    }

                    //UI SPRITE DISPOSE
                    else if (component.Sprite != null)
                    {
                        component.Sprite.Texture.Dispose();
                    }

                    //UI SOUND DISPOSE
                    if (component is UI.Button)
                    {
                        UI.Button button = (UI.Button)component;
                        if (button.ClickSound != null)
                        {
                            button.ClickSound.SoundEffect.Dispose();
                        }
                    }
                }
            }

            //MUSIC DISPOSE
            if (Scene.Musics != null)
            {
                foreach (Sound.Music music in Scene.Musics.Values) music.Song.Dispose();
            }
        }
        */
    }
}
