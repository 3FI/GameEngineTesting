using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Tools
{
    public static class InterpolationManager
    {
        static List<_interpolation> _Interpolations = new List<_interpolation>();
        public static void Add(VarRef<float> value, float target, float time)
        {
            _Interpolations.Add(new _interpolation(value, target, time));
        }
        
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
