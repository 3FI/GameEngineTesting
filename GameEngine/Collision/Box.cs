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
        public float Angle;

        public Box(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        public bool intersectBox(Box b)
        {
            if (Angle % 90 == 0 && b.Angle % 90 == 0)
                return Math.Max(this.topLeft.X, b.topLeft.X) < Math.Min(this.bottomRight.X, b.bottomRight.X) && Math.Min(this.bottomRight.Y, b.bottomRight.Y) > Math.Max(this.topLeft.Y, b.topLeft.Y);
            else
            {
                Vector2 LR = Tools.CustomMath.RotateAround(bottomRight, new Vector2( (bottomRight.X - topLeft.X) / 2 + topLeft.X, (bottomRight.Y - topLeft.Y) / 2 + topLeft.Y ), Angle);
                Vector2 UL = Tools.CustomMath.RotateAround(topLeft, new Vector2((bottomRight.X - topLeft.X) / 2 + topLeft.X, (bottomRight.Y - topLeft.Y) / 2 + topLeft.Y), Angle);
                Vector2 UR = Tools.CustomMath.RotateAround(new Vector2(bottomRight.X, topLeft.Y), new Vector2((bottomRight.X - topLeft.X) / 2 + topLeft.X, (bottomRight.Y - topLeft.Y) / 2 + topLeft.Y), Angle);
                Vector2 LL = Tools.CustomMath.RotateAround(new Vector2(topLeft.X, bottomRight.Y), new Vector2((bottomRight.X - topLeft.X) / 2 + topLeft.X, (bottomRight.Y - topLeft.Y) / 2 + topLeft.Y), Angle);

                Vector2 cLR = Tools.CustomMath.RotateAround(b.bottomRight, new Vector2((b.bottomRight.X - b.topLeft.X) / 2 + b.topLeft.X, (b.bottomRight.Y - b.topLeft.Y) / 2 + b.topLeft.Y), b.Angle);
                Vector2 cUL = Tools.CustomMath.RotateAround(b.topLeft, new Vector2((b.bottomRight.X - b.topLeft.X) / 2 + b.topLeft.X, (b.bottomRight.Y - b.topLeft.Y) / 2 + b.topLeft.Y), b.Angle);
                Vector2 cUR = Tools.CustomMath.RotateAround(new Vector2(b.bottomRight.X, b.topLeft.Y), new Vector2((b.bottomRight.X - b.topLeft.X) / 2 + b.topLeft.X, (b.bottomRight.Y - b.topLeft.Y) / 2 + b.topLeft.Y), b.Angle);
                Vector2 cLL = Tools.CustomMath.RotateAround(new Vector2(b.topLeft.X, b.bottomRight.Y), new Vector2((b.bottomRight.X - b.topLeft.X) / 2 + b.topLeft.X, (b.bottomRight.Y - b.topLeft.Y) / 2 + b.topLeft.Y), b.Angle);

                Vector2[] Points = new Vector2[] { LR, UL, UR, LL };
                Vector2[] cPoints = new Vector2[] { cLR, cUL, cUR, cLL };

                Vector2 Axis1 = UR - UL;
                Vector2 Axis2 = UR - LR;
                Vector2 Axis3 = cUL - cUR;
                Vector2 Axis4 = cUL - cLL;

                Vector2[] Axis = new Vector2[] { Axis1, Axis2, Axis3, Axis4 };

                foreach (Vector2 axis in Axis)
                {
                    float min = float.PositiveInfinity;
                    float max = float.NegativeInfinity;
                    foreach (Vector2 point in Points)
                    {
                        Vector2 p = Tools.CustomMath.Project(point, axis);
                        float scalar = Vector2.Dot(point, axis);
                        if (scalar > max)
                            max = scalar;
                        if (scalar < min)
                            min = scalar;
                    }
                    float cmin = float.PositiveInfinity;
                    float cmax = float.NegativeInfinity;
                    foreach (Vector2 point in cPoints)
                    {
                        Vector2 p = Tools.CustomMath.Project(point, axis);
                        float scalar = Vector2.Dot(point, axis);
                        if (scalar > cmax)
                            cmax = scalar;
                        if (scalar < cmin)
                            cmin = scalar;
                    }
                    if ((cmin <= max && cmax >= min))
                        continue;
                    else
                        return false;
                }
                return true;
            }
        }

        public bool isFullyInside(Box b)
        {
            //TODO : MAKE isFullyInside USE THE ANGLE
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
                zoom = Scene.SceneManager.scene.Camera.Zoom;
                cameraPosition = Scene.SceneManager.scene.Camera.Position;
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

            Vector2 p;
            p = Tools.CustomMath.RotateAround(
                new Vector2(
                    (int)TopLeft.X, 
                    (int)TopLeft.Y + ((int)BottomRight.Y - (int)TopLeft.Y) / 2), 
                new Vector2(
                    ((int)(BottomRight.X) - (int)(TopLeft.X))/2 + TopLeft.X,
                    ((int)(BottomRight.Y) - (int)(TopLeft.Y))/2 + TopLeft.Y),
                Angle);

            spriteBatch.Draw(
                _pointTexture, 
                new Rectangle(
                    (int)p.X,
                    (int)p.Y,
                    lineWidth,
                    (int)(BottomRight.Y) - (int)(TopLeft.Y) + lineWidth),
                new Rectangle(1, 1, 1, 1),
                color,
                Angle / 360 * 2 * (float)Math.PI,
                new Vector2( 0.5f, 0.5f ),
                SpriteEffects.None,
                0.5f
            );

            p = Tools.CustomMath.RotateAround(
                new Vector2(
                    (int)(TopLeft.X) + ((int)BottomRight.X - (int)TopLeft.X) / 2,
                    (int)(TopLeft.Y)),
                new Vector2(
                    ((int)(BottomRight.X) - (int)(TopLeft.X)) / 2 + TopLeft.X,
                    ((int)(BottomRight.Y) - (int)(TopLeft.Y)) / 2 + TopLeft.Y),
                Angle);

            spriteBatch.Draw(
                _pointTexture, 
                new Rectangle(
                    (int)p.X,      
                    (int)p.Y,
                    (int)(BottomRight.X) - (int)(TopLeft.X) + lineWidth,    
                    lineWidth),
                new Rectangle(1, 1, 1, 1),
                color,
                Angle / 360 * 2 * (float)Math.PI,
                new Vector2(0.5f, 0.5f),
                SpriteEffects.None,
                0.5f);

            p = Tools.CustomMath.RotateAround(
                new Vector2(
                    (int)(BottomRight.X),
                    (int)(TopLeft.Y) + ((int)BottomRight.Y - (int)TopLeft.Y) / 2),
                new Vector2(
                    ((int)(BottomRight.X) - (int)(TopLeft.X)) / 2 + TopLeft.X,
                    ((int)(BottomRight.Y) - (int)(TopLeft.Y)) / 2 + TopLeft.Y),
                Angle);

            spriteBatch.Draw(
                _pointTexture, 
                new Rectangle(
                    (int)p.X,  
                    (int)p.Y,
                    lineWidth,
                    (int)(BottomRight.Y) - (int)(TopLeft.Y) + lineWidth),
                new Rectangle(1, 1, 1, 1),
                color,
                Angle / 360 * 2 * (float)Math.PI,
                new Vector2(0.5f, 0.5f),
                SpriteEffects.None,
                0.5f);

            p = Tools.CustomMath.RotateAround(
                new Vector2(
                    (int)(TopLeft.X) + ((int)BottomRight.X - (int)TopLeft.X) / 2,
                    (int)(BottomRight.Y)),
                new Vector2(
                    ((int)(BottomRight.X) - (int)(TopLeft.X)) / 2 + TopLeft.X,
                    ((int)(BottomRight.Y) - (int)(TopLeft.Y)) / 2 + TopLeft.Y),
                Angle);

            spriteBatch.Draw(
                _pointTexture, 
                new Rectangle(
                    (int) p.X,      
                    (int) p.Y,
                    (int)(BottomRight.X) - (int)(TopLeft.X) + lineWidth,    
                    lineWidth),
                new Rectangle(1, 1, 1, 1),
                color,
                Angle / 360 * 2 * (float)Math.PI,
                new Vector2(0.5f, 0.5f),
                SpriteEffects.None,
                0.5f);
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
