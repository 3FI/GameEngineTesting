 using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics
{
    /// <summary>
    /// An animation that can be played in an animation manager.
    /// </summary>
    public class Animation
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Internal texture of this animation (Accessor : <paramref name="Texture"/>)
        /// </summary>
        private Texture2D _texture;
        /// <summary>
        /// Texture accessor of this animation (also sets the origin as the center of the new texture)
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Origin = new Vector2(_texture.Width / 2 / FrameCount, _texture.Height / 2);
            }
        }
        /// <summary>
        /// Represents the address of the file that will be used as a the texture of the animation
        /// </summary>
        public String TextureAdress;

        /// <summary>
        /// The # of the current frame of the animation
        /// </summary>
        public int CurrentFrame;
        /// <summary>
        /// The total number of frame of the animation. Notably used to test when an animation ended and to get the width of a single frame.
        /// </summary>
        public int FrameCount;
        /// <summary>
        /// The time in seconds it takes inbetween frames.
        /// </summary>
        public float FrameSpeed;
        /// <summary>
        /// Wether or not the animation restart once it ended.
        /// </summary>
        public bool IsLooping;

        /// <summary>
        /// The origin around which the animation rotates.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The height in px of a frame of the animation.
        /// </summary>
        public int FrameHeight { get { return Texture.Height; } }
        /// <summary>
        /// The width in px of a frame of the animation.
        /// </summary>
        public int FrameWidth { get { return Texture.Width / FrameCount; } }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Animation constructor w/out speed
        /// </summary>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the animation</param>
        /// <param name="frameCount">The total number of frame of the animation.</param>
        public Animation(String texture, int frameCount)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = false;
            FrameSpeed = 0.2f;
        }
        /// <summary>
        /// Animation constructor w/ speed
        /// </summary>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the animation</param>
        /// <param name="frameCount">The total number of frame of the animation.</param>
        /// <param name="speed">The time in seconds it takes inbetween frames.</param>
        public Animation(String texture, int frameCount, float speed)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = false;
            FrameSpeed = speed;
        }
        /// <summary>
        /// Looped animation constructor w/out speed
        /// </summary>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the animation</param>
        /// <param name="frameCount">The total number of frame of the animation.</param>
        /// <param name="loop">Wether or not the animation restart once it ended.</param>
        public Animation(String texture, int frameCount, bool loop)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = loop;
            FrameSpeed = 0.2f;
        }
        /// <summary>
        /// Looped animation constructor w/ speed
        /// </summary>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the animation</param>
        /// <param name="frameCount">The total number of frame of the animation.</param>
        /// <param name="speed">The time in seconds it takes inbetween frames.</param>
        /// <param name="loop">Wether or not the animation restart once it ended.</param>
        public Animation(String texture, int frameCount, float speed, bool loop)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = loop;
            FrameSpeed = speed;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            try { Texture = content.Load<Texture2D>(TextureAdress); }
            catch (ContentLoadException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to load texture " + TextureAdress + " in animation " + this);
                Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                result = false;
            }
            return result;
        }

        public override String ToString()
        {
            return "Animation(\n\tTexture: " + TextureAdress + ", \n\tOrigin: " + Origin + ", \n\tFrame Count: " + FrameCount + ", \n\tFrame Speed: " + FrameSpeed + ", \n\tIs looping: " + IsLooping + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (TextureAdress == ((Animation)obj).TextureAdress) && (Texture == ((Animation)obj).Texture) && (FrameCount == ((Animation)obj).FrameCount) && (FrameSpeed == ((Animation)obj).FrameSpeed) && (IsLooping == ((Animation)obj).IsLooping);
            }
        }
    }
}
