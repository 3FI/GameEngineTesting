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
        protected MouseState _previousMouse;
        protected MouseState _currentMouse;

        public Action OnClick;

        public Rectangle MouseRectangle {
                                         get { return new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1); }
                                        }


        public Button(string texture, Vector2 position, SpriteFont font, string text, Color textColour,  Sound.Sound sound)
        {
            sprite = new Sprite(texture, position, true);
            this.ClickSound = sound;
            _font = font;
            _text = text;
            _fontColour = textColour;
        }

        public Button(Dictionary<String, Animation> animations, Vector2 position, SpriteFont font, string text, Color textColour, Sound.Sound sound)
        {
            this.animationDict = animations;
            this.ClickSound = sound;
            try
            {
                this.animationManager = new AnimationManager(this.animationDict["Default"], true);
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                Animation error = new Animation("ball", 1);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.animationManager = new AnimationManager(error, true);
            }
            this.animationManager.Position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            if (MouseRectangle.Intersects(Rectangle))
            {
                sprite.Colour = Color.Yellow;

                if (_previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
                {
                    OnClick?.Invoke();
                    try
                    {
                        Sound.SoundManager.Add(this.ClickSound.CreateInstance());
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                    }
                }
            }
            else sprite.Colour = Color.White;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}