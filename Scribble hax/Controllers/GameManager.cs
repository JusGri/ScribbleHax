
using Scribble_hax.Helpers.Enums;

namespace Scribble_hax.Controllers
{
    /// <summary>
    /// Class responsible for returning everything that the front end needs and interacting with other internal classes.
    /// </summary>
    public class GameManager
    {
        private Board.Board gameBoard;
        private WordManager.WordManager wordManager;
        private GameState gameState;
        private string currentInstructions = "If you are starting type \"y\" else type \"n\"";

        public GameManager()
        {
            gameBoard = new Board.Board();
            wordManager = new WordManager.WordManager();
            gameState = GameState.GameStart;
        }

        public string GetInstructions()
        {
            return currentInstructions;
        }

        private List<List<char>> availableCharsOfEachRound = new List<List<char>>();

        public void HandleUserInput(string? response)
        {
            if(response == null || response == string.Empty)
            {
                currentInstructions = "No input detected \n" + currentInstructions;
                return;
            }

            switch (gameState)
            {
                case GameState.GameStart:
                    if (response == "y")
                    {
                        currentInstructions = "Write your current letters with \";\" in between";
                        gameState = GameState.PlayerGivesChars;
                    }
                    else if (response == "n")
                    {
                        currentInstructions = "Write your opponents word in this format: \"word;startRow;startColumn\" (Rows and columns are from 0 to 14).";
                        gameState = GameState.OpponentWordInput;
                    }
                    else
                    {
                        currentInstructions = "Incorrect response format. Just write a single character \"y\" for yes \"n\" for no.";
                    }
                    break;

                case GameState.PlayerGivesChars:
                    var stringListOfChars = response.Split(';');
                    if(stringListOfChars == null || !stringListOfChars.Any() || stringListOfChars.Any(ch => ch.Length != 1)){
                        currentInstructions = "Incorrect response format. Write your current letters with \";\" in between. E.g.: a;b;c;d (case is not important)";
                        break;
                    }
                    var listOfChars = stringListOfChars.Select(ch => ch[0]).ToList();

                    availableCharsOfEachRound.Add(listOfChars);

                    var bestWords = wordManager.GetAvailableWords(listOfChars, getBoardLayout());

                    currentInstructions = "Top 3 best words: \n";

                    foreach(var chosenWord in bestWords.Take(3))
                    {
                        currentInstructions += chosenWord.ToString() + " \n";
                    }

                    currentInstructions += "Write your chosen word int this format: word;startRow;startColumn;axis(horizontal/vertical) (Rows and columns are from 0 to 14).";
                    gameState = GameState.PlayerChoosesWord;
                    break;

                case GameState.PlayerChoosesWord:
                    var stringList = response.Split(";");
                    var formattingError = "Formatting error. Write your chosen word int this format: word;startRow;startColumn;axis(horizontal/vertical). ";
                    if (stringList.Length < 4)
                    {
                        currentInstructions = formattingError + "Are you sure you wrote everything?";
                        break;
                    }

                    //TODO: check if the word is made up of the characters that the user gave.
                    var word = stringList[0];
                    if (!wordManager.IsValidWord(word))
                    {
                        currentInstructions = formattingError + "The list of words does not contain this word, are you sure it's valid?";
                        break;
                    }

                    int startRow = -1;
                    int startColumn = -1;
                    try
                    {
                        startRow = int.Parse(stringList[1]);
                        startColumn = int.Parse(stringList[2]);
                    }catch
                    {
                        currentInstructions = formattingError + "Could not parse the coordinates into numbers.";
                        break;
                    }

                    if(startRow > 14 || startColumn > 14 || startRow < 0 || startColumn < 0)
                    {
                        currentInstructions = formattingError + "Coordinates don't fit on the board.";
                        break;
                    }

                    Axis axis;
                    if (!Enum.TryParse<Axis>(stringList[3], true, out axis))
                    {
                        currentInstructions = formattingError + "Can't determine if axis is horizontal or vertical, did you spell it correctly?";
                    }

                    if (gameBoard.AddWord(word, (startRow, startColumn), axis) != "success")
                    {

                    }

                    break;

                case GameState.OpponentWordInput: 
                    break;
            }
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
            var availableWords = wordManager.GetAvailableWords(availableChars, getBoardLayout());


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
