namespace Scribble_hax.Controllers.WordManager;

/// <summary>
/// Class which handles reading all characters and their respective points.
/// </summary>
public class CharacterReader
{
    private string wordPath = "..\\Data\\characters.txt";

    /// <summary>
    /// A default constructor.
    /// </summary>
    public CharacterReader()
    {

    }

    /// <summary>
    /// Reads all possible chars from a text file where each char with their respective point number are on separate rows.
    /// Chars and points are separated by a space.
    /// </summary>
    /// <returns>Dictionary where character is the key and their point value is the value.</returns>
    public Dictionary<char, int> ReadCharacters()
    {
        StreamReader reader = new StreamReader(wordPath);

        var chars = new Dictionary<char, int>();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var splitLine = line.Split(" ");
            chars.Add(char.Parse(splitLine.First()), int.Parse(splitLine.Last()));
        }

        return chars;
    }
}