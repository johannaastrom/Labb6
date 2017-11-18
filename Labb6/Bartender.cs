using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

//Bartender-tråden ska vänta i baren tills en kund dyker upp. Så fort kunden kommer till baren går bartendern till hyllan och plockar ett glas. Om det inte finns något glas i hyllan så väntar bartendern tills det kommer tillbaka ett glas. Sedan häller bartendern upp öl till kunden och väntar på nästa.
//Det tar tre sekunder att hämta ett glas och tre sekunder till att hälla upp öl.
//När alla besökare har gått så går bartendern hem.
//hälla upp öl, plocka glas, vänta på besökare, stänga baren och gå hem? (bör va en eller flera metoder).

namespace Labb6
{
    public class Bartender
    {
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Chair> AvailableChairQueue;
        private BlockingCollection<Patron> PatronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Patron> PubQueue;

        public Func<bool> isBarOpen { get; set; }

        int numberofGlasses = 20;

        public Bartender()
        {
        }

        public Bartender(BlockingCollection<Patron> bartenderQueue, BlockingCollection<Glass> CleanGlassQueue)
        {
            this.BartenderQueue = bartenderQueue;
            this.CleanGlassQueue = CleanGlassQueue;
        }

        public void PourBeer(Action<string> callback)
        {
            while (isBarOpen())
            {
                while (BartenderQueue.Count() > 0)
                {
                    callback($"Gets a glass for ");/*{((Patron)BartenderQueue.First()).Name}*/
                    Thread.Sleep(3000);
                    callback($"Pours a beer to {((Patron)BartenderQueue.First()).Name} ");
                    Thread.Sleep(3000);
                    if (BartenderQueue.TryTake(out Patron p))
                        --numberofGlasses;
                    if (CleanGlassQueue.TryTake(out Glass g))
                        ++numberofGlasses;

                    //  BartenderQueue.First().PatronFoundChair(callback, DirtyGlassQueue, AvailableChairQueue, PatronQueue);     BEHÖVS DETTA?
                }
            }
            if (!isBarOpen())
                callback("The bartender goes home.");
        }
    }
}
