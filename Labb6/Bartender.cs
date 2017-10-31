using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Bartender-tråden ska vänta i baren tills en kund dyker upp. Så fort kunden kommer till baren går bartendern till hyllan och plockar ett glas. Om det inte finns något glas i hyllan så väntar bartendern tills det kommer tillbaka ett glas. Sedan häller bartendern upp öl till kunden och väntar på nästa.
//Det tar tre sekunder att hämta ett glas och tre sekunder till att hälla upp öl.
//När alla besökare har gått så går bartendern hem.
//hälla upp öl, plocka glas, vänta på besökare, stänga baren och gå hem? (bör va en eller flera metoder).

namespace Labb6
{
    public class Bartender
    {
        Glass newGlass = new Glass();

    }
}
