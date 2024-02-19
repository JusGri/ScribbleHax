using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

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
        public List<ChosenWord> nnnnn(List<char> availableCharacters, Dictionary<(int row,int column), string> charactersOnTheBoard)
        {
            List<ChosenWord> availableWords = new List<ChosenWord>();
            //Iterates through each word to check if it can fit on the table somewhere.
            foreach (var word in words)
            {
                //Iterates through each row and column at the same time
                for (int i = 0; i < 15; i++)
                {
                    //Add all characters of row i to charactersOnRow dictionary.
                    var charactersOnRow = new Dictionary<(int row, int column), string>();
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
                    var charactersOnColumn = new Dictionary<(int row, int column), string>();
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

            availableWords = CheckForWordsPerpendicular(charactersOnTheBoard, availableWords);

            return availableWords; 
        }

        private List<ChosenWord> ValidateWord(string word, List<char> availableCharacters, Dictionary<(int row, int column), string> charactersOnTheLine, lineAxis axis)
        {
            var wordFits = new List<ChosenWord>();

            var wordDeconstructed = new List<char>(word);

            //Check for matches in row and save which letters are still left unfilled when the word fits and where in line the word starts.
            var matchesFound = new List<(List<char> remainingLetters, int wordStart)>();
            for (int i = 0; i < wordDeconstructed.Count; i++)
            {
                var matched = charactersOnTheLine.Where(lineChar => lineChar.Value == wordDeconstructed[i].ToString());
                if (matched.Any())
                {
                    foreach (var match in matched)
                    {
                        if (WordFitsBetweenEdges(i, word, match, axis))
                        {
                            (List<char> remainingLetters, int wordStart) matchResult;
                            if (axis == lineAxis.horizontal)
                            {
                                matchResult = HandleMatch(wordDeconstructed, i,
                                    new KeyValuePair<int, string>(match.Key.column, match.Value),
                                    charactersOnTheLine.ToDictionary(k => k.Key.column, v => v.Value));
                            }
                            else
                            {
                                matchResult = HandleMatch(wordDeconstructed, i,
                                    new KeyValuePair<int, string>(match.Key.row, match.Value),
                                    charactersOnTheLine.ToDictionary(k => k.Key.row, v => v.Value));
                            }

                            //If match fit, and it's not already added because of other letters, add it to the matches.
                            if (matchResult.wordStart >= 0 && !matchesFound.Contains(matchResult))
                            {
                                matchesFound.Add(matchResult);
                            }
                        }
                    }
                }
            }

            //If no matches found in the line return empty list.
            if (!matchesFound.Any())
            {
                return new List<ChosenWord>();
            }

            //Go through matches and add chosen words without calculating the points.
            foreach (var match in matchesFound)
            {
                //Check if the word can be finished with available letters.
                var charactersStillAvailable = new List<char>(availableCharacters);
                bool missingCharacters = false;
                foreach (var letter in match.remainingLetters)
                {
                    if (charactersStillAvailable.Contains(letter))
                    {
                        charactersStillAvailable.Remove(letter);
                    }
                    else
                    {
                        missingCharacters = true;
                        break;
                    }
                }

                //If the word can't be finished with available letters, don't add it to available.
                if (missingCharacters)
                {
                    continue;
                }

                //If it can be finished add it to the list of fitting words, but no points calculation.
                ChosenWord chosenWord;
                if (axis == lineAxis.horizontal)
                {
                    int points = CalculatePoints(word, match.wordStart, charactersOnTheLine.ToDictionary(k => k.Key.column, v => v.Value));
                    chosenWord = new ChosenWord(word, (charactersOnTheLine.First().Key.row, match.wordStart),
                        (charactersOnTheLine.First().Key.row, match.wordStart + word.Length-1), points);
                }
                else
                {
                    int points = CalculatePoints(word, match.wordStart, charactersOnTheLine.ToDictionary(k => k.Key.row, v => v.Value));
                    chosenWord = new ChosenWord(word, (charactersOnTheLine.First().Key.column, match.wordStart),
                        (charactersOnTheLine.First().Key.column, match.wordStart + word.Length - 1), points);
                }
                wordFits.Add(chosenWord);
            }

            return wordFits;
        }

        private bool WordFitsBetweenEdges(int matchPosition, string word, KeyValuePair<(int row, int column), string> match, lineAxis axis)
        {
            if (axis == lineAxis.horizontal)
            {
                if (match.Key.column - matchPosition >= 0 || match.Key.column + ((word.Length - 1) - matchPosition) <= 14)
                {
                    return true;
                }
            }
            else
            {
                if (match.Key.row - matchPosition >= 0 || match.Key.row + ((word.Length - 1) - matchPosition) <= 14)
                {
                    return true;
                }
            }

            return false;
        }

        private (List<char> remainingLetters, int wordStart) HandleMatch(List<char> word, int matchPosition, KeyValuePair<int, string> match, Dictionary<int, string> charactersOnTheLine)
        {
            var copyOfWord = new List<char>(word);

            //Check if word fits in the slot it matched
            if (WordFitsBetweenOtherWords(word, matchPosition, match.Key, charactersOnTheLine))
            {
                //If fits in the slot it matched iterate through the positions on the line.
                var wordStartIndex = match.Key - matchPosition;
                var currentCharInWordIndex = 0;
                for (int i = wordStartIndex; i < wordStartIndex + word.Count; i++)
                {
                    //If there is a letter that matches the word, remove it from the remaining letters.
                    if (word[currentCharInWordIndex].ToString() == charactersOnTheLine[i])
                    {
                        copyOfWord.Remove(word[currentCharInWordIndex]);
                    }
                    //If neither the letter is matching, nor is the tile empty, return the word with a negative word start.
                    else if (charactersOnTheLine[i].Length == 1)
                    {
                        return (word, -1);
                    }

                    currentCharInWordIndex++;
                }
                //If the cycle completed that means that the word fits return the remaining letters and the index where the word starts.
                return (copyOfWord, wordStartIndex);
            }

            return (word, -1);
        }

        private bool WordFitsBetweenOtherWords(List<char> word, int matchPosition, int match, Dictionary<int, string> charactersOnTheLine)
        {
            var tileBeforeWord = match - matchPosition - 1;
            var tileAfterWord = match + word.Count - matchPosition; //NOTE: count is from 1 to N and positions are from 0 to N-1, but since we need the tile after last character, this works.
            
            //Checking if tiles before and after where the word is placed are empty, bonus tiles(more than 1 char) or out of the board.
            if ((tileBeforeWord <  0 || tileAfterWord <= 14) ||
                charactersOnTheLine[tileBeforeWord] == "" || charactersOnTheLine[tileBeforeWord].Length > 1 ||
                charactersOnTheLine[tileAfterWord] == "" || charactersOnTheLine[tileAfterWord].Length > 1)
            {
                return true;
            }

            //In case they make up a new word combined, they should be matched by the algorithm, so we can just send false here.
            return false;
        }

        private int CalculatePoints(string word, int wordStart, Dictionary<int, string> charsOnTheLine)
        {
            var points = 0;

            var currentLetterIndex = 0;
            var doubleWord = 0;
            var tripleWord = 0;
            for (int i = wordStart; i < wordStart + word.Length - 1; i++) 
            {
                switch (charsOnTheLine[i])
                {
                    case "DL":
                        points += charsToPoints[word[currentLetterIndex]] * 2;
                        break;
                    case "DW":
                        doubleWord++;
                        break;
                    case "TL":
                        points += charsToPoints[word[currentLetterIndex]] * 3;
                        break;
                    case "TW":
                        tripleWord++;
                        break;
                    default:
                        points += charsToPoints[word[currentLetterIndex]];
                        break;
                }
            }

            while (doubleWord > 0)
            {
                points *= 2;
                doubleWord--;
            }

            while (tripleWord > 0)
            {
                points *= 3;
                tripleWord--;
            }

            return points;
        }

        private List<ChosenWord> CheckForWordsPerpendicular(Dictionary<(int row, int column), string> charactersOnTheBoard, List<ChosenWord> availableWords)
        {
            List<ChosenWord> alteredAvailableWords = new List<ChosenWord>();

            foreach(ChosenWord word in availableWords)
            {
                var wordFailedCheck = false; 
                if (word.WordIsHorizontal())
                {
                    //Track the index of the letter in the word.
                    var wordIndex = 0;
                    for(int column = word.GetStartColumn();  column <= word.GetEndColumn(); column++)
                    {
                        //If the space above and bellow the letter of the word are empty check for the next letter.
                        if ((word.GetStartRow() + 1 > 14 || charactersOnTheBoard[(word.GetStartRow() + 1, column)].Length != 1) && 
                            (word.GetStartRow() - 1 < 0 || charactersOnTheBoard[(word.GetStartRow() - 1, column)].Length != 1))
                        {
                            wordIndex++;
                            continue;
                        }

                        //If it's not emtpty we have to check if they form a word.
                        StringBuilder wordCandidateBuilder = new StringBuilder();
                        int wordStart = 0;
                        for(int row = 0; row < 15; row++)
                        {
                            if (charactersOnTheBoard[(row, column)].Length == 1)
                            {
                                if(wordCandidateBuilder.Length == 0)
                                {
                                    wordStart = row;
                                }
                                wordCandidateBuilder.Append(charactersOnTheBoard[(row, column)]);
                            }
                            else if (row < word.GetStartRow())
                            {
                                wordCandidateBuilder.Clear();
                            }
                            else
                            {
                                break;
                            }
                        }

                        string wordCandidate = wordCandidateBuilder.ToString();
                        if (!words.Contains(wordCandidate))
                        {
                            wordFailedCheck = true;
                            break;
                        }

                        var additionalPoints = CalculatePoints(wordCandidate, wordStart,
                            charactersOnTheBoard.Where(chars => chars.Key.column == column).ToDictionary(k => k.Key.row, v => v.Value));

                        word.SetPoints(word.GetPoints() + additionalPoints);
                    }
                }
                else
                {

                }

                if (!wordFailedCheck)
                {
                    alteredAvailableWords.Add(word);
                }
            }

            return alteredAvailableWords;
        }
    }
}
