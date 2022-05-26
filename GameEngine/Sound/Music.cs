using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    public class Music
    {
        public Song Song;
        public String SongID;
        
        public Music(string id)
        {
            SongID = id;
        }
        
        public void Load(ContentManager content)
        {
            Song = content.Load<Song>(SongID);
        }

        public void Play()
        {
            MediaPlayer.Play(Song);
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
