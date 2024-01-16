using System.Reflection;

namespace Scribble_hax.Controllers.Board
{
    public class BonusTileReader
    {
        private string bonusTileBoardFilePath = ".\\Data\\board.txt"; 
        public BonusTileReader() { 
            
        }

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
