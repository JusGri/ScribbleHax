using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace Scribble_hax.Controllers.WordManager;

/// <summary>
/// Class which handles reading all possible words from the specified file. 
/// </summary>
public class WordReader
{
    private string wordPath = "..\\Data\\words.txt";

    /// <summary>
    /// A default constructor.
    /// </summary>
    public WordReader()
    {

    }

    /// <summary>
    /// Reads all the words from a text file where each line is a new word.
    /// </summary>
    /// <returns>A hashset of all words.</returns>
    public HashSet<string> ReadWords()
    {
        StreamReader reader = new StreamReader(wordPath);

        var words = new HashSet<string>();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Length > 15)
            {
                //Won't fit on the board
                continue;
            }
            words.Add(line);
        }

        return words;
    }

}