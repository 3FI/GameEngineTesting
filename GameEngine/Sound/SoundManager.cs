using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    class SoundManager
    {
        static public Vector2 Position;
        static public List<SoundEffectInstance> soundEffectInstances = new List<SoundEffectInstance>();

        static public void Add(SoundEffectInstance soundEffect, Vector2 source)
        {
            soundEffect.Volume = Math.Max(0,1 - Vector2.Distance(Position, source)/8);
            soundEffectInstances.Add(soundEffect);
        }

        static public void Update()
        {
            foreach (SoundEffectInstance soundEffect in soundEffectInstances)
            {
                soundEffect.Play();
            }
            soundEffectInstances.Clear();
        }
    }
}
