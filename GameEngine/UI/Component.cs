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
        protected SpriteFont _font = Game1.BaseFont;
        protected Color _fontColour = Color.White;
        protected string _text = "";

        public Sprite sprite;
        public AnimationManager animationManager;
        public Dictionary<String, Animation> animationDict;
        public Sound.Sound ClickSound;

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)(sprite.Position.X * Game1.pxPerUnit) - sprite.Texture.Width/2, (int)(sprite.Position.Y * Game1.pxPerUnit) - sprite.Texture.Height/2, sprite.Texture.Width, sprite.Texture.Height); }
        }

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

            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);

            var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
            var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);
            spriteBatch.DrawString(_font, _text, new Vector2(x, y), _fontColour);
        }
    }
}
