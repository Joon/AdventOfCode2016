using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    public class Puzzle18
    {
        public int SolvePuzzle(string input)
        {
            int safeCounter = 0;

            char[] previousTiles = input.ToCharArray();
            foreach (var c in previousTiles)
            {
                if (c == '.')
                    safeCounter++;
            }
            List<char> tiles = new List<char>();
            for (int i = 2; i <= 40; i++)
            {
                for (int j = 0; j < previousTiles.Count(); j++)
                {
                    bool leftIsTrap = j == 0 ? false : previousTiles[j - 1] == '^';
                    bool centerIsTrap = previousTiles[j] == '^';
                    bool rightIsTrap = j == previousTiles.Count() - 1 ? false : previousTiles[j + 1] == '^';

                    bool tileIsTrap = false;
                    // Its left and center tiles are traps, but its right tile is not.
                    tileIsTrap |= leftIsTrap && centerIsTrap && !rightIsTrap;

                    // Its center and right tiles are traps, but its left tile is not.
                    tileIsTrap |= centerIsTrap && rightIsTrap && !leftIsTrap;
                    // Only its left tile is a trap.
                    tileIsTrap |= leftIsTrap && !rightIsTrap && !centerIsTrap;
                    // Only its right tile is a trap.
                    tileIsTrap |= rightIsTrap && !leftIsTrap && !centerIsTrap;
                    if (tileIsTrap)
                        tiles.Add('^');
                    else
                    {
                        tiles.Add('.');
                        safeCounter++;
                    }
                }
                previousTiles = tiles.ToArray();
                tiles.Clear();
            }
            return safeCounter;
        }
        public int SolvePuzzlePart2(string input)
        {
            int safeCounter = 0;

            char[] previousTiles = input.ToCharArray();
            foreach (var c in previousTiles)
            {
                if (c == '.')
                    safeCounter++;
            }
            List<char> tiles = new List<char>();
            for (int i = 2; i <= 400000; i++)
            {
                for (int j = 0; j < previousTiles.Count(); j++)
                {
                    bool leftIsTrap = j == 0 ? false : previousTiles[j - 1] == '^';
                    bool centerIsTrap = previousTiles[j] == '^';
                    bool rightIsTrap = j == previousTiles.Count() - 1 ? false : previousTiles[j + 1] == '^';

                    bool tileIsTrap = false;
                    // Its left and center tiles are traps, but its right tile is not.
                    tileIsTrap |= leftIsTrap && centerIsTrap && !rightIsTrap;

                    // Its center and right tiles are traps, but its left tile is not.
                    tileIsTrap |= centerIsTrap && rightIsTrap && !leftIsTrap;
                    // Only its left tile is a trap.
                    tileIsTrap |= leftIsTrap && !rightIsTrap && !centerIsTrap;
                    // Only its right tile is a trap.
                    tileIsTrap |= rightIsTrap && !leftIsTrap && !centerIsTrap;
                    if (tileIsTrap)
                        tiles.Add('^');
                    else
                    {
                        tiles.Add('.');
                        safeCounter++;
                    }
                }
                previousTiles = tiles.ToArray();
                tiles.Clear();
            }
            return safeCounter;
        }

    }
}
