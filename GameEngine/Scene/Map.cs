using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    public class Map
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private int _width;
        private int _height;
        private Char[,,] _sprites;
        private Char[,] _rigidBodies;
        public LinkedList<Graphics.Sprite>[] sprites;
        public LinkedList<Collision.RigidBody> rigidBodies;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Map(Char[,,] Sprite, Dictionary<char,String> [] Texture, Char[,] RigidBody, int width, int height)
        {
            _width = width;
            _height = height;
            _sprites = Sprite;
            _rigidBodies = RigidBody;

            if (Sprite.GetLength(1) != _height) System.Diagnostics.Debug.WriteLine("Scene Height not Equal to Map Height : " + _height + " != " + Sprite.GetLength(1));
            if (Sprite.GetLength(2) != _width) System.Diagnostics.Debug.WriteLine("Scene Width not Equal to Map Width : " + _width + " != " + Sprite.GetLength(2));

            sprites = new LinkedList<Graphics.Sprite>[Texture.Length];
            for (int i = 0; i < sprites.Length; i++) sprites[i] = new LinkedList<Graphics.Sprite>();
            rigidBodies = new LinkedList<Collision.RigidBody>();

            for (int k = 0; k < Sprite.GetLength(0); k++)
            {
                Dictionary<String, LinkedList<Vector2>> Result = new Dictionary<string, LinkedList<Vector2>>();
                for (int i = 0; i < Sprite.GetLength(1); i++)
                {
                    for (int j = 0; j < Sprite.GetLength(2); j++)
                    {
                        if (Sprite[k, i, j] != '.')
                        {
                            if (Texture[k].ContainsKey(Sprite[k, i, j]))
                            {
                                if (Result.ContainsKey(Texture[k][Sprite[k, i, j]]))
                                    Result[Texture[k][Sprite[k, i, j]]].AddLast(new Vector2(j + 0.5f, i + 0.5f));
                                else
                                {
                                    Result.Add(Texture[k][Sprite[k, i, j]], new LinkedList<Vector2>());
                                    Result[Texture[k][Sprite[k, i, j]]].AddLast(new Vector2(j + 0.5f, i + 0.5f));
                                }
                            }
                        }
                            
                    }
                }
                foreach (String texture in Result.Keys)
                {
                    sprites[k].AddLast(new Graphics.Sprite(texture, Result[texture]));
                }
            }

            for (int k = 0; k < RigidBody.GetLength(0); k++)
            {
                for (int i = 0; i < RigidBody.GetLength(1); i++)
                {
                    if (RigidBody[k, i] == '.') continue;
                    if (RigidBody[k,i] == '-')
                    {
                        Vector2 left = new Vector2(i, k - 0.5f);
                        i++;
                        while (i < RigidBody.GetLength(1) && RigidBody[k,i] == '-')
                        {
                            i++;
                        }
                        Vector2 right = new Vector2(i - 1 + 1, k + 0.5f);
                        rigidBodies.AddLast(new Collision.RB_Square(left, right, new Vector2((right.X + left.X)/2, right.Y), true));
                    }
                    else if (RigidBody[k, i] == '|')
                    {
                        int j = k;
                        Vector2 up = new Vector2(i - 0.5f, k);
                        j++;
                        while (RigidBody[k, j] == '|' && k < RigidBody.GetLength(0))
                        {
                            j++;
                        }
                        Vector2 down = new Vector2(j - 0.5f, k - 1 - 1);
                        rigidBodies.AddLast(new Collision.RB_Square(up, down, new Vector2(down.X, (down.Y + up.Y)/2), true));
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (LinkedList<Graphics.Sprite> layer in sprites) foreach (Graphics.Sprite sprite in layer) sprite.Draw(gameTime, spriteBatch);
            if (Game1.debug == true) foreach (Collision.RigidBody rb in rigidBodies) rb.Draw(spriteBatch);
        }
        public override String ToString()
        {
            string result = "Map(\n\tWidth: " + _width + ", \n\tHeight: " + _height + ", \n\tSprites: \n\t\t";
            for (int i = 0; i < _sprites.GetLength(0); i++)
            {
                for (int j = 0; j < _sprites.GetLength(1); j++)
                {
                    for (int k = 0; k < _sprites.GetLength(2); k++)
                    {
                        result += _sprites[i, j, k];
                    }
                    result += "\n\t\t";
                }
                result += "\n\t\t";
            }
            result += "\n\tRigidBody: \n\t\t";
            for (int i = 0; i < _rigidBodies.GetLength(0); i++)
            {
                for (int j = 0; j < _rigidBodies.GetLength(1); j++)
                {
                    result += _rigidBodies[i, j];
                }
                result += "\n\t\t";
            }
            result += "\n)";
            return result;
        }
    }
}
