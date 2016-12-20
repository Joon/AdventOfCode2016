using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class MoveMaker
    {
        Dictionary<long, int> _processedHashes = new Dictionary<long, int>();
        HashSet<string> _processedHashesString = new HashSet<string>();
        
        public bool HashStrings { get; set; }

        public int CalcMoveDepth(Building startState)
        {            
            // Calculate the top level
            List<BuildingMove> addMovesTo = new List<BuildingMove>();
            CalcAllPossibleValidMoves(startState, addMovesTo, 1);
            List<Task<int>> tasks = new List<Task<int>>();
            Dictionary<int, int> currentProcessingDepth = new Dictionary<int, int>();
            int taskCount = 0;

            bool breaker = false;
            foreach (BuildingMove bm in addMovesTo)
            {
                taskCount++;
                Task<int> calculator = new Task<int>(o =>
                {
                    List<BuildingMove> moves = new List<BuildingMove>();
                    moves.Add(bm);
                    int moveLevel = 1;
                    while (!breaker)
                    {
                        moveLevel++;
                        lock (currentProcessingDepth)
                        {
                            currentProcessingDepth[(int)o] = moveLevel;
                        }

                        Console.WriteLine("Trying to solve task " + o.ToString() + " at level " + moveLevel);

                        List<BuildingMove> leafNodes = new List<BuildingMove>();
                        foreach (BuildingMove move in moves)
                        {
                            CalcAllPossibleValidMoves(move.StateAfterMove, leafNodes, moveLevel);

                            foreach (BuildingMove newMove in leafNodes)
                            {
                                if (newMove.MoveSolvesBuilding)
                                {
                                    lock (currentProcessingDepth)
                                    {
                                        currentProcessingDepth.Remove((int)o);
                                    }
                                    return moveLevel;
                                }
                            }
                        }

                        if (leafNodes.Count == 0)
                        {
                            Console.WriteLine("Out of ideas. Aborting...");
                            lock (currentProcessingDepth)
                            {
                                currentProcessingDepth.Remove((int)o);
                            }
                            return -1;
                        }

                        moves.Clear();
                        moves.AddRange(leafNodes);
                    }
                    lock (currentProcessingDepth)
                    {
                        currentProcessingDepth.Remove((int)o);
                    }
                    return -1;
                }, taskCount);
                calculator.Start();
                tasks.Add(calculator);
            }

            while (true)
            {
                List<int> possibleAnswers = new List<int>();
                foreach (Task<int> t in tasks)
                {
                    if (t.IsCompleted && t.Result > 0)
                        possibleAnswers.Add(t.Result);
                }

                if (possibleAnswers.Count > 0)
                {
                    int bestAnswer = possibleAnswers.Min();
                    lock (currentProcessingDepth)
                    {
                        if (currentProcessingDepth.Count > 0)
                        {
                            if (bestAnswer < currentProcessingDepth.Values.Min())
                            {
                                breaker = true;
                                return bestAnswer;
                            }
                        } else
                        {
                            return bestAnswer;
                        }
                    }
                }
                // Only check for a solution once every half second
                Thread.Sleep(500);
            }
        }

        public void CalcMovesFullState(Building startState, List<BuildingMove> addMovesTo)
        {
            // Calculate the top level
            CalcAllPossibleValidMoves(startState, addMovesTo, 1);
            bool solved = false;
            // Now calculate progressively deeper until the puzzle gets solved somewhere
            while (!solved)
            {
                List<BuildingMove> leafNodes = FindLeafNodes(addMovesTo);
                bool anyChildren = false;
                foreach (BuildingMove move in leafNodes)
                {
                    CalcAllPossibleValidMoves(move.StateAfterMove, move.SubsequentMoves, move.MoveDepth + 1);
                    if (move.SubsequentMoves.Count > 0)
                        anyChildren = true;
                    if (move.CommandTreeSolved())
                        solved = true;
                }

                if (!anyChildren)
                {
                    Console.WriteLine("Ran out of moves. Still not solved.");
                    break;
                }
            }
        }

        private List<BuildingMove> FindLeafNodes(List<BuildingMove> addMovesTo)
        {
            List<BuildingMove> result = new List<BuildingMove>();

            foreach(BuildingMove mv in addMovesTo)
            {
                if (mv.SubsequentMoves.Count == 0)
                    result.Add(mv);
                else
                    result.AddRange(FindLeafNodes(mv.SubsequentMoves));
            }
            return result;
        }

        public void CalcAllPossibleValidMoves(Building startState, List<BuildingMove> addMovesTo, int processingDepth)
        {
            Floor processFloor = startState.Floors[startState.ElevatorOn - 1];
            MakeMicrochipMoves(startState, addMovesTo, processFloor, processingDepth);
            MakeGeneratorOnlyMoves(startState, addMovesTo, processFloor, processingDepth);
        }

        private void MakeGeneratorOnlyMoves(Building startState, List<BuildingMove> result, Floor processFloor, int processingDepth)
        {
            foreach (Generator g1 in processFloor.Generators)
            {
                foreach (Generator g2 in processFloor.Generators)
                {
                    if (g1 == g2)
                    {
                        // Can we go up?
                        if (startState.ElevatorOn < startState.Floors.Count)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn];
                            MakeMoveIfValid(null, null, g1, null, processFloor, endFloor, startState, result, processingDepth);
                        }
                        // Can we go down?
                        if (startState.ElevatorOn > 1)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                            MakeMoveIfValid(null, null, g1, null, processFloor, endFloor, startState, result, processingDepth);
                        }
                    } else
                    {
                        // Can we go up?
                        if (startState.ElevatorOn < startState.Floors.Count)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn];
                            MakeMoveIfValid(null, null, g1, g2, processFloor, endFloor, startState, result, processingDepth);
                        }
                        // Disabled for now: We only want to move one thing down
                        //// Can we go down?
                        //if (startState.ElevatorOn > 1)
                        //{
                        //    Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                        //    MakeMoveIfValid(null, null, g1, g2, processFloor, endFloor, startState, result);
                        //}
                    }
                }
            }
        }

        private void MakeMicrochipMoves(Building startState, List<BuildingMove> result, Floor processFloor,
            int processingDepth)
        {
            foreach (Microchip mc1 in processFloor.MicroChips)
            {
                foreach (Microchip mc2 in processFloor.MicroChips)
                {
                    if (mc1 == mc2)
                    {
                        // Can we go up?
                        if (startState.ElevatorOn < startState.Floors.Count)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn];
                            MakeMoveIfValid(mc1, null, null, null, processFloor, endFloor, startState, result, processingDepth);
                            foreach (Generator g in processFloor.Generators)
                            {
                                MakeMoveIfValid(mc1, null, g, null, processFloor, endFloor, startState, result, processingDepth);
                            }
                        }
                        // Can we go down?
                        if (startState.ElevatorOn > 1)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                            MakeMoveIfValid(mc1, null, null, null, processFloor, endFloor, startState, result, processingDepth);
                            // disabled for now - only take one thing down
                            //foreach (Generator g in processFloor.Generators)
                            //{
                            //    MakeMoveIfValid(mc1, null, g, null, processFloor, endFloor, startState, result);
                            //}
                        }
                    } else
                    {
                        // Can we go up?
                        if (startState.ElevatorOn < startState.Floors.Count)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn];
                            MakeMoveIfValid(mc1, mc2, null, null, processFloor, endFloor, startState, result, processingDepth);
                        }
                        // Disabled for now - only take one thing down
                        //// Can we go down?
                        //if (startState.ElevatorOn > 1)
                        //{
                        //    Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                        //    MakeMoveIfValid(mc1, mc2, null, null, processFloor, endFloor, startState, result);
                        //}
                    }
                }
            }
        }

        private void MakeMoveIfValid(Microchip mc1, Microchip mc2, Generator g1, Generator g2, Floor startFloor,
            Floor endFloor, Building startState, List<BuildingMove> addMoveTo, int processingDepth)
        {
            if (mc1 == null && mc2 == null && g1 == null && g2 == null)
                throw new ArgumentException("Need a generator or a chip in the elevator");
            // If both are supplied, their identifiers have to match else no dice
            if (mc1 != null && g1 != null && mc1.Identifier != g1.Identifier)
                return;
            // We only have a chip so we have to validate it can go to the target floor
            if (mc1 != null && g1 == null)
            {
                if (endFloor.Generators.Count > 0)
                {
                    if (!FloorContainsGenerator(mc1, endFloor))
                        return;
                    if (!FloorContainsGenerator(mc2, endFloor))
                        return;
                }
            }
            // Now we know the move is valid, create a representation of what it would do
            Building moveEffect = startState.Clone();
            Floor newStartFloor = moveEffect.Floors[startFloor.FloorNumber - 1];
            Floor newEndFloor = moveEffect.Floors[endFloor.FloorNumber - 1];
            if (mc1 != null)
            {
                newStartFloor.MicroChips.Remove(mc1);
                newEndFloor.MicroChips.Add(mc1);
            }
            if (mc2 != null)
            {
                newStartFloor.MicroChips.Remove(mc2);
                newEndFloor.MicroChips.Add(mc2);
            }
            if (g1 != null)
            {
                newStartFloor.Generators.Remove(g1);
                newEndFloor.Generators.Add(g1);
            }
            if (g2 != null)
            {
                newStartFloor.Generators.Remove(g2);
                newEndFloor.Generators.Add(g2);
            }
            moveEffect.ElevatorOn = newEndFloor.FloorNumber;

            if (HashStrings)
            {
                // An optmization to stop the same situation from being processed over and over again. Once a
                // particular state has been processed, it isn't ever reprocessed
                long hash = moveEffect.Hash();
                lock (_processedHashes)
                {
                    if (_processedHashes.ContainsKey(hash))
                        return;
                    _processedHashes[hash] = processingDepth;
                }
            } else
            {
                string hashS = moveEffect.HashString();
                lock (_processedHashesString)
                {
                    if (_processedHashesString.Contains(hashS))
                        return;
                    _processedHashesString.Add(hashS);
                }
            }

            // And represent the move
            BuildingMove bm = new BuildingMove();
            bm.StartFloor = newStartFloor;
            bm.EndFloor = newEndFloor;
            bm.StateAfterMove = moveEffect;
            bm.MoveSolvesBuilding = moveEffect.BuildingSolved();
            addMoveTo.Add(bm);
        }

        private static bool FloorContainsGenerator(Microchip mc, Floor endFloor)
        {
            if (mc == null)
                return true;
            bool found = false;
            foreach (Generator gg in endFloor.Generators)
            {
                if (gg.Identifier == mc.Identifier)
                {
                    found = true;
                }
                break;
            }

            return found;
        }
    }
}