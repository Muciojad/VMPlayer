using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System.Timers;
using System.Reflection;
using System.IO;

namespace Player_UnitTests
{

    [TestClass]
    public class ControlsTests
    {
        string resPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string[] test_caseTracks = new string[4];



        public void SetPath()
        {
            string tmp = resPath;
            for (int i = 0; i < 2; i++)
            {
                int slashID = tmp.LastIndexOf("\\");
                tmp = tmp.Substring(0, slashID);
            }
            tmp += "\\Resources\\";
            for (int i = 0; i < 4; i++)
            {
                test_caseTracks[i] = tmp + "track0" + i.ToString() + ".mp3";
            }
        }

        [TestMethod]
        public void PlayNextSong_OK()
        {
            SetPath();

            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            int current = player.CurrentPlayedID;
            player.Next();
            if (current < player.PlaylistCount) Assert.IsTrue(current < player.CurrentPlayedID);
            else if (current >= player.PlaylistCount)
            {
                Assert.IsTrue(current >= player.CurrentPlayedID);
            }
        }

        [TestMethod]
        public void PlaySong_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            player.Play();
            Assert.AreEqual(test_caseTracks[0], player.CurrentPlayedPath);
        }
        `
        [TestMethod]
        public void PauseSong_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            player.Play();
            player.Pause();
            Assert.AreEqual(true, player.PlaybackPaused);
        }

        [TestMethod]
        public void PlayPrevSong_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            player.Next();
            int current = player.CurrentPlayedID;
            player.Previous();
            Console.Write(player.CurrentPlayedID + " vs " + current);
            if (current > 0) Assert.IsTrue(current > player.CurrentPlayedID);
            else if (current == 0)
            {
                Assert.IsTrue(current <= player.CurrentPlayedID);
            }
        }

        [TestMethod]
        public void DeleteSong_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            player.Play();
            int currentPlayCount = player.PlaylistCount;
            player.DeleteCurrentPlayingSong();
            int deletePlayCount = player.PlaylistCount;
            Assert.AreNotEqual(currentPlayCount, deletePlayCount);
        }

        [TestMethod]
        public void AddSongsToPlaylist()
        {
            SetPath();
            Player player = new Player();
            int currentPlayCount = 0;
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            int loadedSongsCount = player.PlaylistCount;
            Assert.AreNotEqual(currentPlayCount, loadedSongsCount);
        }
    }

    [TestClass]
    public class PlayerOptionsTests
    {
        string[] test_caseTracks = new string[4];
        string resPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public void SetPath()
        {
            string tmp = resPath;
            for (int i = 0; i < 2; i++)
            {
                int slashID = tmp.LastIndexOf("\\");
                tmp = tmp.Substring(0, slashID);
            }
            tmp += "\\Resources\\";
            for (int i = 0; i < 4; i++)
            {
                test_caseTracks[i] = tmp + "track0" + i.ToString() + ".mp3";
            }
        }
        [TestMethod]
        public void ShuffleOrder_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            int normalPlaybackNextID = 0;
            player.Shuffle = true;
            player.Next();
            int shuffleNextID = player.CurrentPlayedID;
            if (player.CurrentPlayedID < player.PlaylistCount)
            {
                Assert.IsTrue(shuffleNextID != normalPlaybackNextID);
            }
            else Assert.IsTrue(shuffleNextID == normalPlaybackNextID);
        }

        [TestMethod]
        public void RepeatTrack_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            int normalPlaybackNextID = 0;
            player.Repeat = true;
            player.Next();
            int repeatID = player.CurrentPlayedID;
            Assert.IsTrue(repeatID == normalPlaybackNextID);
        }

        [TestMethod]
        public void SetVolume_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            player.Play();
            int currentVolume = player.Volume;
            player.SetVolume(50);
            int changedVolume = player.Volume;
            Assert.AreNotEqual(changedVolume, currentVolume);
        }

        [TestMethod]
        public void SetPlaybackMarkerPosition_OK()
        {
            SetPath();
            Player player = new Player();
            player.AddSelectedMusicToPlaylist(test_caseTracks);
            player.Next();
            double currentMarker = 0;

            player.SetPlaybackMarker(10);
            double changedMarker = player.PlaybackPositionMarker;
            Assert.AreNotEqual(currentMarker, changedMarker);
        }
    }
}
