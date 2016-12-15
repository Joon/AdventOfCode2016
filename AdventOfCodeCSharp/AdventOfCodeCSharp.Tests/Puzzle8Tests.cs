using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventOfCodeCSharp.Puzzle8Assets;

namespace AdventOfCodeCSharp.Tests
{
    [TestClass]
    public class Puzzle8Tests
    {
        [TestMethod]
        public void TestRectCommand()
        {
            bool[,] matrix = new bool[4, 3];
            
            RectCommand cmd = new RectCommand("rect 3x2");
            cmd.ApplyCommand(matrix);

            Assert.IsTrue(
                matrix[0, 0] &&
                matrix[1, 0] &&
                matrix[2, 0] &&
                matrix[0, 1] &&
                matrix[1, 1] &&
                matrix[1, 1]);

            Assert.IsFalse(
                matrix[3, 0] ||
                matrix[3, 1] ||
                matrix[0, 2] ||
                matrix[1, 2] ||
                matrix[2, 2]);
        }

        [TestMethod]
        public void TestRotateColumnCommand()
        {
            bool[,] matrix = new bool[4, 3] 
            { { false,
                false,
                true }, { false,
                           false,
                           false }, { false,
                                      false,
                                      false }, { false,
                                                 false,
                                                 false } };



            RotateColumnCommand cmd = new RotateColumnCommand("rotate column x=0 by 2");
            cmd.ApplyCommand(matrix);
            Assert.AreEqual(false, matrix[0, 0]);
            Assert.AreEqual(true, matrix[0, 1]);
            Assert.AreEqual(false, matrix[0, 2]);
        }

        [TestMethod]
        public void TestRotateRowCommand()
        {
            bool[,] matrix = new bool[4, 3]
            { { false,
                false,
                true }, { false,
                           false,
                           false }, { false,
                                      false,
                                      false }, { false,
                                                 false,
                                                 false } };



            RotateRowCommand cmd = new RotateRowCommand("rotate row y=2 by 2");
            cmd.ApplyCommand(matrix);
            Assert.AreEqual(false, matrix[0, 2]);
            Assert.AreEqual(false, matrix[1, 2]);
            Assert.AreEqual(true, matrix[2, 2]);
            Assert.AreEqual(false, matrix[3, 2]);
        }
}
}
