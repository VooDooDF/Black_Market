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
        public decimal Price { get; private set; }
        public const decimal MinPrice = 49.99m;

        // Конструктор класса Product
        public Product(string name, decimal price)
        {
            if (price < MinPrice)
            {
                throw new ArgumentOutOfRangeException(nameof(price), $"Цена не может быть меньше {MinPrice}!");
            }
            Name = name;
            Price = price;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice < MinPrice)
            {
                throw new ArgumentOutOfRangeException(nameof(newPrice), $"Цена не может быть меньше {MinPrice}!");
            }
            Price = newPrice;
        }
    }
}
