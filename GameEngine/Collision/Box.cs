using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Collision
{
    public class Box
    {
        public Vector2 topLeft;
        public Vector2 bottomRight;

        public Box(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        public bool intersectBox(Box b)
        {
            return Math.Max(this.topLeft.X, b.topLeft.X) < Math.Min(this.bottomRight.X, b.bottomRight.X) && Math.Min(this.bottomRight.Y, b.bottomRight.Y) > Math.Max(this.topLeft.Y, b.topLeft.Y);
        }

        public bool isFullyInside(Box b)
        {
            return this.topLeft.X >= b.topLeft.X && this.bottomRight.X <= b.bottomRight.X && this.topLeft.Y >= b.topLeft.Y && this.bottomRight.Y <= b.bottomRight.Y;
        }

        public double getWidth()
        {
            return this.bottomRight.X - this.topLeft.X;
        }

        public double getHeight()
        {
            return this.bottomRight.Y - this.topLeft.Y;
        }

        public double getMidX()
        {
            return this.topLeft.X + this.getWidth()/2f;
        }

        public double getMidY()
        {
            return this.topLeft.Y + this.getHeight()/2f;
        }

        public double getArea()
        {
            return this.getWidth() * this.getHeight();
        }

        public Vector2 getPosition()
        {
            return Vector2.Divide(Vector2.Add(this.topLeft, this.bottomRight), 2);
        }

        static private Texture2D _pointTexture;
        public void Draw(SpriteBatch spriteBatch, Color color, int lineWidth)
        {
            if(_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            float zoom;
            Vector2 cameraPosition;
            float cameraWidth;
            float cameraHeight;
            if (Scene.SceneManager.scene.Camera != null)
            {
                zoom = Scene.SceneManager.scene.Camera.zoom;
                cameraPosition = Scene.SceneManager.scene.Camera.position;
                cameraWidth = Scene.SceneManager.scene.Camera.Width;
                cameraHeight = Scene.SceneManager.scene.Camera.Height;
            }
            else
            {
                zoom = 1f;
                cameraPosition = new Vector2(Game1.screenWidth / 2 / Game1.pxPerUnit, Game1.screenHeight / 2 / Game1.pxPerUnit);
                cameraWidth = Game1.screenWidth / Game1.pxPerUnit;
                cameraHeight = Game1.screenHeight / Game1.pxPerUnit;
            }

            Vector2 TopLeft = new Vector2(64*zoom* (this.topLeft.X - cameraPosition.X + cameraWidth / 2) , 64*zoom* (this.topLeft.Y - cameraPosition.Y + cameraHeight / 2));
            Vector2 BottomRight = new Vector2(64 * zoom * (this.bottomRight.X - cameraPosition.X + cameraWidth / 2), 64 * zoom * (this.bottomRight.Y - cameraPosition.Y + cameraHeight / 2));
            
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(TopLeft.X),      (int)(TopLeft.Y),      lineWidth,                                              (int)(BottomRight.Y) - (int)(TopLeft.Y) + lineWidth),   color);
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(TopLeft.X),      (int)(TopLeft.Y),      (int)(BottomRight.X) - (int)(TopLeft.X) + lineWidth,    lineWidth),                                             color);
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(BottomRight.X),  (int)(TopLeft.Y),      lineWidth,                                              (int)(BottomRight.Y) - (int)(TopLeft.Y) + lineWidth),   color);
            spriteBatch.Draw(_pointTexture, new Rectangle((int)(TopLeft.X),      (int)(BottomRight.Y),  (int)(BottomRight.X) - (int)(TopLeft.X) + lineWidth,    lineWidth),                                             color);
        }

        public override String ToString()
        {
            return "Box(topLeft: " + topLeft + ", bottomRight: " + bottomRight + ")";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (topLeft == ((Box)obj).topLeft) && (bottomRight == ((Box)obj).bottomRight);
            }
        }
    }
}
