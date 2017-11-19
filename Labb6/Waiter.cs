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
        private BlockingCollection<Patron> LooksForAvailableChairQueue;
        private BlockingCollection<Patron> PubQueue;
        private BlockingCollection<Patron> BartenderQueue;

        public Func<bool> isBarOpen { get; set; }

        //Three constructors
        public Waiter(BlockingCollection<Glass> DirtyGlassQueue)
        {
            this.DirtyGlassQueue = DirtyGlassQueue;
        }

        public Waiter(BlockingCollection<Glass> DirtyGlassQueue, BlockingCollection<Glass> CleanGlassQueue, BlockingCollection<Patron> BartenderQueue, BlockingCollection<Patron> LooksForAvailableChairQueue)
        {
            this.DirtyGlassQueue = DirtyGlassQueue;
            this.CleanGlassQueue = CleanGlassQueue;
            this.BartenderQueue = BartenderQueue;
            this.LooksForAvailableChairQueue = LooksForAvailableChairQueue;
        }

        public Waiter() { }

        public void Work(Action<string> Callback, Action<string> printNumberOfCleanGlasses)
        {
            int numberOfGlasses = 20;

            while (isBarOpen() || BartenderQueue.Count() > 0 || DirtyGlassQueue.Count() > 0 || LooksForAvailableChairQueue.Count() > 0)
            {
                if (DirtyGlassQueue.TryTake(out Glass g))
                {
                    printNumberOfCleanGlasses("Number of clean glasses: " + --numberOfGlasses);
                    Callback("Picks up a dirty glass and washes it");
                    Thread.Sleep(2000);
                    Callback("Places the clean glass back on the shelf.");
                    CleanGlassQueue.Add(new Glass());
                    printNumberOfCleanGlasses("Number of clean glasses: " + ++numberOfGlasses);
                    Thread.Sleep(2000);
                }
            }
            Thread.Sleep(10000); //Annars går det för fort för honom!
            if (isBarOpen() || BartenderQueue.Count() > 0 || DirtyGlassQueue.Count() > 0 || LooksForAvailableChairQueue.Count() > 0)
            {
                if (DirtyGlassQueue.TryTake(out Glass g))
                {
                    printNumberOfCleanGlasses("Number of clean glasses: " + --numberOfGlasses);
                    Callback("Picks up a dirty glass and washes it");
                    Thread.Sleep(2000);
                    Callback("Places the clean glass back on the shelf.");
                    CleanGlassQueue.Add(new Glass());
                    printNumberOfCleanGlasses("Number of clean glasses: " + ++numberOfGlasses);
                    Thread.Sleep(2000);
                }
            }
            if (!isBarOpen())
                Callback("The waiter goes home.");
        }
    }
}
