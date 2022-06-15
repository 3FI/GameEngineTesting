using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    /// <summary>
    /// GameObject that either has a triggerbox or a rigidbody
    /// Kills itself after MaxLifetime
    /// </summary>
    public class Sword : GameObject
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// TriggerBox of the sword
        /// If collision with Ennemy => kill the ennemy
        /// If collision with map => might transform to rigidbody
        /// </summary>
        public Collision.TriggerBox TriggerBox;

        /// <summary>
        /// The position at which this is aimed to be at when this is going to be killed
        /// </summary>
        public Vector2 TargetPosition;
        /// <summary>
        /// The angle at which this is aimed to be at when this is going to be killed
        /// </summary>
        public float TargetAngle;
        /// <summary>
        /// The number of seconds after which this will be killed
        /// </summary>
        public float MaxLifeTime;
        /// <summary>
        /// The number of seconds for which this has been alive
        /// </summary>
        public float LifeTime;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// With TriggerBox
        /// </summary>
        /// <param name="triggerBox"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetAngle"></param>
        /// <param name="clockwise"></param>
        /// <param name="maxLifeTime"></param>
        public Sword(Collision.TriggerBox triggerBox, Vector2 targetPosition, float targetAngle, bool clockwise, float maxLifeTime) : base(Vector2.Zero, Vector2.Zero, Vector2.Zero, "Texture2D/Test/SwordV1",null)
        {
            TriggerBox = triggerBox;

            TargetPosition = targetPosition;
            if (clockwise) TargetAngle = targetAngle;
            else TargetAngle = targetAngle - 360;

            MaxLifeTime = maxLifeTime;
        }
        /// <summary>
        /// Without TriggerBox
        /// </summary>
        /// <param name="rigidbody"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetAngle"></param>
        /// <param name="clockwise"></param>
        /// <param name="maxLifeTime"></param>
        public Sword(Collision.RigidBody rigidbody, Vector2 targetPosition, float targetAngle, bool clockwise, float maxLifeTime) : base(Vector2.Zero, Vector2.Zero, Vector2.Zero, "Texture2D/Test/SwordV1", null)
        {
            Rigidbody = rigidbody;

            TargetPosition = targetPosition;
            if (clockwise) TargetAngle = targetAngle;
            else TargetAngle = targetAngle - 360;

            MaxLifeTime = maxLifeTime;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            Position = new Vector2 (Tools.CustomMath.Lerp(Position.X, TargetPosition.X, MaxLifeTime * (float)gameTime.ElapsedGameTime.TotalSeconds), Tools.CustomMath.Lerp(Position.Y, TargetPosition.Y, MaxLifeTime * (float)gameTime.ElapsedGameTime.TotalSeconds));
            Angle = Tools.CustomMath.Lerp(Angle, TargetAngle, MaxLifeTime * (float)gameTime.ElapsedGameTime.TotalSeconds) ;
            //TODO : Update Position relatively to Player if Holded (IN PLAYER CLASS)

            TriggerBox.Box.Position = Position;
            TriggerBox.Box.Angle = Angle;

            LifeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (LifeTime >= MaxLifeTime)
            {
                //TODO : Kill this
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Game1.debug && this.Rigidbody != null) this.TriggerBox.Draw(spriteBatch, gameTime);
            base.Draw(spriteBatch, gameTime);
        }
        public override String ToString()
        {
            //TODO : REWRITE A CLEANER ToString
            if (AnimationManager != null) return "Sword(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tAnimationManager: \n\t" + AnimationManager.ToString().Replace("\n", "\n\t") + "\n)";
            else if (Sprite != null) return "Sword(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tSprite: \n\t" + Sprite.ToString().Replace("\n", "\n\t") + "\n)";
            return "Sword(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tError : No Texture" + "\n)";
        }

        public override bool Equals(Object obj)
        {
            //TODO : REWRITE A CLEANER Equals
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Position == ((Sword)obj).Position) && (TriggerBox == ((Sword)obj).TriggerBox);
            }
        }
    }
}
