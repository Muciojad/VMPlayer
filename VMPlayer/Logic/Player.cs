﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using System;
using Microsoft.Win32;

namespace Logic
{
    /// <summary>
    /// Main class handling the music player logic
    /// </summary>
    public class Player
    {
        #region Constructor
        public Player()
        {
            player = new WindowsMediaPlayer();
            //player.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Next);
            player.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(EndOfStreamCallback);
            //player.EndOfStream += new WMPLib._WMPOCXEvents_EndOfStreamEventHandler(EndOfStreamCallback);


            playlist = new List<string>();
        }
        #endregion

        #region PRIVATE_VARIABLES
        /// <summary>
        /// Music player instance, using WindowsMediaPlayer class
        /// </summary>
        private WindowsMediaPlayer player;


        /// <summary>
        /// List of all selected tracks paths.
        /// </summary>
        private List<string> playlist;
        private bool shufflePlaylist = false;

        /// <summary>
        /// Current played track. References to specific index in trackURLS list.
        /// </summary>
        private int currentPlayedItem = 0;

        private int lastManuallySetState = -1;
        private int readyCounter = 0;

        #endregion

        #region PUBLIC_VARIABLES
        /// <summary>
        /// Returns amount of tracks in queue to play.
        /// </summary>
        public int PlaylistCount { get { return playlist.Count; } private set { } }

        /// <summary>
        /// Represents the volume of playback.
        /// </summary>
        public int Volume = 100;

        /// <summary>
        /// Should track be repeated over and over again?
        /// </summary>
        public bool Repeat = false;


        /// <summary>
        /// Returns current playing(or ready to be played) song name.
        /// </summary>
        public string SongTitle { get { return GetSongTitle(); } private set { } }

        /// <summary>
        /// Returns song album.
        /// </summary>
        public string SongAlbum {  get { return GetSongAlbum();  } private set { } }

        /// <summary>
        /// Returns song artist.
        /// </summary>
        public string SongArtist { get { return GetSongArtist(); }  private set { } }


        /// <summary>
        /// Returns song duration in seconds.
        /// </summary>
        public double CurrentSongDuration { get { return GetSongDuration(); } private set { } }

        /// <summary>
        /// Returns current playback time.
        /// </summary>
        public double CurrentSongTime { get { return GetElapsedTime(); } private set { } }

        /// <summary>
        /// Should playlist be shuffled?
        /// </summary>
        public bool Shuffle { get { return shufflePlaylist; } set { shufflePlaylist = value; } }
        
        /// <summary>
        /// Returns time remaining to end of song.
        /// </summary>
        public double CurrentSongRemainingTime {  get { return GetRemainingTime(); } private set { } }

        #endregion

        #region METHODS
        public void AddSelectedMusicToPlaylist(string[] filenames)
        {
            if(filenames != null)
            {
                foreach (string path in filenames)
                {
                    playlist.Add(path);
                }
            }
            
        }
        /// <summary>
        /// Plays current item in playlist.
        /// </summary>
        public void Play()
        {
            Console.WriteLine("play() with status " + player.playState.ToString());
            player.URL = playlist[currentPlayedItem];
            player.controls.play();
        }
        /// <summary>
        /// Plays next track in playlist. Current playing track is first stopped.
        /// </summary>
        public void Next()
        {
            if(!Repeat)
            {
                if (!shufflePlaylist)
                {
                    if (currentPlayedItem < playlist.Count - 1) currentPlayedItem++;
                    else currentPlayedItem = 0;
                }
                else
                {
                    Random random = new Random();
                    int randomSong = currentPlayedItem;
                    while (randomSong == currentPlayedItem)
                    {
                        randomSong = random.Next(0, PlaylistCount);
                    }
                    currentPlayedItem = randomSong;
                }
            }           

            Stop();
            Play();
        }

        /// <summary>
        /// Plays previous track in playlist. Current playing track is first stopped.
        /// </summary>
        public void Previous()
        {

            if(!Repeat)
            {
                if (!shufflePlaylist)
                {
                    if (currentPlayedItem < playlist.Count - 1) currentPlayedItem++;
                    else currentPlayedItem = 0;
                }
                else
                {

                    if (currentPlayedItem > 0) currentPlayedItem--;
                    else currentPlayedItem = playlist.Count - 1;
                    if (currentPlayedItem < 0) currentPlayedItem = 0;
                }
            }          

            Stop();
            Play();
        }

        public void SetVolume(int value)
        {
            Volume = value;
            player.settings.volume = Volume;
        }

        /// <summary>
        /// Get title of the current playing (or ready to be played) song.
        /// </summary>
        /// <returns>Song's title.</returns>
        private string GetSongTitle()
        {
            if(player.playState == WMPPlayState.wmppsReady || player.playState ==  WMPPlayState.wmppsPlaying || player.playState == WMPPlayState.wmppsBuffering
                || player.playState == WMPPlayState.wmppsWaiting || player.playState == WMPPlayState.wmppsUndefined || player.playState == WMPPlayState.wmppsStopped || player.playState == WMPPlayState.wmppsTransitioning)
            {
                return player.currentMedia.getItemInfo("Title");
            }
            //return "";
            return player.currentMedia.getItemInfo("Title");
        }
        /// <summary>
        /// Get artist of the current playing (or ready to be played) song.
        /// </summary>
        /// <returns>Song's artist.</returns>
        private string GetSongArtist()
        {
            if (player.playState == WMPPlayState.wmppsReady || player.playState == WMPPlayState.wmppsPlaying)
            {
                return player.currentMedia.getItemInfo("Artist");
            }
            return player.currentMedia.getItemInfo("Artist");

        }
        /// <summary>
        /// Get album of the current playing (or ready to be played) song.
        /// </summary>
        /// <returns>Song's album.</returns>
        private string GetSongAlbum()
        {
            if (player.playState == WMPPlayState.wmppsReady || player.playState == WMPPlayState.wmppsPlaying)
            {
                return player.currentMedia.getItemInfo("Album");
            }
            return player.currentMedia.getItemInfo("Album");
        }

        /// <summary>
        /// Returns current position of the playback from WMPLIB object.
        /// </summary>
        /// <returns>Current playback position in seconds.</returns>
        private double GetElapsedTime()
        {
            return player.controls.currentPosition;
        }
        /// <summary>
        /// Returns remaining time.
        /// </summary>
        /// <returns>Difference between song duration and playback time.</returns>
        private double GetRemainingTime()
        {
            return CurrentSongDuration - GetElapsedTime();
        }
        /// <summary>
        /// Stop the playback.
        /// </summary>
        private void Stop()
        {
            player.controls.stop();
        }


        /// <summary>
        /// Invoked by WMPLIB callback, when play state is changed. Used to play next song when current ended.
        /// Weird thing - ready state have to be reached two times before next song could be played.
        /// When trying play next song and ready state is reached first time, exception is thrown.
        /// </summary>
        /// <param name="state"></param>
        private void EndOfStreamCallback(int state)
        {
            Console.Write("playback state change to state ");
            switch(state)
            {
                case (int)WMPPlayState.wmppsStopped:
                    Console.WriteLine("Stopped");
                    break;
                case (int)WMPPlayState.wmppsWaiting:
                    Console.WriteLine("waiting");
                    break;
                case (int)WMPPlayState.wmppsReady:
                    Console.WriteLine("ready");
                    if(readyCounter == 0)
                    {
                        if (lastManuallySetState == (int)WMPPlayState.wmppsMediaEnded)
                        {
                            readyCounter++;                           
                        }
                    }
                    else
                    {
                        readyCounter++;
                        if(readyCounter == 2)
                        {
                            Next();
                            readyCounter = 0;
                        }
                    }
                    
                  
                    break;
                case (int)WMPPlayState.wmppsPaused:
                    Console.WriteLine("paused");
                    break;
                case (int)WMPPlayState.wmppsPlaying:
                    Console.WriteLine("playing");
                    break;
                case (int)WMPPlayState.wmppsBuffering:
                    Console.WriteLine("buffering");
                    break;
                case (int)WMPPlayState.wmppsMediaEnded:
                    lastManuallySetState = state;
                    Console.WriteLine("media ended");
                    Next();

                    break;
                case (int)WMPPlayState.wmppsUndefined:
                    Console.WriteLine("undefined");
                    break;
                case (int)WMPPlayState.wmppsTransitioning:
                    Console.WriteLine("transitioning");
                    break;
                default: Console.WriteLine("other");
                    break;
            }
       //     Next();
        }

       

        /// <summary>
        /// Returns song duration from WMPLIB object.
        /// </summary>
        /// <returns>Song duration in seconds.</returns>
        private double GetSongDuration()
        {
            return player.currentMedia.duration;
        }


        #endregion

        #region DebugMethods
        public List<string> D_playlistItems()
        {
            return playlist;
        }
        #endregion
    }
}