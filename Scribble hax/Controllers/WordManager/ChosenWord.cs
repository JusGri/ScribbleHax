
namespace Scribble_hax.Controllers.WordManager
{
    /// <summary>
    /// Class for handling data of words that were chosen as available to be placed on the board.
    /// Data includes:
    /// *String of the word.
    /// *Starting coordinate of the word.
    /// *End coordinate of the word.
    /// *Points that will be rewarded for using the word.
    /// </summary>
    public class ChosenWord
    {
        private readonly string word;
        private readonly (int row, int column) startCoordinate;
        private readonly (int row, int column) endCoordinate;
        private int pointsForWord;

        /// <summary>
        /// Initiates the selected word object.
        /// </summary>
        /// <param name="word">the word string</param>
        /// <param name="startCoordinate">where the word starts in the table</param>
        /// <param name="endCoordinate">where the word ends in the table</param>
        /// <param name="points">points that are attributed to the word</param>
        public ChosenWord(string word, (int row, int column) startCoordinate, (int row, int column) endCoordinate, int points)
        {
            this.word = word;
            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;
            this.pointsForWord = points;
        }

        /// <summary>
        /// Gets the word string value.
        /// </summary>
        /// <returns>Word string.</returns>
        public string GetWord()
        {
            return word;
        }

        /// <summary>
        /// Full coordinate where the word starts on the table.
        /// </summary>
        /// <returns>Tuple of row and column ints.</returns>
        public (int row, int column) GetStartCoordinate()
        {
            return startCoordinate;
        }

        /// <summary>
        /// Index of the row where the word starts on the table.
        /// </summary>
        /// <returns>Int of the row</returns>
        public int GetStartRow()
        {
            return startCoordinate.row;
        }

        /// <summary>
        /// Index of the column where the word starts on the table.
        /// </summary>
        /// <returns>Int of the column</returns>
        public int GetStartColumn()
        {
            return startCoordinate.column;
        }

        /// <summary>
        /// Full coordinate where the word ends on the table.
        /// </summary>
        /// <returns>Tuple of row and column ints.</returns>
        public (int row, int column) GetEndCoordinate()
        {
            return endCoordinate;
        }

        /// <summary>
        /// Index of the row where the word ends on the table.
        /// </summary>
        /// <returns>Int of the row</returns>
        public int GetEndRow()
        {
            return endCoordinate.row;
        }

        /// <summary>
        /// Index of the column where the word ends on the table.
        /// </summary>
        /// <returns>Int of the column</returns>
        public int GetEndColumn()
        {
            return endCoordinate.column;
        }

        /// <summary>
        /// Indicates if the word is placed horizontaly on the table.
        /// </summary>
        /// <returns>True if word is horizontal on the table.</returns>
        public bool WordIsHorizontal()
        {
            if(startCoordinate.row == endCoordinate.row)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points attributed to word.
        /// </summary>
        /// <returns>Int number of how many points are given for placing this word on the coordinates.n</returns>
        public int GetPoints()
        {
            return pointsForWord;
        }

        /// <summary>
        /// Sets points for the word when needed.
        /// </summary>
        /// <param name="points">New value for points</param>
        public void SetPoints(int points)
        {
            pointsForWord = points;
        }

        /// <summary>
        /// Returns a string of all the needed information for the player to place this word.
        /// </summary>
        /// <returns>String of all the info about the word.</returns>
        public override string ToString()
        {
            return $"Word: {word}; \nPoints: {pointsForWord}; \nStart coordinate row: {startCoordinate.row}, column: {startCoordinate.column}; \nEnd coordinate row: {endCoordinate.row}, column: {endCoordinate.column}; \n";
        }
    }
}
