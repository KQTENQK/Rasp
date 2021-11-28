using System;
using System.Collections.Generic;
using System.Linq;

namespace T2
{
    class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Deliver(iPhone12, 10);
            warehouse.Deliver(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public interface IReadOnlyGood
    {
        string Name { get; }
    }

    public class Good : IReadOnlyGood
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            Name = name;
        }
    }

    public class Warehouse
    {
        private List<Good> _goodsInStock = new List<Good>();
        private List<IReadOnlyGood> _reservedGoods = new List<IReadOnlyGood>();

        public IReadOnlyList<IReadOnlyGood> GoodsInStock => _goodsInStock;

        public void Deliver(Good good, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _goodsInStock.Add(good);
            }
        }

        public void Reserve(IReadOnlyList<IReadOnlyGood> orderedGoods)
        {
            _reservedGoods.AddRange(orderedGoods);

            foreach (IReadOnlyGood good in orderedGoods)
            {
                Good goodToReserve = _goodsInStock.Where(p => p.Name == good.Name).FirstOrDefault();

                if (goodToReserve != null)
                    _goodsInStock.Remove(goodToReserve);
            }
        }
    }

    public class Shop
    {
        private Warehouse _connectedWarehouse;

        public Shop(Warehouse warehouse)
        {
            _connectedWarehouse = warehouse;
        }

        public int AmountInStock(IReadOnlyGood good)
        {
            return _connectedWarehouse.GoodsInStock.Where(p => p.Name == good.Name).Count();
        }

        public void Reserve(IReadOnlyList<IReadOnlyGood> orderedGoods)
        {
            _connectedWarehouse.Reserve(orderedGoods);
        }

        public Cart Cart()
        {
            return new Cart(this);
        }
    }

    public class Cart
    {
        private Shop _connectedShop;
        private List<IReadOnlyGood> _goodsInCart;

        public Cart(Shop shop)
        {
            _connectedShop = shop;
            _goodsInCart = new List<IReadOnlyGood>();
        }

        public IReadOnlyList<IReadOnlyGood> GoodsInCart => _goodsInCart;

        public int AmountInCart(IReadOnlyGood good)
        {
            return _goodsInCart.Where(p => p.Name == good.Name).Count();
        }

        public void Add(IReadOnlyGood good, int count)
        {
            if (AmountInCart(good) + count > _connectedShop.AmountInStock(good))
                throw new InvalidOperationException("Out of stock");

            for (int i = 0; i < count; i++)
            {
                _goodsInCart.Add(good);
            }
        }

        public Order Order()
        {
            _connectedShop.Reserve(GoodsInCart);
            return new Order(Guid.NewGuid().ToString());
        }
    }

    public class Order
    {
        private string _paylink;

        public Order(string paylink)
        {
            _paylink = paylink;
        }

        public string Paylink => _paylink;
    }
}
