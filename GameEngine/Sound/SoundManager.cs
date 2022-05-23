using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    static class SoundManager
    {

        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        private static void _defaultBehavior()
        {
            if (Player.Instance != null)
            {
                SoundManager.Position = Player.Instance.Position;
            }
        }
        static public Action Behaviour = new Action(_defaultBehavior);

        //Position of the listener
        static public Vector2 Position;
        static public List<SoundEffectInstance> soundEffectInstances = new List<SoundEffectInstance>();

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Plays the sound effect
        /// </summary>
        /// <param name="soundEffect"></param>
        static public void Add(SoundEffectInstance soundEffect)
        {
            if (soundEffect != null)
            {
                soundEffectInstances.Add(soundEffect);
            }
        }
        /// <summary>
        /// Plays the sound effect at the specified position
        /// </summary>
        /// <param name="soundEffect"></param>
        /// <param name="source"></param>
        static public void Add(SoundEffectInstance soundEffect, Vector2 source)
        {
            if (soundEffect != null)
            {
                soundEffect.Volume = Math.Max(0, 1 - Vector2.Distance(Position, source) / 8);
                soundEffectInstances.Add(soundEffect);
            }
        }

        /// <summary>
        /// Plays all the sound effect for this frame
        /// </summary>
        static public void Update()
        {
            Behaviour?.Invoke();
            foreach (SoundEffectInstance soundEffect in soundEffectInstances)
            {
                soundEffect.Play();
            }
            soundEffectInstances.Clear();
        }
    }
}
