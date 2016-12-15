﻿using AdventOfCodeCSharp.Puzzle8Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCSharp
{
    class Puzzle8
    {
        public string ParsePuzzle(string input)
        {
            bool[,] screen = new bool[50, 6];

            foreach (string s in input.Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries))
            {
                BaseCommand bc = CommandFactory.CreateCommand(s);
                bc.ApplyCommand(screen);
            }
       
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    Console.Write(screen[x, y] ? '#' : ' ');
                }
                Console.WriteLine();
            }
            return "read it!!";
        }

        public int CalculatePuzzle(string input)
        {
            bool[,] screen = new bool[50, 6];

            foreach(string s in input.Split(Environment.NewLine.ToCharArray(), 
                StringSplitOptions.RemoveEmptyEntries))
            {
                BaseCommand bc = CommandFactory.CreateCommand(s);
                bc.ApplyCommand(screen);
            }
            int onPixels = 0;
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (screen[x, y])
                        onPixels++;
                }
            }
            return onPixels;
        }
    }

    
}
