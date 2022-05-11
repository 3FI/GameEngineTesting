using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    internal class Map
    {
        //TODO : Implement Width failsafe
        int Width;
        int Height;

        public LinkedList<Sprite>[] sprites;
        public LinkedList<Collision.RigidBody> rigidBodies;

        public Map(Char[,,] Sprite, Dictionary<char,String> [] Texture, Char[,] RigidBody)
        {
            sprites = new LinkedList<Sprite>[Texture.Length];
            for (int i = 0; i < sprites.Length; i++) sprites[i] = new LinkedList<Sprite>();
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
                    sprites[k].AddLast(new Sprite(texture, Result[texture]));
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
            foreach (LinkedList<Sprite> layer in sprites) foreach (Sprite sprite in layer) sprite.Draw(gameTime, spriteBatch);
            if (Game1.debug == true) foreach (Collision.RigidBody rb in rigidBodies) rb.Draw(spriteBatch);
        }
    }
}
