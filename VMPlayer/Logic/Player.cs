using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using System;
using Microsoft.Win32;
using System.Timers;


namespace Logic
{
    /// <summary>
    /// Main class handling the music player logic
    /// </summary>
    public class Player
    {
        #region Constructor
        /// <summary>
        /// Main constructor for the Player instances.
        /// </summary>
        public Player()
        {
            player = new WindowsMediaPlayer();
            player.PlayStateChange += OnPlayStateChange;

            playlist = new List<string>();

            timerOnPlayChange_Play = new System.Timers.Timer();
            timerOnPlayChange_Play.Interval = 500;
            timerOnPlayChange_Play.Elapsed += delayedPlay;
            timerOnPlayChange_Play.AutoReset = true;

            resumingTimer = new Timer(500);
            resumingTimer.Elapsed += Unpause;
            resumingTimer.AutoReset = true;

        }

        private void Unpause(object sender, ElapsedEventArgs e)
        {
            PlaybackPaused = false;
            resumingTimer.Enabled = false;
            resumingTimer.Stop();
        }
        #endregion

        #region PRIVATE_VARIABLES
        /// <summary>
        /// Music player instance, using WindowsMediaPlayer class
        /// </summary>
        private WindowsMediaPlayer player;

        private System.Timers.Timer timerOnPlayChange_Play;
        private System.Timers.Timer resumingTimer;

        /// <summary>
        /// List of all selected tracks paths.
        /// </summary>
        private List<string> playlist;

        private bool playlistPopulated = false;

        private bool shufflePlaylist = false;

        private double pausedPlaybackPosition;

        /// <summary>
        /// Current played track. References to specific index in trackURLS list.
        /// </summary>
        private int currentPlayedItem = 0;

        /// <summary>
        /// Helper variable. Used to deal with weird WMPLIB change play state behavior.
        /// </summary>
        /// <summary>
        /// Helper variable. Used to deal with weird WMPLIB change play state behavior.
        /// Counts how many times "Ready" occurs.
        /// </summary>
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
        /// Returns current played item ID.
        /// </summary>
        public int CurrentPlayedID { get { return currentPlayedItem; } private set { } }
        
        /// <summary>
        /// Returns current played item path.
        /// </summary>
        public string CurrentPlayedPath { get { return player.URL; } private set { } }


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

        /// <summary>
        /// Returns value [0,100] corresponding to current playback time / duration.
        /// Use for updating song time progressbar with max value 100.
        /// </summary>
        public int PlaybackDisplayPosition { get { return GetPlaybackDisplayPosition(); } private set { } }

        /// <summary>
        /// Returns the playback marker position, and when set - playback position in seconds is calculated.
        /// </summary>
        public double PlaybackPositionMarker { get { return GetPlaybackDisplayPosition(); } private set { } }

        /// <summary>
        /// Returns true if playback is paused.
        /// </summary>
        public bool PlaybackPaused = false;
        

        #endregion

        #region METHODS

        /// <summary>
        /// Populating playlist with filenames passed from filedialog result.
        /// </summary>
        /// <param name="filenames"></param>
        public void AddSelectedMusicToPlaylist(string[] filenames)
        {
            if(filenames != null)
            {
                foreach (string path in filenames)
                {
                    playlist.Add(path);
                }
            }
            if (playlist.Count > 0) playlistPopulated = true;
            
        }
        /// <summary>
        /// Plays current item in playlist.
        /// </summary>
        private void Play()
        {
            player.URL = playlist[currentPlayedItem];
            player.controls.play();
          
        }
        /// <summary>
        /// Resume playback.
        /// </summary>
        public void Resume()
        {
            if (PlaybackPaused)
            {
                //PlaybackPaused = false;
                resumingTimer.Enabled = true;
                resumingTimer.Start();
            }
            Play();
            player.controls.currentPosition = pausedPlaybackPosition;
        }

        /// <summary>
        /// Pause playback.
        /// </summary>
        public void Pause()
        {
            pausedPlaybackPosition = CurrentSongTime;
            player.controls.pause();
            PlaybackPaused = true;
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
            //Play();
        }

        /// <summary>
        /// Plays previous track in playlist. Current playing track is first stopped.
        /// </summary>
        public void Previous()
        {
            if (!Repeat)
            {
                if (!shufflePlaylist)
                {
                    if (currentPlayedItem > 0) currentPlayedItem--;
                    else currentPlayedItem = PlaylistCount - 1;
                }
                else
                {
                    Random random = new Random();
                    int randomSong = currentPlayedItem;
                    if (currentPlayedItem < playlist.Count - 1)
                    {
                        while (randomSong == currentPlayedItem)
                        {
                            randomSong = random.Next(0, PlaylistCount);
                        }
                    }
                    currentPlayedItem = randomSong;
                }
            }

            Stop();
            //Play();
        }

        /// <summary>
        /// Sets the playback volume.
        /// </summary>
        /// <param name="value"></param>
        public void SetVolume(int value)
        {
            Volume = value;
            player.settings.volume = Volume;
        }

        /// <summary>
        /// Sets the playback position from the double click event position.
        /// </summary>
        /// <param name="clickValue"></param>
        public void SetPlaybackMarker(double clickValue)
        {
            PlaybackPositionMarker = clickValue;
            SetPlaybackPosition(clickValue);
        }

        private void SetPlaybackPosition(double clickValue)
        {
            if (playlistPopulated)
            {
                double secondsValue = CurrentSongDuration * (clickValue / 100f);
                player.controls.currentPosition = secondsValue;

            }
        }
        /// <summary>
        /// Delete current playing song from playlist.
        /// </summary>
        public void DeleteCurrentPlayingSong()
        {
            if (playlist.Count > 0)
            {
                Stop();
                playlist.RemoveAt(currentPlayedItem);
                currentPlayedItem--;
                if (currentPlayedItem < 0) currentPlayedItem = 0;

                if (playlist.Count == 0)
                {
                    SongAlbum = SongArtist = SongTitle = "";                   
                }
                else Next();

            }

        }

        /// <summary>
        /// Returns the elapsed time / song duration in %.
        /// </summary>
        /// <returns></returns>
        private int GetPlaybackDisplayPosition()
        {
            if (playlistPopulated)
            {
                double result = (CurrentSongTime / CurrentSongDuration) * 100;
                return (int)result;
            }
            else return 0;
            
        }

        /// <summary>
        /// Get title of the current playing (or ready to be played) song.
        /// </summary>
        /// <returns>Song's title.</returns>
        private string GetSongTitle()
        {
            if (playlistPopulated)
            {


                if (player.playState == WMPPlayState.wmppsReady || player.playState == WMPPlayState.wmppsPlaying || player.playState == WMPPlayState.wmppsBuffering
                || player.playState == WMPPlayState.wmppsWaiting || player.playState == WMPPlayState.wmppsUndefined || player.playState == WMPPlayState.wmppsStopped || player.playState == WMPPlayState.wmppsTransitioning)
                {
                    return player.currentMedia.getItemInfo("Title");
                }
                //return "";
                return player.currentMedia.getItemInfo("Title");
            }
            else return "";
        }
        /// <summary>
        /// Get artist of the current playing (or ready to be played) song.
        /// </summary>
        /// <returns>Song's artist.</returns>
        private string GetSongArtist()
        {
            if (playlistPopulated)
            {


                if (player.playState == WMPPlayState.wmppsReady || player.playState == WMPPlayState.wmppsPlaying)
                {
                    return player.currentMedia.getItemInfo("Artist");
                }
                return player.currentMedia.getItemInfo("Artist");
            }
            else return "";

        }
        /// <summary>
        /// Get album of the current playing (or ready to be played) song.
        /// </summary>
        /// <returns>Song's album.</returns>
        private string GetSongAlbum()
        {
            if (playlistPopulated)
            {
                if (player.playState == WMPPlayState.wmppsReady || player.playState == WMPPlayState.wmppsPlaying)
                {
                    return player.currentMedia.getItemInfo("Album");
                }
                return player.currentMedia.getItemInfo("Album");
            }
            else return "";
        }

        /// <summary>
        /// Returns current position of the playback from WMPLIB object.
        /// </summary>
        /// <returns>Current playback position in seconds.</returns>
        private double GetElapsedTime()
        {
            if (playlistPopulated)
                return player.controls.currentPosition;
            else return 0;
        }
        /// <summary>
        /// Returns remaining time.
        /// </summary>
        /// <returns>Difference between song duration and playback time.</returns>
        private double GetRemainingTime()
        {
            if (playlistPopulated)
                return CurrentSongDuration - GetElapsedTime();
            else return 0;
        }
        /// <summary>
        /// Stop the playback.
        /// </summary>
        private void Stop()
        {
            player.controls.stop();
            PlaybackPaused = false;
            pausedPlaybackPosition = 0;
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


        #region Delegates
        
        
        private static void Foo()
        { // just used to avoid not initialized callback execution
        }
        /// <summary>
        /// Delegate definition for callbacks invoked when WMPLIB playState changes.
        /// </summary>
        public delegate void PlayStateChanged();
        /// <summary>
        /// Add method updating song info UI(title, artist, album) to this delegate
        /// This method updates UI as fast as song details are available
        /// </summary>
        public PlayStateChanged playStateChanged = new PlayStateChanged(Foo);

        private void OnPlayStateChange(int _newstate)
        {
            if ((WMPLib.WMPPlayState)_newstate == WMPLib.WMPPlayState.wmppsPlaying)
            {
                playStateChanged();
            }
            if ((WMPLib.WMPPlayState)_newstate == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                Next();
            }
            if ((WMPLib.WMPPlayState)_newstate == WMPLib.WMPPlayState.wmppsReady)
            {
                readyCounter++;
                if (readyCounter == 2)
                {
                    if(!PlaybackPaused)
                    {
                        timerOnPlayChange_Play.Enabled = true;
                        timerOnPlayChange_Play.Start();
                    }                    
                    readyCounter = 0;
                }
            }
            if ((WMPLib.WMPPlayState)_newstate == WMPLib.WMPPlayState.wmppsStopped)
            {
                //Play();
                timerOnPlayChange_Play.Enabled = true;

                timerOnPlayChange_Play.Start();
            }
        } 

        private void delayedPlay(Object source, ElapsedEventArgs e)
        {
            Play();
            readyCounter = 0;
            timerOnPlayChange_Play.Stop();
            timerOnPlayChange_Play.Enabled = false;
        }

        #endregion
    }
}
