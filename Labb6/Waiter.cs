using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows;

namespace Labb6
{
    public class Waiter
    {
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Patron> LooksForAvailableChairQueue;
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Patron> PubCount;

        public Func<bool> isBarOpen { get; set; }

        //Three constructors
        public Waiter(BlockingCollection<Glass> DirtyGlassQueue)
        {
            this.DirtyGlassQueue = DirtyGlassQueue;
        }

        public Waiter(BlockingCollection<Glass> DirtyGlassQueue, BlockingCollection<Glass> CleanGlassQueue, BlockingCollection<Patron> BartenderQueue, BlockingCollection<Patron> LooksForAvailableChairQueue, BlockingCollection<Patron> PubCount)
        {
            this.DirtyGlassQueue = DirtyGlassQueue;
            this.CleanGlassQueue = CleanGlassQueue;
            this.BartenderQueue = BartenderQueue;
            this.LooksForAvailableChairQueue = LooksForAvailableChairQueue;
            this.PubCount = PubCount;
        }

        public Waiter() { }

        public void Work(Action<string> Callback)
        {
            while (isBarOpen() || PubCount.Count > 0 /*BartenderQueue.Count() > 0 || DirtyGlassQueue.Count() > 0 || LooksForAvailableChairQueue.Count() > 0*/)
            {
                //if (DirtyGlassQueue.TryTake(out Glass g))
                //{
                if (DirtyGlassQueue.Count > 0)   ///här bör villkor finnas för att kolla om det finns glas i dirtyglassqueue och ta alla
                {
                    Callback("Picks up all dirty glasses and washes it");
                    Thread.Sleep(10000);
                    Callback("Places the clean glasses back on the shelf.");
                    CleanGlassQueue.Add(new Glass());
                    Thread.Sleep(15000);
                    foreach (var dirtyglass in DirtyGlassQueue)
                    {
                        DirtyGlassQueue.TryTake(out Glass gl);
                        CleanGlassQueue.TryAdd(new Glass());
                    }
                }
                //}
            }
            Thread.Sleep(10000); //Annars går det för fort för honom!
            if (isBarOpen() || BartenderQueue.Count() > 0 || DirtyGlassQueue.Count() > 0 || LooksForAvailableChairQueue.Count() > 0)
            {
                if (DirtyGlassQueue.TryTake(out Glass g))
                {
                    Callback("Picks up a dirty glass and washes it");
                    Thread.Sleep(10000);
                    Callback("Places the clean glass back on the shelf.");
                    CleanGlassQueue.Add(new Glass());
                    Thread.Sleep(15000);
                }
            }
            if (!isBarOpen())
                Callback("The waiter goes home.");
        }
    }
}
