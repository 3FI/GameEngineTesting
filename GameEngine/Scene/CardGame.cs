using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.Scene
{
    /// <summary>
    /// The test scenee for a card game
    /// </summary>
    public sealed class CardGame : Scene                  //sealed means impossible to inherit
    {
        /// <summary>
        /// The private constructor used since this is a singleton
        /// </summary>
        private CardGame()
        {

            /////////////////////////////////////////////////////////////////////////////////
            //                                    CONTENT                                  //
            /////////////////////////////////////////////////////////////////////////////////

            this.Width = 32;
            this.Height = 18;

            this.Musics = new Dictionary<string, Sound.Music>() 
            {
                { "Test", new Sound.Music("Music/Test/test")}
            };

            this.Content = new LinkedList<GameObject>();
            this.Content.AddLast(
                new Player(
                    new Vector2(4, 4),
                    new Vector2(0, 0), new Vector2(0, 0),
                    new Dictionary<string, Graphics.Animation>()
                    {
                        { "Default", new Graphics.Animation("Texture2D/Test/ball", 2, true) },
                    },
                    //new Collision.RB_Circle(0.5f),
                    new Collision.RB_Square(1, 1),
                    new Dictionary<string, Sound.Sound>()
                    {
                        {"test", new Sound.Sound("SoundEffect/Test/test",false)},
                        {"test2", new Sound.Sound("SoundEffect/Test/test2",false)}
                    }
                )
                { Angle = 0 }
            );
            this.Content.AddLast(
                new Obstacle(new Vector2(8, 8), new Vector2(0, 0), new Vector2(0, 0), 45f, "Texture2D/Test/SwordV1", new Collision.RB_Square(1,1))
            );

            this.Ui = new LinkedList<UI.Component>(); 
            static void test(GameTime gameTime) { System.Diagnostics.Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAA"); }
            Ui.AddLast( new UI.Button("Texture2D/Test/SwordV1", new Vector2(0.5f,0.5f), Game1.BaseFont, "AAA", Color.Red, new Sound.Sound("SoundEffect/Test/test", false)) { OnClick = new Action<GameTime>(test)} );

            this.TriggerBoxes = new LinkedList<Collision.TriggerBox>();
            TriggerBoxes.AddLast(new Collision.TriggerBox(new Vector2(1, 8), new Vector2(2, 11), 45) { OnCollisionPlayer = new Action<GameTime>(test) } );
            static void zooming(GameTime gameTime) { if(Instance.Camera != null) Instance.Camera.Zoom += 0.001f; }
            static void dezooming(GameTime gameTime) { if (Instance.Camera != null) Instance.Camera.SetZoom(1, 5); }                                     //static void dezooming(GameTime gameTime) { Tools.InterpolationManager.Add( new Tools.VarRef<float>(() => Instance.Camera.Zoom, val => { Instance.Camera.Zoom = val; }), 1, 3); }
            TriggerBoxes.AddLast(new Collision.TriggerBox(new Vector2(17, 1), new Vector2(30, 16)) { OnCollisionPlayer = new Action<GameTime>(zooming), OnExitPlayer = new Action<GameTime>(dezooming) } );
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The Scene1 instance
        /// </summary>
        private static CardGame instance = null;
        /// <summary>
        /// Access to the Scene1 instance. Creates it if null
        /// </summary>
        public static CardGame Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CardGame();
                }
                return instance;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Plays the scene
        /// </summary>
        public static void Play()
        {
            Game1.isMouseVisible = false;
            SceneManager.Load(CardGame.Instance);
            Instance.Musics["Test"].Play();
        }

        public override void Kill()
        {
            instance = null;
        }
        public override void Update(GameTime gameTime)
        {

            if (!Game1.pauseHandling())
            {
                //Content Update
                if (Content != null)
                    foreach (GameObject gameobject in Content)
                    {
                        gameobject.Update(gameTime);
                    }

                //TriggerBox Update
                if (TriggerBoxes != null)
                    foreach (Collision.TriggerBox triggerBox in TriggerBoxes)
                        triggerBox.Update(gameTime);
            }

            //UI Update
            if (Ui != null)
                foreach (UI.Component component in Ui)
                {
                    component.Update(gameTime);
                }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}

