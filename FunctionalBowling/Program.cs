using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalBowling
{
    static class BowlingScoreCalculator
    {
        static readonly int LastFrameNumber = 10;
        static readonly int MaxFramePinCount = 10;
        static readonly int ScoreAggregationRollCountWithBonus = 3;
        static readonly int ScoreAggregationRollCount = 2;
        static readonly int FrameRollCountOnStrike = 1;
        static readonly int FrameRollCount = 1;
        static readonly int EmptyScore = 0;
        
        public static int CalculateScore(IEnumerable<int> falls)
        {
            return CalculateScoreRecursive(falls, 1);
        }
        
        static int CalculateScoreRecursive(IEnumerable<int> falls, int frameNumber)
        {
            return !IsValidFrame(frameNumber) || !HasFalls(falls) ? EmptyScore :
                CalculateFrameScore(falls) + CalculateScoreRecursive(GetNextFrameFalls(falls), frameNumber + 1);
        }
    
        static bool IsValidFrame(int frameNumber) => frameNumber <= LastFrameNumber;
        static bool HasFalls(IEnumerable<int> falls) => falls.Any();
        static bool IsStrike(IEnumerable<int> falls) => falls.First() == MaxFramePinCount;
        static bool IsSpare(IEnumerable<int> falls) => falls.Take(2).Sum() == MaxFramePinCount;
        
        static int  GetNeedScoreAggregationRollCount(IEnumerable<int> falls)
        {
            return IsStrike(falls) || IsSpare(falls) ? ScoreAggregationRollCountWithBonus : ScoreAggregationRollCount;
        }
        
        static int  CalculateFrameScore(IEnumerable<int> falls)
        {
            return CanCalculateFrameScore(falls) ? EmptyScore : falls.Take(GetNeedScoreAggregationRollCount(falls)).Sum();
        }
        
        static bool CanCalculateFrameScore(IEnumerable<int> falls)
        {
            return falls.Count() < GetNeedScoreAggregationRollCount(falls);
        }
        
        static IEnumerable<int> GetNextFrameFalls(IEnumerable<int> falls)
        {
            return falls.Skip(GetFrameRollCount(falls));
        }
        
        static int  GetFrameRollCount(IEnumerable<int> falls)
        {
            return IsStrike(falls) ? FrameRollCountOnStrike : FrameRollCount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The score of empty fall is {BowlingScoreCalculator.CalculateScore(Enumerable.Empty<int>())}");
            Console.WriteLine($"The score of (5) is {BowlingScoreCalculator.CalculateScore(new[] { 5 })}");
            Console.WriteLine($"The score of (5, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 5, 5 })}");
            Console.WriteLine($"The score of (5, 4) is {BowlingScoreCalculator.CalculateScore(new[] { 5, 4 })}");
            Console.WriteLine($"The score of (5, 5, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 5, 5, 5 })}");
            Console.WriteLine($"The score of (10) is {BowlingScoreCalculator.CalculateScore(new[] { 10 })}");
            Console.WriteLine($"The score of (10, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 10, 5 })}");
            Console.WriteLine($"The score of (10, 5, 5) is {BowlingScoreCalculator.CalculateScore(new[] { 10, 5, 5 })}");
            Console.WriteLine($"The score of (10, 5, 4) is {BowlingScoreCalculator.CalculateScore(new[] { 10, 5, 4 })}");
            Console.WriteLine($"The score of perfect falls is {BowlingScoreCalculator.CalculateScore(Enumerable.Repeat(10, 12))}");

            /*
             
            Output
            The score of empty fall is 0
            The score of (5) is 0
            The score of (5, 5) is 0
            The score of (5, 4) is 9
            The score of (5, 5, 5) is 15
            The score of (10) is 0
            The score of (10, 5) is 0
            The score of (10, 5, 5) is 20
            The score of (10, 5, 4) is 28
            The score of perfect falls is 300
            */
        }
    }
}
