namespace Scribble_hax.Controllers.Board
{
    /// <summary>
    /// Class for holding the scribble board matrix and remembering all letters. 
    /// </summary>
    public class Board
    {
        private char[,] valuesMatrix;
        private char[,] bonusMatrix;

        public Board()
        {
            valuesMatrix = new char[15, 15];
            bonusMatrix = new char[15, 15];

            BonusTileReader bonusReader = new BonusTileReader();
        }
    }
}
