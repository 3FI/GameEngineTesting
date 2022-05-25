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
        private Char[,,] _content;
        private Char[,] _rigidBodies;
        public LinkedList<Tile>[] tiles;
        public LinkedList<Collision.RigidBody> rigidBodies;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Map(Char[,,] Content, Dictionary<char,Tile> [] Tiles, Char[,] RigidBody, int width, int height)
        {
            _width = width;
            _height = height;
            _content = Content;
            _rigidBodies = RigidBody;

            if (Content.GetLength(1) != _height) System.Diagnostics.Debug.WriteLine("Scene Height not Equal to Map Height : " + _height + " != " + Content.GetLength(1));
            if (Content.GetLength(2) != _width) System.Diagnostics.Debug.WriteLine("Scene Width not Equal to Map Width : " + _width + " != " + Content.GetLength(2));

            tiles = new LinkedList<Tile>[Tiles.Length];
            for (int i = 0; i < tiles.Length; i++) tiles[i] = new LinkedList<Tile>();
            rigidBodies = new LinkedList<Collision.RigidBody>();

            for (int k = 0; k < Content.GetLength(0); k++)
            {
                Dictionary<Tile, LinkedList<Vector2>> Result = new Dictionary<Tile, LinkedList<Vector2>>();
                for (int i = 0; i < Content.GetLength(1); i++)
                {
                    for (int j = 0; j < Content.GetLength(2); j++)
                    {
                        if (Content[k, i, j] != '.')
                        {
                            if (Tiles[k].ContainsKey(Content[k, i, j]))
                            {
                                if (Result.ContainsKey(Tiles[k][Content[k, i, j]]))
                                    Result[Tiles[k][Content[k, i, j]]].AddLast(new Vector2(j + 0.5f, i + 0.5f));
                                else
                                {
                                    Result.Add(Tiles[k][Content[k, i, j]], new LinkedList<Vector2>());
                                    Result[Tiles[k][Content[k, i, j]]].AddLast(new Vector2(j + 0.5f, i + 0.5f));
                                }
                            }
                        }
                            
                    }
                }
                foreach (Tile tile in Result.Keys)
                {
                    if (tile.animationManager != null)
                    {
                        tile.animationManager.MultiplePosition = Result[tile];
                        tile.animationManager.Layer = k;
                        tiles[k].AddLast(tile);
                    }
                    else if (tile.sprites != null)
                    {
                        tile.sprites.MultiplePosition = Result[tile];
                        tiles[k].AddLast(tile);
                    }
                    else System.Diagnostics.Debug.WriteLine("Missing tile at positions" + Result[tile]);
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
            foreach (LinkedList<Tile> layer in tiles)
            {
                foreach (Tile tile in layer) tile.Draw(spriteBatch, gameTime);
            }
            if (Game1.debug == true) foreach (Collision.RigidBody rb in rigidBodies) rb.Draw(spriteBatch);
        }
        public override String ToString()
        {
            string result = "Map(\n\tWidth: " + _width + ", \n\tHeight: " + _height + ", \n\tSprites: \n\t\t";
            for (int i = 0; i < _content.GetLength(0); i++)
            {
                for (int j = 0; j < _content.GetLength(1); j++)
                {
                    for (int k = 0; k < _content.GetLength(2); k++)
                    {
                        result += _content[i, j, k];
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
