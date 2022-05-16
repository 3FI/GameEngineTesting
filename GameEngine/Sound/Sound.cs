using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    public class Sound
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////
        //The sound effect and sound effect instance are loaded in scene manager
        public SoundEffect SoundEffect;

        public String SoundEffectId;
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
