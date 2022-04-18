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
            }
        }
        public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
        public GameEngine.Collision.RigidBody Rigidbody { get { return _rigidBody; } set { _rigidBody = value; _rigidBody.GameObject = this; } }

        public abstract override String ToString();
        public abstract override bool Equals(Object obj);


    }
}
