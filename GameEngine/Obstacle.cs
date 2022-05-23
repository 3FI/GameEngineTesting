using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameEngine
{
    class Obstacle : GameObject
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Obstacle(Vector2 position, Vector2 Velocity, Vector2 acceleration, String texture, GameEngine.Collision.RigidBody rigidBody)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.Sprite = new Graphics.Sprite(texture, position);
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }

        public override String ToString()
        {
            if (animationManager != null) return "Obstacle(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tAnimationManager: \n\t" + animationManager.ToString().Replace("\n", "\n\t") + "\n)";
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
