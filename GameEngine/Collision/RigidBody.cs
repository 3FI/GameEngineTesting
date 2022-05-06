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

        /// <summary>
        /// Return a tight box around the rigidbody
        /// </summary>
        /// <returns></returns>
        public abstract Box getBoundingBox();

        /// <summary>
        /// Test if there's a collision with inputed rigidbody and return information about it
        /// </summary>
        /// <param name="rigidBody"></param>
        /// <returns></returns>
        public abstract ContactResult isContacting(RigidBody rigidBody);

        public abstract Vector2 leftMostPoint();
        public abstract Vector2 rightMostPoint();
        public abstract Vector2 upMostPoint();
        public abstract Vector2 downMostPoint();
        public static Texture2D pointTexture;
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract override String ToString();
        public abstract override bool Equals(Object obj);
    }
}
