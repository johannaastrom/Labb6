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
      //  BlockingCollection<string> PatronQueue = new BlockingCollection<string>();  // VAD ÄR DETTA?
        private BlockingCollection<Patron> AvailableChairQueue;
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Patron> PubQueue;
        private BlockingCollection<Patron> patronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        public Func<bool> isBarOpen { get; set; }
        public bool patronHasBeer { get; set; }

        public Patron(string name)
        {
            //  PatronQueue.Add(name);
            this.patronHasBeer = true;
        }

        public Patron() { }

        public Patron(BlockingCollection<Patron> AvailableChairQueue, BlockingCollection<Glass> dirtyGlassQueue)
        {
            this.AvailableChairQueue = AvailableChairQueue;
            this.DirtyGlassQueue = dirtyGlassQueue;
        }

        //the patron looks for an available chair, sits down and then leaves the bar. A new Glass is added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback, Action<string> printNumberOfEmptyChairs)
        {
            int numberOfChairs = 21;

            while (isBarOpen() || AvailableChairQueue.Count() > 0)
            { 
                //    while (AvailableChairQueue.Count() > 0)
                //    {
                //hasBeer = PatronQueue.FirstOrDefault(); //detta ska kanske bort och bytas ut mot kön "patronQueue"?
                //PatronQueue.TryTake(out string str);  //samma här
                //callback($"{hasBeer} looks for an available chair.");//Print out names here.
                //Thread.Sleep(2000);
                if (AvailableChairQueue.TryTake(out Patron p))
                {
                    --numberOfChairs;
                    printNumberOfEmptyChairs("Number of empty chairs: " + --numberOfChairs);
                    callback($"{((Patron)AvailableChairQueue.First()).Name} sits down on a chair and drinks the beer.");
                    Thread.Sleep(2000);
                    callback($"{((Patron)AvailableChairQueue.First()).Name} leaves the bar.");
                    AvailableChairQueue.Add(new Patron());
                    ++numberOfChairs;
                    DirtyGlassQueue.Add(new Glass());
                    this.patronHasBeer = false;
                }
            }
        }
    }
}
