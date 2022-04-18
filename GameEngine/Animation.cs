 using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class Animation
    {
        private Texture2D _texture;
        public int CurrentFrame { get; set; }
        public int FrameCount { get; private set; }
        public int FrameHeight { get { return Texture.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public Vector2 Origin { get; set; }
        public bool IsLooping { get; set; }
        public String TextureAdress;
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            }
        }

        public Animation(String texture, int frameCount)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = false;
            FrameSpeed = 0.2f;
        }
        public Animation(String texture, int frameCount, float speed)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = false;
            FrameSpeed = speed;
        }
        public Animation(String texture, int frameCount, bool loop)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = loop;
            FrameSpeed = 0.2f;
        }
        public Animation(String texture, int frameCount, float speed, bool loop)
        {
            TextureAdress = texture;
            FrameCount = frameCount;
            IsLooping = loop;
            FrameSpeed = speed;
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
