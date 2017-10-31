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
using System.Runtime.InteropServices;
using System.Threading;

namespace Labb6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isBarOpen = false;
        CancellationTokenSource cts = new CancellationTokenSource();
        int numberofGuests;

        private void printBouncerInfo(string bInfo)
        {
            Dispatcher.Invoke(() =>
            {
                BouncerListBox.Items.Insert(0, bInfo);
            });

            if(!isBarOpen)
            {
                Dispatcher.Invoke(() =>
                {
                    BouncerListBox.Items.Insert(0, "Bouncern går hem");
                });
            }
                
        }

        public MainWindow()
        {
            InitializeComponent();

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isBarOpen = false;
            OpenButton.IsEnabled = true;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenButton.IsEnabled = false;
            isBarOpen = true;
            if (isBarOpen == true)
            {
                Bouncer b = new Bouncer();
                Task.Run(() =>
               {
                   while (isBarOpen) {
                       b.CreateGuest(printBouncerInfo);
                       Dispatcher.Invoke(() =>
                       {
                           NumberOfGuests.Content = "Number of guests: " + ++numberofGuests;
                       });
                       if (!isBarOpen)
                       {
                           cts.Cancel();
                       }
                   }
               });



            }

        }
    }

}
