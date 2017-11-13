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
using System.Collections.Concurrent;

namespace Labb6
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts = new CancellationTokenSource(); //property för att stoppa trådar.
        bool isBarOpen = false;
        bool stillGuestsInBar = false; //sätts till true när den första gästen kommer och sätts till false när den sista gästen går
        int numberofGuests;
        private ConcurrentQueue<Glass> shelf = new ConcurrentQueue<Glass>();

        public void PlaceGlassOnShelf(Glass cleanGlass)
        {
            shelf.Enqueue(cleanGlass);
        }
        private void GetGlassFromShelf(string takeGlass)
        {
            while (!shelf.TryDequeue(out Glass result))
            { }

            Dispatcher.Invoke(() =>
            {
                BartenderListBox.Items.Insert(0, "Pours a beer");
            });

        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void printBouncerInfo(string bInfo)
        {
            Dispatcher.Invoke(() =>
            {
                BouncerListBox.Items.Insert(0, bInfo);
            });

            if (!isBarOpen)
            {
                Dispatcher.Invoke(() =>
                {
                    BouncerListBox.Items.Insert(0, "Bouncern goes home");
                });
            }

        }

        private void printBartenderInfo(string barInfo)
        {
            Dispatcher.Invoke(() =>
            {
                BartenderListBox.Items.Insert(0, barInfo);
            });

            if (!isBarOpen /*och när gästerna har gått*/)
            {
                BartenderListBox.Items.Insert(0, "Bartendern goes home");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isBarOpen = false;
            OpenButton.IsEnabled = true;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (cts.IsCancellationRequested)
            {
                cts = new CancellationTokenSource();
            }
            CancellationToken ct = cts.Token;

            OpenButton.IsEnabled = false;
            isBarOpen = true;                //Baren öppnas
            stillGuestsInBar = true; //Det finns gäster i baren
            if (isBarOpen == true)
            {
                Bouncer b = new Bouncer();
                Bartender Bar = new Bartender();
                Task.Run(() =>
               {
                   while (isBarOpen)
                   {
                       b.CreateGuest(printBouncerInfo);
                       Bar.PourBeer(GetGlassFromShelf);
                       //Queue<Bouncer> guestList = new Queue<Bouncer>();
                       //guestList.Enqueue();

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

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}

