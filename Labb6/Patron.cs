using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Controls;

namespace Labb6
{
    public class Patron
    {
        public string Name { get; set; }

        private BlockingCollection<Patron> LooksForAvailableChairQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Patron> PubCount;
        private BlockingCollection<Chair> FreeChairs;
        public Func<bool> isBarOpen { get; set; }

        public Patron(string name) { }

        public Patron() { }

        public Patron(BlockingCollection<Patron> AvailableChairQueue, BlockingCollection<Glass> dirtyGlassQueue,
            BlockingCollection<Patron> PubCount, BlockingCollection<Chair> FreeChairs)
        {
            this.LooksForAvailableChairQueue = AvailableChairQueue;
            this.DirtyGlassQueue = dirtyGlassQueue;
            this.PubCount = PubCount;
            this.FreeChairs = FreeChairs;
        }

        //The patron gets in the queue for the free chairs, sits down and then leaves the bar. A new Glass is then added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback)
        {
            Random rTime = new Random();
            int numberOfChairs = 20;

            while (isBarOpen() || LooksForAvailableChairQueue.Count > 0 || PubCount.Count() > 0)
            {
                //if (LooksForAvailableChairQueue.TryTake(out Patron p))
                //{
                if (FreeChairs.Count != 0)
                {
                    Thread.Sleep(6200);
                    LooksForAvailableChairQueue.TryTake(out Patron p);
                    LooksForAvailableChairQueue.TryTake(out Patron p2);//Couples night
                    FreeChairs.TryTake(out Chair c);
                    FreeChairs.TryTake(out Chair c2);//Couples night
                    callback($"{p.Name} and {p2.Name} sits down at a table and drinks the beers.");
                    int randomTimePosition = rTime.Next(10, 20) * 1000;
                    Thread.Sleep(randomTimePosition);
                    DirtyGlassQueue.Add(new Glass());//Patron har druckit upp ölen och lägger till ett smutsigt glas
                    DirtyGlassQueue.Add(new Glass());//Couples night
                    FreeChairs.Add(c);
                    //callback($"{p.Name} leaves the bar.");
                    callback($"{p.Name} and {p2.Name} leaves the bar.");//couples night
                    PubCount.TryTake(out Patron patron);//Patron lämnar puben
                    PubCount.TryTake(out Patron patron2);//Couples night
                }
                else if (FreeChairs.Count == 0)
                {
                    LooksForAvailableChairQueue.TryTake(out Patron p);
                    callback($"{p.Name} looks for a chair");
                    LooksForAvailableChairQueue.Add(p);
                }
                //}
            }
            callback("The bar is now empty.");
        }

    }
}
