using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp.Puzzle11Assets
{
    public class MoveMaker
    {
        Dictionary<long, int> _processedHashes = new Dictionary<long, int>();
        Dictionary<string, int> _processedHashesString = new Dictionary<string, int>();
        
        public bool HashStrings { get; set; }
        private int _consoleY;
        public int CalcMoveDepth(Building startState, Building desiredEndState)
        {
            Console.CursorVisible = false;
            _consoleY = Console.CursorTop;
            Task<int> solveFromBottom = new Task<int>(() =>
            {
                return SolveBuildingState(startState, true);
            });

            Task<int> solveFromTop = new Task<int>(() =>
            {
                return SolveBuildingState(desiredEndState, false);
            });

            solveFromBottom.Start();
            solveFromTop.Start();

            solveFromBottom.Wait();
            solveFromTop.Wait();

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, _consoleY + _tracking.Count + 2);

            return Math.Min(solveFromBottom.Result, solveFromTop.Result); 
        }

        private int _currentBestCandidate = int.MaxValue;

        private Dictionary<int, long> _tracking = new Dictionary<int, long>();
        private void UpdateProcessingTracking(int level, int amountToAdd)
        {
            lock (_tracking)
            {
                if (_tracking.ContainsKey(level))
                    _tracking[level] = _tracking[level] + amountToAdd;
                else
                    _tracking[level] = amountToAdd;
            }
        }

        private void DisplayTracking()
        {
            int deepestFromTop = 0;
            int deepestFromBottom = 0;
            long widest = 0;

            lock (_tracking)
            {
                deepestFromTop = _tracking.Keys.Max();
                deepestFromBottom = _tracking.Keys.Min();
                widest = _tracking.Values.Max();
            }
            long divider = widest / 40;
            for (int i = deepestFromBottom; i <= deepestFromTop; i++)
            {
                long dispValue;
                lock (_tracking)
                {
                    if (i == 0)
                        dispValue = 0;
                    else
                        dispValue = _tracking[i];
                }
                int consoleHeightAdjuster = 0;
                if (i >= 0)
                    consoleHeightAdjuster = deepestFromTop - i;
                if (i < 0)
                    consoleHeightAdjuster = deepestFromTop + Math.Abs(i);
                int consoleY = _consoleY + consoleHeightAdjuster;
                lock (consoleAcces)
                {
                    Console.SetCursorPosition(0, consoleY);
                    Console.Write(i.ToString().PadRight(5));
                    Console.SetCursorPosition(6, consoleY);
                    Console.Write("".PadRight(50));
                    Console.SetCursorPosition(6, consoleY);
                    Console.Write(dispValue.ToString().PadRight((int)(dispValue / (divider > 0 ? divider : 1)), '#'));
                }
            }
            lock (consoleAcces)
            {
                int height = 0;
                long totalMoves = 0;
                lock (_tracking)
                {
                    height = _tracking.Count + 1;
                    totalMoves = _tracking.Values.Sum();
                }
                Console.SetCursorPosition(0, _consoleY + height);
                Console.WriteLine("Total Moves: " + totalMoves);
            }
        }

        private object consoleAcces = new object();

        private int SolveBuildingState(Building startState, bool moveUpBias)
        {
            // Calculate the top level
            List<BuildingMove> addMovesTo = new List<BuildingMove>();
            int directionMultiplier = moveUpBias ? 1 : -1;

            CalcAllPossibleValidMoves(startState, addMovesTo, directionMultiplier, moveUpBias);
            if (addMovesTo.Count == 0)
            {
                Debug.WriteLine("Moving " + (moveUpBias ? " up " : "down") + "found no possible moves");
                return 0;
            }
            UpdateProcessingTracking(directionMultiplier, addMovesTo.Count);
            
            List<Task<BuildingMove>> tasks = new List<Task<BuildingMove>>();
            Dictionary<int, int> currentProcessingDepth = new Dictionary<int, int>();
            int taskCount = 0;

            bool breaker = false;
            foreach (BuildingMove bm in addMovesTo)
            {
                taskCount++;
                Task<BuildingMove> calculator = new Task<BuildingMove>(o =>
                {
                    List<BuildingMove> moves = new List<BuildingMove>();
                    moves.Add(bm);
                    int moveLevel = directionMultiplier;
                    while (!breaker)
                    {
                        moveLevel = moveLevel + directionMultiplier;
                        lock (currentProcessingDepth)
                        {
                            currentProcessingDepth[(int)o] = Math.Abs(moveLevel);
                        }
                        if (Math.Abs(moveLevel) > _currentBestCandidate)
                        {
                            Debug.WriteLine((moveUpBias ? "U " : "B ") + "Task " + o.ToString() + 
                                " is now deeper than current best answer " + _currentBestCandidate + 
                                ". Aborting...");
                            lock (currentProcessingDepth)
                            {
                                currentProcessingDepth.Remove((int)o);
                            }
                            return null;
                        }

                        Debug.WriteLine((moveUpBias ? "U " : "B ") + "Trying to solve task " + o.ToString() + 
                            " at level " + moveLevel);
                        
                        List<BuildingMove> leafNodes = new List<BuildingMove>();
                        
                        foreach (BuildingMove move in moves)
                        {
                            UpdateProcessingTracking(moveLevel, 1);
                            CalcAllPossibleValidMoves(move.StateAfterMove, leafNodes, moveLevel, moveUpBias);

                            foreach (BuildingMove newMove in leafNodes)
                            {
                                if (newMove.MoveSolvesBuilding)
                                {
                                    lock (currentProcessingDepth)
                                    {
                                        currentProcessingDepth.Remove((int)o);
                                    }                        
                                    return newMove;
                                }
                            }
                        }

                        if (leafNodes.Count == 0)
                        {
                            Debug.WriteLine((moveUpBias ? "U " : "B ") + o.ToString() + " Out of ideas. Aborting...");
                            lock (currentProcessingDepth)
                            {
                                currentProcessingDepth.Remove((int)o);
                            }
                            return null; 
                        }
                        
                        moves.Clear();
                        moves.AddRange(leafNodes);
                    }
                    lock (currentProcessingDepth)
                    {
                        currentProcessingDepth.Remove((int)o);
                    }
                    return null;
                }, taskCount);
                calculator.Start();
                tasks.Add(calculator);
            }

            while (true)
            {
                DisplayTracking();
                List<int> possibleAnswers = new List<int>();
                foreach (Task<BuildingMove> t in tasks)
                {
                    if (t.IsCompleted && t.Result != null && t.Result.MoveSolvesBuilding)
                        possibleAnswers.Add(t.Result.MoveDepth);
                }

                if (possibleAnswers.Count > 0)
                {
                    int bestAnswer = possibleAnswers.Min();
                    _currentBestCandidate = bestAnswer;
                    lock (currentProcessingDepth)
                    {
                        if (currentProcessingDepth.Count > 0)
                        {
                            if (bestAnswer < currentProcessingDepth.Values.Min())
                            {
                                Debug.WriteLine("Solved in direction " + (moveUpBias ? "Up" : "Down") + " with value of " + bestAnswer);
                                breaker = true;
                                return bestAnswer;
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Solved in direction " + (moveUpBias ? "Up" : "Down") + " with value of " + bestAnswer);
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
            CalcAllPossibleValidMoves(startState, addMovesTo, 1, true);
            bool solved = false;
            // Now calculate progressively deeper until the puzzle gets solved somewhere
            while (!solved)
            {
                List<BuildingMove> leafNodes = FindLeafNodes(addMovesTo);
                bool anyChildren = false;
                foreach (BuildingMove move in leafNodes)
                {
                    CalcAllPossibleValidMoves(move.StateAfterMove, move.SubsequentMoves, move.MoveDepth + 1, true);
                    if (move.SubsequentMoves.Count > 0)
                        anyChildren = true;
                    if (move.CommandTreeSolved())
                        solved = true;
                }

                if (!anyChildren)
                {
                    Debug.WriteLine("Ran out of moves. Still not solved.");
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

        public void CalcAllPossibleValidMoves(Building startState, List<BuildingMove> addMovesTo, 
            int processingDepth, bool moveUpBias)
        {
            Floor processFloor = startState.Floors[startState.ElevatorOn - 1];
            MakeMicrochipMoves(startState, addMovesTo, processFloor, processingDepth, moveUpBias);
            MakeGeneratorOnlyMoves(startState, addMovesTo, processFloor, processingDepth, moveUpBias);
        }

        private void MakeGeneratorOnlyMoves(Building startState, List<BuildingMove> result, Floor processFloor, 
            int processingDepth, bool moveUpBias)
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
                    }
                    else
                    {
                        if (moveUpBias)
                        {
                            // Can we go up?
                            if (startState.ElevatorOn < startState.Floors.Count)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn];
                                MakeMoveIfValid(null, null, g1, g2, processFloor, endFloor, startState, result, processingDepth);
                            }
                        }
                        else
                        { 
                            // Can we go down?
                            if (startState.ElevatorOn > 1)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                                MakeMoveIfValid(null, null, g1, g2, processFloor, endFloor, startState, result, processingDepth);
                            }
                        }
                    }
                }
            }
        }

        private void MakeMicrochipMoves(Building startState, List<BuildingMove> result, Floor processFloor,
            int processingDepth, bool moveUpBias)
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
                            if (moveUpBias)
                            {
                                foreach (Generator g in processFloor.Generators)
                                {
                                    MakeMoveIfValid(mc1, null, g, null, processFloor, endFloor, startState, result, processingDepth);
                                }
                            }
                        }
                        // Can we go down?
                        if (startState.ElevatorOn > 1)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                            MakeMoveIfValid(mc1, null, null, null, processFloor, endFloor, startState, result, processingDepth);
                            if (!moveUpBias)
                            {
                                foreach (Generator g in processFloor.Generators)
                                {
                                    MakeMoveIfValid(mc1, null, g, null, processFloor, endFloor, startState, result, processingDepth);
                                }
                            }
                        }
                    } else
                    {
                        if (moveUpBias)
                        {
                            // Can we go up?
                            if (startState.ElevatorOn < startState.Floors.Count)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn];
                                MakeMoveIfValid(mc1, mc2, null, null, processFloor, endFloor, startState, result, processingDepth);
                            }
                        }
                        else
                        {
                            // Can we go down?
                            if (startState.ElevatorOn > 1)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                                MakeMoveIfValid(mc1, mc2, null, null, processFloor, endFloor, startState, result, processingDepth);
                            }
                        }
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

            int foundOppositeProcessingTerminus = 0;
            // An optmization to stop the same situation from being processed over and over again. Once a
            // particular state has been processed, it isn't ever reprocessed
            if (HashStrings)
            {
                string hashS = moveEffect.HashString();
                lock (_processedHashesString)
                {
                    if (_processedHashesString.ContainsKey(hashS))
                    {
                        if (Math.Sign(_processedHashesString[hashS]) == Math.Sign(processingDepth))
                            return;
                        else
                            foundOppositeProcessingTerminus = _processedHashesString[hashS];
                        _processedHashesString[hashS] = processingDepth;
                    }
                }
            } else
            {
                long hash = moveEffect.Hash();
                lock (_processedHashes)
                {
                    if (_processedHashes.ContainsKey(hash))
                    {
                        if (Math.Sign(_processedHashes[hash]) == Math.Sign(processingDepth))
                            return;
                        else
                            foundOppositeProcessingTerminus = _processedHashes[hash];
                    }
                    _processedHashes[hash] = processingDepth;
                }
            }

            // And represent the move
            BuildingMove bm = new BuildingMove();
            bm.StartFloor = newStartFloor;
            bm.EndFloor = newEndFloor;
            bm.StateAfterMove = moveEffect;
            bm.MoveDepth = Math.Abs(processingDepth) + Math.Abs(foundOppositeProcessingTerminus);

            if (foundOppositeProcessingTerminus > 0)
            {
                Debug.WriteLine("Found opposite processing end at level " + foundOppositeProcessingTerminus +
                    " while processing at level " + processingDepth); 
            }

            bm.MoveSolvesBuilding = (foundOppositeProcessingTerminus != 0) || (processingDepth > 0 && moveEffect.BuildingSolved());
            bm.ProcessedAtLevel = processingDepth;

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