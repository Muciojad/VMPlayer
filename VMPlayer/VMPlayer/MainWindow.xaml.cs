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
        public MainWindow()
        {
            InitializeComponent();
        }

        /*private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Mp3 files (.mp3)|*.mp3|WAV files (.wav)|*.wav";
            bool? result = dlg.ShowDialog();
            if(result != null)
            {
                musicPlayer.AddSelectedMusicToPlaylist(dlg.FileNames);
                musicPlayer.Play();
            }
        }*/
    }
}
