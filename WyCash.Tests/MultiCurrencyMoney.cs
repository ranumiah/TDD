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
            Assert.That("USD", Is.EqualTo(Money.Dollar(1).Currency()));
            Assert.That("CHF", Is.EqualTo(Money.Franc(1).Currency()));
        }
    }

    public abstract class Money
    {
        protected int Amount { get; set; }
        protected string _currency;

        protected Money(int amount, string currency)
        {
            Amount = amount;
            _currency = currency;
        }

        public override bool Equals(object obj)
        {
            return GetType() == obj.GetType()
                && ((Money)obj).Amount == Amount;
        }

        public abstract Money Times(int multiplier);
        public abstract string Currency();

        public static Money Dollar(int amount)
        {
            return new Dollar(amount, "USD");
        }

        public static Money Franc(int amount)
        {
            return new Franc(amount, "CHF");
        }
    }

    public class Dollar : Money
    {
        public Dollar(int amount, string currency) : base(amount, currency)
        {
        }

        public override Money Times(int multiplier)
        {
            return Dollar(Amount * multiplier);
        }

        public override string Currency()
        {
            return _currency;
        }
    }

    public class Franc : Money
    {
        public Franc(int amount, string currency) : base(amount, currency)
        {
        }

        public override Money Times(int multiplier)
        {
            return Franc(Amount * multiplier);
        }

        public override string Currency()
        {
            return _currency;
        }
    }
}
