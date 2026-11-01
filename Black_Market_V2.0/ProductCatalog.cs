using System;
using System.Collections.Generic;
using System.Text;

namespace Black_Market
{
    internal class ProductCatalog
    {
        public List<Product> Products { get; private set; }

        // Конструктор класса ProductCatalog
        public ProductCatalog()
        {
            Products = new List<Product>
            {
                new Product("Eye of God", 200),
                new Product("Dragon's Tongue", 120),
                new Product("Heart of the Sea", 160),
                new Product("Phoenix Feather", 100)
            };
        }

        // Поиск продукта по Id
        public Product? FindProductById(Guid productId)
        {
            return Products.FirstOrDefault(p => p.Id == productId);
        }
    }
}