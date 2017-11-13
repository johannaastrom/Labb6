using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Servitrisen ska plocka upp alla tomma glas som finns på borden. Sedan diskar hon glasen och ställer dem i hyllan.
//Det tar tio sekunder att plocka glasen från borden och femton sekunder att diska dem.
//När alla besökare har gått så går servitrisen hem.

namespace Labb6
{
    public class Waiter
    {
        private System.Collections.Concurrent.BlockingCollection<Glass> Haj;
        public Waiter(System.Collections.Concurrent.BlockingCollection<Glass> haj)
        {
            this.Haj = haj;
        }
    }
}
