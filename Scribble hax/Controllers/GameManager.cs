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
    }

}
