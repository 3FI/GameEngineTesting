using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameEngine
{
    /// <summary>
    /// Genereic gameobject used to test some specific things (currently used to test rotated collisions)
    /// </summary>
    class Obstacle : GameObject
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initialize with a sprite
        /// </summary>
        /// <param name="position">Initialization position of the player</param>
        /// <param name="velocity">Initialization velocity of the player</param>
        /// <param name="acceleration">Initialization acceleration of the player</param>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="rigidBody">RigidBody of the player</param>
        public Obstacle(Vector2 position, Vector2 velocity, Vector2 acceleration, String texture, Collision.RigidBody rigidBody) : base(position, velocity, acceleration, texture, rigidBody)
        {
        }
        /// <summary>
        /// Initialize with a sprite & angle
        /// </summary>
        /// <param name="position">Initialization position of the player</param>
        /// <param name="velocity">Initialization velocity of the player</param>
        /// <param name="acceleration">Initialization acceleration of the player</param>
        /// <param name="angle">Initialization angle of the player</param>
        /// <param name="rigidBody">RigidBody of the player</param>
        public Obstacle(Vector2 position, Vector2 velocity, Vector2 acceleration, float angle, String texture, GameEngine.Collision.RigidBody rigidBody) : base(position, velocity, acceleration, angle, texture, rigidBody)
        {
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            Angle += 0.1f;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }

        public override String ToString()
        {
            if (AnimationManager != null) return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tAnimationManager: \n\t" + AnimationManager.ToString().Replace("\n", "\n\t") + "\n)";
            else if (Sprite != null) return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tSprite: \n\t" + Sprite.ToString().Replace("\n", "\n\t") + "\n)";
            return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tError : No Texture" + "\n)";
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
