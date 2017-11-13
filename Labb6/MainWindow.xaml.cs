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
        private BlockingCollection<Glass> shelf = new BlockingCollection<Glass>();
        public BlockingCollection<Patron> barQueue = new BlockingCollection<Patron>();

        public void GoToBarQueue(Patron newPatron)
        {
            barQueue.Add(newPatron);
        }

        public void GotBeer()
        {
            Patron p = barQueue.Take();
        }

        public void PlaceGlassOnShelf(Glass cleanGlass)
        {
            shelf.Add(cleanGlass);
        }
        private void GetGlassFromShelf(string takeGlass)
        {
            while (!shelf.TryTake(out Glass result))
            { }

            Dispatcher.Invoke(() =>
            {
                BartenderListBox.Items.Insert(0, "pours a beer.");
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
                    BouncerListBox.Items.Insert(0, "Bouncer goes home");
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
                BartenderListBox.Items.Insert(0, "Bartender goes home");
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
                Bouncer b = new Bouncer(barQueue);
                Bartender Bar = new Bartender(barQueue);
                Task.Run(() =>
               {
                   while (isBarOpen)
                   {
                       b.Work(printBouncerInfo);
                       Bar.PourBeer(GetGlassFromShelf);

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

