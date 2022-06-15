using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.UI
{
    /// <summary>
    /// Extends UI.Component but with mouse click interactibility
    /// </summary>
    public class Button : Component
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// State of the mouse in the previous frame
        /// Used to compare for button release 
        /// </summary>
        protected MouseState _previousMouse;
        /// <summary>
        /// Current state of the mouse
        /// Used to know if hovering or clicking this button
        /// </summary>
        protected MouseState _currentMouse;
        /// <summary>
        /// Gets the position of the mouse as a rectangle instead of a Vector2
        /// Used for Rectangle.Intersects Method
        /// </summary>
        public Rectangle MouseRectangle { get { return new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1); } }

        /// <summary>
        /// Action that is invoked if the button is clicked
        /// Might be null
        /// </summary>
        public Action<GameTime> OnClick;
        /// <summary>
        /// Sound that is played if the button is clicked
        /// Might be null
        /// </summary>
        public Sound.Sound ClickSound;

        /// <summary>
        /// Color that the color of the underlying component will be set to if not hovering the button
        /// Must be changed manually if needed
        /// </summary>
        public Color BaseColor = Color.White;
        /// <summary>
        /// Color that the color of the underlying component will be set to if the user is hovering the button
        /// Must be changed manually if needed
        /// </summary>
        public Color HoverColor = Color.Gray;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// With Text
        /// Without Texture
        /// </summary>
        /// <param name="height"> Height of this textureless button </param>
        /// <param name="width"> Width of this textureless button </param>
        /// <param name="position"> Center of this button </param>
        /// <param name="font"> Font of the text </param>
        /// <param name="text"> Text at the center of the button </param>
        /// <param name="textColour"> Colour of the text </param>
        /// <param name="sound"> Sound that the button will made when clicked. Is nullable</param>
        /// <param name="layer"> Layer of this button in the UI (between 0 and 500) </param>
        public Button(float height, float width, Vector2 position, SpriteFont font, string text, Color textColour, Sound.Sound sound, int layer = 0) : base(height, width, position, font, text, textColour, layer)
        {
            ClickSound = sound;
            //If you want another color you need to set it manually after
            //It is set to black since the background is by default white
            BaseColor = Color.Black;
        }
        /// <summary>
        /// With Text
        /// With Sprite
        /// </summary>
        /// <param name="texture"> Represents the address of the file that will be used as a the texture of the sprite </param>
        /// <param name="position"> Center of this button </param>
        /// <param name="font"> Font of the text </param>
        /// <param name="text"> Text at the center of the button </param>
        /// <param name="textColour"> Colour of the text </param>
        /// <param name="sound"> Sound that the button will made when clicked. Is nullable</param>
        /// <param name="layer"> Layer of this button in the UI (between 0 and 500) </param>
        public Button(string texture, Vector2 position, SpriteFont font, string text, Color textColour,  Sound.Sound sound, int layer = 0) : base(texture, position, font, text, textColour, layer)
        {
            ClickSound = sound;
        }
        /// <summary>
        /// With Text With Animator
        /// </summary>
        /// <param name="animations"> Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="position"> Center of this button </param>
        /// <param name="font"> Font of the text </param>
        /// <param name="text"> Text at the center of the button </param>
        /// <param name="textColour"> Colour of the text </param>
        /// <param name="sound"> Sound that the button will made when clicked. Is nullable</param>
        /// <param name="layer"> Layer of this button in the UI (between 0 and 500) </param>
        public Button(Dictionary<String, Graphics.Animation> animations, Vector2 position, SpriteFont font, string text, Color textColour, Sound.Sound sound, int layer = 0) : base(animations, position, font, text, textColour, layer)
        {
            ClickSound = sound;
        }

        /// <summary>
        /// Without Text
        /// Without Texture
        /// </summary>
        /// <param name="height"> Height of this textureless button </param>
        /// <param name="width"> Width of this textureless button </param>
        /// <param name="position"> Center of this button </param>
        /// <param name="sound"> Sound that the button will made when clicked. Is nullable</param>
        /// <param name="layer"> Layer of this button in the UI (between 0 and 500) </param>
        public Button(float height, float width, Vector2 position, Sound.Sound sound, int layer = 0) : base(height, width, position, layer)
        {
            ClickSound = sound;
            //If you want another color you need to set it manually after
            //It is set to black since the background is by default white
            BaseColor = Color.Black;
        }
        /// <summary>
        /// Without Text
        /// With Sprite
        /// </summary>
        /// <param name="texture"> Represents the address of the file that will be used as a the texture of the sprite </param>
        /// <param name="position"> Center of this button </param>
        /// <param name="sound"> Sound that the button will made when clicked. Is nullable</param>
        /// <param name="layer"> Layer of this button in the UI (between 0 and 500) </param>
        public Button(string texture, Vector2 position, Sound.Sound sound, int layer = 0) : base(texture, position, layer)
        {
            ClickSound = sound;
        }
        /// <summary>
        /// Without Text With Animation
        /// </summary>
        /// <param name="animations"> Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="position"> Center of this button </param>
        /// <param name="sound"> Sound that the button will made when clicked. Is nullable</param>
        /// <param name="layer"> Layer of this button in the UI (between 0 and 500) </param>
        public Button(Dictionary<String, Graphics.Animation> animations, Vector2 position, Sound.Sound sound, int layer = 0) : base(animations, position, layer)
        {
            ClickSound = sound;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public override void Update(GameTime gameTime)
        {
            //Updates the mouse states
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            //If hovering the button
            if (MouseRectangle.Intersects(Rectangle))
            {
                //Changes the color to the hovering color
                if (Sprite != null)
                    Sprite.Colour = HoverColor;
                else if (AnimationManager != null)
                    AnimationManager.Colour = HoverColor;
                else
                    _color = HoverColor;

                //If pressing the button invoke the action and plays the sound
                if (_previousMouse.LeftButton == ButtonState.Pressed && _currentMouse.LeftButton == ButtonState.Released)
                {
                    OnClick?.Invoke(gameTime);
                    if (ClickSound != null)
                        Sound.SoundManager.Add(this.ClickSound.CreateInstance());
                }

            }
            //If not hovering
            else
            {
                //Changes the color to the normal color
                if (Sprite != null)
                    Sprite.Colour = BaseColor;
                else if (AnimationManager != null)
                    AnimationManager.Colour = BaseColor;
                else
                    _color = BaseColor;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
        public override String ToString()
        {
            if (Sprite != null)
            {
                string result = "Button(\n\tSprite: " + Sprite.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + ", \n\tSound: ";
                if (ClickSound != null)
                    result += ClickSound.ToString().Replace("\n", "\n\t");
                else
                    result += "NULL";
                result += ", \n\tClick Action: " + OnClick + "\n)";
                return result;
            }
            else if (AnimationManager != null)
            {
                string result = "Button(\n\tAnimationManager: " + AnimationManager.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + ", \n\tSound: ";
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