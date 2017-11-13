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


//Inkastaren släpper in kunder slumpvis, efter tre till tio sekunder. Inkastaren kontrollerar leg, så att alla i baren kan veta vad kunden heter. (Slumpa ett namn åt nya kunder från en lista) Inkastaren slutar släppa in nya kunder när baren stänger och går hem direkt.

namespace Labb6
{
    public class Bouncer
    {
        private BlockingCollection<Patron> barQueue;
        public Bouncer(BlockingCollection<Patron> barqueue)
        {
            this.barQueue = barqueue;
        }

        public Patron CreateGuest() //Creates guests by random time and name from list
        {
            List<string> guestList = new List<string>();
            guestList.Add("Karl");
            guestList.Add("Kim");
            guestList.Add("Alex");
            guestList.Add("Charlie");
            guestList.Add("Robin");
            guestList.Add("Sam");
            guestList.Add("Johanna");
            guestList.Add("Andreas");
            guestList.Add("David");
            guestList.Add("Johan");
            guestList.Add("Anders");
            guestList.Add("Erik");
            guestList.Add("Elin");
            guestList.Add("Molly");
            guestList.Add("Patrik");
            guestList.Add("Victoria");
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
        public void Work(Action<string> callback) //creates a guest
        {
            Random rTime = new Random();
            while (true)
            {
                Patron p = CreateGuest();
                callback($"Bouncern lets {p.Name} into the bar.");
                barQueue.Add(p); //Guest goes to the bar.
                int randomTimePosition = rTime.Next(3, 10) * 1000;
                Thread.Sleep(randomTimePosition);
            }
        }
    }
}
