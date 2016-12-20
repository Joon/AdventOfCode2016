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
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 1, Identifier = "Whocares?" });
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 2, Identifier = "Whocares?" });
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 3, Identifier = "Whocares?" });
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 4, Identifier = "Whocares?" });
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 5, Identifier = "Whocares?" });
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 6, Identifier = "Whocares?" });
            testBed.Floors[0].MicroChips.Add(new Microchip() { MicrochipNumber = 7, Identifier = "Whocares?" });
            testBed.Floors[0].Generators.Add(new Generator() { GeneratorNumber = 1, Identifier = "Whocares?" });
            testBed.Floors[0].Generators.Add(new Generator() { GeneratorNumber = 2, Identifier = "Whocares?" });
            testBed.Floors[0].Generators.Add(new Generator() { GeneratorNumber = 3, Identifier = "Whocares?" });
            testBed.Floors[0].Generators.Add(new Generator() { GeneratorNumber = 4, Identifier = "Whocares?" });
            testBed.Floors[0].Generators.Add(new Generator() { GeneratorNumber = 5, Identifier = "Whocares?" });
            testBed.Floors[0].Generators.Add(new Generator() { GeneratorNumber = 6, Identifier = "Whocares?" });
            testBed.Floors[3].Generators.Add(new Generator() { GeneratorNumber = 7, Identifier = "Whocares?" });

            long hash = testBed.Hash();

            Assert.AreEqual("0100000000000000100000000000000000000000000000000011111101111111", Convert.ToString(hash, 2).PadLeft(64, '0'));
        }

    }
}
