using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Windows;

namespace Labb6
{
    public class Bartender
    {
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Patron> LooksForAvailableChairQueue;

        public Func<bool> isBarOpen { get; set; }

        public Bartender() { }

        public Bartender(BlockingCollection<Patron> bartenderQueue, BlockingCollection<Glass> CleanGlassQueue, BlockingCollection<Patron> AvailableChairQueue)
        {
            this.BartenderQueue = bartenderQueue;
            this.CleanGlassQueue = CleanGlassQueue;
            this.LooksForAvailableChairQueue = AvailableChairQueue;
        }

        //Bartender takes a glass from the shelf and pours a beer to patron.
        public void PourBeer(Action<string> callback, Action<string> printNumberOfCleanGlasses)
        {
            while (isBarOpen() || BartenderQueue.Count() > 0)
            {
                try
                {
                    Thread.Sleep(3000);
                    //callback($"Gets a glass from the shelf.");
                    callback($"Gets two glasses from the shelf.");//Couples night
                    CleanGlassQueue.TryTake(out Glass g);
                    CleanGlassQueue.TryTake(out Glass g2);//Couples night
                    Thread.Sleep(3000);
                    //BartenderQueue.TryTake(out Patron patron);//Patron går från baren
                    //callback($"Pours a beer to {patron.Name}");
                    //LooksForAvailableChairQueue.Add(patron);//Patron letar efter ledig stol
                    BartenderQueue.TryTake(out Patron patron);//Couples night ↓
                    BartenderQueue.TryTake(out Patron patron2);
                    callback($"Pours a beer to {patron.Name} and {patron2.Name}");
                    LooksForAvailableChairQueue.Add(patron);
                    LooksForAvailableChairQueue.Add(patron2);
                }
                catch (Exception e)
                {
                    callback("Waiting");
                }
                Thread.Sleep(3000);

                //if (BartenderQueue.TryTake(out Patron p))
                //{
                //    LooksForAvailableChairQueue.Take(); //Patron looks for a chair. 

                //    if (CleanGlassQueue.TryTake(out Glass g))
                //        CleanGlassQueue.TryTake(out Glass glass);
                //}
            }

            if (!isBarOpen())
                callback("The bartender goes home.");
        }
    }
}
