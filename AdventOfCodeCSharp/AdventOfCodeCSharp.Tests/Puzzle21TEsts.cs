using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCodeCSharp.Tests
{
    [TestClass]
    public class Puzzle21Tests
    {
        [TestMethod]
        public void TestRotateRight()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "rotate right 1 steps";

            string output = toTest.ApplyRotateInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("51234", output);
        }

        [TestMethod]
        public void TestRotateLeft()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "rotate left 1 steps";

            string output = toTest.ApplyRotateInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("23451", output);
        }

        [TestMethod]
        public void TestRotateRightRollOver()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "rotate right 14 steps";

            string output = toTest.ApplyRotateInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("23451", output);
        }

        [TestMethod]
        public void TestRotateLeftRollOver()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "rotate left 14 steps";

            string output = toTest.ApplyRotateInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("51234", output);
        }

        [TestMethod]
        public void TestSwapPosition()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "swap position 0 with position 4";

            string output = toTest.ApplySwapInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("52341", output);
        }
        
        [TestMethod]
        public void TestSwapCharacter()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "swap letter 1 with letter 5";

            string output = toTest.ApplySwapInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("52341", output);
        }


        [TestMethod]
        public void TestReverse()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "reverse positions 2 through 4";

            string output = toTest.ApplyReverseInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("12543", output); 
        }


        [TestMethod]
        public void TestMove()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "move position 1 to position 3";

            string output = toTest.ApplyMoveInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("13425", output);
        }


        [TestMethod]
        public void TestMoveToZero()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "move position 1 to position 0";

            string output = toTest.ApplyMoveInstruction(instruction.Split(' '),
                new char[] { '1', '2', '3', '4', '5' });

            Assert.AreEqual("21345", output);
        }
        
        [TestMethod]
        public void Test6RightRotations()
        {
            Puzzle21 toTest = new Puzzle21();
            string instruction = "rotate based on position of letter d";
            string output = toTest.ApplyRotateInstruction(instruction.Split(' '),
                new char[] { 'e', 'c', 'a', 'b', 'd' });

            Assert.AreEqual("decab", output);
        }
    }
}
