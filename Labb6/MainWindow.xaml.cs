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
        CancellationTokenSource cts = new CancellationTokenSource(); //property för att stoppa trådar.

        bool isBarOpen = false;

        private void printBouncerInfo(string bInfo)
        {
            Dispatcher.Invoke(() =>
            {
                BouncerListBox.Items.Insert(0, bInfo);
            });

        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isBarOpen = false;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (cts.IsCancellationRequested)
            {
                cts = new CancellationTokenSource();
            }

            CancellationToken ct = cts.Token;

            //Task.Run(() =>
            //{
            //    while (!ct.IsCancellationRequested)
            //    {
                    isBarOpen = true;
                    if (isBarOpen == true)
                    {
                        Bouncer b = new Bouncer();
                        Task.Run(() =>
                        {
                            while (isBarOpen)
                                b.CreateGuest(printBouncerInfo);
                        });
                    }
            //    }
            //});
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
    }

}
