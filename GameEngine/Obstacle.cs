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
            this.Sprite = new Sprite(texture);
        }

        public override String ToString()
        {
            if (animationManager != null) return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: " + Rigidbody + ", \n\tSprite: " + Sprite + "\n)";
            else if (Sprite != null) return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: " + Rigidbody + ", \n\tSprite: " + Sprite + "\n)";
            return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: " + Rigidbody + ", \n\tError : No Texture" + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Position == ((Obstacle)obj).Position) && (Rigidbody == ((Obstacle)obj).Rigidbody);
            }
        }
    }
}
