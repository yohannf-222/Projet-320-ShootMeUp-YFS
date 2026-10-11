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

        [TestMethod]
        public void Ennemi_DefineIntersections_Test()
        {
            // Arrange
            List<int> axesX = new List<int> { 1, 2, 3 };
            List<int> axesY = new List<int> { 4, 5 };

            Location[,] expectedPoints = new Location[2, 3]
            {
                { new Location(1, 4), new Location(2, 4) , new Location(3, 4)},
                { new Location(1, 5), new Location(2, 5) , new Location(3, 5)}                
            };

            // Act
            Location[,] actualPoints = MathHelpers.DefineIntersections(axesX, axesY);

            // Assert
            for (int i = 0; i < expectedPoints.GetLength(0); i++)
            {
                for (int j = 0; j < expectedPoints.GetLength(1); j++)
                {
                    Assert.AreEqual(expectedPoints[i, j].X, actualPoints[i, j].X);
                    Assert.AreEqual(expectedPoints[i, j].Y, actualPoints[i, j].Y);
                }
            }
        }
    }
}
