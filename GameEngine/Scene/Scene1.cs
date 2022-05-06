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
    public sealed class Scene1 : Scene
    {
        
        private Scene1()
        {
            this.Width = 20;
            this.Height = 20;

            this.Content = new LinkedList<GameObject>();
            this.Content.AddLast(
                new Player(
                    new Vector2(4, 4),
                    new Vector2(0, 0), new Vector2(0, 12),
                    new Dictionary<string, Animation>()
                    {
                        { "Default", new Animation("ball", 2, true) },
                    },
                    new Collision.RB_Circle(0.5f),
                    new Dictionary<string, Sound.Sound>()
                    {
                        {"test", new Sound.Sound("test",false)},
                        {"test2", new Sound.Sound("test2",false)}
                    }
                )
            );

            this.Content.AddLast(
                new Obstacle(new Vector2(8, 8), new Vector2(0, 0), new Vector2(0, 0), "SwordV1", new GameEngine.Collision.RB_Circle(0.5f))
            );
        }
        private static Scene1 instance = null;
        public static Scene1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Scene1();
                }
                return instance;
            }
        }

        public static void Play()
        {
            SceneManager.Load(Scene1.Instance);
        }

        public override void Kill()
        {
            instance = null;
        }
        public override void Update(GameTime gameTime)
        {
            Collision.Collision.simulate(Content);
            foreach (GameObject gameobject in Content)
            {
                gameobject.Position += gameobject.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + 0.5f * gameobject.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds;
                gameobject.Velocity += gameobject.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}

