using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace GameEngine
{
    class Obstacle : GameObject
    {
        public Obstacle() { }

        public Obstacle(Vector2 position, Vector2 Velocity, Vector2 acceleration, String texture, GameEngine.Collision.RigidBody rigidBody)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.TextureAdress = texture;
        }
    }
}
