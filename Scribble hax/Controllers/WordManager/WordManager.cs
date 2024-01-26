namespace Scribble_hax.Controllers.WordManager
{
    /// <summary>
    /// Class for managing actions related to words/characters and calculating points for them.
    /// </summary>
    public class WordManager
    {
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
    }
}
