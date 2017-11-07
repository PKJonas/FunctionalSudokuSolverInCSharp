using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalSudokuSolver
{
    class SudokuSolver
    {
        public const int ROW_LENGTH = 9;
        public const int SECTOR_SIZE = 3;
        public static ImmutableList<int> LegalCellValues = 
            ImmutableList.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);

        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            SudokuSolver solver = new SudokuSolver();

            ImmutableList<int> solvedBoard = solver.Solve(puzzle);

            for (var i = 0; i < solvedBoard.Count(); i++)
            {
                if (i % ROW_LENGTH == 0) Console.Write("\n");
                Console.Write($"{solvedBoard[i]} ");
            }

            Console.WriteLine($"\n Solution took {sw.Elapsed} to find");
            Console.ReadKey();
        }

        private static ImmutableList<int> puzzle = ImmutableList.Create<int>(

        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0,
        //0, 0, 0, 0, 0, 0, 0, 0, 0

        1, 0, 3, 4, 0, 6, 0, 0, 0,
        0, 6, 2, 0, 7, 0, 0, 0, 1,
        0, 5, 0, 1, 0, 0, 0, 3, 7,
        2, 0, 9, 0, 1, 0, 0, 0, 8,
        0, 1, 6, 0, 0, 0, 3, 4, 0,
        5, 0, 0, 7, 3, 0, 0, 6, 0,
        4, 8, 0, 3, 0, 0, 0, 1, 0,
        3, 0, 0, 2, 0, 0, 0, 8, 4,
        0, 2, 0, 8, 4, 0, 0, 7, 0


        //8, 9, 0, 0, 0, 7, 0, 0, 0,
        //0, 0, 0, 0, 2, 0, 3, 0, 0,
        //6, 0, 0, 0, 8, 0, 0, 4, 0,
        //0, 0, 8, 0, 0, 9, 0, 0, 0,
        //0, 1, 0, 7, 0, 0, 0, 0, 0,
        //0, 5, 0, 0, 0, 0, 8, 1, 6,
        //0, 7, 0, 0, 0, 0, 0, 6, 3,
        //0, 0, 0, 0, 0, 5, 0, 0, 1,
        //0, 0, 0, 4, 0, 0, 0, 0, 2
        );

        public ImmutableList<int> Solve(ImmutableList<int> currentGameboard, int index = 0)
        {
            if (index == currentGameboard.Count() || !IsSolvable(currentGameboard)) return currentGameboard;

            if (currentGameboard[index] == 0)
            {
                foreach (int possibleNumber in GetRemainingCellValues(currentGameboard, index))
                {
                    ImmutableList<int> attempt = Solve(currentGameboard.SetItem(index, possibleNumber), index);

                    if (IsSolved(attempt))
                        return attempt;
                }
            }
            else
            {
                return Solve(currentGameboard, ++index);
            }
            return currentGameboard;
        }

        public bool IsSolved(ImmutableList<int> board) =>
            board.Where(cell => cell == 0).Count() == 0;

        public bool IsSolvable(ImmutableList<int> board) =>
            GetAllRemainingCellPossibilities(board).Where((cellPossibilities, cidx) => (cellPossibilities.Count() == 0 && board[cidx] == 0)).Count() == 0;

        private int GetRow(int index) => index / ROW_LENGTH;
        private int GetColumn(int index) => index - GetRow(index) * ROW_LENGTH;
        private string GetElementSector(int index) => $"{GetRow(index) / SECTOR_SIZE}_{GetColumn(index) / SECTOR_SIZE}";

        public ImmutableList<ImmutableList<int>> GetAllRemainingCellPossibilities(ImmutableList<int> currentGameboard) =>
            currentGameboard.Select((cell, cidx) => GetRemainingCellValues(currentGameboard, cidx)).ToImmutableList();

        public ImmutableList<int> GetRemainingCellValues(ImmutableList<int> currentGameboard, int index) =>
            LegalCellValues.Except(
                currentGameboard.Where((cell, cidx) => 
                                       (GetColumn(cidx) == GetColumn(index) ||
                                        GetRow(cidx) == GetRow(index) ||
                                        GetElementSector(cidx) == GetElementSector(index)))
                                  ).ToImmutableList();  
    }
}
