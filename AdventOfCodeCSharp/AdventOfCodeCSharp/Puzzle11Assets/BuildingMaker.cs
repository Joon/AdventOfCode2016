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
            floor1.AddMicrochip(1); // Identifier = "hydrogen"
            floor1.AddMicrochip(2); // Identifier = "lithium"
            commandState.Floors.Add(floor1);

            // The second floor contains a hydrogen generator.
            Floor floor2 = new Floor();
            floor2.FloorNumber = 2;
            floor2.AddGenerator(1); // Identifier = "hydrogen"
            commandState.Floors.Add(floor2);

            //The third floor contains a lithium generator.
            Floor floor3 = new Floor();
            floor3.FloorNumber = 3;
            floor3.AddGenerator(2); // Identifier = "lithium"
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
                foreach(int mc in floor.MicroChips)
                {
                    destFloor.AddMicrochip(mc);
                    floor.RemoveMicrochip(mc);
                }
                foreach(int g in floor.Generators)
                {
                    destFloor.AddGenerator(g);
                    floor.RemoveGenerator(g);
                }                
            }
            result.ElevatorOn = result.Floors.Count;
            return result;
        }

        public Building BuildingForPuzzleInput()
        {
            Building commandState = new Building();
            Floor floor1 = new Floor();
            floor1.FloorNumber = 1;

            int polonium = 1;
            int thulium = 2;
            int promethium = 3;
            int ruthenium = 4;
            int cobalt = 5;

            // The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, 
            // a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, 
            // and a cobalt-compatible microchip.
            floor1.AddGenerator(polonium);
            floor1.AddGenerator(thulium);
            floor1.AddMicrochip(thulium);
            floor1.AddGenerator(promethium);
            floor1.AddGenerator(ruthenium);
            floor1.AddMicrochip(ruthenium);
            floor1.AddGenerator(cobalt);
            floor1.AddMicrochip(cobalt);
            commandState.Floors.Add(floor1);

            Floor floor2 = new Floor();
            //  The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
            floor2.FloorNumber = 2;
            floor2.AddMicrochip(polonium);
            floor2.AddMicrochip(promethium);
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
            int elerium = 6;
            int dilithium = 7;
            Building commandState = BuildingForPuzzleInput();
            Floor floor1 = commandState.Floors[0];
            floor1.AddGenerator(elerium);
            floor1.AddMicrochip(elerium);
            floor1.AddGenerator(dilithium);
            floor1.AddMicrochip(dilithium);
            return commandState;
        }
    }
}
