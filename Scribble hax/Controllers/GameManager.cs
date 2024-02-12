namespace Scribble_hax.Controllers
{
    /// <summary>
    /// Class responsible for returning everything that the front end needs and interacting with other internal classes.
    /// </summary>
    public class GameManager
    {
        private Board.Board gameBoard;
        private WordManager.WordManager wordManager;

        public GameManager()
        {
            gameBoard = new Board.Board();
            wordManager = new WordManager.WordManager();
        }

        /// <summary>
        /// Gets value of a tile from given coordinates.
        /// </summary>
        /// <param name="x">Row coordinate</param>
        /// <param name="y">Column coordinate</param>
        /// <returns>String which should be displayed on the board tile.</returns>
        public string GetTileValue(int x, int y)
        {
            return gameBoard.GetTileValue(x, y);
        }

        public void GetBestWords(List<char> availableChars)
        {
            var availableWords = wordManager.GetAvailableWords(availableChars, getBoardLayout(), );


        }

        private Dictionary<(int row, int column), string> getBoardLayout()
        {
            var boardLayout = new Dictionary<(int row, int column), string>();
            for (int row = 0; row < 15; row++)
            {
                for (int column = 0; column < 15; column++)
                {
                    boardLayout.Add((row,column),gameBoard.GetTileValue(row, column));
                }
            }

            return boardLayout;
        }
    }

}
