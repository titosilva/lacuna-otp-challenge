using Lacuna.Shared.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lacuna.Tests
{
    [TestClass]
    public class TestBinary
    {
        [TestMethod]
        [TestCategory("Shared.Entities")]
        [DataTestMethod]
        [DataRow(new byte[] { 0, 2, 4, 6, 3, 2 }, new byte[] { 2, 4, 6 }, 1)]
        [DataRow(new byte[] { 0, 2, 4, 6, 3, 2 }, new byte[] { 4, 6, 3 }, 2)]
        [DataRow(new byte[] { 0, 2, 4, 6, 3, 2 }, new byte[] { 3, 2 }, 4)]
        [DataRow(new byte[] { 2, 1, 2 }, new byte[] { 2, 1 }, 0)]
        [DataRow(new byte[] { 2, 1, 2 }, new byte[] { 1, 2 }, 1)]
        [DataRow(new byte[] { 2, 1, 2 }, new byte[] { 1 }, 1)]
        public void ShouldReturnIndex_WhenMatchExists(byte[] binaryBytes,
                                                      byte[] sequenceBytes,
                                                      int expectedMatch)
        {
            var bin = new Binary(binaryBytes);
            var seq = new Binary(sequenceBytes);

            var match = bin.Match(seq);

            Assert.AreEqual(expectedMatch, match);
        }

        [TestMethod]
        [TestCategory("Shared.Entities")]
        [DataTestMethod]
        [DataRow(new byte[] { 0, 2, 4, 6, 3, 2 }, new byte[] { 2, 4, 5 })]
        [DataRow(new byte[] { 0, 2, 4, 6, 3, 2 }, new byte[] { 4, 6, 2 })]
        [DataRow(new byte[] { 0, 2, 4, 6, 3, 2 }, new byte[] { 3, 4 })]
        [DataRow(new byte[] { 2, 1, 2 }, new byte[] { 2, 2 })]
        [DataRow(new byte[] { 2, 1, 2 }, new byte[] { 3 })]
        public void ShouldReturnNegative1_WhenMatchDoesNotExist(byte[] binaryBytes,
                                                               byte[] sequenceBytes)
        {
            var bin = new Binary(binaryBytes);
            var seq = new Binary(sequenceBytes);

            var match = bin.Match(seq);

            Assert.AreEqual(-1, match);
        }
    }
}
