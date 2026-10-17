using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Product
    {
        public string name { get; set; }
        public int price { get; set; }
        public int quantityOnMarket { get; set; }
        public int quantityOnPlayer { get; set; }

        public Product(string name, int price, int quantityOnMarket, int quantityOnPlayer)
        {
            this.name = name;
            this.price = price;
            this.quantityOnMarket = quantityOnMarket;
            this.quantityOnPlayer = quantityOnPlayer;
        }


    }
}
