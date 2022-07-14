using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.UI
{
    /// <summary>
    /// Everything that is part of the UI is a component. 
    /// A component is the baseline of the UI from which can be built button or sliders
    /// A component either has a sprite, an animation manager (with it's animation dictionnary) or a simple rectangle
    /// All of the UI is stored as a linked list of component in the scene from which they will be drawn and updated
    /// </summary>
    public class Component
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Single pixel texture that will be initialized if needed (if there's no sprite or animation manager)
        /// </summary>
        private static Texture2D _pointTexture;

        /// <summary>
        /// Font of the text that will be drawn at the center of the component
        /// </summary>
        protected SpriteFont _font = Game1.BaseFont;
        /// <summary>
        /// Colr of the text that will be drawn at the center of the component
        /// </summary>
        protected Color _fontColour = Color.White;
        /// <summary>
        /// Text that will be drawn at the center of the component
        /// </summary>
        protected string _text = "";

        /// <summary>
        /// Position of the center of this component
        /// </summary>
        protected Vector2 _position;

        /// <summary>
        /// Height of the rectangle that will be drawn.
        /// Is initialized only if there's no sprite or animation
        /// </summary>
        protected float _height;
        /// <summary>
        /// Width of the rectangle that will be drawn.
        /// Is initialized only if there's no sprite or animation
        /// </summary>
        protected float _width;

        /// <summary>
        /// Color of the texture. Not in the constructor
        /// </summary>
        protected Color _color = Color.White;

        public Graphics.Sprite Sprite;

        public Graphics.AnimationManager AnimationManager;
        public Dictionary<String, Graphics.Animation> AnimationDict;

        /// <summary>
        /// Get a rectangle englobing the entire component
        /// </summary>
        public Rectangle Rectangle
        {
            get { 
                if (Sprite != null)
                    return new Rectangle(
                        (int)(_position.X * Game1.pxPerUnit) - Sprite.Texture.Width/2, 
                        (int)(_position.Y * Game1.pxPerUnit) - Sprite.Texture.Height/2, 
                        Sprite.Texture.Width, 
                        Sprite.Texture.Height); 
                else if (AnimationManager != null)
                    return new Rectangle(
                        (int)(_position.X * Game1.pxPerUnit) - AnimationManager.FrameWidth / 2,
                        (int)(_position.Y * Game1.pxPerUnit) - AnimationManager.FrameHeight / 2,
                        AnimationManager.FrameWidth,
                        AnimationManager.FrameHeight);
                else
                    return new Rectangle(
                        (int)(_position.X * Game1.pxPerUnit - _width * Game1.pxPerUnit / 2),
                        (int)(_position.Y * Game1.pxPerUnit - _height * Game1.pxPerUnit / 2),
                        (int)(_width * Game1.pxPerUnit),
                        (int)(_height * Game1.pxPerUnit));
            }
        }
        /// <summary>
        /// Access point for the position and setting the position of the texture
        /// </summary>
        public Vector2 Position 
        {   
            get { return _position; }
            set { _position = value; if (Sprite != null) Sprite.Position = value; if (AnimationManager != null) AnimationManager.Position = value; }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// With Text
        /// Without Texture
        /// </summary>
        /// <param name="height"> Height of this textureless component </param>
        /// <param name="width"> Width of this textureless component </param>
        /// <param name="position"> Center of this component </param>
        /// <param name="font"> Font of the text </param>
        /// <param name="text"> Text at the center of the component </param>
        /// <param name="textColour"> Colour of the text </param>
        /// <param name="layer"> Layer of this component in the UI (between 0 and 500) </param>
        public Component(float height, float width, Vector2 position, SpriteFont font, string text, Color textColour, int layer = 0)
        {
            _height = height;
            _width = width;

            Position = position;

            _font = font;
            _text = text;
            _fontColour = textColour;

            _color = Color.Black;
        }
        /// <summary>
        /// With Text
        /// With Sprite
        /// </summary>
        /// <param name="texture"> Represents the address of the file that will be used as a the texture of the sprite </param>
        /// <param name="position"> Center of this component </param>
        /// <param name="font"> Font of the text </param>
        /// <param name="text"> Text at the center of the component </param>
        /// <param name="textColour"> Colour of the text </param>
        /// <param name="layer"> Layer of this component in the UI (between 0 and 500) </param>
        public Component(string texture, Vector2 position, SpriteFont font, string text, Color textColour, int layer = 0)
        {
            //The true at the end is used to fix the sprite such that it doesn't move with the camera
            Sprite = new Graphics.Sprite(texture, position, true);

            Position = position;

            _font = font;
            _text = text;
            _fontColour = textColour;
        }
        /// <summary>
        /// With Text
        /// With Animator
        /// </summary>
        /// <param name="animations"> Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="position"> Center of this component </param>
        /// <param name="font"> Font of the text </param>
        /// <param name="text"> Text at the center of the component </param>
        /// <param name="textColour"> Colour of the text </param>
        /// <param name="layer"> Layer of this component in the UI (between 0 and 500) </param>
        public Component(Dictionary<String, Graphics.Animation> animations, Vector2 position, SpriteFont font, string text, Color textColour, int layer = 0)
        {
            this.AnimationDict = animations;
            try
            {
                //The true at the end is used to fix the AnimationManager position such that it doesn't move with the camera
                this.AnimationManager = new Graphics.AnimationManager(this.AnimationDict["Default"], true);
            }
            catch (KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("Texture2D/PlaceHolderTexture", 0);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                //The true at the end is used to fix the AnimationManager position such that it doesn't move with the camera
                this.AnimationManager = new Graphics.AnimationManager(error, true);
            }

            Position = position;

            _font = font;
            _text = text;
            _fontColour = textColour;
        }

        /// <summary>
        /// Without Text
        /// Without Texture
        /// </summary>
        /// <param name="height"> Height of this textureless component </param>
        /// <param name="width"> Width of this textureless component </param>
        /// <param name="position"> Center of this component </param>
        /// <param name="layer"> Layer of this component in the UI (between 0 and 500) </param>
        public Component(float height, float width, Vector2 position, int layer = 0)
        {
            _height = height;
            _width = width;

            Position = position;

            _color = Color.Black;
        }
        /// <summary>
        /// Without Text
        /// With Sprite
        /// </summary>
        /// <param name="texture"> Represents the address of the file that will be used as a the texture of the sprite </param>
        /// <param name="position"> Center of this component </param>
        /// <param name="layer"> Layer of this component in the UI (between 0 and 500) </param>
        public Component(string texture, Vector2 position, int layer = 0)
        {
            //The true at the end is used to fix the sprite such that it doesn't move with the camera
            Sprite = new Graphics.Sprite(texture, position, true);

            Position = position;
        }
        /// <summary>
        /// Without Text
        /// With Animation
        /// </summary>
        /// <param name="animations"> Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="position"> Center of this component </param>
        /// <param name="layer"> Layer of this component in the UI (between 0 and 500) </param>
        public Component(Dictionary<String, Graphics.Animation> animations, Vector2 position, int layer = 0)
        {
            this.AnimationDict = animations;
            try
            {
                //The true at the end is used to fix the AnimationManager position such that it doesn't move with the camera
                this.AnimationManager = new Graphics.AnimationManager(this.AnimationDict["Default"], true);
            }
            catch (KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("Texture2D/PlaceHolderTexture", 0);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                //The true at the end is used to fix the AnimationManager position such that it doesn't move with the camera
                this.AnimationManager = new Graphics.AnimationManager(error, true);
            }
            
            Position = position;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            //UI ANIMATIONMANAGER LOADING
            if (AnimationManager != null)
            {
                if (!AnimationManager.Load(content)) result = false;
            }
            if (AnimationDict != null)
            {
                foreach (Graphics.Animation animation in AnimationDict.Values)
                {
                    if (!animation.Load(content)) result = false;
                }
            }

            //UI SPRITE LOADING
            else if (Sprite != null)
            {
                if (!Sprite.Load(content)) result = false;
            }

            if (!result)
            {
                System.Diagnostics.Debug.WriteLine("Unable to load component " + this);
            }
            return result;
        }
        public virtual void Update(GameTime gameTime)
        {
            if (this.AnimationManager != null)
                this.AnimationManager.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (this.AnimationManager != null)
                this.AnimationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (this.Sprite != null)
                this.Sprite.Draw(gameTime, spriteBatch);

            //If there's no sprite or animation manager, draw the rectangle
            else 
            {
                if (_pointTexture == null)
                {
                    _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                    _pointTexture.SetData<Color>(new Color[] { Color.White });
                }
                spriteBatch.Draw(_pointTexture, 
                                 new Rectangle(
                                     (int)(Game1.pxPerUnit * Position.X - Game1.pxPerUnit * _width / 2), 
                                     (int)(Game1.pxPerUnit * Position.Y - Game1.pxPerUnit * _height / 2), 
                                     (int)(Game1.pxPerUnit * _width), 
                                     (int)(Game1.pxPerUnit * _height)), 
                                     _color);
            }

            //If there's text draws it to the screen
            if (_text != null)
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);
                spriteBatch.DrawString(_font, _text, new Vector2(x, y), _fontColour);
            }            
        }
        public override String ToString()
        {
            if (Sprite != null)
                return "Component(\n\tSprite: " + Sprite.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + "\n)";
            else if (AnimationManager != null)
                return "Component(\n\tAnimationManager: " + AnimationManager.ToString().Replace("\n", "\n\t") + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + "\n)";
            else
                return "Component(\n\tPosition: " + _position + ",\n\tWidth:" + _width + ",\n\tHeight:" + _height + ",\n\tColor:" + _color + ", \n\tText: " + _text + ", \n\tFont: " + _font + ", \n\tFont Colour: " + _fontColour + "\n)";
        }
    }
}
