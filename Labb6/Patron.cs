using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Labb6
{
    public class Patron
    {
        public string Name { get; set; }

        private BlockingCollection<Patron> LooksForAvailableChairQueue;
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Patron> patronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        public Func<bool> isBarOpen { get; set; }

        public Patron(string name) { }

        public Patron() { }

        public Patron(BlockingCollection<Patron> AvailableChairQueue, BlockingCollection<Glass> dirtyGlassQueue, BlockingCollection<Patron> BartenderQueue)
        {
            this.LooksForAvailableChairQueue = AvailableChairQueue;
            this.DirtyGlassQueue = dirtyGlassQueue;
            this.BartenderQueue = BartenderQueue;
        }

        //The patron gets in the queue for the free chairs, sits down and then leaves the bar. A new Glass is then added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback, Action<string> printNumberOfEmptyChairs)
        {
            Random rTime = new Random();
            int numberOfChairs = 20;

            while (isBarOpen() || LooksForAvailableChairQueue.Count() > 0 || BartenderQueue.Count() > 0)
            {
                if (LooksForAvailableChairQueue.TryTake(out Patron p))
                {
                    if (numberOfChairs > 0)
                    {
                        callback($"{p.Name} sits down on a chair and drinks the beer.");
                        printNumberOfEmptyChairs("Number of empty chairs: " + --numberOfChairs);
                        int randomTimePosition = rTime.Next(10, 20) * 1000;
                        Thread.Sleep(randomTimePosition);
                        callback($"{p.Name} leaves the bar.");
                     //   printNumberOfGuests("Number of guests: " + --numberOfGuests);
                        printNumberOfEmptyChairs("Number of empty chairs: " + ++numberOfChairs);
                        DirtyGlassQueue.Add(new Glass());
                    }
                    else
                        LooksForAvailableChairQueue.Add(p);
                }
            }
            callback("The bar is now empty.");
        }
    }
}
