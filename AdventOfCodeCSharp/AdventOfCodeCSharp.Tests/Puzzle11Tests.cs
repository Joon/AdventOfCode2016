using AdventOfCodeCSharp.Puzzle11Assets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Tests
{
    [TestClass]
    public class Puzzle11Tests
    {

        [TestMethod]
        public void TestBuildingHashing()
        {
            Building testBed = new Building();
            testBed.Floors.Add(new Floor() { FloorNumber = 1 });
            testBed.Floors.Add(new Floor() { FloorNumber = 2 });
            testBed.Floors.Add(new Floor() { FloorNumber = 3 });
            testBed.Floors.Add(new Floor() { FloorNumber = 4 });
            // We hash the floors using two bytes per floor (one for generators, one for microchips) with one of the generator bits used
            // to store the elevator present flag
            // 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
            // |             f4               |               f3              |              f2                |              f1
            // e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m   
            testBed.ElevatorOn = 3;
            testBed.Floors[0].AddMicrochip(1);
            testBed.Floors[0].AddMicrochip(2);
            testBed.Floors[0].AddMicrochip(3);
            testBed.Floors[0].AddMicrochip(4);
            testBed.Floors[0].AddMicrochip(5);
            testBed.Floors[0].AddMicrochip(6);
            testBed.Floors[0].AddMicrochip(7);
            testBed.Floors[0].AddGenerator(1);
            testBed.Floors[0].AddGenerator(2);
            testBed.Floors[0].AddGenerator(3);
            testBed.Floors[0].AddGenerator(4);
            testBed.Floors[0].AddGenerator(5);
            testBed.Floors[0].AddGenerator(6);
            testBed.Floors[3].AddGenerator(7);

            long hash = testBed.Hash();

            Assert.AreEqual("0100000000000000100000000000000000000000000000000011111101111111", Convert.ToString(hash, 2).PadLeft(64, '0'));
        }

        [TestMethod]
        public void MoveGenerator()
        {
            Building testBed = new Building();
            testBed.Floors.Add(new Floor() { FloorNumber = 1 });
            testBed.Floors.Add(new Floor() { FloorNumber = 2 });
            testBed.Floors.Add(new Floor() { FloorNumber = 3 });
            testBed.Floors.Add(new Floor() { FloorNumber = 4 });
            // We hash the floors using two bytes per floor (one for generators, one for microchips) with one of the generator bits used
            // to store the elevator present flag
            // 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
            // |             f4               |               f3              |              f2                |              f1
            // e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m   
            testBed.ElevatorOn = 3;
            testBed.Floors[0].AddMicrochip(1);
            testBed.Floors[0].AddMicrochip(2);
            testBed.Floors[0].AddMicrochip(3);
            testBed.Floors[0].AddMicrochip(4);
            testBed.Floors[0].AddMicrochip(5);
            testBed.Floors[0].AddMicrochip(6);
            testBed.Floors[0].AddMicrochip(7);
            testBed.Floors[0].AddGenerator(1);
            testBed.Floors[0].AddGenerator(2);
            testBed.Floors[0].AddGenerator(3);
            testBed.Floors[0].AddGenerator(4);
            testBed.Floors[0].AddGenerator(5);
            testBed.Floors[0].AddGenerator(6);
            testBed.Floors[3].AddGenerator(7);

            testBed.Floors[3].RemoveGenerator(7);
            testBed.Floors[2].AddGenerator(7);

            long hash = testBed.Hash();

            Assert.AreEqual("0000000000000000110000000000000000000000000000000011111101111111", Convert.ToString(hash, 2).PadLeft(64, '0'));
        }

        [TestMethod]
        public void RetrieveMicrochips()
        {
            Building testBed = new Building();
            testBed.Floors.Add(new Floor() { FloorNumber = 1 });
            testBed.Floors.Add(new Floor() { FloorNumber = 2 });
            testBed.Floors.Add(new Floor() { FloorNumber = 3 });
            testBed.Floors.Add(new Floor() { FloorNumber = 4 });
            // We hash the floors using two bytes per floor (one for generators, one for microchips) with one of the generator bits used
            // to store the elevator present flag
            // 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
            // |             f4               |               f3              |              f2                |              f1
            // e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m   
            testBed.ElevatorOn = 3;
            testBed.Floors[2].AddMicrochip(1);
            testBed.Floors[0].AddMicrochip(2);
            testBed.Floors[0].AddMicrochip(3);
            testBed.Floors[0].AddMicrochip(4);
            testBed.Floors[0].AddMicrochip(5);
            testBed.Floors[0].AddMicrochip(6);
            testBed.Floors[1].AddMicrochip(7);
            testBed.Floors[0].AddGenerator(1);
            testBed.Floors[0].AddGenerator(2);
            testBed.Floors[0].AddGenerator(3);
            testBed.Floors[0].AddGenerator(4);
            testBed.Floors[0].AddGenerator(5);
            testBed.Floors[0].AddGenerator(6);
            testBed.Floors[3].AddGenerator(7);
            
            IEnumerable<int> chips = testBed.Floors[3].MicroChips;
            Assert.AreEqual(0, chips.Count());
            chips = testBed.Floors[0].MicroChips;
            Assert.AreEqual(5, chips.Count());
            Assert.AreEqual(7, testBed.Floors[1].MicroChips.First());
            Assert.AreEqual(1, testBed.Floors[2].MicroChips.First());
        }

        [TestMethod]
        public void MoveMicrochip()
        {
            Building testBed = new Building();
            testBed.Floors.Add(new Floor() { FloorNumber = 1 });
            testBed.Floors.Add(new Floor() { FloorNumber = 2 });
            testBed.Floors.Add(new Floor() { FloorNumber = 3 });
            testBed.Floors.Add(new Floor() { FloorNumber = 4 });
            // We hash the floors using two bytes per floor (one for generators, one for microchips) with one of the generator bits used
            // to store the elevator present flag
            // 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
            // |             f4               |               f3              |              f2                |              f1
            // e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m e g g g g g g g m m m m m m m m   
            testBed.ElevatorOn = 3;
            testBed.Floors[0].AddMicrochip(1);
            testBed.Floors[0].AddMicrochip(2);
            testBed.Floors[0].AddMicrochip(3);
            testBed.Floors[0].AddMicrochip(4);
            testBed.Floors[0].AddMicrochip(5);
            testBed.Floors[0].AddMicrochip(6);
            testBed.Floors[0].AddMicrochip(7);
            testBed.Floors[0].AddGenerator(1);
            testBed.Floors[0].AddGenerator(2);
            testBed.Floors[0].AddGenerator(3);
            testBed.Floors[0].AddGenerator(4);
            testBed.Floors[0].AddGenerator(5);
            testBed.Floors[0].AddGenerator(6);
            testBed.Floors[3].AddGenerator(7);

            testBed.Floors[0].RemoveMicrochip(6);
            testBed.Floors[1].AddMicrochip(6);

            long hash = testBed.Hash();

            Assert.AreEqual("0100000000000000100000000000000000000000001000000011111101011111", Convert.ToString(hash, 2).PadLeft(64, '0'));
        }



    }
}
