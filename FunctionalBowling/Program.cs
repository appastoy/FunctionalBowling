using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalBowling
{
    static class BowlingScoreCalculator
    {
        public static int CalculateScore(IEnumerable<int> falls)
        {
            return CalculateRecursive(falls, 1);
        }

        static int CalculateRecursive(IEnumerable<int> falls, int frameNumber)
        {
            return IsLastFrameOrHasNoFalls(frameNumber, falls) ? 0 :
                CalculateFrameScore(falls, CalculateRollCountForScoreAggregation(falls)) +
                CalculateRecursive(falls.Skip(CalculateFrameRollCount(falls)), frameNumber + 1);
        }

        static bool IsLastFrameOrHasNoFalls(int frameNumber, IEnumerable<int> falls)
        {
            return frameNumber > 10 || !falls.Any();
        }

        static int CalculateRollCountForScoreAggregation(IEnumerable<int> falls)
        {
            return falls.First() == 10 || falls.Take(2).Sum() == 10 ? 3 : 2;
        }

        static int CalculateFrameScore(IEnumerable<int> falls, int scoreRollCount)
        {
            return falls.Count() < scoreRollCount ? 0 : falls.Take(scoreRollCount).Sum();
        }

        static int CalculateFrameRollCount(IEnumerable<int> falls)
        {
            return falls.First() == 10 ? 1 : 2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The score of empty falls is {BowlingScoreCalculator.CalculateScore(Enumerable.Empty<int>())}");
            Console.WriteLine($"The score of (5) is {BowlingScoreCalculator.CalculateScore(new[] { 5 })}");
            Console.WriteLine($"The score of (5, 4) is {BowlingScoreCalculator.CalculateScore(new[] { 5, 4 })}");
            Console.WriteLine($"The score of (5, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 5, 5 })}");
            Console.WriteLine($"The score of (5, 5, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 5, 5, 5 })}");
            Console.WriteLine($"The score of (10, 5, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 10, 5, 5 })}");
            Console.WriteLine($"The score of perfect falls is {BowlingScoreCalculator.CalculateScore(Enumerable.Repeat(10, 12))}");
        }
    }
}
