using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics
{
    /// <summary>
    /// A texture drawn to the screen
    /// </summary>
    public class Sprite
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Whether or not this sprite is part of the UI. If it is, it will always be drawn on top of the other sprites (layer 500-1000).
        /// </summary>
        public bool IsUI = false;

        /// <summary>
        /// Internal texture of this sprite (Accessor : <paramref name="Texture"/>)
        /// </summary>
        private Texture2D _texture;
        /// <summary>
        /// Represents the address of the file that will be used as a the texture of the sprite
        /// </summary>
        public String TextureAdress;
        /// <summary>
        /// Texture accessor of this Sprite (also sets the origin as the center of the new texture)
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            }
        }

        /// <summary>
        /// Value between 0-500 that is the layer at which the sprite will be drawn.
        /// </summary>
        public float Layer = 0;
        /// <summary>
        /// The origin around which the sprite will rotate. Automatically set to the center when setting a new texture.
        /// </summary>
        public Vector2 Origin;
        /// <summary>
        /// The angle (clockwise degree) at which the sprite will be drawn
        /// </summary>
        public float Rotation;
        /// <summary>
        /// The colour filter of the sprite
        /// </summary>
        public Color Colour;
        /// <summary>
        /// The alpha canal of the sprite
        /// </summary>
        public float Opacity;
        /// <summary>
        /// The size multiplicator of the sprite
        /// </summary>
        public float Scale;
         
        /// <summary>
        /// A LinkedList of all the position to which the sprite shall be drawn (nullable but in that case Position isn't). Mainly used in maps.
        /// </summary>
        public LinkedList<Vector2> MultiplePosition;
        /// <summary>
        /// The single position of the sprite. (nullable but in that case MultiplePosition isn't). Mainly used in GameObject.
        /// </summary>
        public Vector2 Position;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Sprite constructor w/ multiple positions
        /// </summary>
        /// <param name="texture">String that represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="Positions">A LinkedList of all the position to which the sprite shall be drawn</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the sprite will be drawn.</param>
        public Sprite(String texture, LinkedList<Vector2> Positions, float layer = 0)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            MultiplePosition = Positions;

            Layer = layer;
        }
        /// <summary>
        /// Sprite constructor w/ single position
        /// </summary>
        /// <param name="texture">String that represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="position">The position of the sprite</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the sprite will be drawn.</param>
        public Sprite(String texture, Vector2 position, float layer = 0)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            Position = position;

            Layer = layer;
        }
        /// <summary>
        /// Sprite constructor w/out position
        /// </summary>
        /// <param name="texture">String that represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the sprite will be drawn.</param>
        public Sprite(String texture, float layer = 0)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            Layer = layer;
        }
        /// <summary>
        /// Sprite constructor w/ multiple position for UI
        /// </summary>
        /// <param name="texture">String that represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="Positions">A LinkedList of all the position to which the sprite shall be drawn</param>
        /// <param name="isUI">Whether or not this sprite is part of the UI.</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the sprite will be drawn.</param>
        public Sprite(String texture, LinkedList<Vector2> Positions, bool isUI, float layer = 0)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            MultiplePosition = Positions;

            IsUI = isUI;

            Layer = layer;
        }
        /// <summary>
        /// Sprite constructor w/ single position for UI
        /// </summary>
        /// <param name="texture">String that represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="position">The position of the sprite</param>
        /// <param name="isUI">Whether or not this sprite is part of the UI.</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the sprite will be drawn.</param>
        public Sprite(String texture, Vector2 position, bool isUI, float layer = 0)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            Position = position;

            IsUI = isUI;

            Layer = layer;
        }
        /// <summary>
        /// Sprite constructor w/out position for UI
        /// </summary>
        /// <param name="texture">String that represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="isUI">Whether or not this sprite is part of the UI.</param>
        /// <param name="layer">Value between 0-500 that is the layer at which the sprite will be drawn.</param>
        public Sprite(String texture, bool isUI, float layer = 0)
        {
            TextureAdress = texture;

            Opacity = 1f;

            Scale = 1f;

            Origin = new Vector2(0, 0);

            Colour = Color.White;

            IsUI = isUI;

            Layer = layer;
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
                    if (Texture != null)
                        spriteBatch.Draw(
                            Texture,
                            Game1.pxPerUnit * zoom * new Vector2(
                                Position.X - (cameraPosition.X - cameraWidth / 2),
                                Position.Y - (cameraPosition.Y - cameraHeight / 2)),
                            null,
                            Colour * Opacity,
                            Rotation / 360 * 2 * (float)Math.PI,
                            Origin,
                            zoom * Scale,
                            SpriteEffects.None,
                            Layer/1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
                //Draw if there's multiple position to draw to
                else
                {
                    if (Texture != null) foreach (Vector2 position in MultiplePosition)
                            spriteBatch.Draw(
                                Texture,
                                Game1.pxPerUnit * zoom * new Vector2(
                                    position.X - (cameraPosition.X - cameraWidth / 2),
                                    position.Y - (cameraPosition.Y - cameraHeight / 2)),
                                null,
                                Colour * Opacity,
                                Rotation / 360 * 2 * (float)Math.PI,
                                Origin,
                                zoom * Scale,
                                SpriteEffects.None,
                                Layer/1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
            }

            //If the sprite is part of the UI
            else
            {
                if (MultiplePosition == null)
                {
                    if (Texture != null)
                        spriteBatch.Draw(
                            Texture,
                            Game1.pxPerUnit * Position,
                            null,
                            Colour * Opacity,
                            Rotation / 360 * 2 * (float)Math.PI,
                            Origin,
                            Scale,
                            SpriteEffects.None,
                            (Layer+500)/1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
                else
                {
                    if (Texture != null) foreach (Vector2 position in MultiplePosition)
                            spriteBatch.Draw(
                                Texture,
                                Game1.pxPerUnit * Position,
                                null,
                                Colour * Opacity,
                                Rotation / 360 * 2 * (float)Math.PI,
                                Origin,
                                Scale,
                                SpriteEffects.None,
                                (Layer+500)/1000);
                    else System.Diagnostics.Debug.WriteLine("Missing Texture in " + this);
                }
            }
        }

        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            try { Texture = content.Load<Texture2D>(TextureAdress); }
            catch (ContentLoadException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to load texture " + TextureAdress);
                Texture = content.Load<Texture2D>("Texture2D/PlaceHolderTexture");
                result = false;
            }
            return result;
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