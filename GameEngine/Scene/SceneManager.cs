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
        static public Scene scene;
        static public ContentManager content;

        public static void Load(Scene Scene)
        {
            Unload();
            scene = Scene;
            foreach (GameObject gameobject in scene.Content)
            {
                if (gameobject.animationManager != null)
                {
                    foreach (Animation animation in gameobject.animationDict.Values) animation.Texture = content.Load<Texture2D>(animation.TextureAdress);
                    if (gameobject.animationManager.DefaultAnimation.Texture == null) gameobject.animationManager.DefaultAnimation.Texture = content.Load<Texture2D>(gameobject.animationManager.DefaultAnimation.TextureAdress);
                }
                else if (gameobject.Sprite != null) gameobject.Sprite.Texture = content.Load<Texture2D>(gameobject.Sprite.TextureAdress);
                if (gameobject.Sounds != null)
                {
                    foreach (Sound.Sound sound in gameobject.Sounds.Values) sound.SoundEffect = content.Load<SoundEffect>(sound.SoundEffectId);
                }
            }
            if (scene.map != null) foreach (LinkedList<Sprite> layer in scene.map.sprites) foreach (Sprite sprite in layer) sprite.Texture = content.Load<Texture2D>(sprite.TextureAdress);
        }
        
        public static void Unload()
        {
            if (scene != null) scene.Kill();
        }

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

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (scene != null)
            {
                //TODO ADD LAYER HANDLING
                if (scene.map != null) foreach (LinkedList<Sprite> layer in scene.map.sprites) foreach (Sprite sprite in layer) sprite.Draw(gameTime,spriteBatch);
                scene.Draw(spriteBatch, gameTime);
                foreach (GameObject gameobject in scene.Content)
                {
                    gameobject.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}
