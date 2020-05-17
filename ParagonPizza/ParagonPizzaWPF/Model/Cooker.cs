using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonPizzaWPF.Model
{
    public class Cooker
    {
        private int counter;
        public int CookingTime { get; set; }
        public List<Pizza> PizzaQueue { get; set; }
        public int NextOrderNumber 
        {
            get 
            {
                counter++; 
                return counter; 
            } 
        }

        public Cooker()
        {
            PizzaQueue = new List<Pizza>();
            counter = 0;
        }
    }
}
