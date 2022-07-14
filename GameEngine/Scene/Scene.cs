using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
    /// <summary>
    /// Abstract class that represents a scene in the game
    /// </summary>
    public abstract class Scene
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Width of the scene in units
        /// </summary>
        public int Width;
        /// <summary>
        /// Height of the scene in units
        /// </summary>
        public int Height;
        /// <summary>
        /// The camera around which the scene is drawn. (Nullable)
        /// </summary>
        public Camera Camera;
        /// <summary>
        /// Map of the scene. (Nullable)
        /// </summary>
        public Map map;
        /// <summary>
        /// LinkedList of the Gameobject in the scene. (Nullable)
        /// </summary>
        public LinkedList<GameObject> Content;
        /// <summary>
        /// LinkedList of the UI.Component in the scene. (Nullable)
        /// </summary>
        public LinkedList<UI.Component> Ui;
        /// <summary>
        /// LinkedList of the TriggerBoxes in the scene. (Nullable)
        /// </summary>
        public LinkedList<Collision.TriggerBox> TriggerBoxes;
        /// <summary>
        /// Dictionnary of all the music that can be played (Nullable)
        /// </summary>
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
        public virtual void Update(GameTime gameTime)           
            //The base that is called for every in game scene. Menus are completly overwriten instead
        {
            if (!Game1.pauseHandling())
            {
                //Collisions Update
                LinkedList<Collision.RigidBody> rb = new LinkedList<Collision.RigidBody>();
                if (Content != null) foreach (GameObject gameObject in Content)
                        rb.AddLast(gameObject.Rigidbody);
                if (map != null) foreach (Collision.RigidBody rigidbody in map.RigidBodies)
                        rb.AddLast(rigidbody);
                Collision.Collision.simulate(rb);

                //Content Update
                if (Content != null)
                    foreach (GameObject gameobject in Content)
                    {
                        gameobject.Update(gameTime);
                    }

                //TODO : MAP UPDATE

                //TriggerBox Update
                if (TriggerBoxes != null)
                    foreach (Collision.TriggerBox triggerBox in TriggerBoxes)
                        triggerBox.Update(gameTime);

                //Camera Update
                if (Camera != null)
                {
                    Camera.Update(gameTime);
                }
            }

            //UI Update
            if (Ui != null)
                foreach (UI.Component component in Ui)
                {
                    component.Update(gameTime);
                }
        }

        /// <summary>
        /// Draw the scene to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Drawing the map
            if (map != null)
                map.Draw(spriteBatch, gameTime);
            //Drawing the gameobjects
            if (Content != null)
                foreach (GameObject gameobject in Content)
                {
                    gameobject.Draw(spriteBatch, gameTime);
                }
            //Drawing the ui component
            if (Ui != null)
                foreach (UI.Component component in Ui)
                {
                    component.Draw(spriteBatch, gameTime);
                }
            //Drawing the triggerboxes if the debug menu is enabled
            if (Game1.debug && TriggerBoxes != null)
                foreach (Collision.TriggerBox triggerBox in TriggerBoxes)
                {
                    triggerBox.Draw(spriteBatch, gameTime);
                }
        }

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
