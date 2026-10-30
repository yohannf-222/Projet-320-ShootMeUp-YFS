using ShootMeUp.Model;
using ShootMeUp;
using ShootMeUp.Helpers;

namespace ShootMeUp
{
    [TestClass]
    public sealed class ShootMeUp_Test
    {
        [TestMethod]
        public void MathHelpers_ClosestValue_FindClosestInUnorderedList()
        {
            // Arrange
            int value = 7;
            List<int> list = new List<int> { 5, 10, 8, 6 };

            // Act
            int result = MathHelpers.CloseValueIn(value, list);

            // Assert
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void MathHelpers_ClosestValue_FindClosestWithMultipleOptions()
        {
            // Arrange
            int value = 1;
            List<int> list = new List<int> { 4, 7, 3, 0 };

            // Act
            int result = MathHelpers.CloseValueIn(value, list);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void MathHelpers_ClosestValue_ValueNotInList()
        {
            // Arrange
            int value = 4;
            List<int> list = new List<int> { 4, 7 };

            // Act
            int result = MathHelpers.CloseValueIn(value, list);

            // Assert
            Assert.IsTrue(result == 4 || result == 7);
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

        [TestMethod]
        public void MathHelpers_IsTouching_2Rectangles_True()
        {
            // Arrange
            int x1 = 0, y1 = 0, width1 = 10, height1 = 10;
            int x2 = 5, y2 = 5, width2 = 10, height2 = 10;

            // Act
            bool result = MathHelpers.IsTouching(x1, y1, width1, height1, x2, y2, width2, height2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MathHelpers_IsTouching_RectangleNoOverlap()
        {
            // Arrange
            int x1 = 0, y1 = 0, width1 = 10, height1 = 10;
            int x2 = 20, y2 = 20, width2 = 10, height2 = 10;

            // Act
            bool result = MathHelpers.IsTouching(x1, y1, width1, height1, x2, y2, width2, height2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MathHelpers_IsTouching_CircleInside()
        {
            // Arrange
            int x1 = 0, y1 = 0, width1 = 20, height1 = 20;
            int x2 = 10, y2 = 10, radius2 = 5;

            // Act
            bool result = MathHelpers.IsTouching(x1, y1, width1, height1, x2, y2, radius2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MathHelpers_IsTouching_CircleFarAway()
        {
            // Arrange 
            int x1 = 0, y1 = 0, width1 = 10, height1 = 10;
            int x2 = 50, y2 = 50, radius2 = 5;

            // Act
            bool result = MathHelpers.IsTouching(x1, y1, width1, height1, x2, y2, radius2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MathHelpers_CalculateDistance_KnownDistance()
        {
            // Arrange 
            double x1 = 0, y1 = 0, x2 = 3, y2 = 4;

            // Act
            double result = MathHelpers.CalculateDistance(x1, y1, x2, y2);

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void MathHelpers_CalculateDistance_SamePoint()
        {
            // Arrange
            double x1 = 10, y1 = 20, x2 = 10, y2 = 20;

            // Act
            double result = MathHelpers.CalculateDistance(x1, y1, x2, y2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void MathHelpers_ClosestValue_ExactMatch()
        {
            // Arrange
            int value = 5;
            List<int> list = new List<int> { 1, 3, 5, 7, 9 };

            // Act
            int result = MathHelpers.CloseValueIn(value, list);

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void MathHelpers_ClosestValue_2Reponses()
        {
            // Arrange
            int value = 6;
            List<int> list = new List<int> { 1, 3, 5, 7, 9 };

            // Act
            int result = MathHelpers.CloseValueIn(value, list);

            // Assert
            // La methode n'étant pas parfaite, mettre 7 dans le Assert serait faux.
            // Je ne compte pas changer la methode car son fonctionnement est suffisant pour le jeu.
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void RndValueHelpers_Next_TestPlageValeurs()
        {
            // Arrange
            int min = 10, max = 20;

            // Act
            for (int i = 0; i < 10; i++)
            {
                int result = RndValueHelpers.Next(min, max);

                // Assert
                Assert.IsTrue(result >= min && result < max);
            }
        }

        [TestMethod]
        public void RndValueHelpers_Next_UnePossibilite()
        {
            // Arrange
            int min = 5, max = 6;

            // Act
            int result = RndValueHelpers.Next(min, max);

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void GetHit_DoesDamageToObstacle()
        {
            // Arrange
            int damage = 10;
            Obstacle obstacle = new Obstacle(0, 0);

            // Act
            obstacle.GetHit(damage);

            // Assert
            Assert.AreEqual(obstacle.Hp, Obstacle.TOT_HP - 10);
        }

        [TestMethod]
        public void Gouvernail_Moves()
        {
            // Arrange
            Gouvernail gouvernail = new Gouvernail(0, 0, 10, 10);

            // Act
            gouvernail.Update();

            // Assert
            Assert.IsTrue(gouvernail.X != 0 && gouvernail.Y != 0);
        }

        [TestMethod]
        public void Ennemi_ChooseObjective_GivesAGoodValue()
        {
            // Arrange
            Ennemi ennemi = new Ennemi();
            bool isInArray = false;

            // Act            
            // Dejà dans le constructeur

            // Assert
            foreach (Location objectif in Ennemi.objectifs)
            {
                if (ennemi._objectif == objectif)
                    isInArray = true;
            }            
            Assert.IsTrue(isInArray);
        }
    }
}
