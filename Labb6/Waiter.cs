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
        private ConcurrentStack<Glass> DirtyGlassQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Patron> PatronQueue;
        public bool isBarOpen { get; set; }
       
        public Waiter(ConcurrentStack<Glass> dirtyGlassQueue)
        {
            this.DirtyGlassQueue = dirtyGlassQueue;
        }

        public void Work(Action<string> Callback, ConcurrentStack<Glass> DirtyGlassQueue,
           BlockingCollection<Glass> cleanGlassQueue, bool bouncerIsWorking, BlockingCollection<Patron> PatronQueue)
        {
            this.Callback = Callback;
            this.DirtyGlassQueue = DirtyGlassQueue;
            this.CleanGlassQueue = cleanGlassQueue;
            this.isBarOpen = bouncerIsWorking;
            this.PatronQueue = PatronQueue;

            Task.Run(() =>
            {
                while (isBarOpen)
                {
                    while (CleanGlassQueue.Count() != 8)
                    {
                        if (!DirtyGlassQueue.IsEmpty)
                        {
                            DirtyGlassQueue.TryPop(out Glass g);
                            Thread.Sleep(10000);
                            Callback("The waiter  picks up a glass and washes it");
                            Thread.Sleep(15000);
                            Callback("The waiter places the clean glass back on the shelf.");
                            CleanGlassQueue.Add(new Glass());
                        }
                    }
                }
            });
        }
        public void StopServing()
        {
            isBarOpen = false;
        }
        //public Waiter(BlockingCollection<Glass> cleanglassqueue) // hur gör med konstruktor??
        //{
        //    this.CleanGlassQueue = cleanglassqueue;
        //}

    }
}
