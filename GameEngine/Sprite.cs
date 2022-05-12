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

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private Texture2D _texture;
        protected Vector2 _position;
        protected float _layer;

        public String TextureAdress;
        public LinkedList<Vector2> MultiplePosition;

        public Vector2 Origin;
        public float Rotation;
        public Color Colour;
        public float Opacity;
        public float Scale;
        public bool IsRemoved;

        public Texture2D Texture { 
                                   get { return _texture; }
                                   set {
                                        _texture = value; 
                                        Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
                                       }
                                 }

        public Vector2 Position {
                                 get { return _position; }
                                 set {
                                      _position = value;
                                     }
                                }
        public float Layer {
                            get { return _layer; }
                            set {
                                 _layer = value;
                                }
                            }


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Sprite(String texture, LinkedList<Vector2> Positions)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            MultiplePosition = Positions;
        }

        public Sprite(String texture, Vector2 position)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            Position = position;
        }

        public Sprite(String texture)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;
        }


        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Submit the current sprite to the sprite batch
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float zoom = Scene.SceneManager.scene.Camera.zoom;

            if (MultiplePosition == null)
            {
                if (Texture != null)
                    spriteBatch.Draw(
                        Texture,
                        Game1.pxPerUnit * zoom * new Vector2(
                            Position.X - Scene.SceneManager.scene.Camera.position.X + Scene.SceneManager.scene.Camera.Width / 2,
                            Position.Y - Scene.SceneManager.scene.Camera.position.Y + Scene.SceneManager.scene.Camera.Height / 2),
                        null,
                        Colour * Opacity,
                        Rotation,
                        Origin,
                        zoom*Scale,
                        SpriteEffects.None,
                        Layer);
                else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
            }
            else
            {
                if (Texture != null) foreach (Vector2 position in MultiplePosition)
                    spriteBatch.Draw(
                        Texture, 
                        Game1.pxPerUnit * zoom * new Vector2(
                            position.X - Scene.SceneManager.scene.Camera.position.X + Scene.SceneManager.scene.Camera.Width / 2, 
                            position.Y - Scene.SceneManager.scene.Camera.position.Y + Scene.SceneManager.scene.Camera.Height / 2), 
                        null, 
                        Colour * Opacity, 
                        Rotation, 
                        Origin, 
                        zoom*Scale, 
                        SpriteEffects.None, 
                        Layer);
                else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
            }
        }

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