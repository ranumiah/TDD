using NUnit.Framework;

namespace WyCash.Tests
{
    [TestFixture]
    class MultiCurrencyMoney
    {
       [Test]
        public void TestMultiplication()
        {
            var five = new Dollar(5);

            //This test speaks to us more clearly, as if it were an assertion of truth, not a sequence of operations.
            Assert.That(five.Times(2), Is.EqualTo(new Dollar(10)));
            Assert.That(five.Times(3), Is.EqualTo(new Dollar(15)));
        }

        [Test]
        public void TestFrancMultiplication()
        {
            var five = new Franc(5);
            Assert.That(five.Times(2), Is.EqualTo(new Franc(10)));
            Assert.That(five.Times(3), Is.EqualTo(new Franc(15)));
        }

        [Test]
        public void TestEquality()
        {
            var firstFiveDollar = new Dollar(5);
            var secondFiveDollar = new Dollar(5);
            var sixDollar = new Dollar(6);

            Assert.That(firstFiveDollar, Is.EqualTo(secondFiveDollar));
            Assert.That(firstFiveDollar, Is.Not.EqualTo(sixDollar));
        }
    }

    public class Dollar
    {
        private readonly int _amount;

        public Dollar(int amount)
        {
            _amount = amount;
        }

        public Dollar Times(int multiplier)
        {
            return new Dollar(_amount * multiplier);
        }

        public override bool Equals(object obj)
        {
            return ((Dollar) obj)._amount == _amount;
        }
    }

    public class Franc
    {
        private readonly int _amount;

        public Franc(int amount)
        {
            _amount = amount;
        }

        public Franc Times(int multiplier)
        {
            return new Franc(_amount * multiplier);
        }

        public override bool Equals(object obj)
        {
            return ((Franc) obj)._amount == _amount;
        }
    }
}
