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
        public string hasBeer { get; set; }
        BlockingCollection<string> PatronQueue = new BlockingCollection<string>();  // VAD ÄR DETTA?
        private BlockingCollection<Chair> AvailableChairQueue;
        private BlockingCollection<Patron> PubQueue;
        private BlockingCollection<Patron> patronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        public Func<bool> isBarOpen { get; set; }

        public Patron(string name)
        {
            this.Name = name;
            //  PatronQueue.Add(name);
        }

        public Patron() { }

        public Patron(BlockingCollection<Chair> AvailableChairQueue, BlockingCollection<Glass> dirtyGlassQueue)
        {
            this.AvailableChairQueue = AvailableChairQueue;
            this.DirtyGlassQueue = dirtyGlassQueue;
        }

        //the patron looks for an available chair, sits down and then leaves the bar. A new Glass is added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback, Action<string> printNumberOfEmptyChairs)
        {
            int numberOfChairs = 21;

            while (isBarOpen())
            {
                while (AvailableChairQueue.Count() > 0)
                {
                    hasBeer = PatronQueue.FirstOrDefault();
                    PatronQueue.TryTake(out string str);
                    callback($"{hasBeer} looks for an available chair.");
                    Thread.Sleep(2000);
                    AvailableChairQueue.TryTake(out Chair chair);
                    printNumberOfEmptyChairs("Number of empty chairs: " + --numberOfChairs);
                    callback($"{hasBeer} sits down on a chair.");
                    Thread.Sleep(2000);
                    callback($"{hasBeer} leaves the bar.");
                    AvailableChairQueue.Add(new Chair());
                    DirtyGlassQueue.Add(new Glass());
                }
            }
        }
    }
}
