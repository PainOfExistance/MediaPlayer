using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vaja1
{
    /// <summary>
    /// Interaction logic for UC.xaml
    /// </summary>
    public partial class UC : UserControl
    {
        public event RoutedEventHandler PlayUC;
        public event RoutedEventHandler NextUC;
        public event RoutedEventHandler PrevUC;
        public event RoutedEventHandler StopUC;
        public event RoutedEventHandler ShuffleUC;
        public event RoutedEventHandler RepeatUC;

        public UC()
        {
            InitializeComponent();
        }

        private void loopList(object sender, RoutedEventArgs e)
        {
            RepeatUC(this, new RoutedEventArgs());
        }

        private void sfuffleList(object sender, RoutedEventArgs e)
        {
            ShuffleUC(this, new RoutedEventArgs());
        }

        private void mediaStop(object sender, RoutedEventArgs e)
        {
            StopUC(this, new RoutedEventArgs());
        }

        private void prevTrack(object sender, RoutedEventArgs e)
        {
            PrevUC(this, new RoutedEventArgs());
        }

        private void mediaPlayPause(object sender, RoutedEventArgs e)
        {
            PlayUC(this, new RoutedEventArgs());
        }

        private void nextTrack(object sender, RoutedEventArgs e)
        {
            NextUC(this, new RoutedEventArgs());
        }
    }
}
