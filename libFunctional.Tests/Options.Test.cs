using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace libFunctional.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void testIsDefined()
        {
            var something = Option.Some(42);
            Assert.IsTrue(something.IsDefined);

            var nothing = Option.None();
            Assert.IsFalse(nothing.IsDefined);
        }

        [Test]
        public void testIsEmpty()
        {
            var something = Option.Some(42);
            Assert.IsFalse(something.IsEmpty);

            var nothing = Option.None();
            Assert.IsTrue(nothing.IsEmpty);
        }

        [Test]
        public void testFlatmap()
        {
            var something = Option.Some(42);
            var result = something.flatMap(s => s * 2);

            Assert.AreEqual(result, 84);
        }

        [Test]
        public void testMap()
        {
            var something = Option.Some(42);
            var result = something.map(s => s * 2);

            Assert.IsInstanceOf<Option<int>>(result);
            Assert.AreEqual(result.flatten, 84);
        }

        [Test]
        public void testFlatten()
        {
            var something = Option.Some(42);

            Assert.IsNotNull(something);

            Assert.AreEqual(42, something.flatten);
        }

        [Test]
        public void testForEach()
        {
            var something = Option.Some(42);

            var i = 24;
            something.forEach(s => i = s);

            Assert.AreEqual(42, i);
        }

        [Test]
        public void testForAll()
        {
            var something = Option.Some(42);

            var result = something.forAll(s => s == 42);
            Assert.IsTrue(result);

            result = something.forAll(s => s == 24);
            Assert.IsFalse(result);
        }

        [Test]
        public void testGetOrElse()
        {
            var something = Option.Some(42);

            var result = something.getOrElse(() => 24);
            Assert.AreEqual(42, result);
            Assert.AreNotEqual(24, result);

            var nothing = Option.None();
            result = (int)nothing.getOrElse(() => 24);
            Assert.AreEqual(24, result);
            Assert.AreNotEqual(42, result);
        }

        [Test]
        public void testValueOr()
        {
            var something = Option.Some(42);

            var result = something.valueOr(() => 24);
            Assert.AreEqual(42, result);
            Assert.AreNotEqual(24, result);

            var nothing = Option.None();
            result = (int)nothing.getOrElse(() => 24);
            Assert.AreEqual(24, result);
        }

        [Test]
        public void testOrElse()
        {
            /**
             * Some 
             */
            var something = Option.Some(42);
            var result = something.orElse(() => Option.Some(24));

            Assert.IsInstanceOf<Option<int>>(result);

            Assert.IsTrue(result.IsDefined);
            Assert.AreEqual(42, result.flatten);
            Assert.AreNotEqual(24, result.flatten);

            /**
             * None
             */

            var nothing = Option.None();
            var otherResult = nothing.orElse(() => Option.Some((object)42));

            Assert.IsInstanceOf<Option<object>>(otherResult);

            Assert.IsTrue(otherResult.IsDefined);
            Assert.AreEqual(42, (int)otherResult.flatten);
            Assert.AreNotEqual(24, (int)otherResult.flatten);
        }

        [Test]
        public void testNoneCompanionObject()
        {
            var nothing = Option.None();

            Assert.IsTrue(nothing.IsEmpty);
            Assert.IsFalse(nothing.IsDefined);
        }

        [Test]
        public void testSomeCompanionObject()
        {
            var something = Option.Some("Hello World");

            Assert.IsTrue(something.IsDefined);
            Assert.IsFalse(something.IsEmpty);
        }

        [Test]
        public void testCanIterateOverOption()
        {
            var something = Option.Some(42);

            foreach (var some in something)
            {
                Assert.AreEqual(42, some);
            }
        }

        [Test]
        public void testWhere()
        {
            var something = Option.Some(42);
            var result = something.where(i => i == 42);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Option<int>>(result);
            Assert.AreEqual(42, result.flatten);

            var otherResult = something.where(i => i == 24);
            Assert.IsNotNull(otherResult);
            Assert.IsInstanceOf<Option<int>>(result);
            Assert.IsTrue(otherResult.IsEmpty);
        }

        [Test]
        public void testCompanionObjects()
        {
            var something = Option.Some(42);

            Assert.IsNotNull(something);
        }
    }
}
