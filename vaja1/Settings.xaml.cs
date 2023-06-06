using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Printing;
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
using System.Text.RegularExpressions;
using MahApps.Metro.Controls;

namespace vaja1
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            if (Properties.Settings.Default.autoSave == 0)
            {
                checkA.IsChecked = false;
                status.Text = "Onemogočeno";
                period.IsEnabled = false;
            }
            else
            {
                checkA.IsChecked = true;
                status.Text = "Omogočeno";
                period.IsEnabled = true;
            }
            period.Text = Properties.Settings.Default.autoSave.ToString();
            change.ItemsSource = Properties.Settings.Default.MusicType;
        }

        private void setType(object sender, RoutedEventArgs e)
        {
            if (adder.Text != "")
            {
                Properties.Settings.Default.MusicType.Add(adder.Text);
                Properties.Settings.Default.Save();
                adder.Text = "";
                change.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Can't input empty string");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (change.SelectedIndex > -1)
            {
                Properties.Settings.Default.MusicType.RemoveAt(change.SelectedIndex);
                Properties.Settings.Default.Save();
                change.Items.Refresh();
                changer.IsEnabled = false;
            }
        }

        private void changeClick(object sender, RoutedEventArgs e)
        {
            if (changing.Text != "")
            {
                int rem = change.SelectedIndex;
                Properties.Settings.Default.MusicType.RemoveAt(rem);
                Properties.Settings.Default.MusicType.Insert(rem, changing.Text);
                Properties.Settings.Default.Save();
                change.Items.Refresh();
                changing.Text = "";
            }
        }

        private void change_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changer.IsEnabled = true;
        }

        private void setAutosave(object sender, RoutedEventArgs e)
        {
            if ((bool)checkA.IsChecked)
            {
                status.Text = "Omogočeno";
                Properties.Settings.Default.autoSave = 1;
                Properties.Settings.Default.Save();
                period.Text = Properties.Settings.Default.autoSave.ToString();
                period.IsEnabled = true;
            }
            else
            {
                status.Text = "Onemogočeno";
                Properties.Settings.Default.autoSave = 0;
                Properties.Settings.Default.Save();
                period.Text = Properties.Settings.Default.autoSave.ToString();
                period.IsEnabled = false;
            }
        }

        private void period_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num = 0;
            bool possible = Int32.TryParse(period.Text, out num);
            if (possible)
            {
                Properties.Settings.Default.autoSave = num;
                Properties.Settings.Default.Save();
            }
        }
    }
}
