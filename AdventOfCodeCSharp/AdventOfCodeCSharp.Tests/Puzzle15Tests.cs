using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventOfCodeCSharp.Puzzle15Assets;

namespace AdventOfCodeCSharp.Tests
{
    [TestClass]
    public class Puzzle15Tests
    {
        [TestMethod]
        public void TestDiscRotationNormal()
        {
            Disc underTest = new Disc();
            underTest.AvailablePositions = 12;
            underTest.CurrentPosition = 6;

            underTest.Rotate();

            Assert.AreEqual(7, underTest.CurrentPosition);
        }

        [TestMethod]
        public void TestDiscRotationRollOver()
        {
            Disc underTest = new Disc();
            underTest.AvailablePositions = 12;
            underTest.CurrentPosition = 11;

            underTest.Rotate();

            Assert.AreEqual(0, underTest.CurrentPosition);
        }

        [TestMethod]
        public void TestDiscProjectionNormal()
        {
            Disc underTest = new Disc();
            underTest.AvailablePositions = 12;
            underTest.CurrentPosition = 1;

            int result = underTest.ProjectMovement(10);

            Assert.AreEqual(11, result);
        }

        [TestMethod]
        public void TestDiscProjectionRollOver()
        {
            Disc underTest = new Disc();
            underTest.AvailablePositions = 12;
            underTest.CurrentPosition = 10;

            int result = underTest.ProjectMovement(2);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestDiscProjectionRollOverByFar()
        {
            Disc underTest = new Disc();
            underTest.AvailablePositions = 12;
            underTest.CurrentPosition = 10;

            int result = underTest.ProjectMovement(24);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestDiscProjectionRollOverByFurther()
        {
            Disc underTest = new Disc();
            underTest.AvailablePositions = 12;
            underTest.CurrentPosition = 10;

            int result = underTest.ProjectMovement(26);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestSculptureRotation()
        {
            KineticSculpture sculpture = new KineticSculpture();
            sculpture.InitialiseDisc(1, 5, 4);
            sculpture.InitialiseDisc(2, 2, 1);
            sculpture.Rotate();

            Assert.AreEqual(0, sculpture.Discs[0].CurrentPosition);
            Assert.AreEqual(0, sculpture.Discs[1].CurrentPosition);
        }

        [TestMethod]
        public void TestSculptureSolution()
        {
            KineticSculpture sculpture = new KineticSculpture();
            sculpture.InitialiseDisc(1, 5, 4);
            sculpture.InitialiseDisc(2, 2, 1);

            for(int i = 0; i < 5; i++)
            {
                if (sculpture.CurrentPositionSolvesPuzzle())
                    throw new ApplicationException("Should not solve before 5");
                sculpture.Rotate();
            }

            Assert.IsTrue(sculpture.CurrentPositionSolvesPuzzle());
        }
    }
}
