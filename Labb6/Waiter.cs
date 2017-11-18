using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

//Servitrisen ska plocka upp alla tomma glas som finns på borden. Sedan diskar hon glasen och ställer dem i hyllan.
//Det tar tio sekunder att plocka glasen från borden och femton sekunder att diska dem.
//När alla besökare har gått så går servitrisen hem.

namespace Labb6
{
    public class Waiter
    {
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Patron> PatronQueue;
        private BlockingCollection<Chair> AvailableChairQueue;
        private BlockingCollection<Patron> PubQueue;
        private BlockingCollection<Patron> BartenderQueue;

        public Func<bool> isBarOpen { get; set; }

        //Three constructors
        public Waiter(BlockingCollection<Glass> DirtyGlassQueue)
        {
            this.DirtyGlassQueue = DirtyGlassQueue;
        }

        public Waiter(BlockingCollection<Glass> DirtyGlassQueue, BlockingCollection<Glass> CleanGlassQueue, BlockingCollection<Patron> BartenderQueue)
        {
            this.DirtyGlassQueue = DirtyGlassQueue;
            this.CleanGlassQueue = CleanGlassQueue;
            this.BartenderQueue = BartenderQueue;
        }

        public Waiter() { }

        public void Work(Action<string> Callback, Action<string> printNumberOfCleanGlasses)
        {
            int numberOfGlasses = 21;

            while (isBarOpen() || BartenderQueue.Count() > 0 || DirtyGlassQueue.Count() > 0)
            {
                if (DirtyGlassQueue.TryTake(out Glass g))
                {
                    Callback("Picks up a dirty glass and washes it");
                    Thread.Sleep(2000);
                    Callback("Places the clean glass back on the shelf.");
                    CleanGlassQueue.Add(new Glass());
                    ++numberOfGlasses;
                    Thread.Sleep(2000);

                    printNumberOfCleanGlasses("Number of clean glasses: " + --numberOfGlasses);
                }
            }
            // if (!isBarOpen())
            //while (BartenderQueue.Count() > 0)
            //{
            //    DirtyGlassQueue.TryTake(out Glass g);
            //    Thread.Sleep(2000);
            //    Callback("Picks up a dirty glass and washes it");
            //    Thread.Sleep(2000);
            //    Callback("Places the clean glass back on the shelf.");
            //    CleanGlassQueue.Add(new Glass());
            //    ++numberOfGlasses;
            //}
            Callback("The waiter goes home.");
        }
    }
}
