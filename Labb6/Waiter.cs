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
        private Action<string> Callback;
        private Action<string> printNumberOfCleanGlasses;
        private ConcurrentStack<Glass> DirtyGlassQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Patron> PatronQueue;
        private BlockingCollection<Glass> dirtyGlassQueue;
        

        public bool isBarOpen { get; set; }
        public int numberOfGlasses = 20;
       
        public Waiter(ConcurrentStack<Glass> dirtyGlassQueue)
        {
            this.DirtyGlassQueue = dirtyGlassQueue;
        }

        public Waiter(BlockingCollection<Glass> dirtyGlassQueue, BlockingCollection<Glass> cleanGlassQueue)
        {
            this.dirtyGlassQueue = dirtyGlassQueue;
            CleanGlassQueue = cleanGlassQueue;
        }

        public void Work(Action<string> Callback, Action<string> printNumberOfCleanGlasses/*ConcurrentStack<Glass> DirtyGlassQueue,
           BlockingCollection<Glass> cleanGlassQueue*/)
        {
            this.Callback = Callback;
            this.printNumberOfCleanGlasses = printNumberOfCleanGlasses;

            //this.DirtyGlassQueue = DirtyGlassQueue;
            //this.CleanGlassQueue = cleanGlassQueue;

            Task.Run(() =>
            {
                while (isBarOpen)
                {
                    while (CleanGlassQueue.Count() != numberOfGlasses)
                    {
                        if (!DirtyGlassQueue.IsEmpty)
                        {
                            DirtyGlassQueue.TryPop(out Glass g);
                            Thread.Sleep(10000);
                            Callback("The waiter  picks up a glass and washes it");
                            Thread.Sleep(15000);
                            Callback("The waiter places the clean glass back on the shelf.");
                            CleanGlassQueue.Add(new Glass());

                            printNumberOfCleanGlasses("Number of clean glasses: " + ++numberOfGlasses);
                        }
                    }
                }
                Callback("The waiter goes home.");
            });
        }
        public void StopServing()
        {
            isBarOpen = false;
        }
    }
}
