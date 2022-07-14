using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Collision
{
    public class TriggerBox
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Box Box;

        private bool _collidedPlayer;
        public Action<GameTime> OnCollisionPlayer;
        public Action<GameTime> OnExitPlayer;

        private bool _collidedEnnemy;
        public Action<GameTime> OnCollisionEnnemy;
        public Action<GameTime> OnExitEnnemy;

        private bool _collidedTile;
        public Action<GameTime> OnCollisionTile;
        public Action<GameTime> OnExitTile;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public TriggerBox(Vector2 topLeft, Vector2 bottomRight, float angle = 0)
        {
            Box = new Box(topLeft, bottomRight) { Angle = angle };
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

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

            if (Scene.SceneManager.Scene.Content != null)
            {
                foreach (GameObject gameObject in Scene.SceneManager.Scene.Content)
                {
                    //TODO : TEST IF gameObject.Type == Ennemy
                    if (gameObject.Box.intersectBox(Box))
                    {
                        OnCollisionEnnemy?.Invoke(gameTime);
                        _collidedEnnemy = true;
                    }
                }
            }

            if (Scene.SceneManager.Scene.map != null) if (Scene.SceneManager.Scene.map.RigidBodies != null)
            {
                foreach (RigidBody rb in Scene.SceneManager.Scene.map.RigidBodies)
                {
                    //TODO : TEST IF gameObject.Type == Ennemy
                    if (rb.getBoundingBox().intersectBox(Box))
                    {
                        OnCollisionTile?.Invoke(gameTime);
                        _collidedTile = true;
                    }
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
