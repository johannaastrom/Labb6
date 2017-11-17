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
        bool stillGuestsInBar;
        public string Name { get; set; }
        BlockingCollection<string> PatronQueue = new BlockingCollection<string>();
        BlockingCollection<Patron> PubQueue;
        public Func<bool> isBarOpen { get; set; }

        public Patron(string name)
        {
            this.Name = name;
            PatronQueue.Add(name);
        }

        public Patron() { }

        private BlockingCollection<Patron> patronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Chair> availableChairQueue;
        public string hasBeer { get; set; }
        private Action<string> Callback;

        //the patron looks for an available chair, sits down and then leaves the bar. A new Glass is added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback, Action<string> printNumberOfEmptyChairs)
        {
            int numberOfChairs = 21;

            while (isBarOpen())
            {
                hasBeer = PatronQueue.FirstOrDefault();
                PatronQueue.Take();
                callback($"{hasBeer} looks for an available chair.");
                Thread.Sleep(4000);
                availableChairQueue.TryTake(out Chair chair);
                printNumberOfEmptyChairs("Number of empty chairs: " + --numberOfChairs);
                callback($"{hasBeer} sits down on a chair.");
                Thread.Sleep(10000);
                callback($"{hasBeer} leaves the bar.");
                availableChairQueue.Add(new Chair());
                DirtyGlassQueue.Add(new Glass());
            }
        }
    }
}
