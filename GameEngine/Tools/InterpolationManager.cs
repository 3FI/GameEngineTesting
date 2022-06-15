using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Tools
{
    //  This file is used to interpolate a value between two point in a set time
    //  The InterpolationManager is updated from Game1 and does the actual interpolation every frame
    //  The Interpolation class is a data structure that holds the reference to the variable to interpolate, the variable to interpolate to and the interpolation time
    //  The VarRef class is a data structure that is used to holds reference to a primitive type in another object

    /// <summary>
    /// A class that does all the scheduled interpolation.
    /// Is updated from Game1
    /// </summary>
    public static class InterpolationManager
    {
        /// <summary>
        /// List of all the scheduled interpolations
        /// </summary>
        static List<_interpolation> _Interpolations = new List<_interpolation>();

        /// <summary>
        /// Adds an interpolation to the scheduled list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <param name="time"></param>
        public static void Add(VarRef<float> value, float target, float time)
        {
            _Interpolations.Add(new _interpolation(value, target, time));
        }
        
        /// <summary>
        /// Does all the scheduled interpolations
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            for (int i = _Interpolations.Count - 1; i >= 0; i--)
            {
                _Interpolations[i].Update(gameTime);
                if (Math.Abs(_Interpolations[i].ValueRef.Value - _Interpolations[i].Target) < 0.001) _Interpolations.RemoveAt(i);
                //if (_Interpolations[i].ElapsedTime >= _Interpolations[i].Time) _Interpolations.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Holds the information to do an interpolation
    /// </summary>
    class _interpolation
    {
        public VarRef<float> ValueRef;
        public float Target;
        public float Time;
        public float ElapsedTime;

        public _interpolation(VarRef<float> valueRef, float target, float time)
        {
            ValueRef = valueRef;
            Target = target;
            Time = time;
        }

        public void Update(GameTime gameTime)
        {
            ValueRef.Value = CustomMath.Lerp(ValueRef.Value, (float)Target,(float)gameTime.ElapsedGameTime.TotalSeconds/Time);
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
    public class VarRef<T>
    {
        private Func<T> _get;
        private Action<T> _set;

        public VarRef(Func<T> @get, Action<T> @set)
        {
            _get = @get;
            _set = @set;
        }

        public T Value
        {
            get { return _get(); }
            set { _set(value); }
        }
    }
}
