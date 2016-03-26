using System;
using System.Diagnostics;
using System.Linq;
using ArturBiniek.Mancala.Game;

namespace ArturBiniek.Mancala.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1m = 0;
            var p1 = new[] { 4, 4, 4, 4, 4, 4 };

            var p2m = 0;
            var p2 = new[] { 4, 4, 4, 4, 4, 4 };

            var gb = new GameBoard(GameStateBase.Player.One, p1, p1m, p2, p2m);
            Console.WriteLine(gb);

            var sw = new Stopwatch();
            var rnd = new Random();

            while (!gb.IsTerminal)
            {
                sw.Reset();
                sw.Start();

                var mcnt = gb.Moves.Count();

                var nextMove = gb.CurentPlayer == GameStateBase.Player.Two ? gb.FindMove() : gb.Moves.Skip(rnd.Next(mcnt) - 1).First();

                Console.WriteLine("Making move: {0}", nextMove);

                gb.MakeMove(nextMove);

                sw.Stop();
                Console.WriteLine(gb);
                Console.WriteLine(":::::::::::::::::::::: {0} :::::::::::::::::::::::", sw.ElapsedMilliseconds);

                controller.Reset();
            }
        }
    }
}
