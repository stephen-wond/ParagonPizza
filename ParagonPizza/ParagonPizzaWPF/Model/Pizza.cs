using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonPizzaWPF.Model
{
    public class Pizza
    {
        public int OrderId { get; set; }
        public Base Base{ get; set; }
        public List<Topping> Toppings { get; set; }
        public Status Status { get; set; }

        public Pizza()
        {
            Status = Status.NewOrder;
            Toppings = new List<Topping>();
        }
    }
}
