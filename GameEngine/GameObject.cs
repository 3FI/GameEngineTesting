using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public abstract class GameObject
    {
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private Vector2 _position;
        private Sprite _sprite;
        public AnimationManager animationManager;
        public Dictionary<String, Animation> animationDict;
        public Dictionary<String, Sound.Sound> Sounds;
        private GameEngine.Collision.RigidBody _rigidBody;

        public Vector2 Velocity { 
            get { return _velocity; } 
            set {
                _velocity = value; 
                if (_rigidBody != null && _rigidBody.Velocity != value) _rigidBody.Velocity = value; 
            } 
        }
        public Vector2 Acceleration { get { return _acceleration; } set { _acceleration = value; } }
        public Vector2 Position {
            get { return _position; } 
            set { 
                _position = value;
                if (_rigidBody != null && _rigidBody.Position!=value) _rigidBody.Position = value;
                if (_sprite != null && _sprite.Position != value) _sprite.Position = value;
                if (animationManager != null && animationManager.Position != value) animationManager.Position = value;
                if (this is Player) 
                {
                    Sound.SoundManager.Position = value;
                    if (Scene.SceneManager.scene != null)
                    {
                        if (value.X - Scene.SceneManager.scene.Camera.Width/2 > 0 && value.X + Scene.SceneManager.scene.Camera.Width / 2 < Scene.SceneManager.scene.Width)
                            Scene.SceneManager.scene.Camera.position.X = value.X;
                        if (value.Y - Scene.SceneManager.scene.Camera.Height / 2 > 0 && value.Y + Scene.SceneManager.scene.Camera.Height / 2 < Scene.SceneManager.scene.Height)
                            Scene.SceneManager.scene.Camera.position.Y = value.Y;
                    }
                }
            }
        }
        public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
        public GameEngine.Collision.RigidBody Rigidbody { get { return _rigidBody; } set { _rigidBody = value; _rigidBody.GameObject = this; } }

        public virtual void Update(GameTime gameTime)
        {
            if (this.animationManager != null)
                this.animationManager.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (this.animationManager != null)
                this.animationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (this.Sprite != null)
                this.Sprite.Draw(gameTime, spriteBatch);

            //If debug menu active, draw the rigid body hitbox to the screen.
            if (Game1.debug && this.Rigidbody != null) this.Rigidbody.Draw(spriteBatch);
        }

        public abstract override String ToString();
        public abstract override bool Equals(Object obj);


    }
}
