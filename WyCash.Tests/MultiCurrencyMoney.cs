using System.Collections.Generic;
using NUnit.Framework;

namespace WyCash.Tests
{
    [TestFixture]
    class MultiCurrencyMoney
    {
        [Test]
        public void TestMultiplication()
        {
            //To be communicative,
            //var five = Money.Dollar(5);
            var five = Money.Dollar(5);

            //This test speaks to us more clearly, as if it were an assertion of truth, not a sequence of operations.
            Assert.That(five.Times(2), Is.EqualTo(Money.Dollar(10)));
            Assert.That(five.Times(3), Is.EqualTo(Money.Dollar(15)));
        }

        [Test]
        public void TestFrancMultiplication()
        {
            var five = Money.Franc(5);
            Assert.That(five.Times(2), Is.EqualTo(Money.Franc(10)));
            Assert.That(five.Times(3), Is.EqualTo(Money.Franc(15)));
        }

        [Test]
        public void TestEquality()
        {
            Assert.That(Money.Dollar(5), Is.EqualTo(Money.Dollar(5)));
            Assert.That(Money.Franc(5), Is.EqualTo(Money.Franc(5)));

            Assert.That(Money.Dollar(5), Is.Not.EqualTo(Money.Dollar(6)));
            Assert.That(Money.Franc(5), Is.Not.EqualTo(Money.Franc(6)));
            Assert.That(Money.Franc(5), Is.Not.EqualTo(Money.Dollar(5)));
        }

        [Test]
        public void TestCurrency()
        {
            Assert.That("USD", Is.EqualTo(Money.Dollar(1).Currency));
            Assert.That("CHF", Is.EqualTo(Money.Franc(1).Currency));
        }

        //[Test]
        //public void TestDifferentClassEquality()
        //{
        //    // Wrote this test to allow us to move in a direction, which showed a clean refactor oppurtonunity. Delete Both Dollar and Franc classes
        //    // but now we will delete this is the conceptional reasoning of the tests are covered in TestFrancMultiplication()
        //    // This also means TestMultiplication() and TestFrancMultiplication() test the exact same behaviour. Tests might be written differently but the behaviour is the same.
        //    var franc = new Franc(10, "CHF");
        //    var money = new Money(10, "CHF");

        //    Assert.That(money, Is.EqualTo(franc));
        //}

        [Test]
        public void TestPlusReturnsSum()
        {
            var five = Money.Dollar(5);
            IExpression result = five.Plus(five);
            var sum = (Sum) result;
            Assert.That(sum.Augend, Is.EqualTo(five));
            Assert.That(sum.Addend, Is.EqualTo(five));
        }

        [Test]
        public void TestReduceSum()
        {
            IExpression sum = new Sum(Money.Dollar(3), Money.Dollar(4));
            Bank bank = new Bank();
            Money result = bank.Reduce(sum, "USD");

            Assert.That(result.Amount, Is.EqualTo(Money.Dollar(7).Amount));
        }

        [Test]
        public void TestReduceMoney()
        {
            var bank = new Bank();
            var result = bank.Reduce(Money.Dollar(1), "USD");

            Assert.That(result.Amount, Is.EqualTo(Money.Dollar(1).Amount));
        }

        [Test]
        public void TestReduceMoneyDifferentCurrency()
        {
            Bank bank = new Bank();
            bank.AddRate("CHF", "USD", 2);
            Money result = bank.Reduce(Money.Franc(2), "USD");

            Assert.That(result.Amount, Is.EqualTo(Money.Dollar(1).Amount));
        }

        [Test]
        public void TestIdentityRate()
        {
            Assert.That(new Bank().Rate("USD", "USD"), Is.EqualTo(1));
        }

        [Test]
        public void TestMixedAddition()
        {
            IExpression fiveBucks = Money.Dollar(5);
            IExpression tenFrancs = Money.Franc(10);
            var bank = new Bank();
            bank.AddRate("CHF", "USD", 2);

            Money result = bank.Reduce(fiveBucks.Plus(tenFrancs), "USD");

            Assert.That(result.Amount, Is.EqualTo(Money.Dollar(10).Amount));
        }

        [Test]
        public void TestSumPlusMoney()
        {
            IExpression fiveBucks = Money.Dollar(5);
            IExpression tenFrancs = Money.Franc(10);
            Bank bank = new Bank();
            bank.AddRate("CHF", "USD", 2);
            IExpression sum = new Sum(fiveBucks, tenFrancs).Plus(fiveBucks);
            Money result = bank.Reduce(sum, "USD");

            Assert.That(result, Is.EqualTo(Money.Dollar(15)));
        }

        [Test]
        public void TestSumTimes()
        {
            IExpression fiveBucks = Money.Dollar(5);
            IExpression tenFrancs = Money.Franc(10);
            Bank bank = new Bank();
            bank.AddRate("CHF", "USD", 2);
            IExpression sum = new Sum(fiveBucks, tenFrancs).Times(2);
            Money result = bank.Reduce(sum, "USD");
            Assert.That(result, Is.EqualTo(Money.Dollar(20)));
        }

        [Test]
        public void TestPlusSameCurrencyReturnsMoney()
        {
            IExpression sum = Money.Dollar(1).Plus(Money.Dollar(1));
            Assert.IsTrue(sum is Sum);
        }
    }

    public class Money : IExpression
    {
        public Money(int amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }


        public int Amount { get; }
        public string Currency { get; }

        public static Money Dollar(int amount)
        {
            return new Money(amount, "USD");
        }

        public static Money Franc(int amount)
        {
            return new Money(amount, "CHF");
        }

        public IExpression Times(int multiplier)
        {
            return new Money(Amount * multiplier, Currency);
        }

        public IExpression Plus(IExpression addend)
        {
            return new Sum(this, addend);
        }

        public Money Reduce(Bank bank, string to)
        {
            int rate = bank.Rate(Currency, to);
            return new Money(Amount / rate, to);
        }

        public override bool Equals(object obj)
        {
            var money = (Money)obj;

            return money != null && (Amount == money.Amount && Currency.Equals(money.Currency));
        }
    }

    public interface IExpression
    {
        Money Reduce(Bank bank, string to);
        IExpression Plus(IExpression addend);
        IExpression Times(int multiplier);
    }

    public class Bank
    {
        private readonly Dictionary<Pair, int> _rates = new Dictionary<Pair, int>();

        public Money Reduce(IExpression source, string to)
        {
            return source.Reduce(this, to);
        }

        public void AddRate(string from, string to, int rate)
        {
            _rates.Add(new Pair(from, to), rate);
        }

        public int Rate(string from, string to)
        {
            return from.Equals(to) ? 1 : _rates[new Pair(from, to)];
        }
    }

    public class Sum : IExpression
    {
        public Sum(IExpression augend, IExpression addend)
        {
            Augend = augend;
            Addend = addend;
        }

        public IExpression Augend { get; set; }
        public IExpression Addend { get; set; }

        public Money Reduce(Bank bank, string to)
        {
            int amount = Augend.Reduce(bank, to).Amount + Addend.Reduce(bank, to).Amount;
            return new Money(amount, to);
        }

        public IExpression Plus(IExpression addend)
        {
            return new Sum(this, addend);
        }

        public IExpression Times(int multiplier)
        {
            return new Sum(Augend.Times(multiplier), Addend.Times(multiplier));
        }
    }

    public class Pair
    {
        public Pair(string from, string to)
        {
            From = from;
            To = to;
        }

        public string From { get; }
        public string To { get; }

        public override bool Equals(object obj)
        {
            var pair = (Pair)obj;
            return pair != null && From.Equals(pair.From) && To.Equals(pair.To);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From != null ? From.GetHashCode() : 0) * 397) ^ (To != null ? To.GetHashCode() : 0);
            }
        }
    }
}