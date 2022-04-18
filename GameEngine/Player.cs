using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class Player : GameObject
    {
        public Player() { }
        
        public Player(Vector2 position, Vector2 Velocity, Vector2 acceleration, String texture, GameEngine.Collision.RigidBody rigidBody)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.Sprite = new Sprite(texture);
        }
        public Player(Vector2 position, Vector2 Velocity, Vector2 acceleration, Dictionary<String,Animation> animations, GameEngine.Collision.RigidBody rigidBody)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.animationDict = animations;
            this.animationManager = new AnimationManager(this.animationDict["Default"]);
        }

        KeyboardState previouskstate;
        /// <summary>
        /// Scan the input from the player
        /// </summary>
        /// <param name="gameTime"></param>
        public void Movement(GameTime gameTime)
        {
            
            KeyboardState kstate = new KeyboardState();
            kstate = Keyboard.GetState();


            if (kstate.IsKeyDown(Keys.Up) && !previouskstate.IsKeyDown(Keys.Up))
                this.Velocity = new Vector2(this.Velocity.X, -400 * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.Left))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(-4, this.Velocity.Y), 10f * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(4, this.Velocity.Y), 10f * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right) || kstate.IsKeyDown(Keys.Left) && kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(0, this.Velocity.Y), 1f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            previouskstate = kstate;
        }

        public override String ToString()
        {
            if (animationManager != null) return "Player(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: " + Rigidbody + ", \n\tSprite: " + Sprite + "\n)";
            else if (Sprite != null) return "Player(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: " + Rigidbody + ", \n\tSprite: " + Sprite + "\n)";
            return "Player(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: " + Rigidbody + ", \n\tError : No Texture" + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Position == ((Player)obj).Position) && (Rigidbody == ((Player)obj).Rigidbody);
            }
        }
    }
}
