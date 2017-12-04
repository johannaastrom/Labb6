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
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Patron> PubCount;
        public Func<bool> isBarOpen { get; set; }

        public Patron(string name) { }

        public Patron() { }

        public Patron(BlockingCollection<Patron> AvailableChairQueue, BlockingCollection<Glass> dirtyGlassQueue,
            BlockingCollection<Patron> PubCount)
        {
            this.LooksForAvailableChairQueue = AvailableChairQueue;
            this.DirtyGlassQueue = dirtyGlassQueue;
            this.PubCount = PubCount;
        }

        //The patron gets in the queue for the free chairs, sits down and then leaves the bar. A new Glass is then added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback)
        {
            Random rTime = new Random();
            int numberOfChairs = 20;

            while (isBarOpen() || LooksForAvailableChairQueue.Count() > 0 || PubCount.Count() > 0)
            {
                if (LooksForAvailableChairQueue.TryTake(out Patron p))
                {
                    if (numberOfChairs > 0)
                    {
                        callback($"{p.Name} sits down on a chair and drinks the beer.");
                        LooksForAvailableChairQueue.TryTake(out Patron pat);
                        int randomTimePosition = rTime.Next(10, 20) * 1000;
                        Thread.Sleep(randomTimePosition);
                        callback($"{p.Name} leaves the bar.");
                        PubCount.TryTake(out Patron patron);
                        //BartenderQueue.TryTake(out Patron patron);
                        LooksForAvailableChairQueue.Add(new Patron());
                        DirtyGlassQueue.Add(new Glass());
                    }
                    else
                    {
                        LooksForAvailableChairQueue.Add(p); //stämmer detta? ska tas bort?
                        callback($"{p.Name} looks for a chair");
                    }
                }
            }
            callback("The bar is now empty.");
        }
    }
}
