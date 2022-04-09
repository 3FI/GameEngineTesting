using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GameEngine.Collision
{
    public abstract class RigidBody
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private GameObject _gameObject;
        private int _id;
        protected static int _idCount;
        public Vector2 Position { get { return _position; } set { _position = value; if(GameObject.Position!=value) _gameObject.Position = value; } }
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; if (GameObject.Velocity != value) _gameObject.Velocity = value; } }
        public int Id { get { return _id; } set { _id = value; } }
        public GameObject GameObject { get { return _gameObject; } set { _gameObject = value; } }


        public abstract Box getBoundingBox();
        public abstract ContactResult isContacting(RigidBody rigidBody);
        public abstract Vector2 leftMostPoint();
        public abstract Vector2 rightMostPoint();
        public abstract Vector2 upMostPoint();
        public abstract Vector2 downMostPoint();
        public abstract void Draw(SpriteBatch spriteBatch, Texture2D texture);
    }
}
