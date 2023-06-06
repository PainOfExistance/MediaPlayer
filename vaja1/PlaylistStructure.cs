using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace vaja1
{
    public class PlaylistStructure : INotifyPropertyChanged
    {

        private string title1;
        public String title
        {
            get
            {
                return title1;
            }
            set
            {
                title1 = value;
                OnPropertyChanged(nameof(title));
            }
        }
        int length;

        private ObservableCollection<Media> media1 = new ObservableCollection<Media>();
        public ObservableCollection<Media> media
        {
            get
            {
                return media1;
            }
            set
            {
                media1 = value;
                OnPropertyChanged(nameof(media));
            }
        }

        public override string ToString()
        {
            String ret = "";
            foreach (Media media in media)
            {
                ret += media.ToString();
            }
            return title + "  " + length + "\n" + ret + "\n";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
