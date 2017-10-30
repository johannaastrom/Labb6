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

namespace Labb6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bouncer b = new Bouncer();

        bool isBarOpen = false;

        public MainWindow()
        {
            InitializeComponent();

            BouncerListBox.DisplayMemberPath = "name";

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isBarOpen = false;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            isBarOpen = true;
            if (isBarOpen == true)
            {
                var instance = new Bouncer();
                instance.CreateGuest();

            }

        }
    }

}
