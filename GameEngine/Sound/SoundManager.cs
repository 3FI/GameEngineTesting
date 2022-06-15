using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    /// <summary>
    /// Static class that will play all the scheduled sound instances at their specified locations
    /// </summary>
    static class SoundManager
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The default behavior of the position of the sound manager. (Always at the player position)
        /// </summary>
        private static void _defaultBehavior()
        {
            if (Player.Instance != null)
            {
                SoundManager.Position = Player.Instance.Position;
            }
        }
        /// <summary>
        /// The behavior of the position of the sound manager
        /// </summary>
        static public Action Behaviour = new Action(_defaultBehavior);

        /// <summary>
        /// Position of the listener
        /// </summary>
        static public Vector2 Position;
        /// <summary>
        /// The scheduled sound effects
        /// </summary>
        static public List<SoundEffectInstance> SoundEffectInstances = new List<SoundEffectInstance>();

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
                SoundEffectInstances.Add(soundEffect);
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
                SoundEffectInstances.Add(soundEffect);
            }
        }

        /// <summary>
        /// Plays all the scheduled sound effect for this frame
        /// </summary>
        static public void Update()
        {
            Behaviour?.Invoke();
            foreach (SoundEffectInstance soundEffect in SoundEffectInstances)
            {
                soundEffect.Play();
            }
            SoundEffectInstances.Clear();
        }
    }
}
