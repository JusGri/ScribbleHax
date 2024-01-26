using System.Reflection;

namespace Scribble_hax.Controllers.Board
{
    /// <summary>
    /// Class that handles reading bonus tile values from a file.
    /// </summary>
    public class BonusTileReader
    {
        private string bonusTileBoardFilePath = "..\\Data\\board.txt";

        /// <summary>
        /// A default constructor.
        /// </summary>
        public BonusTileReader() { 
            
        }

        /// <summary>
        /// Reads bonus tiles from 15x15 format text file where each column is divided by commas and row by new lines.
        /// </summary>
        /// <returns>A double array with the tile value corresponding to its row and column number.</returns>
        public string[,] ReadBonusTiles()
        {
            StreamReader reader = new StreamReader(bonusTileBoardFilePath);

            string[,] bonusTiles = new string[15,15]; 

            string line;
            int lineIndex = 0;
            while ((line = reader.ReadLine()) != null)
            {
                var singleLineBonuses = line.Split(';');
                var columnIndex = 0;
                foreach (var tile in singleLineBonuses)
                {
                    if (tile != "n/a")
                    {
                        bonusTiles[lineIndex, columnIndex] = tile;
                    }
                    else
                    {
                        bonusTiles[lineIndex, columnIndex] = string.Empty;
                    }

                    columnIndex++;
                }
                lineIndex++;
            }

            return bonusTiles;
        }
    }
}
