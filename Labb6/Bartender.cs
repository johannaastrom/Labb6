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
        private BlockingCollection<Patron> barQueue;
        private BlockingCollection<Glass> GlassQueue;
        int numberofGlasses = 20;

        public Bartender(BlockingCollection<Patron> barqueue)
        {
            this.barQueue = barqueue;
        }
        public Bartender(BlockingCollection<Glass> glassqueue)
        {
            this.GlassQueue = glassqueue;
        }

        public void PourBeer(Action<string> callback)
        {
            Glass g = new Glass();

            for (int i = 0; i < numberofGlasses; i++)
            {
                GlassQueue.Add(g);
            }

            while (true)
            {
                callback("Pours a beer");
                barQueue.Take();
                GlassQueue.Take();

            }
            //bartender går hem
            callback("Bartender goes home");


            #region gammal kod
            //Queue<Glass> glassQueue = new Queue<Glass>();
            //for (int i = 0; i < numberofGlasses; i++)
            //{
            //    glassQueue.Enqueue(newGlass);
            //}
            //if(glassQueue.Count > 0)
            //{
            //    glassQueue.Dequeue();
            //    callback("Takes a glass from the shelf");
            //    Thread.Sleep(3000);
            //    callback("Pours a glass of beer");
            //    newGlass.isGlassEmpty = false;
            //    Thread.Sleep(3000);
            //}
            //else if(glassQueue.Count == 0)
            //{
            //    Thread.Sleep(3000);
            //}
            #endregion

        }
    }
}
