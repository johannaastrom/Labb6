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
        CancellationTokenSource cts = new CancellationTokenSource(); 

        Bouncer bouncer = new Bouncer();
        Bartender bartender = new Bartender();
        Waiter waiter = new Waiter();
        Patron patron = new Patron();

        //values to change number of's.
        public bool isBarOpen = false;
        int numberofGuests;
        int numberofGlasses = 20;
        int numberofChairs = 5;

        //Queues
        BlockingCollection<Patron> BartenderQueue = new BlockingCollection<Patron>();
        BlockingCollection<Glass> CleanGlassQueue = new BlockingCollection<Glass>();
        BlockingCollection<Chair> AvailableChairQueue = new BlockingCollection<Chair>();
        BlockingCollection<Glass> DirtyGlassQueue = new BlockingCollection<Glass>();
        BlockingCollection<Patron> PubQueue = new BlockingCollection<Patron>();

        public MainWindow()
        {
            InitializeComponent();
        }

        //Prints info in listboxes of Patron, Waiter and Bartender.
        private void printBouncerInfo(string bouncerInfo)
        {
            Dispatcher.Invoke(() => { BouncerListBox.Items.Insert(0, bouncerInfo); });
        }
        private void printPatronInfo(string patronInfo)
        {
            Dispatcher.Invoke(() => { BouncerListBox.Items.Insert(0, patronInfo); });
        }
        private void printWaiterInfo(string waiterInfo)
        {
            Dispatcher.Invoke(() => { WaiterListBox.Items.Insert(0, waiterInfo); });
        }
        private void printBartenderInfo(string bartenderInfo)
        {
            Dispatcher.Invoke(() => { BartenderListBox.Items.Insert(0, bartenderInfo); });
        }

        //Printing the labels of guests, clean glasses and empty chairs.
        private void printNumberOfGuests(string text)
        {
            Dispatcher.Invoke(() => { NumberOfGuests.Content = text; });
        }
        private void printNumberOfCleanGlasses(string text)
        {
            Dispatcher.Invoke(() => { NumberOfGlasses.Content = text; });
        }
        private void printNumberOfEmptyChairs(string text)
        {
            Dispatcher.Invoke(() => { NumberOfEmptyChairs.Content = text; });
        }

        //Creating chairs and glasses queues.    BEHÖVER VI DESSA?
        private void CreateChairs()
        {
            for (int i = 0; i < numberofChairs; i++)
            {
                AvailableChairQueue.Add(new Chair());
            }
        }
        private void CreateGlasses()
        {
            for (int i = 0; i < numberofGlasses; i++)
            {
                CleanGlassQueue.Add(new Glass());
            }
        }

        //Tasks of Boucer, Bartender, Waiter and Patron.
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            CreateGlasses();
            CreateChairs();

            if (cts.IsCancellationRequested) { cts = new CancellationTokenSource(); }
            CancellationToken ct = cts.Token;

            OpenButton.IsEnabled = false;
            isBarOpen = true;                //Baren öppnas

            if (isBarOpen)
            {
                Bouncer bouncer = new Bouncer(BartenderQueue);
                bouncer.isBarOpen = () => isBarOpen;

                Bartender bartender = new Bartender(BartenderQueue, CleanGlassQueue);
                bartender.isBarOpen = () => isBarOpen;

                Waiter waiter = new Waiter(DirtyGlassQueue, CleanGlassQueue);
                waiter.isBarOpen = () => isBarOpen;

                Patron patron = new Patron(AvailableChairQueue, DirtyGlassQueue);
                patron.isBarOpen = () => isBarOpen;

                Task.Run(() => bouncer.Work(printBouncerInfo, printNumberOfGuests));

                Task.Run(() => bartender.PourBeer(printBartenderInfo));

                Task.Run(() => waiter.Work(printWaiterInfo, printNumberOfCleanGlasses));

                Task.Run(() => patron.PatronFoundChair(printPatronInfo, printNumberOfEmptyChairs));

                if (!isBarOpen)
                {
                    cts.Cancel();
                }
            }

            //if (stillGuestsInBar == true/* && PubQueue.Count() > 0*/) //dvs när patronQueue är tom ska denna bli false.
            //{
            //    //Bartender bartender = new Bartender(BartenderQueue, CleanGlassQueue);
                //bartender.isBarOpen = () => isBarOpen;

                //Waiter waiter = new Waiter(DirtyGlassQueue, CleanGlassQueue);
                //waiter.isBarOpen = () => isBarOpen;

                //Patron patron = new Patron(AvailableChairQueue, DirtyGlassQueue);
                //patron.isBarOpen = () => isBarOpen;

                //Task.Run(() => bartender.PourBeer(printBartenderInfo));

                //Task.Run(() => waiter.Work(printWaiterInfo, printNumberOfCleanGlasses));

                //Task.Run(() => patron.PatronFoundChair(printPatronInfo, printNumberOfEmptyChairs));

                //if (!stillGuestsInBar)
                //{
                //    cts.Cancel();
                //}
            //}
        }

        //Button to close the bar.
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isBarOpen = false;
            OpenButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}

