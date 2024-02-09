using System.Data;
using System.Net;

namespace Scribble_hax.Controllers.WordManager
{
    /// <summary>
    /// Class for managing actions related to words/characters and calculating points for them.
    /// </summary>
    public class WordManager
    {
        private enum lineAxis
        {
            horizontal,
            vertical
        }
        private HashSet<string> words;
        private Dictionary<char, int> charsToPoints;

        /// <summary>
        /// Initializes word manager by reading the all possible words and characters into data structures.
        /// </summary>
        public WordManager()
        {
            WordReader wordReader = new WordReader();
            CharacterReader characterReader = new CharacterReader();

            words = wordReader.ReadWords();
            charsToPoints = characterReader.ReadCharacters();
        }

        /// <summary>
        /// Returns all possible words with their respective points.
        /// </summary>
        /// <param name="availableCharacters"></param>
        /// <param name="charactersOnTheBoard"></param>
        /// <returns>A dictionary where words are keys and point that you can get for them are values.</returns>
        public List<ChosenWord> GetAvailableWords(List<char> availableCharacters, Dictionary<(int row,int column), char> charactersOnTheBoard)
        {
            List<ChosenWord> availableWords = new List<ChosenWord>();
            //Iterates through each word to check if it can fit on the table somewhere.
            foreach (var word in words)
            {
                //Iterates through each row and column at the same time
                for (int i = 0; i < 15; i++)
                {
                    //Add all characters of row i to charactersOnRow dictionary.
                    var charactersOnRow = new Dictionary<(int row, int column), char>();
                    for (int column = 0; column < 15; column++)
                    {
                        charactersOnRow.Add((i, column), charactersOnTheBoard[(i, column)]);
                    }

                    var chosenOnRow = ValidateWord(word, availableCharacters, charactersOnRow, lineAxis.horizontal);
                    if (chosenOnRow.Any())
                    {
                        availableWords.AddRange(chosenOnRow);
                    }

                    //Add all characters on column i to charactersOnColumn dictionary.
                    var charactersOnColumn = new Dictionary<(int row, int column), char>();
                    for (int row = 0; row < 15; row++)
                    {
                        charactersOnColumn.Add((row, i), charactersOnTheBoard[(row, i)]);
                    }

                    var chosenOnColumn = ValidateWord(word, availableCharacters, charactersOnColumn, lineAxis.vertical);
                    if (chosenOnColumn.Any())
                    {
                        availableWords.AddRange(chosenOnColumn);
                    }
                }
            }
            return availableWords; 
        }

        private List<ChosenWord> ValidateWord(string word, List<char> availableCharacters, Dictionary<(int row, int column), char> charactersOnTheLine, lineAxis axis)
        {
            List<char> localAvailableChars = new List<char>(availableCharacters);

            var charArray = new List<char>(word);

            if (!charArray.Contains(boardChar.Value))
            {
                return false;
            }

            var repeatedBoardChar = charArray.Count(ch => ch == boardChar.Value);
            if (repeatedBoardChar > 1)
            {

            }


            foreach (var character in charArray)
            {
                if (localAvailableChars.Contains(character))
                {
                    localAvailableChars.Remove(character);
                }
                else
                {
                    if (localAvailableChars.Contains('*'))
                    {
                        localAvailableChars.Remove('*');
                        continue;
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
