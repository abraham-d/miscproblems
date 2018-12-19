using System;
using BenchmarkDotNet.Running;

namespace ShortestPath
{
    partial class Program
    {
        static void Main(string[] args)
        {
            //bool[] tst = new bool[4];
            //tst[2] = true;
            //Console.WriteLine(string.Join(" ", tst));
            //Add(tst, 2);
            //Console.WriteLine(string.Join(" ", tst));

            //var summary = BenchmarkRunner.Run<GraphBenchMark>();
            //GraphTest();
            //FindShortestPath01();
            //FindShortestPath();
            MSTTest();
            //RankTest();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
