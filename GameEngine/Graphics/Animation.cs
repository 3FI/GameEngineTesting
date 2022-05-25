 using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics
{
    public class Animation
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private Texture2D _texture;

        public int CurrentFrame;
        public int FrameCount;
        public float FrameSpeed; 
        public Vector2 Origin;
        public bool IsLooping;
        public String TextureAdress;

        public int FrameHeight { get { return Texture.Height; } }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Origin = new Vector2(_texture.Width / 2 / FrameCount, _texture.Height / 2);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

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

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public void Load(ContentManager content)
        {
            try { Texture = content.Load<Texture2D>(TextureAdress); }
            catch (ContentLoadException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to load texture " + TextureAdress + " in animation " + this);
                Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
            }
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
