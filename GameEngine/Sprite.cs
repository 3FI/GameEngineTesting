using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class Sprite
    {
        public Texture2D _texture;
        public String TextureAdress;

        protected Vector2 _position;
        protected float _layer;

        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public Color Colour { get; set; }
        public float Opacity { get; set; }
        public float Scale { get; set; }
        public bool IsRemoved { get; set; }
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            }
        }
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }
        public float Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
            }
        }
        public Sprite(String texture)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;
        }

        /// <summary>
        /// Submit the current sprite to the sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, 64 * new Vector2(Position.X,Position.Y), null, Colour * Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }

        /// <summary>
        /// Return a deep copy of the current sprite
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override String ToString()
        {
            return "Sprite(\n\tTexture: " + TextureAdress + ", \n\tPosition: " + Position + ", \n\tColour: " + Colour + ", \n\tOpacity: " + Opacity + ", \n\tRotation: " + Rotation + ", \n\tOrigin: " + Origin + ", \n\tScale: " + Scale + ", \n\tLayer: " + Layer + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (TextureAdress == ((Sprite)obj).TextureAdress) && (Position == ((Sprite)obj).Position) && (Rotation == ((Sprite)obj).Rotation);
            }
        }
    }
}