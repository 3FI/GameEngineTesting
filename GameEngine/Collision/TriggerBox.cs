using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Collision
{
    public class TriggerBox
    {
        public Box Box;

        public Action<GameTime> OnCollisionPlayer;
        public Action<GameTime> OnExitPlayer;
        public Action<GameTime> OnCollisionEnnemy;
        public Action<GameTime> OnExitEnnemy;
        public Action<GameTime> OnCollisionTile;
        public Action<GameTime> OnExitTile;

        private bool _collidedPlayer;
        private bool _collidedEnnemy;
        private bool _collidedTile;

        //TODO: CHANGE TRIGGERBOX TO SUPPORT ANGLES 

        public TriggerBox(Vector2 topLeft, Vector2 bottomRight, float angle = 0)
        {
            Box = new Box(topLeft, bottomRight) { Angle = angle };
        }

        public void Collision(GameTime gameTime)
        {
            if (Player.Instance != null) if (Player.Instance.Box != null)
                {
                    if (Player.Instance.Box.intersectBox(Box))
                    {
                        OnCollisionPlayer?.Invoke(gameTime);
                        _collidedPlayer = true;
                    }
                    else if (_collidedPlayer)
                    {
                        OnExitPlayer?.Invoke(gameTime);
                        _collidedPlayer = false;
                    }
                }
        }

        public void Update(GameTime gameTime)
        {
            Collision(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Box.Draw(spriteBatch, Color.LightBlue, 10);
        }
    }
}
