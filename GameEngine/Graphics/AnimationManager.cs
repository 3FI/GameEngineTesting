using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics
{
    public class AnimationManager
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public bool IsUI = false;
        private Animation _animation;
        private float _timer;

        public Animation DefaultAnimation;
        public Color Colour = Color.White;
        public float Opacity = 1f;
        public float Scale = 1f;


        public int FrameWidth {get { return _animation.FrameWidth; }}
        public int FrameHeight {get { return _animation.FrameHeight; }}
        public Vector2 Position { get; set; }
        public LinkedList<Vector2> MultiplePosition;

        public float Rotation { get; set; }
        public float Layer { get; set; } = 0;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public AnimationManager(Animation animation)
        {
            DefaultAnimation = animation;
            _animation = animation;
        }

        public AnimationManager(Animation animation, bool isUI)
        {
            DefaultAnimation = animation;
            _animation = animation;
            IsUI = isUI;
        }
        public AnimationManager(Animation animation, Vector2 position)
        {
            DefaultAnimation = animation;
            _animation = animation;
            Position = position;
        }

        public AnimationManager(Animation animation, Vector2 position, bool isUI)
        {
            DefaultAnimation = animation;
            _animation = animation;
            IsUI = isUI;
            Position = position;
        }
        public AnimationManager(Animation animation, LinkedList<Vector2> positions)
        {
            DefaultAnimation = animation;
            _animation = animation;
            MultiplePosition = positions;
        }

        public AnimationManager(Animation animation, LinkedList<Vector2> positions, bool isUI)
        {
            DefaultAnimation = animation;
            _animation = animation;
            IsUI = isUI;
            MultiplePosition = positions;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

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
            if (!IsUI)
            {
                float zoom;
                Vector2 cameraPosition;
                float cameraWidth;
                float cameraHeight;
                if (Scene.SceneManager.scene.Camera != null)
                {
                    zoom = Scene.SceneManager.scene.Camera.zoom;
                    cameraPosition = Scene.SceneManager.scene.Camera.position;
                    cameraWidth = Scene.SceneManager.scene.Camera.Width;
                    cameraHeight = Scene.SceneManager.scene.Camera.Height;
                }
                else
                {
                    zoom = 1f;
                    cameraPosition = new Vector2(Game1.screenWidth / 2 / Game1.pxPerUnit, Game1.screenHeight / 2 / Game1.pxPerUnit);
                    cameraWidth = Game1.screenWidth / Game1.pxPerUnit;
                    cameraHeight = Game1.screenHeight / Game1.pxPerUnit;
                }

                if (MultiplePosition == null)
                {
                    if (_animation.Texture != null)
                        spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * zoom * new Vector2(
                                             Position.X - cameraPosition.X + cameraWidth / 2,
                                             Position.Y - cameraPosition.Y + cameraHeight / 2),
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation,
                                         _animation.Origin,
                                         Scene.SceneManager.scene.Camera.zoom * Scale,
                                         SpriteEffects.None,
                                         Layer / 1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + _animation);
                }
                else
                {
                    if (_animation.Texture != null) foreach (Vector2 position in MultiplePosition)
                            spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * zoom * new Vector2(
                                             position.X - cameraPosition.X + cameraWidth / 2,
                                             position.Y - cameraPosition.Y + cameraHeight / 2),
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation,
                                         _animation.Origin,
                                         Scene.SceneManager.scene.Camera.zoom * Scale,
                                         SpriteEffects.None,
                                         Layer / 1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
            }
            else
            {
                if (MultiplePosition == null)
                {
                    if (_animation.Texture != null)
                        spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * Position,
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation,
                                         _animation.Origin,
                                         Scale,
                                         SpriteEffects.None,
                                         Layer / 1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + _animation);
                }
                else
                {
                    if (_animation.Texture != null) foreach (Vector2 position in MultiplePosition)
                            spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * position,
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation,
                                         _animation.Origin,
                                         Scale,
                                         SpriteEffects.None,
                                         Layer / 1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
            }
        }

        public override String ToString()
        {
            return "AnimationManager(\n\tAnimation: " + _animation.ToString().Replace("\n", "\n\t") + ", \n\tPosition: " + Position + ", \n\tColour: " + Colour + ", \n\tOpacity: " + Opacity + ", \n\tRotation: " + Rotation + ", \n\tOrigin: " + Scale + ", \n\tLayer: " + Layer + "\n)";
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
