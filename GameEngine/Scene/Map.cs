using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Scene
{
    class Map
    {
        //TODO
        int Width;
        int Height;
        //IMPLEMENT

        public LinkedList<Sprite>[] sprites;
        public LinkedList<Collision.RigidBody> rigidBodies;

        public Map(Char[][][] Sprite, Dictionary<char,String>[] Texture, Char[][] RigidBody)
        {
            foreach (Char[][] Layer in Sprite)
            {
                Dictionary<String, Vector2> Result;
                //Analyse each Layer
            }

            //Analyse the RigidBodies
        }
    }
}
