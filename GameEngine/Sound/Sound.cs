using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    /// <summary>
    /// A sound effect that can be sent to the sound manager to be played
    /// </summary>
    public class Sound
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////
        //The sound effect and sound effect instance are loaded in scene manager
        /// <summary>
        /// The sound effect that will be played
        /// </summary>
        public SoundEffect SoundEffect;
        /// <summary>
        /// String that represents the address of the audio file that will be used as a the sound effect
        /// </summary>
        public String SoundEffectId;
        /// <summary>
        /// Boolean representing if the audio will be looped once it ended
        /// </summary>
        public bool IsLooped;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        public Sound(String id, bool isLooped)
        {
            SoundEffectId = id;
            IsLooped = isLooped;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Creates an instance of the sound effect
        /// </summary>
        /// <returns> A sound effect instance that can be used in the sound manager</returns>
        public SoundEffectInstance CreateInstance()
        {
            if (SoundEffect != null)
            {
                SoundEffectInstance instance = SoundEffect.CreateInstance();
                instance.IsLooped = IsLooped;
                return instance;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Sound Effect :" + SoundEffectId + " not yet loaded");
                return null;
            }
        }

        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            try { SoundEffect = content.Load<SoundEffect>(SoundEffectId); }
            catch (ContentLoadException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to load sound " + SoundEffectId);
                SoundEffect = content.Load<SoundEffect>("SoundEffect/PlaceHolderSoundEffect");
                result = false;
            }
            return result;
        }

        public override String ToString()
        {
            return "Sound(\n\tSoundEffect: " + SoundEffectId + ", \n\tIsLooped: " + IsLooped + ", \n\tIsLoaded: " + (SoundEffect != null) + "\n)";
        }
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (SoundEffectId == ((Sound)obj).SoundEffectId) && (IsLooped == ((Sound)obj).IsLooped);
            }
        }
    }
}
