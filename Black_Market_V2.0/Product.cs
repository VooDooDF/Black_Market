using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Product
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public decimal Price { get; }

        // Конструктор класса Product
        public Product(string name, decimal price)
        {
            if (price <= 0)
            {
                throw new Exception("Цена не может быть отрицательной или равна нулю!");
            }
            Name = name;
            Price = price;
        }
    }
}
