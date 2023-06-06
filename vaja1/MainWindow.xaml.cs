using ControlzEx.Standard;
using Dynamitey.Internal.Optimization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace vaja1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isPlaying = false;
        public bool loop = false;
        bool importing = false;
        DispatcherTimer timer = new DispatcherTimer();
        bool remove = false;
        public double lenght = 0.0;
        double prevAudioLevel = 0.0;
        public PlaylistStructure playlistStruct = new PlaylistStructure();
        DispatcherTimer autosave = new DispatcherTimer();
        string loadedPlaylist;
        int ticks = 0;
        bool alt = false;

        public MainWindow()
        {
            InitializeComponent();
            player.Volume = 100;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Click;
            player.Pause();
            player.MediaOpened += PreviewMedia_MediaOpened;
            this.DataContext = playlistStruct;
            audioLevel.Value = 100;

            autosave.Interval = TimeSpan.FromSeconds(1);
            autosave.Tick += autoS;
            autosave.Start();

            ColorAnimationUsingKeyFrames rainbow = new ColorAnimationUsingKeyFrames();
            LinearColorKeyFrame temp1 = new LinearColorKeyFrame();
            temp1.Value = Colors.Red;

            LinearColorKeyFrame temp2 = new LinearColorKeyFrame();
            temp2.Value = Colors.Orange;

            LinearColorKeyFrame temp3 = new LinearColorKeyFrame();
            temp3.Value = Colors.Yellow;

            LinearColorKeyFrame temp4 = new LinearColorKeyFrame();
            temp4.Value = Colors.Green;

            LinearColorKeyFrame temp5 = new LinearColorKeyFrame();
            temp5.Value = Colors.Blue;

            LinearColorKeyFrame temp6 = new LinearColorKeyFrame();
            temp6.Value = Colors.Indigo;

            LinearColorKeyFrame temp7 = new LinearColorKeyFrame();
            temp7.Value = Colors.Violet;

            rainbow.KeyFrames.Add(temp1);
            rainbow.KeyFrames.Add(temp2);
            rainbow.KeyFrames.Add(temp3);
            rainbow.KeyFrames.Add(temp4);
            rainbow.KeyFrames.Add(temp5);
            rainbow.KeyFrames.Add(temp6);
            rainbow.KeyFrames.Add(temp7);
            rainbow.Duration = new Duration(TimeSpan.FromSeconds(10));
            rainbow.RepeatBehavior = RepeatBehavior.Forever;
            naslov.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, rainbow);
        }

        void PreviewMedia_MediaOpened(object sender, EventArgs e)
        {
            userPanel.IsEnabled = true;
            usr.IsEnabled = true;
            player.Volume = audioLevel.Value / 100;
            timeSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            TimeSpan time = TimeSpan.FromSeconds(player.NaturalDuration.TimeSpan.TotalSeconds);
            lenght = player.NaturalDuration.TimeSpan.TotalSeconds;
            finalTimeLabel.Content = time.ToString(@"hh\:mm\:ss");
            mute.Source = new BitmapImage(new Uri("resources/unmuted.png", UriKind.Relative));
            audioLevel.IsEnabled = true;
        }

        void autoS(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.autoSave > 0 && playlistStruct.media.Count != 0)
            {
                ticks += 1;
                if (ticks == (Properties.Settings.Default.autoSave * 60))
                {
                    ticks = 0;
                    System.IO.FileStream file = System.IO.File.Open(loadedPlaylist, FileMode.Create, FileAccess.ReadWrite);
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(PlaylistStructure));
                    writer.Serialize(file, playlistStruct);
                    file.Close();
                }
            }
        }

        void Timer_Click(object sender, EventArgs e)
        {
            TimeSpan ret = player.Position;
            timeLabel.Content = ret.ToString(@"hh\:mm\:ss");
            timeSlider.Value = ret.TotalSeconds;
            if (timeSlider.Value == lenght)
            {
                int temp = playlist.SelectedIndex;
                Media play;
                if (temp == playlistStruct.media.Count - 1 && loop == true)
                {
                    temp = 0;
                    playlist.SelectedItem = playlistStruct.media[temp];
                    play = (Media)playlist.SelectedItem;
                    player.Source = new Uri(play.path);
                    odstrani.IsEnabled = true;
                    odstrani1.IsEnabled = true;
                    order.IsEnabled = true;
                    order1.IsEnabled = true;

                }
                else if (temp != playlistStruct.media.Count - 1)
                {
                    playlist.SelectedItem = playlistStruct.media[temp + 1];
                    play = (Media)playlist.SelectedItem;
                    player.Source = new Uri(play.path);
                    odstrani1.IsEnabled = true;
                    odstrani.IsEnabled = true;
                    order.IsEnabled = true;
                    order1.IsEnabled = true;
                }
            }
        }

        void mediaStop(Object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                cont.playButton.Source = new BitmapImage(new Uri("resources/play.png", UriKind.Relative));
                isPlaying = false;
            }
            TimeSpan time = TimeSpan.FromSeconds(0);
            player.Position = time;
            player.Stop();
            timer.Stop();
            timeLabel.Content = "00:00:00";
            timeSlider.Value = 0;
        }

        void mediaMute(Object sender, EventArgs e)
        {
            if (player.Volume > 0)
            {
                mute.Source = new BitmapImage(new Uri("resources/muted.png", UriKind.Relative));
                player.Volume = 0;
                audioLevel.IsEnabled = false;
                prevAudioLevel = audioLevel.Value;
                audioLevel.Value = 0;
            }
            else
            {
                mute.Source = new BitmapImage(new Uri("resources/unmuted.png", UriKind.Relative));
                audioLevel.Value = prevAudioLevel;
                player.Volume = audioLevel.Value / 100;
                audioLevel.IsEnabled = true;

            }
        }

        void mediaPlayPause(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation();
            if (!isPlaying)
            {
                timer.Start();
                player.Play();
                isPlaying = true;
                cont.playButton.BeginAnimation(Button.OpacityProperty, null);
                cont.playButton.BeginAnimation(Button.BorderThicknessProperty, null);
                cont.playButton.Source = new BitmapImage(new Uri("resources/pause.png", UriKind.Relative));
            }
            else
            {
                timer.Stop();
                player.Pause();
                isPlaying = false;
                anim.From = 1;
                anim.To = 0;
                anim.Duration = new Duration(TimeSpan.FromSeconds(1));
                anim.AutoReverse = true;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                cont.playButton.BeginAnimation(Button.OpacityProperty, anim);
                cont.playButton.Source = new BitmapImage(new Uri("resources/play.png", UriKind.Relative));
            }
        }

        void timeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TimeSpan time = TimeSpan.FromSeconds(timeSlider.Value);
            player.Position = time;
            timeLabel.Content = time.ToString(@"hh\:mm\:ss"); ;
        }

        void audioLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = audioLevel.Value / 100;
            volumeIndicator.Content = (int)audioLevel.Value;
            if ((int)audioLevel.Value == 0)
            {
                mute.Source = new BitmapImage(new Uri("resources/muted.png", UriKind.Relative));
            }
            else
            {
                mute.Source = new BitmapImage(new Uri("resources/unmuted.png", UriKind.Relative));
                prevAudioLevel = audioLevel.Value;
            }
        }

        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void changeMediaPlayed(object sender, EventArgs e)
        {
            if (remove == true || importing == true)
            {
                remove = false;
                importing = false;
            }
            else
            {
                var temp = (Media)playlist.SelectedItem;
                player.Source = new Uri(temp.path);
                odstrani.IsEnabled = true;
                odstrani1.IsEnabled = true;
                order.IsEnabled = true;
                order1.IsEnabled = true;
                if (loop == false)
                {
                    cont.playButton.Source = new BitmapImage(new Uri("resources/play.png", UriKind.Relative));
                    isPlaying = false;
                    player.Pause();
                }
            }
        }

        void addToPlaylist(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileInput = new OpenFileDialog();
            fileInput.Multiselect = true;
            fileInput.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;" +
                "*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            fileInput.ShowDialog();
            var path = fileInput.FileNames;
            var title = fileInput.SafeFileNames;
            for (int i = 0; i < title.Length; i++)
            {
                string[] tit = title[i].Split('.');
                Media item = new Media { title = tit[0], author = tit[0], description = "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff", path = path[i], imgPath = "G:/Faks/Sola/2.letnik/uv/naloga1/vaja1/vaja1/resources/skyrim.png", genre = "default" };
                playlistStruct.media.Add(item);
            }
        }

        void playlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (playlist.SelectedIndex != -1)
            {
                if (userPanel.IsEnabled == false)
                {
                    userPanel.IsEnabled = true;
                    usr.IsEnabled = true;
                }
                timer.Start();
                player.Play();
                isPlaying = true;
                cont.playButton.BeginAnimation(Button.OpacityProperty, null);
                cont.playButton.BeginAnimation(Button.BorderThicknessProperty, null);
                cont.playButton.Source = new BitmapImage(new Uri("resources/pause.png", UriKind.Relative));
            }
        }

        void removeFromPlaylist(object sender, RoutedEventArgs e)
        {
            remove = true;
            playlistStruct.media.RemoveAt(playlist.SelectedIndex);
            odstrani.IsEnabled = false;
            odstrani1.IsEnabled = false;
            order.IsEnabled = false;
            order1.IsEnabled = false;
            player.Stop();
            isPlaying = false;
            userPanel.IsEnabled = false;
            usr.IsEnabled = false;
            if (playlist.Items.Count == 0)
            {
                loadedPlaylist = "";
                save.IsEnabled = false;
                save1.IsEnabled = false;
            }
        }

        private void openWindow2(object sender, RoutedEventArgs e)
        {
            Settings win = new Settings();
            win.Show();
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            orderMediaFile win3 = new orderMediaFile((Media)playlist.SelectedItem);
            win3.Show();
        }

        private void importPlaylist(object sender, RoutedEventArgs e)
        {
            player.Stop();
            isPlaying = false;
            userPanel.IsEnabled = false;
            usr.IsEnabled = false;
            importing = true;
            playlist.SelectedIndex = -1;
            importing = false;
            OpenFileDialog fileInput = new OpenFileDialog();
            fileInput.Filter = "XML Files (*.xml)|*.xml";
            fileInput.ShowDialog();
            if (fileInput.FileName != "")
            {
                save.IsEnabled = true;
                save1.IsEnabled = true;
                ticks = 0;
                playlistStruct.title = "";
                playlistStruct.media.Clear();
                System.IO.Stream stream = System.IO.File.Open(fileInput.FileName, System.IO.FileMode.Open);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(PlaylistStructure));
                var temp = (PlaylistStructure)writer.Deserialize(stream);
                playlistStruct.title = fileInput.SafeFileName;
                foreach (Media x in temp.media)
                {
                    playlistStruct.media.Add(x);
                }
                stream.Close();
                loadedPlaylist = fileInput.FileName;
            }
            else if (playlist.SelectedIndex != -1)
            {
                player.Play();
                isPlaying = true;
                userPanel.IsEnabled = true;
                usr.IsEnabled = true;
            }
        }

        private void exportPlaylist(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileInput = new SaveFileDialog();
            fileInput.Filter = "XML Files (*.xml)|*.xml";
            fileInput.ShowDialog();
            if (fileInput.FileName != "")
            {
                string[] tit = fileInput.SafeFileName.Split('.');
                playlistStruct.title = tit[0];
                System.IO.FileStream file = System.IO.File.Create(fileInput.FileName);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(PlaylistStructure));
                writer.Serialize(file, playlistStruct);
                file.Close();
                save.IsEnabled = true;
                save1.IsEnabled = true;
                loadedPlaylist = fileInput.FileName;
            }
        }

        private void nextTrack(object sender, RoutedEventArgs e)
        {
            if (remove == true)
            {
                remove = false;
            }
            else
            {
                int temp = playlist.SelectedIndex;
                Media play;
                if (temp == playlistStruct.media.Count - 1)
                {
                    temp = 0;
                    playlist.SelectedItem = playlistStruct.media[temp];
                    play = (Media)playlist.SelectedItem;
                    player.Source = new Uri(play.path);
                    odstrani.IsEnabled = true;
                    odstrani1.IsEnabled = true;
                    order.IsEnabled = true;
                    order1.IsEnabled = true;
                }
                else
                {
                    playlist.SelectedItem = playlistStruct.media[temp + 1];
                    play = (Media)playlist.SelectedItem;
                    player.Source = new Uri(play.path);
                    odstrani.IsEnabled = true;
                    odstrani1.IsEnabled = true;
                    order.IsEnabled = true;
                    order1.IsEnabled = true;
                }

            }
        }

        private void prevTrack(object sender, RoutedEventArgs e)
        {
            if (remove == true)
            {
                remove = false;
            }
            else
            {
                int temp = playlist.SelectedIndex;
                Media play;
                if (temp == 0)
                {
                    temp = playlistStruct.media.Count - 1;
                    playlist.SelectedItem = playlistStruct.media[temp];
                    play = (Media)playlist.SelectedItem;
                    player.Source = new Uri(play.path);
                    odstrani.IsEnabled = true;
                    odstrani1.IsEnabled = true;
                    order.IsEnabled = true;
                    order1.IsEnabled = true;
                }
                else
                {
                    playlist.SelectedItem = playlistStruct.media[temp - 1];
                    play = (Media)playlist.SelectedItem;
                    player.Source = new Uri(play.path);
                    odstrani.IsEnabled = true;
                    odstrani1.IsEnabled = true;
                    order.IsEnabled = true;
                    order1.IsEnabled = true;
                }

            }
        }

        public void loopList(object sender, RoutedEventArgs e)
        {
            if (loop == true)
            {
                loop = false;
                cont.repeat.Source = new BitmapImage(new Uri("resources/repeat.png", UriKind.Relative));
            }
            else
            {
                loop = true;
                cont.repeat.Source = new BitmapImage(new Uri("resources/repeatSelected.png", UriKind.Relative));
            }
        }

        private void sfuffleList(object sender, RoutedEventArgs e)
        {
            var zac = (Media)playlist.SelectedItem;
            Random rnd = new Random();
            List<int> list = new List<int>();
            int i = 0;
            int rng = rnd.Next(playlistStruct.media.Count);
            list.Add(rng);
            while (i < playlistStruct.media.Count - 1)
            {
                rng = rnd.Next(playlistStruct.media.Count);
                if (!list.Contains(rng))
                {
                    list.Add(rng);
                    i++;
                }
            }
            PlaylistStructure plRet = new PlaylistStructure();
            for (int c = 0; c < list.Count; c++)
            {
                plRet.media.Add(playlistStruct.media[list[c]]);
            }
            playlistStruct.media = plRet.media;
            playlist.SelectedItem = zac;
            odstrani.IsEnabled = true;
            odstrani1.IsEnabled = true;
            order.IsEnabled = true;
            order1.IsEnabled = true;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            System.IO.FileStream file = System.IO.File.Open(loadedPlaylist, FileMode.Create, FileAccess.ReadWrite);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(PlaylistStructure));
            writer.Serialize(file, playlistStruct);
            file.Close();
        }

        private void altView(object sender, RoutedEventArgs e)
        {
            if (alt == false)
            {
                col1.Width = new GridLength(0, GridUnitType.Auto);
                col2.Width = new GridLength(0, GridUnitType.Auto);
                col3.Width = new GridLength(1, GridUnitType.Star);
                playlist.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                playlist.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

                var temp = new FrameworkElementFactory(typeof(StackPanel));
                temp.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                ItemsPanelTemplate vmes = new ItemsPanelTemplate { VisualTree = temp };
                playlist.ItemsPanel = vmes;

                mediaPl.SetValue(Grid.RowProperty, 1);
                mediaPl.SetValue(Grid.ColumnProperty, 2);

                splitter.SetValue(Grid.RowProperty, 1);
                splitter.SetValue(Grid.ColumnProperty, 2);

                viewL.SetValue(Grid.RowProperty, 3);
                viewL.SetValue(Grid.ColumnProperty, 2);

                alt = true;
            }
            else
            {

                col1.Width = new GridLength(1.3, GridUnitType.Star);
                col2.Width = new GridLength(5, GridUnitType.Pixel);
                col3.Width = new GridLength(5, GridUnitType.Star);

                playlist.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                playlist.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

                var temp = new FrameworkElementFactory(typeof(StackPanel));
                temp.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
                ItemsPanelTemplate vmes = new ItemsPanelTemplate { VisualTree = temp };
                playlist.ItemsPanel = vmes;

                mediaPl.SetValue(Grid.RowProperty, 1);
                mediaPl.SetValue(Grid.ColumnProperty, 2);

                splitter.SetValue(Grid.ColumnProperty, 1);
                splitter.SetValue(Grid.RowProperty, 1);

                viewL.SetValue(Grid.RowProperty, 1);
                viewL.SetValue(Grid.ColumnProperty, 0);

                alt = false;
            }

        }
    }
}