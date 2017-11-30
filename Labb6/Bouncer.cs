using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Collections.Concurrent;


namespace Labb6
{
    public class Bouncer
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        private BlockingCollection<Patron> BartenderQueue;

        public Func<bool> isBarOpen { get; set; }

        public Bouncer(BlockingCollection<Patron> barqueue)
        {
            this.BartenderQueue = barqueue;
        }

        public Bouncer() { }

        //Creates patrons by random time and name from list
        public Patron CreatePatron()
        {
            List<string> guestList = new List<string>();
            guestList.Add("KulmageKarl");
            guestList.Add("GamleGreta");
            guestList.Add("KarateKarin");
            guestList.Add("HelikopterHerbert");
            guestList.Add("RundaRobin");
            guestList.Add("Samuel Adams");
            guestList.Add("NyktreNiklas");
            guestList.Add("OnyktreOlle");
            guestList.Add("GalneGunnar");
            guestList.Add("Johan");
            guestList.Add("AlagarAnders");
            guestList.Add("Erik");
            guestList.Add("ElakaElin");
            guestList.Add("Molly");
            guestList.Add("PackadePatrik");
            guestList.Add("VingligaVictoria");
            guestList.Add("Isabella");
            guestList.Add("Gustav");
            guestList.Add("Erika");
            guestList.Add("Jaqueline");
            guestList.Add("TörstigaTina");
            guestList.Add("HalstorreHans");
            guestList.Add("ÖlsugneÖrjan");
            guestList.Add("FulleFelix");
            guestList.Add("DrunkenDennis");
            guestList.Add("WobblyWilly");
            guestList.Add("CrazyCynthia");
            guestList.Add("SoberSandra");
            guestList.Add("DräggigeDan");
            guestList.Add("SnurrigeSamuel");

            Random rGuest = new Random();
            int randomGuestPosition = rGuest.Next(guestList.Count);
            string randomName = guestList[randomGuestPosition];

            var patron = new Patron();

            patron.Name = randomName;

            return patron;
        }

        //Creates a patron
        public void Work(Action<string> callback)
        {
            Random rTime = new Random();

            while (isBarOpen())
            {
                Patron p = CreatePatron();
                BartenderQueue.Add(new Patron()); //Patron goes to the bar.
                callback($"{p.Name} gets into the bar.");
                int randomTimePosition = rTime.Next(3, 10) * 1000;
                Thread.Sleep(randomTimePosition);
            }

            //if (isBarOpen())//Bussload/Couples night
            //{
            //    Thread.Sleep(20000);
            //    AddMorePatrons(callback, printNumberOfGuests);
            //}
            if (!isBarOpen())
                callback("The bouncer goes home.");
        }

        public void AddMorePatrons(Action<string> callback/*, Action<string> printNumberOfGuests*/)
        {
            int NumbOfPatrons = 0;
            int HowManyPatronsEntring = 15;
            int numberOfGuests = 0;
            while (NumbOfPatrons < HowManyPatronsEntring)
            {
                Patron p = CreatePatron();
                BartenderQueue.Add(p); //Patron goes to the bar.
                callback($"{p.Name} gets into the bar.");
                //printNumberOfGuests("Number of guests: " + ++numberOfGuests);
                NumbOfPatrons++;
                Thread.Sleep(13);
            }
        }
    }
}
