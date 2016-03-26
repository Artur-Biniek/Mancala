using System;
using ArturBiniek.Mancala.Game;

namespace ArturBiniek.Mancala.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1m = 0;
            var p1 = new[] { 1, 0, 1 };

            var p2m = 0;
            var p2 = new[] { 2, 3, 4 };

            var gb = new GameBoard(GameStateBase.Player.One, p1, p1m, p2, p2m);

            Console.WriteLine(gb);

            foreach (var moe in gb.Moves)
            {
                Console.WriteLine(moe);
            }


            Console.WriteLine("\n::::::::::::::::::::::::::::::::::::::::::::::\n");


            foreach (var moe in gb.Moves)
            {
                gb.MakeMove(moe);
                Console.WriteLine("Making move: {0}\n -> {1}", moe, gb);
                gb.UndoMove(moe);
                Console.WriteLine("Undoing move: {0}\n -> {1}", moe, gb);
                Console.WriteLine("------------------------------------");
            }
        }
    }
}
