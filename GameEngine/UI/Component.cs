using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.UI
{
    public class Component
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private static Texture2D _pointTexture;

        protected SpriteFont _font = Game1.BaseFont;
        protected Color _fontColour = Color.White;
        protected string _text = "";
        protected Vector2 _position;
        protected float _height;
        protected float _width;
        protected Color _color = Color.White;

        public Graphics.Sprite sprite;
        public Graphics.AnimationManager animationManager;
        public Dictionary<String, Graphics.Animation> animationDict;
        public Rectangle Rectangle
        {
            get { 
                if (sprite != null)
                    return new Rectangle(
                        (int)(sprite.Position.X * Game1.pxPerUnit) - sprite.Texture.Width/2, 
                        (int)(sprite.Position.Y * Game1.pxPerUnit) - sprite.Texture.Height/2, 
                        sprite.Texture.Width, 
                        sprite.Texture.Height); 
                else if (animationManager != null)
                    return new Rectangle(
                        (int)(animationManager.Position.X * Game1.pxPerUnit) - animationManager.FrameWidth / 2,
                        (int)(animationManager.Position.Y * Game1.pxPerUnit) - animationManager.FrameHeight / 2,
                        animationManager.FrameWidth,
                        animationManager.FrameHeight);
                else
                    return new Rectangle(
                        (int)(_position.X * Game1.pxPerUnit - _width * Game1.pxPerUnit / 2),
                        (int)(_position.Y * Game1.pxPerUnit - _height * Game1.pxPerUnit / 2),
                        (int)(_width * Game1.pxPerUnit),
                        (int)(_height * Game1.pxPerUnit));
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Component(float height, float width, Vector2 position, SpriteFont font, string text, Color textColour, int layer = 0)
        {
            _height = height;
            _width = width;
            _position = position;
            _font = font;
            _text = text;
            _fontColour = textColour;
            _color = Color.Black;
        }

        public Component(string texture, Vector2 position, SpriteFont font, string text, Color textColour, int layer = 0)
        {
            sprite = new Graphics.Sprite(texture, position, true);
            _font = font;
            _text = text;
            _fontColour = textColour;
        }

        public Component(Dictionary<String, Graphics.Animation> animations, Vector2 position, SpriteFont font, string text, Color textColour, int layer = 0)
        {
            this.animationDict = animations;
            try
            {
                this.animationManager = new Graphics.AnimationManager(this.animationDict["Default"], true);
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("ball", 1);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.animationManager = new Graphics.AnimationManager(error, true);
            }
            this.animationManager.Position = position;
            _font = font;
            _text = text;
            _fontColour = textColour;
        }

        public Component(float height, float width, Vector2 position, int layer = 0)
        {
            _height = height;
            _width = width;
            _position = position;
            _color = Color.Black;
        }

        public Component(string texture, Vector2 position, int layer = 0)
        {
            sprite = new Graphics.Sprite(texture, position, true);
        }

        public Component(Dictionary<String, Graphics.Animation> animations, Vector2 position, int layer = 0)
        {
            this.animationDict = animations;
            try
            {
                this.animationManager = new Graphics.AnimationManager(this.animationDict["Default"], true);
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("ball", 1);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.animationManager = new Graphics.AnimationManager(error, true);
            }
            this.animationManager.Position = position;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public virtual void Update(GameTime gameTime)
        {
            if (this.animationManager != null)
                this.animationManager.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (this.animationManager != null)
                this.animationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (this.sprite != null)
                this.sprite.Draw(gameTime, spriteBatch);

            else 
            {
                if (_pointTexture == null)
                {
                    _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                    _pointTexture.SetData<Color>(new Color[] { Color.White });
                }
                spriteBatch.Draw(_pointTexture, 
                                 new Rectangle(
                                     (int)(64 * _position.X - 64 * _width / 2), 
                                     (int)(64 * _position.Y - 64 * _height / 2), 
                                     (int)(64 * _width), 
                                     (int)(64 * _height)), 
                                     _color);
            }

            if (_text != null)
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);
                spriteBatch.DrawString(_font, _text, new Vector2(x, y), _fontColour);
            }            
        }
        public override String ToString()
        {
            if (sprite != null)
                return "Component(\n\tSprite: " + sprite.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + "\n)";
            else if (animationManager != null)
                return "Component(\n\tAnimationManager: " + animationManager.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + "\n)";
            else
                return "Component(\n\tPosition: " + _position + ",\n\tWidth:" + _width + ",\n\tHeight:" + _height + ",\n\tColor:" + _color + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + "\n)";
        }
    }
}
