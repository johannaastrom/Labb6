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


//Inkastaren släpper in kunder slumpvis, efter tre till tio sekunder. Inkastaren kontrollerar leg, så att alla i baren kan veta vad kunden heter. (Slumpa ett namn åt nya kunder från en lista) Inkastaren slutar släppa in nya kunder när baren stänger och går hem direkt.

namespace Labb6
{
    public class Bouncer : MainWindow
    {
        public void GuestList()
        {
            Task.Run(() =>
            {

            List<string> guests = new List<string>();
            guests.Add("Karl");
            guests.Add("Kim");
            guests.Add("Alex");
            guests.Add("Charlie");
            guests.Add("Robin");
            guests.Add("Sam");
            guests.Add("Johanna");
            guests.Add("Andreas");
            guests.Add("David");
            guests.Add("Johan");
            guests.Add("Anders");
            guests.Add("Erik");
            guests.Add("Elin");
            guests.Add("Molly");
            guests.Add("Patrik");
            guests.Add("Victoria");
            guests.Add("Isabella");
            guests.Add("Gustav");
            guests.Add("Erika");
            guests.Add("Jaqueline");

            Random r = new Random();
            int randomPosition = r.Next(guests.Count);
            string currrentName = guests[randomPosition];

            });
        }
    }
}
