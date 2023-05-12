namespace Heroes
{
    public class BoardLogic
    {
        /// <summary>
        /// Generowanie planszy do gry
        /// </summary>
        /// <returns> Plansza </returns>
        public List<Field> generateGameBoard()
        {
            List<Field> board = new List<Field>();

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    board.Add(new Field(i, j, new KeyValuePair<Unit, int>(null, 0)));
                }
            }

            return board;
        }
    }
}
