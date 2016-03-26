using System;
using System.Linq;
using ArturBiniek.Mancala.Game;

namespace ArturBiniek.Mancala.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1m = 0;
            var p1 = new[] { 2, 2, 2 };

            var p2m = 0;
            var p2 = new[] { 2, 2, 2 };

            var gb = new GameBoard(GameStateBase.Player.One, p1, p1m, p2, p2m);
            Console.WriteLine(gb);

            while (!gb.IsTerminal)
            {
                var nextMove = gb.Moves.First();

                Console.WriteLine("Making move: {0}", nextMove);

                gb.MakeMove(nextMove);

                Console.WriteLine(gb);
            }
        }
    }
}
