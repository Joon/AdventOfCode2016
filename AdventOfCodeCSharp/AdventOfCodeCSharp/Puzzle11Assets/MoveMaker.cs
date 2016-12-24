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

        private const int INFINITY = -1;

        public int CalcMoveDepthAStar(Building startState, Building desiredEndState)
        {
            Console.CursorVisible = false;
            _consoleY = Console.CursorTop;

            try
            {
                // The set of nodes already evaluated.
                HashSet<long> closedSet = new HashSet<long>();

                // The set of currently discovered nodes still to be evaluated.
                // Initially, only the start node is known.
                List<Building> openSet = new List<Building>();
                openSet.Add(startState);

                // For each node, which node it can most efficiently be reached from.
                // If a node can be reached from many nodes, cameFrom will eventually contain the
                // most efficient previous step.
                Dictionary<long, long> cameFrom = new Dictionary<long, long>();

                // For each node, the cost of getting from the start node to that node.
                Dictionary<Building, int> gScore = new Dictionary<Building, int>();
                // The cost of going from start to start is zero.
                gScore[startState] = 0;

                // For each node, the total cost of getting from the start node to the goal
                // by passing by that node. That value is partly known, partly heuristic.
                Dictionary<long, int> fScore = new Dictionary<long, int>();

                // For the first node, that value is completely heuristic.
                fScore[startState.Hash()] = heuristic_cost_estimate(startState);

                while (openSet.Count > 0)
                {
                    Console.SetCursorPosition(0, _consoleY);
                    Console.Write("Queue depth: " + openSet.Count + " Hashes processed: " + _processedHashes.Count);

                    var v = from f in openSet
                            join j in fScore
                            on f.Hash() equals j.Key
                            select new { score = j.Value, building = f };
                    var scores = v.ToList();
                    int minValue = scores.Min(o => o.score);

                    // the node in openSet having the lowest fScore[] value
                    Building current = scores.First(o => o.score == minValue).building;

                    if (current.Hash() == desiredEndState.Hash())
                        return reconstruct_path(cameFrom, current).Count - 1;

                    openSet.Remove(current);
                    closedSet.Add(current.Hash());
                    ConcurrentQueue<BuildingMove> neighbours = new ConcurrentQueue<BuildingMove>();
                    bool solved = false;
                    CalcAllPossibleValidMoves(current, neighbours, 0, true, ref solved);
                    foreach (BuildingMove bm in neighbours)
                    {
                        // Ignore the neighbor which is already evaluated.
                        if (closedSet.Contains(bm.StateAfterMove.Hash()))
                            continue;
                        // The distance from start to a neighbor
                        int tentative_gScore = gScore[current] + 1;
                        // Discover a new node
                        if (!openSet.Contains(bm.StateAfterMove))
                            openSet.Add(bm.StateAfterMove);
                        else if (tentative_gScore >= gScore[bm.StateAfterMove])
                            continue; // This is not a better path.

                        // This path is the best until now. Record it!
                        cameFrom[bm.StateAfterMove.Hash()] = current.Hash();
                        gScore[bm.StateAfterMove] = tentative_gScore;
                        fScore[bm.StateAfterMove.Hash()] = gScore[bm.StateAfterMove] + heuristic_cost_estimate(bm.StateAfterMove);
                    }
                }
                return -1;
            } finally
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(0, _consoleY + 3);
            }
        }

        private List<long> reconstruct_path(Dictionary<long, long> cameFrom, Building current)
        {
            List<long> result = new List<long>();
            result.Add(current.Hash());
            long tempCurrent = current.Hash();
            while (cameFrom.ContainsKey(tempCurrent))
            {
                tempCurrent = cameFrom[tempCurrent];
                result.Add(tempCurrent);
            }
            return result;
        }

        private int heuristic_cost_estimate(Building state)
        {
            int score = 0;
            for (int i = 1; i <= 3; i++)
            {
                score += state.Floors[3 - i].Generators.Count() * i * 10;
                score += state.Floors[3 - i].MicroChips.Count() * i * 10;
            }
            return score; 
        }

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
            bool allFloorsBelowEmpty = true;
            for (int i = startState.ElevatorOn - 1; i >= 0; i--)
            {
                if (startState.Floors[i].Hash != 0)
                    allFloorsBelowEmpty = false;
            }
            
            IEnumerable<int> generators = processFloor.Generators;
            foreach (int g1 in generators)
            {
                bool movedTwoGennies = false;
                foreach (int g2 in generators)
                {   
                    if (g1 != g2)
                    {
                        if (moveUpBias)
                        {
                            // Can we go up?
                            if (startState.ElevatorOn < startState.Floors.Count)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn];
                                if (MakeMoveIfValid(0, 0, g1, g2, processFloor, endFloor, startState, result, processingDepth, ref solved))
                                    movedTwoGennies = true;
                            }
                        }
                        else
                        { 
                            // Can we go down?
                            if (startState.ElevatorOn > 1 && !allFloorsBelowEmpty)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                                if (MakeMoveIfValid(0, 0, g1, g2, processFloor, endFloor, startState, result, processingDepth, ref solved))
                                    movedTwoGennies = false;
                            }
                        }
                    }
                }
                if (!movedTwoGennies)
                {
                    // Can we go up?
                    if (startState.ElevatorOn < startState.Floors.Count)
                    {
                        Floor endFloor = startState.Floors[startState.ElevatorOn];
                        MakeMoveIfValid(0, 0, g1, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                    }
                    // Can we go down?
                    if (startState.ElevatorOn > 1 && !allFloorsBelowEmpty)
                    {
                        Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                        MakeMoveIfValid(0, 0, g1, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                    }
                }
            }
        }

        private void MakeMicrochipMoves(Building startState, ConcurrentQueue<BuildingMove> result, Floor processFloor,
            int processingDepth, bool moveUpBias, ref bool solved)
        {
            bool allFloorsBelowEmpty = true;
            for (int i = startState.ElevatorOn - 1; i >= 0; i--)
            {
                if (startState.Floors[i].Hash != 0)
                    allFloorsBelowEmpty = false;
            }

            IEnumerable<int> microchips = processFloor.MicroChips;

            foreach (int mc1 in microchips)
            {
                bool movedTwoMicrochips = false;
                foreach (int mc2 in microchips)
                {
                    if (mc1 != mc2)
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
                            if (startState.ElevatorOn > 1 && !allFloorsBelowEmpty)
                            {
                                Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                                MakeMoveIfValid(mc1, mc2, 0, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            }
                        }
                    }
                    
                    // Can we go up?
                    if (startState.ElevatorOn < startState.Floors.Count)
                    {
                        Floor endFloor = startState.Floors[startState.ElevatorOn];
                        if (!movedTwoMicrochips)
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
                    if (startState.ElevatorOn > 1 && !allFloorsBelowEmpty)
                    {
                        Floor endFloor = startState.Floors[startState.ElevatorOn - 2];
                        if (!movedTwoMicrochips)
                            MakeMoveIfValid(mc1, 0, 0, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                        if (!moveUpBias)
                        {
                            foreach (int g in processFloor.Generators)
                            {
                                MakeMoveIfValid(mc1, 0, g, 0, processFloor, endFloor, startState, result, processingDepth, ref solved);
                            }
                        }
                    }
                }
            }
        }

        private bool MakeMoveIfValid(int mc1, int mc2, int g1, int g2, Floor startFloor,
            Floor endFloor, Building startState, ConcurrentQueue<BuildingMove> addMoveTo, 
            int processingDepth, ref bool solved)
        {
            if (mc1 == 0 && mc2 == 0 && g1 == 0 && g2 == 0)
                throw new ArgumentException("Need a generator or a chip in the elevator");
            // If both are supplied, their identifiers have to match else no dice
            if (mc1 != 0 && g1 != 0 && mc1 != g1)
                return false;
            // We only have a chip so we have to validate it can go to the target floor
            if (mc1 != 0 && g1 == 0)
            {
                if (endFloor.Generators.Count() > 0)
                {
                    if (!endFloor.ContainsGenerator(mc1))
                        return false;
                    if (mc2 > 0)
                    {
                        if (!endFloor.ContainsGenerator(mc2))
                            return false;
                    }
                }
            }
            IEnumerable<int> leftChips = startFloor.MicroChips;
            // If this move would leave a currently paired chip behind to fry, it is not valid
            if (g1 > 0 && g1 != mc1 && g1 != mc2)
            {
                if (startFloor.ContainsChip(g1))
                    return false;
            }
            if (g2 > 0 && g2 != mc1 && g2 != mc2)
            {
                if (startFloor.ContainsChip(g2))
                    return false;
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
            if (_processedHashes.Count > 0)
            {
                long hash = moveEffect.Hash();
                long[] equivalentHashes = moveEffect.EquivalentHashes();
                lock (_processedHashes)
                {
                    if (_processedHashes[Math.Abs((int)(hash % 10))].Contains(hash))
                    {
                        return false;
                    }
                    _processedHashes[Math.Abs((int)(hash % 10))].Add(hash);
                }
                foreach (long sameHash in equivalentHashes)
                {
                    if (_processedHashes[Math.Abs((int)(hash % 10))].Contains(sameHash))
                    {
                        return false;
                    }
                    _processedHashes[Math.Abs((int)(hash % 10))].Add(sameHash);
                }
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
            return true;
        }
    }
}