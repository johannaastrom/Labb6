using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb6
{
    public class Glass
    {
        public string glass { get; set; }

        public void Glasses()
        {
            Queue<int> glassList = new Queue<int>();
            for (int i = 0; i < glassList.Count; i++)
            {
                glassList.Enqueue(+1);
            }

            //glassList.Add("Glass 2");
            //glassList.Add("Glass 3");
            //glassList.Add("Glass 4");
            //glassList.Add("Glass 5");
            //glassList.Add("Glass 6");
            //glassList.Add("Glass 7");
            //glassList.Add("Glass 8");
            //glassList.Add("Glass 9");
            //glassList.Add("Glass 10");
        }
    }
}
