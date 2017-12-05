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
using System.Diagnostics;

namespace Labb6
{
    public class Bouncer
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        Stopwatch TimeGone = new Stopwatch();


        private BlockingCollection<Patron> BartenderQueue;
        private BlockingCollection<Patron> PubCount;

        public Func<bool> isBarOpen { get; set; }
        public bool sec = false;

        public Bouncer(BlockingCollection<Patron> bartenderqueue, BlockingCollection<Patron> PubCount)
        {
            this.BartenderQueue = bartenderqueue;
            this.PubCount = PubCount;
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
            sec = true;
            Random rTime = new Random();
            TimeGone.Start();
            TimeSpan ts = TimeGone.Elapsed;
            int HowManyPatronsEntring = 15;

            while (isBarOpen())
            {
                for (int i = 0; i < 2; i++)//Couples night
                {
                    Patron p = CreatePatron();
                    PubCount.Add(p);//Patron enters the pub
                    BartenderQueue.Add(p); //Patron goes to the bar.
                    callback($"{p.Name} gets into the bar.");
                    Thread.Sleep(13);
                }
                int randomTimePosition = rTime.Next(3, 10) * 1000;
                Thread.Sleep(randomTimePosition);
                //AddMorePatrons(callback);

                //if (TimeGone.ElapsedMilliseconds == 20000)//Bussload/Couples night
                if (sec == true)
                {
                    //AddMorePatrons(callback);
                    Thread.Sleep(10000);
                    for (int i = 0; i <= HowManyPatronsEntring; i++)
                    {
                        Patron p = CreatePatron();
                        PubCount.Add(p);
                        callback($"{p.Name} gets into the bar.");
                        BartenderQueue.Add(p); //Patron goes to the bar.
                        Thread.Sleep(13);
                    }
                    sec = false;
                }
            }

            if (!isBarOpen())
                callback("The bouncer goes home.");
        }

        public void AddMorePatrons(Action<string> callback)//bussload
        {
            int HowManyPatronsEntring = 15;
            for (int i = 0; i <= HowManyPatronsEntring; i++)
            {
                Patron p = CreatePatron();
                PubCount.Add(p);
                callback($"{p.Name} gets into the bar.");
                BartenderQueue.Add(p); //Patron goes to the bar.
                Thread.Sleep(13);
            }

        }
    }
}
