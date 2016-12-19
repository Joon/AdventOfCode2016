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
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 1, Identifier = "hydrogen" });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 2, Identifier = "lithium" });
            commandState.Floors.Add(floor1);

            // The second floor contains a hydrogen generator.
            Floor floor2 = new Floor();
            floor2.FloorNumber = 2;
            floor2.Generators.Add(new Generator() { GeneratorNumber = 1, Identifier = "hydrogen" });
            commandState.Floors.Add(floor2);

            //The third floor contains a lithium generator.
            Floor floor3 = new Floor();
            floor3.FloorNumber = 3;
            floor3.Generators.Add(new Generator() { GeneratorNumber = 2, Identifier = "lithium" });
            commandState.Floors.Add(floor3);

            //  The fourth floor contains nothing relevant.             
            Floor floor4 = new Floor();
            floor4.FloorNumber = 4;
            commandState.Floors.Add(floor4);

            commandState.ElevatorOn = 1;

            return commandState;

        }

        public Building BuildingForPuzzleInput()
        {
            Building commandState = new Building();
            Floor floor1 = new Floor();
            floor1.FloorNumber = 1;
            // The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, 
            // a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, 
            // and a cobalt-compatible microchip.
            floor1.Generators.Add(new Generator() { GeneratorNumber = 1, Identifier = "polonium" });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 2, Identifier = "thulium" });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 1, Identifier = "thulium" });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 3, Identifier = "promethium" });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 4, Identifier = "ruthenium" });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 2, Identifier = "ruthenium" });
            floor1.Generators.Add(new Generator() { GeneratorNumber = 5, Identifier = "cobalt" });
            floor1.MicroChips.Add(new Microchip() { MicrochipNumber = 3, Identifier = "cobalt" });
            commandState.Floors.Add(floor1);

            Floor floor2 = new Floor();
            //  The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
            floor2.FloorNumber = 2;
            floor2.MicroChips.Add(new Microchip() { MicrochipNumber = 4, Identifier = "polonium" });
            floor2.MicroChips.Add(new Microchip() { MicrochipNumber = 5, Identifier = "promethium" });
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


    }
}
