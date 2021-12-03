using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace T4
{
    public class Program
    {
        static void Main(string[] args)
        {
            Order order = new Order(1342442, 1200);

            PaymentSystem1 p1 = new PaymentSystem1();
            PaymentSystem2 p2 = new PaymentSystem2();
            PaymentSystem3 p3 = new PaymentSystem3("secret");

            Console.WriteLine(p1.GetPayingLink(order));
            Console.WriteLine(p2.GetPayingLink(order));
            Console.WriteLine(p3.GetPayingLink(order));
        }
    }

    public class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    public interface IPaymentSystem
    {
        public string GetPayingLink(Order order);
    }

    public interface IHasher
    {
        string GetHash(string rootString);
        string ToHexDecimal(byte[] hash);
    }

    public abstract class Hasher : IHasher
    {
        public abstract string GetHash(string rootString);

        public string ToHexDecimal(byte[] hash)
        {
            StringBuilder hexDecimalString = new StringBuilder(hash.Length);
            for (int i = 0; i < hash.Length; i++)
            {
                hexDecimalString.Append(hash[i].ToString("X2"));
            }

            return hexDecimalString.ToString();
        }
    }

    public class MD5Hasher : Hasher
    {
        public override string GetHash(string rootString)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(rootString));

            return base.ToHexDecimal(hash);
        }
    }

    public class SH1Hasher : Hasher
    {
        public override string GetHash(string rootString)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(ASCIIEncoding.ASCII.GetBytes(rootString));

            return base.ToHexDecimal(hash);
        }
    }

    public class PaymentSystem1 : IPaymentSystem
    {
        private string _rootLink;
        private IHasher _hasher;

        public PaymentSystem1()
        {
            _hasher = new MD5Hasher();
            _rootLink = "pay.system1.ru/";
        }

        public string GetPayingLink(Order order)
        {
            string payingLink = _rootLink;
            string hash = _hasher.GetHash(order.Id.ToString());
            payingLink += $"order?amount={order.Amount}RUB&hash={hash}";
            return payingLink;
        }
    }

    public class PaymentSystem2 : IPaymentSystem
    {
        private string _rootLink;
        private IHasher _hasher;

        public PaymentSystem2()
        {
            _hasher = new MD5Hasher();
            _rootLink = "order.system2.ru/";
        }

        public string GetPayingLink(Order order)
        {
            string payingLink = _rootLink;
            string hash = _hasher.GetHash(order.Id.ToString() + order.Amount.ToString());
            payingLink += $"pay?hash={hash}";
            return payingLink;
        }
    }

    public class PaymentSystem3 : IPaymentSystem
    {
        private string _rootLink;
        private string _secretKey;
        private IHasher _hasher;

        public PaymentSystem3(string secretKey)
        {
            _hasher = new SH1Hasher();
            _rootLink = "system3.com/";
            _secretKey = secretKey;
        }

        public string GetPayingLink(Order order)
        {
            string payingLink = _rootLink;
            string hash = _hasher.GetHash(order.Amount.ToString() + order.Id.ToString() + _secretKey);
            payingLink += $"pay?amount={order.Amount}&curency=RUB&hash={hash}";
            return payingLink;
        }
    }
}
