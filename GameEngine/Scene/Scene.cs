using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    public abstract class Scene
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public int Width;
        public int Height;
        public Camera Camera;
        public Map map;
        public LinkedList<GameObject> Content;
        public LinkedList<UI.Component> Ui;
        public Dictionary<String, Sound.Music> Musics;

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Dispose of this scene
        /// </summary>
        public abstract void Kill();

        /// <summary>
        /// Update the scene
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw the scene to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public override String ToString()
        {
            string result = "Scene(\n\tWidth: " + Width + ", \n\tHeight: " + Height + ", \n\tCamera: \n\t";

            if (Camera != null)
                result += Camera.ToString().Replace("\n", "\n\t") + ", \n\tMusic: ";
            else
                result += "NULL \n\tMusic: ";

            if (Musics != null)
                foreach (Sound.Music music in Musics.Values) result += "\n" + music.ToString().Replace("\n", "\n\t");
            else
                result += "NULL \n\t";
            
            if (map != null)
                result += ", \n\tMap: \n\t" + map.ToString().Replace("\n", "\n\t") + ", \n\tContent: ";
            else
                result += ", \n\tMap: NULL" + ", \n\tContent: ";

            if (Content != null)
                foreach (GameObject gameObject in Content) result += "\n\t" + gameObject.ToString().Replace("\n", "\n\t");
            else
                result += "NULL";

            result += ", \n\tUi: ";

            if (Ui != null)
                foreach (UI.Component component in Ui) result += "\n\t" + component.ToString().Replace("\n", "\n\t");
            else
                result += "NULL";

            result += "\n)";
            return result;
        }
    }
}
