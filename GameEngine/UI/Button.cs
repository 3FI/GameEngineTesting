using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.UI
{
    public class Button : Component
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        protected MouseState _previousMouse;
        protected MouseState _currentMouse;

        public Action<GameTime> OnClick;
        public Sound.Sound ClickSound;

        public Color BaseColor = Color.White;
        public Color HoverColor = Color.Gray;

        public Rectangle MouseRectangle {
                                         get { return new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1); }
                                        }


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Button(float height, float width, Vector2 position, SpriteFont font, string text, Color textColour, Sound.Sound sound, int layer = 0) : base(height, width, position, font, text, textColour, layer)
        {
            ClickSound = sound;
            BaseColor = Color.Black;
        }

        public Button(string texture, Vector2 position, SpriteFont font, string text, Color textColour,  Sound.Sound sound, int layer = 0) : base(texture, position, font, text, textColour, layer)
        {
            ClickSound = sound;
        }

        public Button(Dictionary<String, Graphics.Animation> animations, Vector2 position, SpriteFont font, string text, Color textColour, Sound.Sound sound, int layer = 0) : base(animations, position, font, text, textColour, layer)
        {
            ClickSound = sound;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            if (MouseRectangle.Intersects(Rectangle))
            {
                if (sprite != null)
                    sprite.Colour = HoverColor;
                else if (animationManager != null)
                    animationManager.Colour = HoverColor;
                else
                    _color = HoverColor;

                if (_previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
                {
                    OnClick?.Invoke(gameTime);
                    if (ClickSound != null)
                        Sound.SoundManager.Add(this.ClickSound.CreateInstance());
                }
            }
            else
                if (sprite != null)
                    sprite.Colour = BaseColor;
                else if (animationManager != null)
                    animationManager.Colour = BaseColor;
                else
                    _color = BaseColor;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
        public override String ToString()
        {
            if (sprite != null)
            {
                string result = "Button(\n\tSprite: " + sprite.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + ", \n\tSound: ";
                if (ClickSound != null)
                    result += ClickSound.ToString().Replace("\n", "\n\t");
                else
                    result += "NULL";
                result += ", \n\tClick Action: " + OnClick + "\n)";
                return result;
            }
            else if (animationManager != null)
            {
                string result = "Button(\n\tAnimationManager: " + animationManager.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + ", \n\tSound: ";
                if (ClickSound != null)
                    result += ClickSound.ToString().Replace("\n", "\n\t");
                else
                    result += "NULL";
                result += ", \n\tClick Action: " + OnClick + "\n)";
                return result;
            }

            else
            {
                string result = "Button(\n\tPosition: " + _position + ",\n\tWidth:" + _width + ",\n\tHeight:" + _height + ",\n\tColor:" + _color + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + ", \n\tSound: ";
                if (ClickSound != null)
                    result += ClickSound.ToString().Replace("\n", "\n\t");
                else result += "NULL";
                result += ", \n\tClick Action: " + OnClick + "\n)";
                return result;
            }
        }
    }
}