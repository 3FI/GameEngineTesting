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
        public SoundEffect SoundEffect;
        public String SoundEffectId;
        private bool _isLooped;
        
        public Sound(String id, bool IsLooped)
        {
            SoundEffectId = id;
            _isLooped = IsLooped;
        }

    }
}
