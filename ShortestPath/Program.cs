using System;
using BenchmarkDotNet.Running;

namespace ShortestPath
{
    partial class Program
    {
        static void Main(string[] args)
        {

            //var summary = BenchmarkRunner.Run<GraphBenchMark>();
            //GraphTest();
            //FindShortestPath01();
            //FindShortestPath();
            //MSTTest();
            //RankTest();
            //TaskTest();

            //Console.WriteLine(Encryption("haveaniceday"));

            //LCSTest();

            GraphNew.GraphTest();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
