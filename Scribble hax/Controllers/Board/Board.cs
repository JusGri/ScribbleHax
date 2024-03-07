using Scribble_hax.Helpers.Enums;

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

        /// <summary>
        /// Tries to add a word onto the board.
        /// </summary>
        /// <param name="word">Word to be added in string format.</param>
        /// <param name="startCoordinate">A tuple of the starting row and column of the word.</param>
        /// <param name="axis">Indication if the word is placed horizontally or vertically.</param>
        /// <returns>"success" if the placement was successful and an error message if unsuccessful.</returns>
        public string AddWord(string word, (int row, int column) startCoordinate, Axis axis)
        {
            switch (axis)
            {
                case Axis.horizontal:
                    if (startCoordinate.column + word.Length - 1 > 14)
                    {
                        return "Word is too long can't fit on the row.";
                    }

                    var localValuesMatrix = new char[15, 15];
                    for (int i = 0; i < valuesMatrix.GetLength(0)/*rows*/; i++)
                    {
                        for (int j = 0; j < valuesMatrix.GetLength(1)/*columns*/; j++)
                        {
                            localValuesMatrix[i, j] = valuesMatrix[i, j];
                        }
                    }

                    var wordCharIndex = 0;
                    for (int i = startCoordinate.column; i < startCoordinate.column + word.Length; i++)
                    {
                        if (localValuesMatrix[startCoordinate.row, i] != '\0' &&
                            localValuesMatrix[startCoordinate.row, i] != word[wordCharIndex])
                        {
                            return
                                $"{wordCharIndex + 1} letter of the word can't fit, because it's spot is taken by a different letter.";
                        }
                        localValuesMatrix[startCoordinate.row, i] = word[wordCharIndex];

                        wordCharIndex++;
                    }

                    valuesMatrix = localValuesMatrix;

                    break;
                case Axis.vertical:
                    if (startCoordinate.row + word.Length - 1 > 14)
                    {
                        return "Word is too long can't fit on the column.";
                    }

                    break;
            }

            return "success";
        }
    }
}
