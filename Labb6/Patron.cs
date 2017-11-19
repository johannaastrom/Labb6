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

        private BlockingCollection<Patron> AvailableChairQueue;
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Patron> PubQueue;
        private BlockingCollection<Patron> patronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        public Func<bool> isBarOpen { get; set; }

        public Patron(string name) { }

        public Patron() { }

        public Patron(BlockingCollection<Patron> AvailableChairQueue, BlockingCollection<Glass> dirtyGlassQueue)
        {
            this.AvailableChairQueue = AvailableChairQueue;
            this.DirtyGlassQueue = dirtyGlassQueue;
        }

        //the patron gets in the queue for the free chairs, sits down and then leaves the bar. A new Glass is then added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback, Action<string> printNumberOfEmptyChairs)
        {
            int numberOfChairs = 21;

            while (isBarOpen() || AvailableChairQueue.Count() > 0)
            {
                //callback($"{hasBeer} looks for an available chair.");//Print out names here.
                //Thread.Sleep(2000);
                if (AvailableChairQueue.TryTake(out Patron p))
                {
                    if (numberOfChairs > 0)
                    {
                        callback($"{p.Name} sits down on a chair and drinks the beer.");
                        printNumberOfEmptyChairs("Number of empty chairs: " + --numberOfChairs);
                        Thread.Sleep(2000);
                        callback($"{p.Name} leaves the bar.");
                        ++numberOfChairs;
                        DirtyGlassQueue.Add(new Glass());
                    }
                    else
                        AvailableChairQueue.Add(p);
                }
            }
        }
    }
}
