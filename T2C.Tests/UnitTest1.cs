using NUnit.Framework;
using System;
using T2;

namespace T2C.Tests
{
    public class Tests
    {
        private Warehouse _warehouse;
        private Shop _shop;

        [SetUp]
        public void Setup()
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            _warehouse = new Warehouse();

            _shop = new Shop(_warehouse);

            _warehouse.Deliver(iPhone12, 10);
            _warehouse.Deliver(iPhone11, 1);
        }

        [Test]
        public void Test1()
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");
            Cart cart = _shop.Cart();
            cart.Add(iPhone12, 4);
            Assert.Throws<System.InvalidOperationException>(() => cart.Add(iPhone11, 3));
        }

        [Test]
        public void Test2()
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");
            Cart cart = _shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 1);
            string paylink = cart.Order().Paylink;
            Assert.Throws<System.InvalidOperationException>(() => cart.Add(iPhone12, 9));
        }
    }
}