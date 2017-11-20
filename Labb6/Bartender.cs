﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Labb6
{
    public class Bartender
    {
        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Glass> CleanGlassQueue;
        private BlockingCollection<Patron> LooksForAvailableChairQueue;

        public Func<bool> isBarOpen { get; set; }

        int numberOfGlasses = 20;

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
                callback($"Gets a glass from the shelf.");
                Thread.Sleep(3000);
                callback($"Pours a beer to {((Patron)BartenderQueue.First()).Name} "); //Denna kö kan bli 0 och då stannar programmet.
                Thread.Sleep(3000);

                if (BartenderQueue.TryTake(out Patron p))
                {
                    LooksForAvailableChairQueue.Add(p); //Patron looks for a chair. 

                    if (CleanGlassQueue.TryTake(out Glass g))
                        printNumberOfCleanGlasses("Number of clean glasses: " + --numberOfGlasses);
                }
            }

            if (!isBarOpen())
                callback("The bartender goes home.");
        }
    }
}
