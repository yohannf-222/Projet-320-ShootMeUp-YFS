using ShootMeUp;
using ShootMeUp.Helpers;

namespace ShootMeUp
{
    [TestClass]
    public sealed class ShootMeUp_Test
    {
        [TestMethod]
        public void MathHelpers_ClosestValue_Test()
        {
            // Arrange
            int value1 = 7;
            List<int> values1 = new List<int>{ 5, 10, 8, 6 };

            int value2 = 1;
            List<int> values2 = new List<int> { 4, 7, 3, 0 };

            int value3 = 4;
            List<int> values3 = new List<int> { 4, 7, };

            // Act
            int closestValue1 = MathHelpers.ClosestValue(value1, values1);
            int closestValue2 = MathHelpers.ClosestValue(value2, values2);
            int closestValue3 = MathHelpers.ClosestValue(value3, values3);

            // Assert
            Assert.AreEqual(8, closestValue1);
            Assert.AreEqual(0, closestValue2);
            Assert.AreEqual(7, closestValue3);
        }
    }
}
