using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Options.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void testCompanionObjects() {
            var something = Option.Some(42);

            Assert.IsNotNull(something);

            Assert.IsTrue(something.IsDefined);
            Assert.IsFalse(something.isEmpty);

            var result = something.flatten();
            Assert.AreEqual(result, 42);
        }
    }
}
