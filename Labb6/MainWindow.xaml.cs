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
        Manager manager = new Manager();

        //values to change number of's.
        public bool isBarOpen = false;
        public int numberofGlasses = 20;
        public int numberOfChairs = 20;

        //Queues
        BlockingCollection<Patron> BartenderQueue = new BlockingCollection<Patron>();
        BlockingCollection<Glass> CleanGlassQueue = new BlockingCollection<Glass>();
        BlockingCollection<Patron> LooksForAvailableChairQueue = new BlockingCollection<Patron>();
        BlockingCollection<Glass> DirtyGlassQueue = new BlockingCollection<Glass>();

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
            Dispatcher.Invoke(() => { BouncerListBox.Items.Insert(0, patronInfo);
                NumberOfEmptyChairs.Content = $"Empty chairs: {LooksForAvailableChairQueue.Count()} ";
            });
        }
        private void printWaiterInfo(string waiterInfo)
        {
            Dispatcher.Invoke(() => {
                WaiterListBox.Items.Insert(0, waiterInfo);
                NumberOfGlasses.Content = $"Clean glasses: {CleanGlassQueue.Count()} ";
                });
        }
        private void printBartenderInfo(string bartenderInfo)
        {
            Dispatcher.Invoke(() => {
                BartenderListBox.Items.Insert(0, bartenderInfo);
                NumberOfGlasses.Content = $"Clean glasses: {CleanGlassQueue.Count()} ";
            });
        }

        //Printing the labels of guests, clean glasses and empty chairs.
        private void printNumberOfGuests(int text)
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

        //Creating chairs and glasses queues.
        //private void CreateChairs()
        //{
        //    for (int i = 0; i < numberOfChairs; i++)
        //    {
        //        LooksForAvailableChairQueue.Add(new Patron());
        //    }
        //}
        private void CreateGlasses()
        {
            for (int i = 0; i < numberofGlasses; i++)
            {
                CleanGlassQueue.Add(new Glass());
            }
        }

        //Tasks of Bouncer, Bartender, Waiter and Patron.
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            CreateGlasses();
            //CreateChairs();

            if (cts.IsCancellationRequested) { cts = new CancellationTokenSource(); }
            CancellationToken ct = cts.Token;

            OpenButton.IsEnabled = false;
            isBarOpen = true;

            if (isBarOpen)
            {
                //Creating agents
                Bouncer bouncer = new Bouncer(BartenderQueue);
                bouncer.isBarOpen = () => isBarOpen;

                Bartender bartender = new Bartender(BartenderQueue, CleanGlassQueue, LooksForAvailableChairQueue);
                bartender.isBarOpen = () => isBarOpen;

                Waiter waiter = new Waiter(DirtyGlassQueue, CleanGlassQueue, BartenderQueue, LooksForAvailableChairQueue);
                waiter.isBarOpen = () => isBarOpen;

                Patron patron = new Patron(LooksForAvailableChairQueue, DirtyGlassQueue, BartenderQueue);
                patron.isBarOpen = () => isBarOpen;
                //Running
                Task.Run(() => bouncer.Work(printBouncerInfo /*, printNumberOfGuests*/));

                Task.Run(() => bartender.PourBeer(printBartenderInfo, printNumberOfCleanGlasses));

                Task.Run(() => patron.PatronFoundChair(printPatronInfo, printNumberOfEmptyChairs/*, printNumberOfCleanGlasses, printNumberOfGuests*/));

                Task.Run(() => waiter.Work(printWaiterInfo/*, printNumberOfCleanGlasses*/));

                //Task.Run(() => printNumberOfGuests(manager.GetGuests()));
                // printNumberOfGuests(manager.GetGuests());

                if (!isBarOpen || BartenderQueue.Count == 0 || LooksForAvailableChairQueue.Count == 0)
                {
                    cts.Cancel();
                }
            }
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

