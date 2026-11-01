using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Product
    {
        private Guid Id;
        private string Name { get; set; }
        private double Price { get; set; }
        private int QuantityOnMarket { get; set; }
        private int QuantityOnPlayer { get; set; }

        public Product(string name, int price, int quantityOnMarket, int quantityOnPlayer)
        {
            Id = NewGuid(); 
            Name = name;
            Price = price;
            QuantityOnMarket = quantityOnMarket;
            QuantityOnPlayer = quantityOnPlayer;
        }


    }
}
