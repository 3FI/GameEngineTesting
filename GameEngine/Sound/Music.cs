using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Sound
{
    //TODO : IMPLEMENT LOOP ON MUSIC
    /// <summary>
    /// A music that can be played in the MediaPlayer
    /// </summary>
    public class Music
    {
        /////////////////////////////////////////////////////////////////////////////////
        //                                  PROPERTIES                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The music that will be played
        /// </summary>
        public Song Song;
        /// <summary>
        /// String that represents the address of the audio file that will be used as a the music
        /// </summary>
        public String SongID;

        /////////////////////////////////////////////////////////////////////////////////
        //                                 CONSTRUCTOR                                 //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Standard Music Initialisation
        /// </summary>
        /// <param name="id"></param>
        public Music(string id)
        {
            SongID = id;
        }

        /////////////////////////////////////////////////////////////////////////////////
        //                                   METHODS                                   //
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Loads <paramref name="SongID"/> in <paramref name="Song"/>
        /// </summary>
        /// <param name="content"></param>
        public virtual bool Load(ContentManager content)
        {
            bool result = true;
            try { Song = content.Load<Song>(SongID); }
            catch (ContentLoadException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to load music " + SongID);
                Song = content.Load<Song>("Music/PlaceHolderMusic");
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Starts playing <paramref name="Song"/> in the MediaPlayer
        /// </summary>
        public void Play()
        {
            MediaPlayer.Play(Song);
        }

        /// <summary>
        /// Stop playing <paramref name="Song"/> in the MediaPlayer
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
