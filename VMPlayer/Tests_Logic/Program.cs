using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Microsoft.Win32;

namespace Tests_Logic
{
    class Program
    {
       static void Main(string[] args)
        {
            //Test cases and tests will be added, now just manual tests of player methods

            string[] testFilepaths = new string[4] { "C:\\Users\\Muciojad\\Downloads\\FirstBlood.mp3", "C:\\Users\\Muciojad\\Downloads\\2592978876.mp3",
            "C:\\Users\\Muciojad\\Downloads\\3562739019.mp3", "C:\\Users\\Muciojad\\Downloads\\48046921.mp3" };
            Player musicPlayer = new Player();

            musicPlayer.AddSelectedMusicToPlaylist(testFilepaths);
            musicPlayer.Resume();
            bool loop = true;
            ConsoleKeyInfo input;
            musicPlayer.SetVolume(50);

            while (loop)
            {
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.Escape) loop = false;
                if(input.Key == ConsoleKey.L)
                {
                    musicPlayer.Next();

                    Console.Clear();
                }
                if (input.Key == ConsoleKey.V)
                {
                    if (!musicPlayer.PlaybackPaused) musicPlayer.Pause();
                    else musicPlayer.Resume();
                }
                Console.WriteLine("Title: " + GetSongTitle(musicPlayer));
                Console.WriteLine("Artist: " + GetSongArtist(musicPlayer));
                Console.WriteLine("Album: " + GetSongAlbum(musicPlayer));
                Console.WriteLine("Duration: " + GetSongDuration(musicPlayer));
                Console.WriteLine("Playback time: " + GetSongElapsedTime(musicPlayer));
                Console.WriteLine("Remaining time: " + GetSongRemaining(musicPlayer));
            }
        }
        
        public static string GetSongTitle(Player pl)
        {
            return pl.SongTitle;
        }
        public static string GetSongAlbum(Player pl)
        {
            return pl.SongAlbum;
        }
        public static string GetSongArtist(Player pl)
        {
            return pl.SongArtist;
        }

        public static double GetSongDuration(Player pl)
        {
            return pl.CurrentSongDuration;
        }
        public static double GetSongElapsedTime(Player pl)
        {
            return pl.CurrentSongTime;
        }

        public static double GetSongRemaining(Player pl)
        {
            return pl.CurrentSongRemainingTime;
        }
    }
}
