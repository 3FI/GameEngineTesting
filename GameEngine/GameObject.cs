using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    /// <summary>
    /// Basic content block along with Tile(Map) and Component(UI)
    /// Holds a sprite or animation manager along with a rigidbody
    /// </summary>
    public abstract class GameObject
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Internal velocity of this gameobject (Accessor : <paramref name="Velocity"/>)
        /// </summary>
        private Vector2 _velocity;
        /// <summary>
        /// Internal acceleration of this gameobject (Accessor : <paramref name="Acceleration"/>)
        /// </summary>
        private Vector2 _acceleration;
        /// <summary>
        /// Internal position of this gameobject (Accessor : <paramref name="Position"/>)
        /// </summary>
        private Vector2 _position;
        /// <summary>
        /// Internal angle of this gameobject (Accessor : <paramref name="Angle"/>)
        /// </summary>
        private float _angle;
        /// <summary>
        /// Internal sprite of this gameobject (Accessor : <paramref name="Sprite"/>) (nullable but if so, _animationManager isn't)
        /// </summary>
        private Graphics.Sprite _sprite;
        /// <summary>
        /// Internal rigidbody of this gameobject (Accessor : <paramref name="RigidBody"/>)
        /// </summary>
        private Collision.RigidBody _rigidBody;
        /// <summary>
        /// Internal animation manager of this gameobject (Accessor : <paramref name="AnimationManager"/>) (nullable but if so, _sprite isn't)
        /// </summary>
        private Graphics.AnimationManager _animationManager;
        /// <summary>
        /// Internal animation dictionnary of this gameobject (Accessor : <paramref name="AnimationDict"/>) (nullable but if so, _sprite isn't)
        /// </summary>
        private Dictionary<String, Graphics.Animation> _animationDict;

        //TODO: ERROR HANDLING : STACK OVERFLOW
        /// <summary>
        /// Position accessor of this gameobject (sets the internal value <paramref name="_position"/>, <paramref name="RigidBody.Position"/>, <paramref name="Sprite.Position"/> OR <paramref name="AnimationManager.Position"/>) It is updated within the Update method of GameObject
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (Rigidbody != null && Rigidbody.Position != value) Rigidbody.Position = value;
                if (Sprite != null && Sprite.Position != value) Sprite.Position = value;
                if (AnimationManager != null && AnimationManager.Position != value) AnimationManager.Position = value;
            }
        }
        /// <summary>
        /// Velocity accessor of this gameobject (sets the internal value <paramref name="_velocity"/> and <paramref name="Rigidbody.Velocity"/>) It is updated within the Update method of GameObject
        /// </summary>
        public Vector2 Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity = value;
                if (Rigidbody != null && Rigidbody.Velocity != value) Rigidbody.Velocity = value;
            }
        }
        /// <summary>
        /// Acceleration accessor of this gameobject (sets the internal value <paramref name="_acceleration"/>) It is updated within the Update method of GameObject
        /// </summary>
        public Vector2 Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }
        /// <summary>
        /// Angle accessor of this gameobject (sets the internal value <paramref name="_angle"/>, <paramref name="RigidBody.Angle"/>, <paramref name="Sprite.Rotation"/> OR <paramref name="AnimationManager.Rotation"/>)
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                if (Rigidbody != null && Rigidbody.Angle != value) Rigidbody.Angle = value;
                if (Sprite != null && Sprite.Rotation != value) Sprite.Rotation = value;
                if (AnimationManager != null && AnimationManager.Rotation != value) AnimationManager.Rotation = value;
            }
        }
        /// <summary>
        /// Sprite accessor of this gameobject (sets the internal value <paramref name="_sprite"/>)
        /// </summary>
        public Graphics.Sprite Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }
        /// <summary>
        /// Rigidbody accessor of this gameobject (sets the internal value <paramref name="_rigidbody"/> also updates <paramref name="_rigidbody.Gameobject"/> to <paramref name="this"/>)
        /// </summary>
        public Collision.RigidBody Rigidbody
        {
            get { return _rigidBody; }
            set { _rigidBody = value; _rigidBody.GameObject = this; }
        }
        /// <summary>
        /// AnimationManager accessor of this gameobject (sets the internal value <paramref name="_animationManager"/>)
        /// </summary>
        public Graphics.AnimationManager AnimationManager
        {
            get { return _animationManager; }
            set { _animationManager = value; }
        }
        /// <summary>
        /// Animation Dictionnary accessor of this gameobject (sets the internal value <paramref name="_animationDict"/>)
        /// </summary>
        public Dictionary<String, Graphics.Animation> AnimationDict
        {
            get { return _animationDict; }
            set { _animationDict = value; }
        }

        /// <summary>
        /// Returns the tightest rectangle containing this gameobject
        /// </summary>
        public Collision.Box Box
        {
            get { return _rigidBody?.getBoundingBox(); }
        }

        /// <summary>
        /// Action that will be invoked when the bottom of this enters in contact with another rigidbody. Is nullable
        /// </summary>
        public Action DownContact;
        /// <summary>
        /// Action that will be invoked when the top of this enters in contact with another rigidbody. Is nullable
        /// </summary>
        public Action UpContact;
        /// <summary>
        /// Action that will be invoked when the left of this enters in contact with another rigidbody. Is nullable
        /// </summary>
        public Action LeftContact;
        /// <summary>
        /// Action that will be invoked when the right of this enters in contact with another rigidbody. Is nullable
        /// </summary>
        public Action RightContact;

        /// <summary>
        /// Dictionnary of sounds that could be played. Is nullable and only initialized trough children constructor
        /// </summary>
        public Dictionary<String, Sound.Sound> Sounds;


        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initialize with a sprite
        /// </summary>
        /// <param name="position">Initialization position of the gameobject</param>
        /// <param name="velocity">Initialization velocity of the gameobject</param>
        /// <param name="acceleration">Initialization acceleration of the gameobject</param>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="rigidBody">RigidBody of the gameobject</param>

        public GameObject(Vector2 position, Vector2 velocity, Vector2 acceleration, String texture, GameEngine.Collision.RigidBody rigidBody)
        {
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Sprite = new Graphics.Sprite(texture, position);
            this.Rigidbody = rigidBody;
            this.Position = position; //KEEP POSITION AT THE BOTTOM SUCH THAT IT INITIALIZE THE ANIMATION AND RIGIDBODY POSITION
        }
        /// <summary>
        /// Initialize with a sprite & angle
        /// </summary>
        /// <param name="position">Initialization position of the gameobject</param>
        /// <param name="velocity">Initialization velocity of the gameobject</param>
        /// <param name="acceleration">Initialization acceleration of the gameobject</param>
        /// <param name="angle">Initialization angle of the gameobject</param>
        /// <param name="rigidBody">RigidBody of the gameobject</param>

        public GameObject(Vector2 position, Vector2 velocity, Vector2 acceleration, float angle, String texture, GameEngine.Collision.RigidBody rigidBody)
        {
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Angle = angle;
            this.Sprite = new Graphics.Sprite(texture, position);
            this.Rigidbody = rigidBody;
            this.Position = position; //KEEP POSITION AT THE BOTTOM SUCH THAT IT INITIALIZE THE ANIMATION AND RIGIDBODY POSITION
        }
        /// <summary>
        /// Initialize with animation manager
        /// </summary>
        /// <param name="position">Initialization position of the gameobject</param>
        /// <param name="velocity">Initialization velocity of the gameobject</param>
        /// <param name="acceleration">Initialization acceleration of the gameobject</param>
        /// <param name="animations">Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="rigidBody">RigidBody of the gameobject</param>
        public GameObject(Vector2 position, Vector2 velocity, Vector2 acceleration, Dictionary<String, Graphics.Animation> animations, Collision.RigidBody rigidBody)
        {
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.AnimationDict = animations;
            this.Rigidbody = rigidBody;

            try
            {
                this.AnimationManager = new Graphics.AnimationManager(this.AnimationDict["Default"]);
            }
            catch (KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("Texture2D/PlaceHolderTexture", 0);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.AnimationManager = new Graphics.AnimationManager(error);
            }

            this.Position = position; //KEEP POSITION AT THE BOTTOM SUCH THAT IT INITIALIZE THE ANIMATION AND RIGIDBODY POSITION
        }
        /// <summary>
        /// Initialize with animation manager & angle
        /// </summary>
        /// <param name="position">Initialization position of the gameobject</param>
        /// <param name="velocity">Initialization velocity of the gameobject</param>
        /// <param name="acceleration">Initialization acceleration of the gameobject</param>
        /// <param name="angle">Initialization angle of the gameobject</param>
        /// <param name="animations">Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="rigidBody">RigidBody of the gameobject</param>
        public GameObject(Vector2 position, Vector2 velocity, Vector2 acceleration, float angle, Dictionary<String, Graphics.Animation> animations, Collision.RigidBody rigidBody)
        {
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Angle = angle;
            this.AnimationDict = animations;
            this.Rigidbody = rigidBody;

            try
            {
                this.AnimationManager = new Graphics.AnimationManager(this.AnimationDict["Default"]);
            }
            catch (KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("Texture2D/PlaceHolderTexture", 0);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.AnimationManager = new Graphics.AnimationManager(error);
            }

            this.Position = position; //KEEP POSITION AT THE BOTTOM SUCH THAT IT INITIALIZE THE ANIMATION AND RIGIDBODY POSITION
        }


        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        public virtual void Update(GameTime gameTime)
        {
            if (this.AnimationManager != null)
                this.AnimationManager.Update(gameTime);

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + 0.5f * Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //If there's an animationManager, draw the current frame
            if (this.AnimationManager != null)
                this.AnimationManager.Draw(spriteBatch);

            //If it's a sprite instead, draws it
            else if (this.Sprite != null)
                this.Sprite.Draw(gameTime, spriteBatch);

            else System.Diagnostics.Debug.WriteLine("Missing Sprite in " + this);

            //If debug menu active, draw the rigid body hitbox to the screen.
            if (Game1.debug && this.Rigidbody != null) this.Rigidbody.Draw(spriteBatch);
        }

        public abstract override String ToString();
        public abstract override bool Equals(Object obj);


    }
}
