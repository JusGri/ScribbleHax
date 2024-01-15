namespace Scribble_hax.Controllers.Board
{
    public class BonusTileReader
    {
        private string bonusTileBoardFilePath = ".\\Scribble hax\\bin"; 
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

                    columnIndex++;
                }
                lineIndex++;
            }
        }
    }
}
