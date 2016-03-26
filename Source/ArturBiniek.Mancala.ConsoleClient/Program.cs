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

            var ind = 0;

            while (!gb.IsTerminal)
            {
                sw.Reset();
                sw.Start();

                var mcnt = gb.Moves.Count();

                var nextMove = gb.FindMove();

                sw.Stop();

                Console.WriteLine("[{1}] Making move: {0}", nextMove, ind);

                gb.MakeMove(nextMove);


                Console.WriteLine(gb);
                Console.WriteLine(":::::::::::::::::::::: {0} :::::::::::::::::::::::", sw.ElapsedMilliseconds);

                ind++;
            }
        }
    }
}
