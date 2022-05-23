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
            //GAMEOBJECT LOADING
            if (scene.Content != null)
            {
                foreach (GameObject gameobject in scene.Content)
                {
                    //GAMEOBJECT ANIMATIONMANAGER LOADING
                    if (gameobject.animationManager != null)
                    {
                        foreach (Graphics.Animation animation in gameobject.animationDict.Values)
                        {
                            try { animation.Texture = content.Load<Texture2D>(animation.TextureAdress); }
                            catch (ContentLoadException)
                            {
                                System.Diagnostics.Debug.WriteLine("Unable to load texture " + animation.TextureAdress + " in animation " + animation + " in gameobject " + gameobject);
                                animation.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                            }
                        }
                        if (gameobject.animationManager.DefaultAnimation.Texture == null)
                        {
                            try { gameobject.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>(gameobject.animationManager.DefaultAnimation.TextureAdress); }
                            catch (ContentLoadException)
                            {
                                System.Diagnostics.Debug.WriteLine("Unable to load texture " + gameobject.animationManager.DefaultAnimation.TextureAdress + " as the default error handling animation of gameobject " + gameobject);
                                gameobject.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                            }
                        }
                    }

                    //GAMEOBJECT SPRITE LOADING
                    else if (gameobject.Sprite != null)
                    {
                        try { gameobject.Sprite.Texture = content.Load<Texture2D>(gameobject.Sprite.TextureAdress); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load texture " + gameobject.Sprite.TextureAdress + " in gameobject " + gameobject);
                            gameobject.Sprite.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                        }
                    }

                    //GAMEOBJECT SOUND LOADING
                    if (gameobject.Sounds != null)
                    {
                        foreach (Sound.Sound sound in gameobject.Sounds.Values)
                        {
                            try { sound.SoundEffect = content.Load<SoundEffect>(sound.SoundEffectId); }
                            catch (ContentLoadException)
                            {
                                System.Diagnostics.Debug.WriteLine("Unable to load sound " + sound.SoundEffectId + " in gameobject " + gameobject);
                                sound.SoundEffect = content.Load<SoundEffect>("SoundEffect/PlaceHolderSoundEffect");
                            }
                        }
                    }
                }
            }
            else System.Diagnostics.Debug.WriteLine("Content not initialized in " + scene.GetType());

            //MAP SPRITES LOADING
            if (scene.map != null)
            {
                foreach (LinkedList<Graphics.Sprite> layer in scene.map.sprites) foreach (Graphics.Sprite sprite in layer)
                    {
                        try { sprite.Texture = content.Load<Texture2D>(sprite.TextureAdress); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load texture " + sprite.TextureAdress + " in map " + scene.map);
                            sprite.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                        }
                    }
            }
            else System.Diagnostics.Debug.WriteLine("Map not initialized in " + scene.GetType());

            //UI LOADING
            if (scene.Ui != null) 
            {
                foreach (UI.Component component in scene.Ui)
                {
                    //UI ANIMATIONMANAGER LOADING
                    if (component.animationManager != null)
                    {
                        foreach (Graphics.Animation animation in component.animationDict.Values)
                        {
                            try { animation.Texture = content.Load<Texture2D>(animation.TextureAdress); }
                            catch (ContentLoadException)
                            {
                                System.Diagnostics.Debug.WriteLine("Unable to load texture " + animation.TextureAdress + " in animation " + animation + " in component " + component);
                                animation.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                            }
                        }
                        if (component.animationManager.DefaultAnimation.Texture == null)
                        {
                            try { component.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>(component.animationManager.DefaultAnimation.TextureAdress); }
                            catch (ContentLoadException)
                            {
                                System.Diagnostics.Debug.WriteLine("Unable to load texture " + component.animationManager.DefaultAnimation.TextureAdress + " as the default error handling animation of component " + component);
                                component.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                            }
                        }
                    }

                    //UI SPRITE LOADING
                    else if (component.sprite != null)
                    {
                        try { component.sprite.Texture = content.Load<Texture2D>(component.sprite.TextureAdress); }
                        catch (ContentLoadException)
                        {
                            System.Diagnostics.Debug.WriteLine("Unable to load texture " + component.sprite.TextureAdress + " in component " + component);
                            component.sprite.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                        }
                    }

                    //UI SOUND LOADING
                    if (component is UI.Button)
                    {
                        UI.Button button = (UI.Button)component;
                        if (button.ClickSound != null)
                        {
                            try { button.ClickSound.SoundEffect = content.Load<SoundEffect>(button.ClickSound.SoundEffectId); }
                            catch (ContentLoadException)
                            {
                                System.Diagnostics.Debug.WriteLine("Unable to load sound " + button.ClickSound.SoundEffectId + " in Button " + button);
                                button.ClickSound.SoundEffect = content.Load<SoundEffect>("SoundEffect/PlaceHolderSoundEffect");
                            }
                        }
                    }
                }
            }
            else System.Diagnostics.Debug.WriteLine("Ui not initialized in " + scene.GetType());


            //TODO : Implement AnimationManagers for map tiles

            //TODO : LOAD MUSIC

            System.Diagnostics.Debug.WriteLine(Scene);
        }
        
        /// <summary>
        /// Dispose of the current scene
        /// </summary>
        public static void Unload()
        {
            if (scene != null)
            {
                scene.Kill();
            }
            content.Unload();
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
                if (scene.map != null) 
                    scene.map.Draw(spriteBatch,gameTime);
                if (scene.Content != null)
                    foreach (GameObject gameobject in scene.Content)
                    {
                        gameobject.Draw(spriteBatch, gameTime);
                    }

                if (scene.Ui != null)
                    foreach (UI.Component component in scene.Ui)
                    {
                        component.Draw(spriteBatch, gameTime);
                    }
                scene.Draw(spriteBatch, gameTime);
            }
        }


        private static void Dispose()
        {
            //GAMEOBJECT DISPOSE
            if (scene.Content != null)
            {
                foreach (GameObject gameobject in scene.Content)
                {
                    //GAMEOBJECT ANIMATIONMANAGER DISPOSE
                    if (gameobject.animationManager != null)
                    {
                        foreach (Graphics.Animation animation in gameobject.animationDict.Values)
                        {
                            animation.Texture.Dispose();
                        }
                        if (gameobject.animationManager.DefaultAnimation.Texture != null)
                        {
                            gameobject.animationManager.DefaultAnimation.Texture.Dispose();
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
            if (scene.map != null)
            {
                foreach (LinkedList<Graphics.Sprite> layer in scene.map.sprites) foreach (Graphics.Sprite sprite in layer)
                    {
                        sprite.Texture.Dispose();
                    }
            }

            //UI DISPOSE
            if (scene.Ui != null)
            {
                foreach (UI.Component component in scene.Ui)
                {
                    //UI ANIMATIONMANAGER DISPOSE
                    if (component.animationManager != null)
                    {
                        foreach (Graphics.Animation animation in component.animationDict.Values)
                        {
                            animation.Texture.Dispose();
                        }
                        if (component.animationManager.DefaultAnimation.Texture != null)
                        {
                            component.animationManager.DefaultAnimation.Texture.Dispose();
                        }
                    }

                    //UI SPRITE DISPOSE
                    else if (component.sprite != null)
                    {
                        component.sprite.Texture.Dispose();
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
        }
    }
}
