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

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private Vector2 _velocity;
        private Vector2 _acceleration;
        private Vector2 _position;
        private Graphics.Sprite _sprite;
        private Collision.RigidBody _rigidBody;
        private Graphics.AnimationManager _animationManager;
        private Dictionary<String, Graphics.Animation> _animationDict;

        public Action DownContact;
        public Action UpContact;
        public Action LeftContact;
        public Action RightContact;

        public Dictionary<String, Sound.Sound> Sounds;
        public Graphics.Sprite Sprite {
                                        get { return _sprite; } 
                                        set { _sprite = value; } 
                                      }
        public Collision.RigidBody Rigidbody {
                                                get { return _rigidBody; } 
                                                set { _rigidBody = value; _rigidBody.GameObject = this; } 
                                             }
        public Graphics.AnimationManager AnimationManager 
                                                         {
                                                            get { return _animationManager; }
                                                            set { _animationManager = value; }
                                                         }
        public Dictionary<String, Graphics.Animation> AnimationDict
                                                                    {
                                                                        get { return _animationDict; }
                                                                        set { _animationDict = value; }
                                                                    }

        public Collision.Box Box
        {
            get { return _rigidBody.getBoundingBox(); }
        }

        //TODO: ERROR HANDLING : STACK OVERFLOW
        public Vector2 Velocity { 
                                 get { return _velocity; } 
                                 set {
                                     _velocity = value; 
                                     if (_rigidBody != null && _rigidBody.Velocity != value) _rigidBody.Velocity = value; 
                                     } 
                                 }
        public Vector2 Acceleration { 
                                     get { return _acceleration; } 
                                     set { _acceleration = value; } 
                                    }
        public Vector2 Position {
                                 get { return _position; } 
                                 set { 
                                       _position = value;
                                        if (_rigidBody != null && _rigidBody.Position!=value) _rigidBody.Position = value;
                                        if (_sprite != null && _sprite.Position != value) _sprite.Position = value;
                                        if (AnimationManager != null && AnimationManager.Position != value) AnimationManager.Position = value;
                                     }
                                }


        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public virtual void Update(GameTime gameTime)
        {
            if (this.AnimationManager != null)
                this.AnimationManager.Update(gameTime);

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + 0.5f * Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (this.AnimationManager != null)
                this.AnimationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (this.Sprite != null)
                this.Sprite.Draw(gameTime, spriteBatch);

            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);

            //If debug menu active, draw the rigid body hitbox to the screen.
            if (Game1.debug && this.Rigidbody != null) this.Rigidbody.Draw(spriteBatch);
        }

        public abstract override String ToString();
        public abstract override bool Equals(Object obj);


    }
}
