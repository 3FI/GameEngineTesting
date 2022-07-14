using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    /// <summary>
    /// Gridmap object that represents the map of a scene. Both the rigidbodies and the sprites.
    /// </summary>
    public class Map
    {
        //SPRITE
        //"." means an empty tile
        //The other char are defined by the dictionnary

        //RIGIDBODY
        //"-" means an horizontal rigidbody
        //"|" means a vertical rigidbody

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Height of the scene, Is used to make sure the height of the scene is the same as the height of the map
        /// </summary>
        private int _width;
        /// <summary>
        /// Width of the scene, Is used to make sure the width of the scene is the same as the width of the map
        /// </summary>
        private int _height;
        /// <summary>
        /// 3 Dimensional array that represents the sprites of the map. The first dimension is the layer of the sprite and the two others the coordinate. This is the original input in the constructor
        /// </summary>
        private Char[,,] _content;
        /// <summary>
        /// 2 Dimensional array that represents the rigidbodies of the map. This is the original input in the constructor
        /// </summary>
        private Char[,] _rigidBodies;
        /// <summary>
        /// LinkedList of all the Tiles in the map. Each tiles has multiple position not single.
        /// </summary>
        public LinkedList<Tile>[] Tiles;
        /// <summary>
        /// LinkedList of all the RigidBodies in the map.
        /// </summary>
        public LinkedList<Collision.RigidBody> RigidBodies;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructs a map from the given arrays
        /// </summary>
        /// <param name="content">3 Dimensional array that represents the sprites of the map. The first dimension is the layer of the sprite and the two others the coordinates.</param>
        /// <param name="tiles">Dictionnary of tiles that the Content search in when reading it's character. ("."=empty)</param>
        /// <param name="rigidbody">2 Dimensional array that represents the rigidbodies of the map. This is the original input in the constructor. ("."=empty, "-"=horizontal hitbox, "|"=vertical hitbox)</param>
        /// <param name="width">Width of the Scene for which this map is built. Used to verify the map fits the scene.</param>
        /// <param name="height">Height of the Scene for which this map is built. Used to verify the map fits the scene.</param>
        public Map(Char[,,] content, Dictionary<char,Tile> [] tiles, Char[,] rigidbody, int width, int height)
        {
            //Stores all the input in the backup variables
            _width = width;
            _height = height;
            _content = content;
            _rigidBodies = rigidbody;

            //Verify the map fits the scene in height and width
            if (content.GetLength(1) != _height) System.Diagnostics.Debug.WriteLine("Scene Height not Equal to Map Height : " + _height + " != " + content.GetLength(1));
            if (content.GetLength(2) != _width) System.Diagnostics.Debug.WriteLine("Scene Width not Equal to Map Width : " + _width + " != " + content.GetLength(2));

            //Initialize Tiles
            Tiles = new LinkedList<Tile>[tiles.Length];
            for (int i = 0; i < this.Tiles.Length; i++) this.Tiles[i] = new LinkedList<Tile>();

            //Initialize RigidBodies
            RigidBodies = new LinkedList<Collision.RigidBody>();

            // Analyse _content to get the coordinates at which the tiles shall be draw

            // Analyse each layer
            for (int k = 0; k < content.GetLength(0); k++)
            {
                //Initialize the result of the analysis for this layer
                Dictionary<Tile, LinkedList<Vector2>> Result = new Dictionary<Tile, LinkedList<Vector2>>();

                //Analyse each rows
                for (int i = 0; i < content.GetLength(1); i++)
                {
                    //Analyse each collumns
                    for (int j = 0; j < content.GetLength(2); j++)
                    {
                        if (content[k, i, j] != '.')
                        {
                            //If the char is in the dictionnary, adds the position to the result
                            if (tiles[k].ContainsKey(content[k, i, j]))
                            {
                                //If this tile has already been initialized in results, simply add the position to the linkedlist.
                                if (Result.ContainsKey(tiles[k][content[k, i, j]]))
                                    Result[tiles[k][content[k, i, j]]].AddLast(new Vector2(j + 0.5f, i + 0.5f));
                                //Else : Initialize the tile in the Dictionnary with this Vector2 as the LinkedList.
                                else
                                {
                                    Result.Add(tiles[k][content[k, i, j]], new LinkedList<Vector2>());
                                    Result[tiles[k][content[k, i, j]]].AddLast(new Vector2(j + 0.5f, i + 0.5f));
                                }
                            }
                        }
                            
                    }
                }

                //Puts the positions in the animationManager/Sprite and then puts the tiles inside of Tiles
                foreach (Tile tile in Result.Keys)
                {
                    if (tile.AnimationManager != null)
                    {
                        tile.AnimationManager.MultiplePosition = Result[tile];
                        tile.AnimationManager.Layer = k;
                        Tiles[k].AddLast(tile);
                    }
                    else if (tile.Sprite != null)
                    {
                        tile.Sprite.MultiplePosition = Result[tile];
                        Tiles[k].AddLast(tile);
                    }
                    else System.Diagnostics.Debug.WriteLine("Missing texture for tile at positions" + Result[tile]);
                }
            }

            // Analyse _rigidBodies to get where there is horizontal plateform

            //Analyse each row (Rows by Rows first because when we find an horizontal rb, we continue in the same row to see how long it is)
            for (int k = 0; k < rigidbody.GetLength(0); k++)
            {
                //Analyse each collumns
                for (int i = 0; i < rigidbody.GetLength(1); i++)
                {
                    if (rigidbody[k, i] == '.') continue;
                    if (rigidbody[k,i] == '-')
                    {
                        Vector2 left = new Vector2(i, k - 0.5f);
                        i++;
                        while (i < rigidbody.GetLength(1) && rigidbody[k,i] == '-')
                        {
                            i++;
                        }
                        Vector2 right = new Vector2(i, k + 0.5f);
                        RigidBodies.AddLast(new Collision.RB_Square(left, right, new Vector2((right.X + left.X)/2, right.Y), true));
                    }
                }
            }

            // Analyse _rigidBodies to get where there is vertical walls

            //Analyse each collumns (Collumns by Collumns first because when we find a vertical rb, we continue in the same collumn to see how long it is)
            for (int i = 0; i < rigidbody.GetLength(1); i++) 
            {
                //Analyse each rows
                for (int k = 0; k < rigidbody.GetLength(0); k++)
                {
                    if (rigidbody[k, i] == '.') continue;
                    if (rigidbody[k, i] == '|')
                    {
                        Vector2 up = new Vector2(i, k);
                        k++;
                        while (rigidbody[k, i] == '|' && k < rigidbody.GetLength(0))
                        {
                            k++;
                        }
                        Vector2 down = new Vector2(i + 1f, k);
                        RigidBodies.AddLast(new Collision.RB_Square(up, down, new Vector2(down.X - 0.5f, (down.Y + up.Y) / 2), true));
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (LinkedList<Tile> layer in Tiles)
            {
                foreach (Tile tile in layer) tile.Draw(spriteBatch, gameTime);
            }
            if (Game1.debug == true) foreach (Collision.RigidBody rb in RigidBodies) rb.Draw(spriteBatch);
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
