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
        BlockingCollection<string> PatronQueue = new BlockingCollection<string>();

        public Patron(string name)
        {
            this.Name = name;
            PatronQueue.Add(name);
        }

       public Patron() { }

        private BlockingCollection<Patron> patronQueue;
        private BlockingCollection<Glass> DirtyGlassQueue;
        private BlockingCollection<Chair> availableChairQueue;
        public string hasBeer { get; set; }
        private Action<string> Callback;

        //the patron looks for an available chair, sits down and then leaves the bar. A new Glass is added to DirtyGlassQueue.
        public void PatronFoundChair(Action<string> callback, BlockingCollection<Glass> DirtyGlassQueue, BlockingCollection<Chair> availableChairQueue, BlockingCollection<Patron> patronQueue)
        {
            this.Callback = callback;
            this.DirtyGlassQueue = DirtyGlassQueue;
            this.availableChairQueue = availableChairQueue;
            this.patronQueue = patronQueue;

            Task.Run(() =>
            {
                hasBeer = PatronQueue.FirstOrDefault();
                PatronQueue.Take();
                callback($"{hasBeer} looks for an available chair.");
                Thread.Sleep(4000);
                callback($"{hasBeer} sits down on a chair.");
                Thread.Sleep(10000);
                callback($"{hasBeer} leaves the bar.");
                DirtyGlassQueue.Add(new Glass());
            });
        }


    }
}
