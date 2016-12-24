using System;
using System.Collections.Concurrent;
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
        List<HashSet<long>> _processedHashes = new List<HashSet<long>>();

        private int _consoleY;

        public int CalcMoveDepth(Building startState, Building desiredEndState)
        {
            InitHashes();

            Console.CursorVisible = false;
            _consoleY = Console.CursorTop;
            Task<int> solveFromBottom = new Task<int>(() =>
            {
                return SolveBuildingStateThreadWide(startState, true);
            });

            Task<int> solveFromTop = new Task<int>(() =>
            {
                return SolveBuildingStateThreadWide(desiredEndState, false);
            });

            solveFromBottom.Start();
            //solveFromTop.Start();

            while (true)
            {
                DisplayTracking();                
                // Only check for a solution once every half second
                Thread.Sleep(500);

                if (solveFromBottom.IsCompleted)// && solveFromTop.IsCompleted)
                    break;
            }

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, _consoleY + 4);

            //return Math.Min(solveFromBottom.Result, solveFromTop.Result);
            return solveFromBottom.Result;
        }

        private void InitHashes()
        {
            for (int i = 0; i < 10; i++)
            {
                _processedHashes.Add(new HashSet<long>());
            }
        }

        private Dictionary<int, int> _tracking = new Dictionary<int, int>();

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
                if (_tracking.Count == 0)
                    return;

                deepestFromTop = _tracking.Keys.Max();
                deepestFromBottom = _tracking.Keys.Min();
                widest = _tracking.Values.Max();
            }
            long divider = widest / 40;
            int[] displayLevels = { deepestFromBottom, deepestFromTop };
            int height = 0;
            foreach (int i in displayLevels)
            {
                height++;
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
                int consoleY = _consoleY + height;
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
                long totalMoves = 0;
                lock (_tracking)
                {
                  totalMoves = _tracking.Values.Sum();
                }
                Console.SetCursorPosition(0, _consoleY + height + 1);
                Console.WriteLine("Total Moves: " + totalMoves);
            }
        }

        private object consoleAcces = new object();

        private int PROCESSING_POOL_SIZE = 5;

        private int SolveBuildingStateThreadWide(Building startState, bool moveUpBias)
        {
            // Calculate the top level
            ConcurrentQueue<BuildingMove> nextLevelMoves = new ConcurrentQueue<BuildingMove>();
            ConcurrentQueue<BuildingMove> thisLevelMoves = new ConcurrentQueue<BuildingMove>();
            
            int directionMultiplier = moveUpBias ? 1 : -1;
            bool solved = false;
            CalcAllPossibleValidMoves(startState, thisLevelMoves, directionMultiplier, moveUpBias, ref solved);
            if (thisLevelMoves.IsEmpty)
            {
                Debug.WriteLine("Moving " + (moveUpBias ? " up " : "down") + "found no possible moves");
                return 0;
            }
            UpdateProcessingTracking(directionMultiplier, thisLevelMoves.Count);
                        
            int moveLevel = directionMultiplier;
            while (!solved)
            {
                moveLevel = moveLevel + directionMultiplier;
                while (!thisLevelMoves.IsEmpty)
                {
                    BuildingMove move;
                    if (thisLevelMoves.TryDequeue(out move))
                    {                                
                        UpdateProcessingTracking(moveLevel, 1);
                        CalcAllPossibleValidMoves(move.StateAfterMove, nextLevelMoves, moveLevel, moveUpBias, ref solved);
                        if (solved)
                            return moveLevel;
                    }
                    if (solved)
                        break;
                }
                
                if (nextLevelMoves.Count == 0)
                {
                    Debug.WriteLine("Ran out of possible moves without a solution :-(");
                    return Int32.MaxValue;
                }

                foreach (BuildingMove newMove in nextLevelMoves)
                {
                    if (newMove.MoveSolvesBuilding)
                    {
                        return newMove.MoveDepth;
                    }
                }
                var temp = thisLevelMoves;
                thisLevelMoves = nextLevelMoves;
                nextLevelMoves = temp;
            }
            return Int32.MaxValue;
        }
        
        public void CalcAllPossibleValidMoves(Building startState, ConcurrentQueue<BuildingMove> addMovesTo, 
            int processingDepth, bool moveUpBias, ref bool solved)
        {
            Floor processFloor = startState.Floors[startState.ElevatorOn - 1];
            MakeMicrochipMoves(startState, addMovesTo, processFloor, processingDepth, moveUpBias, ref solved);
            MakeGeneratorOnlyMoves(startState, addMovesTo, processFloor, processingDepth, moveUpBias, ref solved);
        }

        private void MakeGeneratorOnlyMoves(Building startState, ConcurrentQueue<BuildingMove> result, Floor processFloor, 
            int processingDepth, bool moveUpBias, ref bool solved)
        {
            IEnumerable<int> generators = processFloor.Generators;
            foreach (int g1 in generators)
            {
                foreach (int g2 in generators)
                {
                    if (g1 == g2)
                    {
                        // Can we go up?
                        if (startState.ElevatorOn < startState.Floors.Count)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn];
                            MakeMoveIfValid(0, 0, g1, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                        }
                        // Can we go down?
                        if (startState.ElevatorOn > 1)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                            MakeMoveIfValid(0, 0, g1, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
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
                                MakeMoveIfValid(0, 0, g1, g2, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            }
                        }
                        else
                        { 
                            // Can we go down?
                            if (startState.ElevatorOn > 1)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                                MakeMoveIfValid(0, 0, g1, g2, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            }
                        }
                    }
                }
            }
        }

        private void MakeMicrochipMoves(Building startState, ConcurrentQueue<BuildingMove> result, Floor processFloor,
            int processingDepth, bool moveUpBias, ref bool solved)
        {
            IEnumerable<int> microchips = processFloor.MicroChips;

            foreach (int mc1 in microchips)
            {
                foreach (int mc2 in microchips)
                {
                    if (mc1 == mc2)
                    {
                        // Can we go up?
                        if (startState.ElevatorOn < startState.Floors.Count)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn];
                            MakeMoveIfValid(mc1, 0, 0, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            if (moveUpBias)
                            {
                                foreach (int g in processFloor.Generators)
                                {
                                    MakeMoveIfValid(mc1, 0, g, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                                }
                            }
                        }
                        // Can we go down?
                        if (startState.ElevatorOn > 1)
                        {
                            Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                            MakeMoveIfValid(mc1, 0, 0, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            if (!moveUpBias)
                            {
                                foreach (int g in processFloor.Generators)
                                {
                                    MakeMoveIfValid(mc1, 0, g, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
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
                                MakeMoveIfValid(mc1, mc2, 0, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            }
                        }
                        else
                        {
                            // Can we go down?
                            if (startState.ElevatorOn > 1)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                                MakeMoveIfValid(mc1, mc2, 0, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            }
                        }
                    }
                }
            }
        }

        private void MakeMoveIfValid(int mc1, int mc2, int g1, int g2, Floor startFloor,
            Floor endFloor, Building startState, ConcurrentQueue<BuildingMove> addMoveTo, 
            int processingDepth, ref bool solved)
        {
            if (mc1 == 0 && mc2 == 0 && g1 == 0 && g2 == 0)
                throw new ArgumentException("Need a generator or a chip in the elevator");
            // If both are supplied, their identifiers have to match else no dice
            if (mc1 != 0 && g1 != 0 && mc1 != g1)
                return;
            // We only have a chip so we have to validate it can go to the target floor
            if (mc1 != 0 && g1 == 0)
            {
                if (endFloor.Generators.Count() > 0)
                {
                    if (!endFloor.ContainsGenerator(mc1))
                        return;
                    if (mc2 > 0)
                    {
                        if (!endFloor.ContainsGenerator(mc2))
                            return;
                    }
                }
            }
            IEnumerable<int> leftChips = startFloor.MicroChips;
            // If this move would leave a currently paired chip behind to fry, it is not valid
            if (g1 > 0 && g1 != mc1 && g1 != mc2)
            {
                if (startFloor.ContainsChip(g1))
                    return;
            }
            if (g2 > 0 && g2 != mc1 && g2 != mc2)
            {
                if (startFloor.ContainsChip(g2))
                    return;
            }
            // Now we know the move is valid, create a representation of what it would do
            Building moveEffect = startState.Clone();
            Floor newStartFloor = moveEffect.Floors[startFloor.FloorNumber - 1];
            Floor newEndFloor = moveEffect.Floors[endFloor.FloorNumber - 1];
            if (mc1 != 0)
            {
                newStartFloor.RemoveMicrochip(mc1);
                newEndFloor.AddMicrochip(mc1);
            }
            if (mc2 != 0)
            {
                newStartFloor.RemoveMicrochip(mc2);
                newEndFloor.AddMicrochip(mc2);
            }
            if (g1 != 0)
            {
                newStartFloor.RemoveGenerator(g1);
                newEndFloor.AddGenerator(g1);
            }
            if (g2 != 0)
            {
                newStartFloor.RemoveGenerator(g2);
                newEndFloor.AddGenerator(g2);
            }
            moveEffect.ElevatorOn = newEndFloor.FloorNumber;

            int foundOppositeProcessingTerminus = 0;
            // An optmization to stop the same situation from being processed over and over again. Once a
            // particular state has been processed, it isn't ever reprocessed
            long hash = moveEffect.Hash();
            long[] equivalentHashes = moveEffect.EquivalentHashes();
            lock (_processedHashes)
            {
                if (_processedHashes[Math.Abs((int)(hash % 10))].Contains(hash))
                {
                    return;
                }
                _processedHashes[Math.Abs((int)(hash % 10))].Add(hash);
            }
            foreach(long sameHash in equivalentHashes)
            {
                if (_processedHashes[Math.Abs((int)(hash % 10))].Contains(sameHash))
                {
                    return;
                }
                _processedHashes[Math.Abs((int)(hash % 10))].Add(sameHash);
            }
            
            // And represent the move
            BuildingMove bm = new BuildingMove();
            bm.StateAfterMove = moveEffect;
            bm.MoveDepth = Math.Abs(processingDepth) + Math.Abs(foundOppositeProcessingTerminus);

            if (foundOppositeProcessingTerminus > 0)
            {
                Debug.WriteLine("Found opposite processing end at level " + foundOppositeProcessingTerminus +
                    " while processing at level " + processingDepth);
            }

            bm.MoveSolvesBuilding = (foundOppositeProcessingTerminus != 0) || (processingDepth > 0 && moveEffect.BuildingSolved());
            if (bm.MoveSolvesBuilding)
                solved = true;
            lock(addMoveTo)
                addMoveTo.Enqueue(bm);
        }
    }
}