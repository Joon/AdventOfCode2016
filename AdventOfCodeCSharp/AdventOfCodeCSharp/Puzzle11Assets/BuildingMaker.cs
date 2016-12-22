using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class BuildingMaker
    {

        public Building TestPuzzleInput()
        {
            Building commandState = new Building();
            Floor floor1 = new Floor();
            floor1.FloorNumber = 1;
            //The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 1, Identifier = "hydrogen".GetHashCode() });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 2, Identifier = "lithium".GetHashCode() });
            commandState.Floors.Add(floor1);

            // The second floor contains a hydrogen generator.
            Floor floor2 = new Floor();
            floor2.FloorNumber = 2;
            floor2.Generators.Add(new Generator() { GeneratorNumber = 1, Identifier = "hydrogen".GetHashCode() });
            commandState.Floors.Add(floor2);

            //The third floor contains a lithium generator.
            Floor floor3 = new Floor();
            floor3.FloorNumber = 3;
            floor3.Generators.Add(new Generator() { GeneratorNumber = 2, Identifier = "lithium".GetHashCode() });
            commandState.Floors.Add(floor3);

            //  The fourth floor contains nothing relevant.             
            Floor floor4 = new Floor();
            floor4.FloorNumber = 4;
            commandState.Floors.Add(floor4);

            commandState.ElevatorOn = 1;

            return commandState;

        }

        internal Building SolvedBuilding(Building startState)
        {
            Building result = startState.Clone();

            Floor destFloor = result.Floors[result.Floors.Count - 1];
            foreach(Floor floor in result.Floors)
            {
                if (floor == destFloor)
                    continue;
                foreach(Microchip mc in floor.MicroChips)
                {
                    destFloor.MicroChips.Add(mc);                    
                }
                floor.MicroChips.Clear();
                foreach(Generator g in floor.Generators)
                {
                    destFloor.Generators.Add(g);
                }
                floor.Generators.Clear();
            }
            result.ElevatorOn = result.Floors.Count;
            return result;
        }

        public Building BuildingForPuzzleInput()
        {
            Building commandState = new Building();
            Floor floor1 = new Floor();
            floor1.FloorNumber = 1;
            // The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, 
            // a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, 
            // and a cobalt-compatible microchip.
            floor1.Generators.Add(new Generator() { GeneratorNumber = 1, Identifier = "polonium".GetHashCode() });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 2, Identifier = "thulium".GetHashCode() });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 1, Identifier = "thulium".GetHashCode() });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 3, Identifier = "promethium".GetHashCode() });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 4, Identifier = "ruthenium".GetHashCode() });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 2, Identifier = "ruthenium".GetHashCode() });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 5, Identifier = "cobalt".GetHashCode() });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 3, Identifier = "cobalt".GetHashCode() });
            commandState.Floors.Add(floor1);

            Floor floor2 = new Floor();
            //  The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
            floor2.FloorNumber = 2;
            floor2.MicroChips.Add(new Microchip() { MicrochipNumber = 4, Identifier = "polonium".GetHashCode() });
            floor2.MicroChips.Add(new Microchip() { MicrochipNumber = 5, Identifier = "promethium".GetHashCode() });
            commandState.Floors.Add(floor2);

            Floor floor3 = new Floor();
            //  The third floor contains nothing relevant.
            floor3.FloorNumber = 3;
            commandState.Floors.Add(floor3);

            //  The fourth floor contains nothing relevant.             
            Floor floor4 = new Floor();
            floor4.FloorNumber = 4;
            commandState.Floors.Add(floor4);


            commandState.ElevatorOn = 1;

            return commandState;
        }

        public Building BuildingForPuzzle2Input()
        {
            Building commandState = BuildingForPuzzleInput();
            Floor floor1 = commandState.Floors[0];
            floor1.Generators.Add(new Generator() { GeneratorNumber = 6, Identifier = "elerium".GetHashCode() });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 6, Identifier = "elerium".GetHashCode() });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 7, Identifier = "dilithium".GetHashCode() });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 7, Identifier = "dilithium".GetHashCode() });
            return commandState;
        }
    }
}
