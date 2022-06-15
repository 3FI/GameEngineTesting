using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    /// <summary>
    /// This is the player gameobject
    /// </summary>
    public class Player : GameObject
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Latest instance of the player. If another instance is initialized it will take this place.
        /// </summary>
        public static Player Instance;

        /// <summary>
        /// Current state of the keyboard. Used to get which keys are pressed. Kept tracked in Update method.
        /// </summary>
        private KeyboardState _kstate = new KeyboardState();
        /// <summary>
        /// Current state of the controller. Used to get which buttons are pressed. Kept tracked in Update method.
        /// </summary>
        private GamePadState _gstate = new GamePadState();
        /// <summary>
        /// Previous state of the keyboard. Used to get which keys are released. Kept tracked in Update method.
        /// </summary>
        private KeyboardState _previouskstate;
        /// <summary>
        /// Previous state of the controller. Used to get which buttons are released. Kept tracked in Update method.
        /// </summary>
        private GamePadState _previousgstate;

        /// <summary>
        /// The number of jump the player can perform every time they touch the ground.
        /// </summary>
        private int _baseJumps=1;
        /// <summary>
        /// The current number of jump the player still has before needing to touch the ground. The logic of its reset to _baseJumps is in the player constructor in
        /// </summary>
        private int _currentJumps=1;
        /// <summary>
        /// Keep track of wether or not the user has pressed the jump key a bit early.
        /// </summary>
        private bool _jumpBuffer = false;
        /// <summary>
        /// Current lifetime in seconds of the jump buffer. Once it reaches 0 _jumpbuffer is set to false;
        /// </summary>
        private float _jumpBufferLifetime = 0;
        /// <summary>
        /// The lifetime at which _jumpBufferLifetime is set when a jump gets buffer.
        /// </summary>
        private float _jumpBufferMaxLifetime = 0.2f;

        /// <summary>
        /// Path of the right joystick. Is emptied if it has been more than a certain time during which the speed of the joystick was below a certain value.
        /// </summary>
        private LinkedList<Vector2> _rightStickTracking = new LinkedList<Vector2>();
        /// <summary>
        /// Number of frames during which the joystick speed was below a certain value.
        /// </summary>
        private int _rightStickNumberOfFailedSpd = 0;

        /// <summary>
        /// Track whether or not the player has a sword.
        /// </summary>
        private bool _hasSword = true;
        /// <summary>
        /// The current Sword gameobject the player is holding.
        /// </summary>
        private Sword _sword;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        //Needed Sounds:
        // test2

        /// <summary>
        /// Initialize with a sprite
        /// </summary>
        /// <param name="position">Initialization position of the player</param>
        /// <param name="velocity">Initialization velocity of the player</param>
        /// <param name="acceleration">Initialization acceleration of the player</param>
        /// <param name="texture">Represents the address of the file that will be used as a the texture of the sprite</param>
        /// <param name="rigidBody">RigidBody of the player</param>
        /// <param name="sounds">Sound Dictionnary (see documentation for needed sound)</param>
        public Player(Vector2 position, Vector2 velocity, Vector2 acceleration, String texture, Collision.RigidBody rigidBody, Dictionary<String, Sound.Sound> sounds) : base(position, velocity, acceleration, texture, rigidBody)
        {
            this.Sounds = sounds;
            Instance = this;

            //Makes the jump reset when touching the ground
            void _downContact()
            {
                _currentJumps = _baseJumps;
            }
            DownContact = new Action(_downContact);
        }
        /// <summary>
        /// Initialize with animation manager
        /// </summary>
        /// <param name="position">Initialization position of the player</param>
        /// <param name="velocity">Initialization velocity of the player</param>
        /// <param name="acceleration">Initialization acceleration of the player</param>
        /// <param name="animations">Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="rigidBody">RigidBody of the player</param>
        /// <param name="sounds">Sound Dictionnary (see documentation for needed sound)</param>
        public Player(Vector2 position, Vector2 velocity, Vector2 acceleration, Dictionary<String, Graphics.Animation> animations, Collision.RigidBody rigidBody, Dictionary<String, Sound.Sound> sounds) : base(position, velocity, acceleration, animations, rigidBody)
        {
            this.Sounds = sounds;
            Instance = this;

            //Makes the jump reset when touching the ground
            void _downContact()
            {
                _currentJumps = _baseJumps;
            }
            DownContact = new Action(_downContact);
        }
        /// <summary>
        /// Initialize with a sprite & angle
        /// </summary>
        /// <param name="position">Initialization position of the player</param>
        /// <param name="velocity">Initialization velocity of the player</param>
        /// <param name="acceleration">Initialization acceleration of the player</param>
        /// <param name="angle">Initialization angle of the player</param>
        /// <param name="rigidBody">RigidBody of the player</param>
        /// <param name="sounds">Sound Dictionnary (see documentation for needed sound)</param>
        public Player(Vector2 position, Vector2 velocity, Vector2 acceleration, float angle, String texture, Collision.RigidBody rigidBody, Dictionary<String, Sound.Sound> sounds) : base(position, velocity, acceleration, angle, texture, rigidBody)
        {
            this.Sounds = sounds;
            Instance = this;

            //Makes the jump reset when touching the ground
            void _downContact()
            {
                _currentJumps = _baseJumps;
            }
            DownContact = new Action(_downContact);
        }
        /// <summary>
        /// Initialize with animation manager & angle
        /// </summary>
        /// <param name="position">Initialization position of the player</param>
        /// <param name="velocity">Initialization velocity of the player</param>
        /// <param name="acceleration">Initialization acceleration of the player</param>
        /// <param name="angle">Initialization angle of the player</param>
        /// <param name="animations">Dictionnary of animations that will be used by the animation manager. Must have a "Default" animation.</param>
        /// <param name="rigidBody">RigidBody of the player</param>
        /// <param name="sounds">Sound Dictionnary (see documentation for needed sound)</param>
        public Player(Vector2 position, Vector2 velocity, Vector2 acceleration, float angle, Dictionary<String, Graphics.Animation> animations, Collision.RigidBody rigidBody, Dictionary<String, Sound.Sound> sounds) : base(position, velocity, acceleration, angle, animations, rigidBody)
        {
            this.Sounds = sounds;
            Instance = this;

            //Makes the jump reset when touching the ground
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
        /// Scan the input from the user and update the player velocity accordingly
        /// </summary>
        /// <param name="gameTime"></param>
        public void Movement(GameTime gameTime)
        {
            //Movement Constants

            //Jump speed of the player
            float VerticalSpeed = -850f;
            //Lateral movment speed of the player
            float HorizontalSpeed = 5f;
            //Time it takes (seconds) before the player velocity.X reaches HorizontalSpeed
            float HorizontalLerping = 0.3f;
            //Time it takes (seconds) before the player velocity.X reaches 0
            float HorizontalDrag = 0.35f;


            //Momentum Drag if both right and left or none
            if ((_kstate.IsKeyUp(Keys.Left) && _kstate.IsKeyUp(Keys.Right) && (-0.5f < _gstate.ThumbSticks.Left.X && _gstate.ThumbSticks.Left.X < 0.5f)) || (_kstate.IsKeyDown(Keys.Left) && _kstate.IsKeyDown(Keys.Right)))
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(0, this.Velocity.Y), (float)gameTime.ElapsedGameTime.TotalSeconds / HorizontalDrag);

            //Go Left
            else if (_kstate.IsKeyDown(Keys.Left) || _gstate.ThumbSticks.Left.X <= -0.5)
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(-HorizontalSpeed, this.Velocity.Y), (float)gameTime.ElapsedGameTime.TotalSeconds / HorizontalLerping);

            //Go Right
            else if (_kstate.IsKeyDown(Keys.Right) || _gstate.ThumbSticks.Left.X >= 0.5)
                this.Velocity = Vector2.Lerp(this.Velocity, new Vector2(HorizontalSpeed, this.Velocity.Y), (float)gameTime.ElapsedGameTime.TotalSeconds / HorizontalLerping);

            //JumpBufferLifetime Managing
            if (_jumpBufferLifetime > 0)
                _jumpBufferLifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                _jumpBuffer = false;

            //If the player jump
            if ( ( (_kstate.IsKeyDown(Keys.Up) && !_previouskstate.IsKeyDown(Keys.Up)) || (_gstate.Buttons.X == ButtonState.Released && _previousgstate.Buttons.X == ButtonState.Pressed) ))
            {
                if (_currentJumps > 0)
                {
                    //The logic for the reset of # of jump is set within the Player constructor
                    _currentJumps -= 1;

                    //Set upward velocity
                    this.Velocity = new Vector2(this.Velocity.X, VerticalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    //Try running the jump sound
                    try
                    {
                        Sound.SoundManager.Add(this.Sounds["test2"].CreateInstance(), this.Position);
                    }
                    catch (KeyNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                    }
                }

                //If there's no more jump : buffer the input
                else
                {
                    _jumpBuffer = true;
                    _jumpBufferLifetime = _jumpBufferMaxLifetime;
                }
            }

            //If the player jump because of the buffer
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
                    catch (KeyNotFoundException e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception caught" + e);
                    }
                }
            }
        }
        /// <summary>
        /// Scan the input from the user and triggers according attacks
        /// </summary>
        /// <param name="gameTime"></param>
        public void Attacks(GameTime gameTime)
        {
            //The minimum speed that the joystick needs to rotate at
            float MinRotSpd = 0.00000001f;

            //Time in seconds that must be elapsed with the joystick speed below the threshold before the input is read
            float MaxBufferTime = 0.7f;
            
            //Current speed of the right joystick
            Vector2 delta = _gstate.ThumbSticks.Right - _previousgstate.ThumbSticks.Right;

            //If the current speed is larger than the minimum, register the current position of the joystick in the trajectory
            if (Math.Abs(delta.X) >= MinRotSpd || Math.Abs(delta.Y) >= MinRotSpd)
            {
                _rightStickTracking.AddLast(_gstate.ThumbSticks.Right);
            }

            else
            {
                //Increase by one the number of frame with joystick speed below th threshold
                _rightStickNumberOfFailedSpd++;

                //Calculate the max time in frame
                double maxFails = MaxBufferTime / gameTime.ElapsedGameTime.TotalSeconds;

                //If it has been long enough, start reading the input
                if (_rightStickNumberOfFailedSpd > maxFails)
                {
                    _rightStickNumberOfFailedSpd = 0;

                    //If the input isn't empty
                    if (_rightStickTracking.Count != 0)
                    {
                        //Analyse each node of the tracker and turn all position to absolute cardinal position
                        LinkedListNode<Vector2> positionNode = _rightStickTracking.First;
                        while (positionNode != null)
                        {
                            float angle = (float)Math.Atan(positionNode.Value.Y / positionNode.Value.X) * 180 / (float)Math.PI;
                            if (positionNode.Value.X < 0) angle += 180;
                            if (angle < 0) angle += 360;

                            if (337.5 <= angle || angle < 22.5)
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
                            
                            //Remove redondance (if there's two consecutive position that are the same)
                            if (positionNode.Previous != null) if (positionNode.Value == positionNode.Previous.Value) _rightStickTracking.Remove(positionNode.Previous);

                            positionNode = positionNode.Next;
                        }

                        foreach (Vector2 print in _rightStickTracking)
                            System.Diagnostics.Debug.WriteLine(print);

                        //LEGACY : Add a (0, 0) for debugging purpose (it removes null exception in the middle of a move isn't complete)
                        //_rightStickTracking.AddLast(new Vector2(0, 0));

                        //Import the attackLibrary to which compare the move
                        Dictionary<Vector2[], Action<Player>> attackLibrary = PlayerMoveset.attackLibrary;

                        foreach (Vector2[] possibleMove in attackLibrary.Keys)
                        {
                            LinkedListNode<Vector2> Node = _rightStickTracking.First;

                            bool fail = false;
                            
                            //Compare each Vector2 to each other
                            for (int i = 0; i<possibleMove.Length; i++)
                            {
                                if (Node.Value != possibleMove[i])
                                {
                                    //If the Vector2 aren't the same, it isn't the right move
                                    fail = true;
                                    break;
                                }
                                if(Node.Next != null)
                                    Node = Node.Next;
                                else
                                {
                                    //If the lenght of the input is smaller to which of the compared move, it isn't the right move
                                    fail = true;
                                    break;
                                }
                            }
                            if (!fail)
                            {
                                //If it is the right one, invoke the according function
                                attackLibrary[possibleMove].Invoke(this);
                                break;
                            }
                        }

                        //Clear the tracker for the next move
                        _rightStickTracking.Clear();
                    }
                }
            }
        }


        public override void Update(GameTime gameTime)
        {
            _kstate = Keyboard.GetState();
            _gstate = GamePad.GetState(PlayerIndex.One);

            this.Movement(gameTime);
            this.Attacks(gameTime);

            this._sword?.Update(gameTime);

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

/*
if (Node.Value == new Vector2(1, 0))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(0.5f, 0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0, 1))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(-0.5f, 0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(-1, 0))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(-0.5f, -0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0, -1))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0.5f, -0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(1, 0))
                                    System.Diagnostics.Debug.WriteLine("⟳ inverse");
                            }
                        }
                    }
                }
            }
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(0.5f, -0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0, -1))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(-0.5f, -0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(-1, 0))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(-0.5f, 0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0, 1))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0.5f, 0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(1, 0))
                                    System.Diagnostics.Debug.WriteLine("⟳");
                            }
                        }
                    }
                }
            }
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("→");
    }
}


else if (Node.Value == new Vector2(0.5f, 0.5f))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(0, 1))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-0.5f, 0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(-1, 0))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(-0.5f, -0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0, -1))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0.5f, -0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(1, 0))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0.5f, 0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟳ inverse + 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬈");
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(1, 0))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-0.5f, -0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0, -1))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(-0.5f, -0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(-1, 0))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(-0.5f, 0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0, 1))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0.5f, 0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟳ + 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬈");
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("⬈");
    }
}

else if (Node.Value == new Vector2(0, 1))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(-0.5f, 0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-1, 0))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(-0.5f, -0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0, -1))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0.5f, -0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(1, 0))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0.5f, 0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0, 1))
                                    System.Diagnostics.Debug.WriteLine("⥀");
                            }
                        }
                    }
                }
            }
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(0.5f, 0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(1, 0))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0.5f, -0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0, -1))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(-0.5f, -0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(-1, 0))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(-0.5f, 0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0, 1))
                                    System.Diagnostics.Debug.WriteLine("⥁");
                            }
                        }
                    }
                }
            }
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("↑");
    }
}


else if (Node.Value == new Vector2(-0.5f, 0.5f))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(-1, 0))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-0.5f, -0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0, -1))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0.5f, -0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(1, 0))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0.5f, 0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0, 1))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(-0.5f, 0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟲ - 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬉");
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(0, 1))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0.5f, 0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(1, 0))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0.5f, -0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0, -1))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(-0.5f, -0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(-1, 0))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(-0.5f, 0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟲ inverse - 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬉");
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("⬉");
    }
}


else if (Node.Value == new Vector2(-1, 0))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(-0.5f, -0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0, -1))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0.5f, -0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(1, 0))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0.5f, 0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0, 1))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(-0.5f, 0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(-1, 0))
                                    System.Diagnostics.Debug.WriteLine("⟲");
                            }
                        }
                    }
                }
            }
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(-0.5f, 0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0, 1))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0.5f, 0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(1, 0))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0.5f, -0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0, -1))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(-0.5f, -0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(-1, 0))
                                    System.Diagnostics.Debug.WriteLine("⟲ inverse");
                            }
                        }
                    }
                }
            }
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("←");
    }
}


else if (Node.Value == new Vector2(-0.5f, -0.5f))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(0, -1))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0.5f, -0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(1, 0))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0.5f, 0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0, 1))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(-0.5f, 0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(-1, 0))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(-0.5f, -0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟲ + 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬋");
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(-1, 0))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-0.5f, 0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0, 1))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0.5f, 0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(1, 0))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0.5f, -0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0, -1))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(-0.5f, -0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟲ inverse + 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬋");
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("⬋");
    }
}


else if (Node.Value == new Vector2(0, -1))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(0.5f, -0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(1, 0))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0.5f, 0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0, 1))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(-0.5f, 0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(-1, 0))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(-0.5f, -0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0, -1))
                                    System.Diagnostics.Debug.WriteLine("⥀ upsidedown");
                            }
                        }
                    }
                }
            }
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(-0.5f, -0.5f))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-1, 0))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(-0.5f, 0.5f))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(0, 1))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0.5f, 0.5f))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(1, 0))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0.5f, -0.5f))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0, -1))
                                    System.Diagnostics.Debug.WriteLine("⥁ upsidedown");
                            }
                        }
                    }
                }
            }
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("↓");
    }
}


else if (Node.Value == new Vector2(0.5f, -0.5f))
{
    Node = Node.Next;
    //CounterClockWise Rotation
    if (Node.Value == new Vector2(1, 0))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(0.5f, 0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(0, 1))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(-0.5f, 0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(-1, 0))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(-0.5f, -0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(0, -1))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0.5f, -0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟳ inverse - 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬊");
        }
    }
    //ClockWise Rotation
    else if (Node.Value == new Vector2(0, -1))
    {
        Node = Node.Next;
        if (Node.Value == new Vector2(-0.5f, -0.5f))
        {
            Node = Node.Next;
            if (Node.Value == new Vector2(-1, 0))
            {
                Node = Node.Next;
                if (Node.Value == new Vector2(-0.5f, 0.5f))
                {
                    Node = Node.Next;
                    if (Node.Value == new Vector2(0, 1))
                    {
                        Node = Node.Next;
                        if (Node.Value == new Vector2(0.5f, 0.5f))
                        {
                            Node = Node.Next;
                            if (Node.Value == new Vector2(1, 0))
                            {
                                Node = Node.Next;
                                if (Node.Value == new Vector2(0.5f, -0.5f))
                                    System.Diagnostics.Debug.WriteLine("⟳ - 45 degree");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("⬊");
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("⬊");
    }
}
*/