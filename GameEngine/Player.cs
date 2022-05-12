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

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        KeyboardState previouskstate;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Player(Vector2 position, Vector2 Velocity, Vector2 acceleration, String texture, GameEngine.Collision.RigidBody rigidBody, Dictionary<String,GameEngine.Sound.Sound> sounds)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.Sprite = new Sprite(texture);
            this.Sounds = sounds;
        }

        public Player(Vector2 position, Vector2 Velocity, Vector2 acceleration, Dictionary<String,Animation> animations, GameEngine.Collision.RigidBody rigidBody, Dictionary<String, GameEngine.Sound.Sound> sounds)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.animationDict = animations;
            this.Sounds = sounds;
            try
            {
                this.animationManager = new AnimationManager(this.animationDict["Default"]);
            } 
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                Animation error = new Animation("ball", 1);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.animationManager = new AnimationManager(error);
            }

        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Scan the input from the player and update the player velocity accordingly
        /// </summary>
        /// <param name="gameTime"></param>
        public void Movement(GameTime gameTime)
        {
            KeyboardState kstate = new KeyboardState();
            kstate = Keyboard.GetState();

            float VerticalSpeed = -400f;
            float HorizontalSpeed = 4f;
            float HorizontalLerping = 10f;
            float HorizontalDrag = 1f;

            //If player jumping
            if (kstate.IsKeyDown(Keys.Up) && !previouskstate.IsKeyDown(Keys.Up))
            {
                //Set upward velocity
                this.Velocity = new Vector2(this.Velocity.X, VerticalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                
                //Try running the jump sound
                try
                {
                    Sound.SoundManager.Add(this.Sounds["test2"].SoundEffect.CreateInstance(), this.Position);
                }
                catch (System.Collections.Generic.KeyNotFoundException e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                }
            }
            
            //Go Left
            if (kstate.IsKeyDown(Keys.Left))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(-HorizontalSpeed, this.Velocity.Y), HorizontalLerping * (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Go Right
            if (kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2( HorizontalSpeed, this.Velocity.Y), HorizontalLerping * (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Momentum Drag
            if (kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right) || kstate.IsKeyDown(Keys.Left) && kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2( 0, this.Velocity.Y), HorizontalDrag * (float)gameTime.ElapsedGameTime.TotalSeconds);

            previouskstate = kstate;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.Movement(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
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
