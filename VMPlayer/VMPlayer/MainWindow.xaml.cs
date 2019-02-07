using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.Win32;
using Logic;

namespace VMPlayer
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player musicPlayer = new Player();
        private List<string> tracklist = new List<string>();

        private Timer viewUpdateTimer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            musicPlayer.playStateChanged += UpdateSongInfo;

            AsyncViewUpdate();
        }

       private void ViewUpdate(object sender, ElapsedEventArgs e)
        {
            UpdateProgressBar();
        }
       

        private void repeatChanged(object sender, RoutedEventArgs e)
        {
            bool? val = Repeat.IsChecked;
            if (val == null) val = false;
            musicPlayer.Repeat = (bool)val;
        }

        private void shuffleChanged(object sender, RoutedEventArgs e)
        {
            bool? val = Shuffle.IsChecked;
            if (val == null) val = false;
            musicPlayer.Shuffle = (bool)val;
        }

        private void ShowOpenFileDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Mp3 files (.mp3)|*.mp3|WAV files (.wav)|*.wav";
            bool? result = dlg.ShowDialog();
            bool playOnLoad = false;
            if (result != null)
            {
                if (musicPlayer.PlaylistCount == 0) playOnLoad = true;
                musicPlayer.AddSelectedMusicToPlaylist(dlg.FileNames);

                if (playOnLoad) musicPlayer.Resume();

            }
        }

        private void SetPlaybackPosition(object sender, MouseButtonEventArgs e)
        {
            double mousePos = e.GetPosition(SongElapsedTime).X;
            double ratio = mousePos / SongElapsedTime.ActualWidth;
            double newPlaybackPosition = ratio * SongElapsedTime.Maximum;

            musicPlayer.SetPlaybackMarker(newPlaybackPosition);
        }


        private void UpdateVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            var value = slider.Value;
            double ratio = value / slider.Maximum;
            double newVolume = ratio * 100; //sets the value of volume from 0 to 100
            musicPlayer.SetVolume((int)newVolume);
        }


        private void NextTrack(object sender, RoutedEventArgs e)
        {
            if (musicPlayer.CurrentSongTime < musicPlayer.CurrentSongDuration - 3)
            {
                musicPlayer.Next();
                UpdateSongInfo();
            }

        }

        private void PlayTrack(object sender, RoutedEventArgs e)
        {
            if (musicPlayer.PlaybackPaused) musicPlayer.Resume();
            else musicPlayer.Pause();
            UpdateSongInfo();
        }

        private void PreviousTrack(object sender, RoutedEventArgs e)
        {
            if (musicPlayer.CurrentSongTime > 2)
            {
                musicPlayer.Previous();
                UpdateSongInfo();
            }
        }

        private void UpdateProgressBar()
        {
            this.Dispatcher.Invoke(() =>
            {
                SongElapsedTime.Value = musicPlayer.PlaybackDisplayPosition;
                UpdateElapsedLabel();
            });
        }

        private void UpdateSongInfo()
        {
            this.Dispatcher.Invoke(() =>
            {
                Title.Content = musicPlayer.SongTitle;
                Artist.Content = musicPlayer.SongArtist;
                Album.Content = musicPlayer.SongAlbum;
                SetDuration();

            });

        }

        private void SetDuration()
        {
            double min = musicPlayer.CurrentSongDuration / 60f;
            double sec = musicPlayer.CurrentSongDuration % 60f;

            SongDuration.Content = ((int)min).ToString("00") + ":" + ((int)sec).ToString("00");
        }

        private void UpdateElapsedLabel()
        {
            double min = musicPlayer.CurrentSongTime / 60f;
            double sec = musicPlayer.CurrentSongTime % 60f;

            SongElapsedLabel.Content = ((int)min).ToString("00") + ":" + ((int)sec).ToString("00");
        }

      

        private void DeleteCurrentSong(object sender, RoutedEventArgs e)
        {
            musicPlayer.DeleteCurrentPlayingSong();
        }


        void AsyncViewUpdate()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    // update the UI
                    Dispatcher.Invoke(() => UpdateProgressBar());
                    // don't run again for at least 200 milliseconds
                    await Task.Delay(200);
                }
            });
        }

    }
}
