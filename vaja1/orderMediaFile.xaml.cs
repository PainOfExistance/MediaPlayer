using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Windows.Xps.Serialization;

namespace vaja1
{
    /// <summary>
    /// Interaction logic for orderMediaFile.xaml
    /// </summary>
    public partial class orderMediaFile : Window
    {
        Media edditing;
        public orderMediaFile(Media item)
        {
            InitializeComponent();
            edditing = item;
            this.DataContext = edditing;
            genres.ItemsSource = Properties.Settings.Default.MusicType;
        }
        private void changeGenre(object sender, RoutedEventArgs e)
        {
            edditing.genre = (String)genres.SelectedItem;
            gen.Text = (String)genres.SelectedItem;
        }
        private void changeIcon(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fileInput = new OpenFileDialog();
            fileInput.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.ico;*.svg";
            fileInput.ShowDialog();
            if (fileInput.FileName != "")
            {
                img.Source = new BitmapImage(new Uri(fileInput.FileName, UriKind.Absolute));
                imgPath.Text = fileInput.FileName;
            }
        }
    }
}
