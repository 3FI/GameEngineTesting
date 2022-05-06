using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class AnimationManager
    {
        private Animation _animation;
        public Animation DefaultAnimation;
        private float _timer;

        public int FrameWidth {get { return _animation.FrameWidth; }}
        public int FrameHeight {get { return _animation.FrameHeight; }}
        public Vector2 Position { get; set; }
        public Color Colour { get; set; } = Color.White;
        public float Rotation { get; set; } = 0f;
        public float Layer { get; set; }
        public float Opacity { get; set; } = 1f;
        public float Scale { get; set; } = 1f;


        public AnimationManager(Animation animation)
        {
            DefaultAnimation = animation;
            _animation = animation;
        }

        /// <summary>
        /// Update the animation frame
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    _animation.CurrentFrame = 0;
                    if (!_animation.IsLooping)
                    {
                        _animation = DefaultAnimation;
                        _timer = 0f;
                    }
                }
            }
        }

        /// <summary>
        /// Draw the current frame of the animation to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture,
                             64 * new Vector2(Position.X, Position.Y),
                             new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                             Colour,
                             Rotation,
                             _animation.Origin,
                             Scale,
                             SpriteEffects.None,
                             Layer);
        }

        /// <summary>
        /// Start playing the inputed animation
        /// </summary>
        /// <param name="animation"></param>
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;
            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }

        /// <summary>
        /// Stop current animation and go back to the default animation
        /// </summary>
        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
            _animation = DefaultAnimation;
        }

        public override String ToString()
        {
            return "AnimationManager(\n\tAnimation: " + _animation + ", \n\tPosition: " + Position + ", \n\tColour: " + Colour + ", \n\tOpacity: " + Opacity + ", \n\tRotation: " + Rotation + ", \n\tOrigin: " + Scale + ", \n\tLayer: " + Layer + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Position == ((AnimationManager)obj).Position) && (Rotation == ((AnimationManager)obj).Rotation) && (Layer == ((AnimationManager)obj).Layer) && (_timer == ((AnimationManager)obj)._timer);
            }
        }
    }
}
