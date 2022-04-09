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
            this.TextureAdress = texture;
        }

        KeyboardState previouskstate;

        public void Movement(GameTime gameTime)
        {
            
            KeyboardState kstate = new KeyboardState();
            kstate = Keyboard.GetState();


            if (kstate.IsKeyDown(Keys.Up) && !previouskstate.IsKeyDown(Keys.Up))
                this.Velocity = new Vector2(this.Velocity.X, -30000 * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.Left))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(-250, this.Velocity.Y), 10f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //    ball.acceleration.X += -movmentVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(250, this.Velocity.Y), 10f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //    ball.acceleration.X += movmentVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right) || kstate.IsKeyDown(Keys.Left) && kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(0, this.Velocity.Y), 1f * (float)gameTime.ElapsedGameTime.TotalSeconds);
            previouskstate = kstate;
        }
    }
}
