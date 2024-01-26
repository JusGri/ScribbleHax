namespace Scribble_hax.Controllers.Board
{
    /// <summary>
    /// Class for holding the scribble board matrix and remembering all letters. 
    /// </summary>
    public class Board
    {
        private char[,] valuesMatrix;
        private string[,] bonusMatrix;
        private readonly string[] possibleBonuses;

        /// <summary>
        /// Initializes the board by creating arrays and setting initial values.
        /// </summary>
        public Board()
        {
            valuesMatrix = new char[15, 15];
            possibleBonuses = new string[] { "DL", "DW", "TL", "TW" };

            BonusTileReader bonusReader = new BonusTileReader();
            bonusMatrix = bonusReader.ReadBonusTiles();
        }

        /// <summary>
        /// Gets value of a tile from given coordinates.
        /// </summary>
        /// <param name="x">Row coordinate</param>
        /// <param name="y">Column coordinate</param>
        /// <returns>String which should be displayed on the board tile.</returns>
        public string GetTileValue(int x, int y)
        {
            var tileValue = string.Empty;
            if (valuesMatrix[x, y] != (char)0)
            {
                tileValue = valuesMatrix[x, y].ToString();
            }
            else if (bonusMatrix[x,y] != string.Empty)
            {
                tileValue = bonusMatrix[x, y];
            }
            return tileValue;
        }
    }
}
