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
        private readonly int pointsForWord;

        public ChosenWord(string word, (int row, int column) startCoordinate, (int row, int column) endCoordinate, int points)
        {
            this.word = word;
            this.startCoordinate = startCoordinate;
            this.endCoordinate = endCoordinate;
            this.pointsForWord = points;
        }
    }
}
