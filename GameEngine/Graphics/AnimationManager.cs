using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics
{
    /// <summary>
    /// An object that draws an animation to the screen 
    /// </summary>
    public class AnimationManager
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Whether or not this animation is part of the UI. If it is, it will always be drawn on top of the other sprites (layer 500-1000).
        /// </summary>
        public bool IsUI = false;

        /// <summary>
        /// Time in seconds the current frame of the animation has been playing for. Is used to know when to go to next frame.
        /// </summary>
        private float _timer;
        /// <summary>
        /// The current animation that is played
        /// </summary>
        private Animation _animation; //TODO : ADD AN ANIMATION SCHEDULE
        /// <summary>
        /// The default animation to fallback to when an animation finishes.
        /// </summary>
        public Animation DefaultAnimation;

        /// <summary>
        /// Value between 0-500 that is the layer at which the animation will be drawn
        /// </summary>
        public float Layer = 0;
        //The Origin is in the Animation object instead
        /// <summary>
        /// The angle (clockwise degree) at which the animation will be drawn
        /// </summary>
        public float Rotation;
        /// <summary>
        /// The colour filter of the animation
        /// </summary>
        public Color Colour = Color.White;
        /// <summary>
        /// The alpha canal of the animation
        /// </summary>
        public float Opacity = 1f;
        /// <summary>
        /// The size multiplicator of the animation
        /// </summary>
        public float Scale = 1f;

        /// <summary>
        /// The single position of the animation. (nullable but in that case MultiplePosition isn't). Mainly used in GameObject.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// A LinkedList of all the position to which the animation shall be drawn (nullable but in that case Position isn't). Mainly used in maps.
        /// </summary>
        public LinkedList<Vector2> MultiplePosition;

        /// <summary>
        /// Accessor to the width of the animation in px
        /// </summary>
        public int FrameWidth {get { return _animation.FrameWidth; }}
        /// <summary>
        /// Accessor to the height of the animation in px
        /// </summary>
        public int FrameHeight {get { return _animation.FrameHeight; }}



        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// AnimationManager constructor w/ multiple positions
        /// </summary>
        /// <param name="animation">The initialization animation which is also the default one</param>
        /// <param name="positions">A LinkedList of all the position to which the animation shall be drawn</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the animation will be drawn.</param>
        public AnimationManager(Animation animation, LinkedList<Vector2> positions, float layer = 0)
        {
            DefaultAnimation = animation;
            _animation = animation;
            MultiplePosition = positions;
            Layer = layer;
        }
        /// <summary>
        /// AnimationManager constructor w/ single position
        /// </summary>
        /// <param name="animation">The initialization animation which is also the default one</param>
        /// <param name="position">The position at which the animation shall be drawn</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the animation will be drawn.</param>
        public AnimationManager(Animation animation, Vector2 position, float layer = 0)
        {
            DefaultAnimation = animation;
            _animation = animation;
            Position = position;
            Layer = layer;
        }
        /// <summary>
        /// Animation Manager constructor w/out position
        /// </summary>
        /// <param name="animation">The initialization animation which is also the default one</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the animation will be drawn.</param>
        public AnimationManager(Animation animation, float layer = 0)
        {
            DefaultAnimation = animation;
            _animation = animation;
            Layer = layer;
        }
        /// <summary>
        /// AnimationManager constructor w/ multiple positions for UI
        /// </summary>
        /// <param name="animation">The initialization animation which is also the default one</param>
        /// <param name="positions">A LinkedList of all the position to which the animation shall be drawn</param>
        /// <param name="isUI">Whether or not this animation is part of the UI.</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the animation will be drawn.</param>
        public AnimationManager(Animation animation, LinkedList<Vector2> positions, bool isUI, float layer = 0)
        {
            DefaultAnimation = animation;
            _animation = animation;
            IsUI = isUI;
            MultiplePosition = positions;
            Layer = layer;
        }
        /// <summary>
        /// AnimationManager constructor w/ single position for UI
        /// </summary>
        /// <param name="animation">The initialization animation which is also the default one</param>
        /// <param name="position">The position at which the animation shall be drawn</param>
        /// <param name="isUI">Whether or not this animation is part of the UI.</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the animation will be drawn.</param>
        public AnimationManager(Animation animation, Vector2 position, bool isUI, float layer = 0)
        {
            DefaultAnimation = animation;
            _animation = animation;
            IsUI = isUI;
            Position = position;
            Layer = layer;
        }
        /// <summary>
        /// Animation Manager constructor w/out position for UI
        /// </summary>
        /// <param name="animation">The initialization animation which is also the default one</param>
        /// <param name="isUI">Whether or not this animation is part of the UI.</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the animation will be drawn.</param>
        public AnimationManager(Animation animation, bool isUI, float layer = 0)
        {
            DefaultAnimation = animation;
            _animation = animation;
            IsUI = isUI;
            Layer = layer;
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

        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            if (DefaultAnimation.Texture == null)
            {
                try { DefaultAnimation.Texture = content.Load<Texture2D>(DefaultAnimation.TextureAdress); }
                catch (ContentLoadException)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to load texture " + DefaultAnimation.TextureAdress + " as the default error handling animation");
                    DefaultAnimation.Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Update the animation frame
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Updates the timer
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //If the playtime is over the max playtime of a frame, go to the next
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                //If this was the last frame of the animation. Stop it.
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
            //If the Sprite isn't part of the UI
            if (!IsUI)
            {
                //Querry all the important camera value to draw. If there's no camera, takes default values.
                float zoom;
                Vector2 cameraPosition;
                float cameraWidth;
                float cameraHeight;
                if (Scene.SceneManager.Scene.Camera != null)
                {
                    zoom = Scene.SceneManager.Scene.Camera.Zoom;
                    cameraPosition = Scene.SceneManager.Scene.Camera.Position;
                    cameraWidth = Scene.SceneManager.Scene.Camera.Width;
                    cameraHeight = Scene.SceneManager.Scene.Camera.Height;
                }
                else
                {
                    zoom = 1f;
                    cameraPosition = new Vector2(Game1.ScreenWidth / 2 / Game1.pxPerUnit, Game1.ScreenHeight / 2 / Game1.pxPerUnit);
                    cameraWidth = Game1.ScreenWidth / Game1.pxPerUnit;
                    cameraHeight = Game1.ScreenHeight / Game1.pxPerUnit;
                }

                //Draw if there's a single position to draw to
                if (MultiplePosition == null)
                {
                    if (_animation.Texture != null)
                        spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * zoom * new Vector2(
                                             Position.X - (cameraPosition.X - cameraWidth / 2),
                                             Position.Y - (cameraPosition.Y - cameraHeight / 2)),
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation / 360 * 2 * (float)Math.PI,
                                         _animation.Origin,
                                         zoom * Scale,
                                         SpriteEffects.None,
                                         Layer / 1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + _animation);
                }
                //Draw if there's multiple position to draw to
                else
                {
                    if (_animation.Texture != null) foreach (Vector2 position in MultiplePosition)
                            spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * zoom * new Vector2(
                                             position.X - (cameraPosition.X - cameraWidth / 2),
                                             position.Y - (cameraPosition.Y - cameraHeight / 2)),
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation / 360 * 2 * (float)Math.PI,
                                         _animation.Origin,
                                         zoom * Scale,
                                         SpriteEffects.None,
                                         Layer / 1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
            }
            //If the sprite is part of the UI
            else
            {
                if (MultiplePosition == null)
                {
                    if (_animation.Texture != null)
                        spriteBatch.Draw(_animation.Texture,
                                         Game1.pxPerUnit * Position,
                                         new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                                         Colour,
                                         Rotation / 360 * 2 * (float)Math.PI,
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
                                         Rotation / 360 * 2 * (float)Math.PI,
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
