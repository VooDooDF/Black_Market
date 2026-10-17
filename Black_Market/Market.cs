using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Market
{
    internal class Market
    {
        Random random = new Random();
        public List<Product> products { get; private set; } = new List<Product>()
        {
            new Product("Eye of God", 200, 2, 0),
            new Product("Dragon's Tongue", 120, 8, 0),
            new Product("Heart of the Sea", 160, 4, 0),
            new Product("Phoenix Feather", 100, 6, 0),
        };

        public string TryBuy(Player player, int productIndex, int quantity)
        {

            int inventoryCount = 0;
            foreach (var product in products)
            {
                inventoryCount += product.quantityOnPlayer;
            }

            if (player.InventoryCapacity - inventoryCount < quantity)
            {
                return "Недостаточно места в инвентаре.";
            }

            if (productIndex < 0 || productIndex >= products.Count)
            {
                return "Неверный номер товара.";
            }

            int totalCost = (int)(products[productIndex].price * quantity);

            if (totalCost > player.Money)
            {
                return "Недостаточно средств для покупки.";
            }

            if (quantity <= 0 || quantity > products[productIndex].quantityOnMarket)
            {
                return "Неверное количество.";
            }

            player.SubtractMoney(totalCost);
            products[productIndex].quantityOnMarket -= quantity;
            products[productIndex].quantityOnPlayer += quantity;

            return $"Вы успешно купили {quantity} {products[productIndex].name}(ов) за {totalCost} монет.";
        }

        public string TrySell(Player player, int productIndex, int quantity)
        {
            if (productIndex < 0 || productIndex >= products.Count)
            {
                return "Неверный номер товара.";
            }

            if (quantity <= 0 || quantity > products[productIndex].quantityOnPlayer)
            {
                return "Неверное количество.";
            }

            int totalRevenue = (int)(products[productIndex].price * quantity);

            player.AddMoney(totalRevenue);
            products[productIndex].quantityOnMarket += quantity;
            products[productIndex].quantityOnPlayer -= quantity;

            return $"Вы успешно продали {quantity} {products[productIndex].name}(ов) за {totalRevenue} монет.";
        }

        public void ChangePrices()
        {
            for (int i = 0; i < products.Count; i++)
            {
                int priceChange = random.Next(-20, 20);
                int newPrice = Math.Max(1, (int)Math.Round(products[i].price * (1 + (priceChange / 100.0))));

                products[i].price = newPrice;
            }
        }

        public void ChangeQuantities()
        {
            for (int i = 0; i < products.Count; i++)
            {
                int quantityChange = random.Next(-2, 3);
                int newQuantity = Math.Max(0, products[i].quantityOnMarket + quantityChange);
                products[i].quantityOnMarket = newQuantity;
            }
        }

        public void MarketEvent(int randomProductIndex, double priceMultiplier)
        {
            products[randomProductIndex].price = (int)(products[randomProductIndex].price * priceMultiplier);
        }

        public string GetProduct(Player player, int randomProductIndex)
        {
            if (randomProductIndex < 0 || randomProductIndex >= products.Count)
            {
                return "Неверный номер товара.";
            }

            int inventoryCount = 0;
            foreach (var product in products)
            {
                inventoryCount += product.quantityOnPlayer;
            }

            if (inventoryCount >= player.InventoryCapacity)
            {
                return "Ваш инвентарь полон!";
            }
            products[randomProductIndex].quantityOnPlayer += 1;
            return "Вы получили товар!";
        }
    }
}
