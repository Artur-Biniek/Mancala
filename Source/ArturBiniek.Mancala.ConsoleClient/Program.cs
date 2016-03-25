using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArturBiniek.Mancala.Game;

namespace ArturBiniek.Mancala.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1m = 7;
            var p1 = new[] { 1, 2, 3, 4, 5, 6 };

            var p2m = 8;
            var p2 = new[] { 9, 10, 11, 12, 13, 14 };

            var gb = new GameBoard(GameStateBase.Player.One, p1, p1m, p2, p2m);

            Console.WriteLine(gb);
        }
    }
}
