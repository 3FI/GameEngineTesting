using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class Player : GameObject
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private KeyboardState _kstate = new KeyboardState();
        private GamePadState _gstate = new GamePadState();
        private KeyboardState _previouskstate;
        private GamePadState _previousgstate;

        public static Player Instance;

        private int _baseJumps=1;
        private int _currentJumps=1;
        private bool _jumpBuffer = false;
        private float _jumpBufferLifetime = 0;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Player(Vector2 position, Vector2 Velocity, Vector2 acceleration, String texture, GameEngine.Collision.RigidBody rigidBody, Dictionary<String,GameEngine.Sound.Sound> sounds)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.Sprite = new Graphics.Sprite(texture, position);
            this.Sounds = sounds;
            Instance = this;

            void _downContact()
            {
                _currentJumps = _baseJumps;
            }
            DownContact = new Action(_downContact);
        }

        public Player(Vector2 position, Vector2 Velocity, Vector2 acceleration, Dictionary<String, Graphics.Animation> animations, GameEngine.Collision.RigidBody rigidBody, Dictionary<String, GameEngine.Sound.Sound> sounds)
        {
            this.Rigidbody = rigidBody;
            this.Position = position;
            this.Velocity = Velocity;
            this.Acceleration = acceleration;
            this.AnimationDict = animations;
            this.Sounds = sounds;
            try
            {
                this.AnimationManager = new Graphics.AnimationManager(this.AnimationDict["Default"]);
            } 
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                Graphics.Animation error = new Graphics.Animation("ball", 1);
                System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                this.AnimationManager = new Graphics.AnimationManager(error);
            }
            Instance = this;

            void _downContact()
            {
                _currentJumps = _baseJumps;
            }
            DownContact = new Action(_downContact);
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Scan the input from the player and update the player velocity accordingly
        /// </summary>
        /// <param name="gameTime"></param>
        public void Movement(GameTime gameTime)
        {
            float VerticalSpeed = -850f;
            float HorizontalSpeed = 4f;
            float HorizontalLerping = 10f;
            float HorizontalDrag = 1f;
            
            
            //If player jumping
            
            if (_jumpBufferLifetime > 0)
                _jumpBufferLifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                _jumpBuffer = false;
            if ( ( (_kstate.IsKeyDown(Keys.Up) && !_previouskstate.IsKeyDown(Keys.Up)) || (_gstate.Buttons.X == ButtonState.Released && _previousgstate.Buttons.X == ButtonState.Pressed) ))
            {
                if (_currentJumps > 0)
                {
                    _currentJumps -= 1;

                    //Set upward velocity
                    this.Velocity = new Vector2(this.Velocity.X, VerticalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    //Try running the jump sound
                    try
                    {
                        Sound.SoundManager.Add(this.Sounds["test2"].CreateInstance(), this.Position);
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                    }
                }
                else
                {
                    _jumpBuffer = true;
                    _jumpBufferLifetime = 0.2f;
                }
            }
            if (_jumpBuffer)
            {
                if (_currentJumps > 0)
                {
                    _jumpBuffer = false;
                    _currentJumps -= 1;

                    //Set upward velocity
                    this.Velocity = new Vector2(this.Velocity.X, VerticalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    //Try running the jump sound
                    try
                    {
                        Sound.SoundManager.Add(this.Sounds["test2"].CreateInstance(), this.Position);
                    }
                    catch (System.Collections.Generic.KeyNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                    }
                }
            }
            
            //Go Left
            if (_kstate.IsKeyDown(Keys.Left) || _gstate.ThumbSticks.Left.X <= -0.5)
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(-HorizontalSpeed, this.Velocity.Y), HorizontalLerping * (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Go Right
            if (_kstate.IsKeyDown(Keys.Right) || _gstate.ThumbSticks.Left.X >= 0.5)
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2( HorizontalSpeed, this.Velocity.Y), HorizontalLerping * (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Momentum Drag
            if (_kstate.IsKeyUp(Keys.Left) && _kstate.IsKeyUp(Keys.Right) &&  (-0.5f < _gstate.ThumbSticks.Left.X && _gstate.ThumbSticks.Left.X < 0.5f) || _kstate.IsKeyDown(Keys.Left) && _kstate.IsKeyDown(Keys.Right))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2( 0, this.Velocity.Y), HorizontalDrag * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        
        private LinkedList<Vector2> _rightStickTracking = new LinkedList<Vector2>();
        public void Attacks(GameTime gameTime)
        {
            float MinRotSpd = 0.1f;
                
            Vector2 delta = _gstate.ThumbSticks.Right - _previousgstate.ThumbSticks.Right;
            
            if (Math.Abs(delta.X) < MinRotSpd && Math.Abs(delta.Y) < MinRotSpd)
            {
                if (_rightStickTracking.Count != 0)
                {
                    //Analyse the tracker
                    LinkedListNode<Vector2> positionNode = _rightStickTracking.First;
                    while (positionNode != null)
                    {
                        float angle = (float)Math.Atan(positionNode.Value.Y / positionNode.Value.X) / 180 / (float)Math.PI;
                        if (angle < 0) angle += 360;

                        if (337.5 <= angle && angle < 22.5)
                            positionNode.Value = new Vector2(1, 0);
                        else if (22.5 <= angle && angle < 67.5)
                            positionNode.Value = new Vector2(0.5f, 0.5f);
                        else if (67.5 <= angle && angle < 112.5)
                            positionNode.Value = new Vector2(0, 1);
                        else if (112.5 <= angle && angle < 157.5)
                            positionNode.Value = new Vector2(-0.5f, 0.5f);
                        else if (157.5 <= angle && angle < 202.5)
                            positionNode.Value = new Vector2(-1, 0);
                        else if (202.5 <= angle && angle < 247.5)
                            positionNode.Value = new Vector2(-0.5f, -0.5f);
                        else if (247.5 <= angle && angle < 292.5)
                            positionNode.Value = new Vector2(0, -1);
                        else if (292.5 <= angle && angle < 337.5)
                            positionNode.Value = new Vector2(0.5f, -0.5f);

                        System.Diagnostics.Debug.WriteLine(positionNode.Value);

                        positionNode = positionNode.Next;
                    }

                    //IF Final == Top
                    //IF Final == Right
                    //IF Final == Bottom
                    //IF Final == Left
                    //IF Final == Middle

                    _rightStickTracking.Clear();
                }
            }
            
            if (Math.Abs(delta.X) >= MinRotSpd || Math.Abs(delta.Y) >= MinRotSpd)
            {
                _rightStickTracking.AddLast(_gstate.ThumbSticks.Right);
            }
        }


        public override void Update(GameTime gameTime)
        {
            _kstate = Keyboard.GetState();
            _gstate = GamePad.GetState(PlayerIndex.One);

            this.Movement(gameTime);
            this.Attacks(gameTime);
            base.Update(gameTime);

            _previousgstate = _gstate;
            _previouskstate = _kstate;
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
        public override String ToString()
        {
            if (AnimationManager != null) return "Player(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tAnimationManager: \n\t" + AnimationManager.ToString().Replace("\n", "\n\t") + "\n)";
            else if (Sprite != null) return "Player(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tSprite: \n\t" + Sprite.ToString().Replace("\n", "\n\t") + "\n)";
            return "Player(\n\tPosition: " + Position + ", \n\tVelocity: " + Velocity + ", \n\tAcceleration: " + Acceleration + ", \n\tRigidBody: \n\t\t" + Rigidbody.ToString().Replace("\n", "\n\t") + ", \n\tError : No Texture" + "\n)";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (Position == ((Player)obj).Position) && (Rigidbody == ((Player)obj).Rigidbody);
            }
        }
    }
}
