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
        public Topping Topping1 { get; set; }
        public Topping Topping2 { get; set; }
        public Topping Topping3 { get; set; }
        public Topping Topping4 { get; set; }
        public Status Status { get; set; }

        public Pizza()
        {
            Status = Status.NewOrder;
        }
    }
}
