namespace Heroes
{
    public class ArmyGenerator
    {
        /// <summary>
        /// Wygenerowanie liczebności armii
        /// </summary>
        /// <returns> Tablica z liczebnością jednostek posortowane od poziomu 1 do 7 </returns>
        public int[] GenerateArmyNumbers()
        {
            int[] armyNumbers = new int[7];

            Random random = new Random();

            for (int level = 0; level < 7; level++)
            {
                armyNumbers[level] = random.Next((int)Math.Pow(2, 8 - level)) + 1;
            }

            return armyNumbers;
        }

        /// <summary>
        /// Wygenerowanie armii z jednostkami w kolejności poziomów (od 1 do 7)
        /// </summary>
        /// <param name="unitList"> Lista jednostek jakie mają znaleźć się w armii (musi być 7 elementów) </param>
        /// <param name="armyNumbers"> Tablica z liczebnością jednostek w armii (musi być 7 elementów) </param>
        /// <returns> Słownik <Jednostka, liczebność> armii </returns>
        public Dictionary<Unit, int> GenerateArmy(List<Unit> unitList, int[] armyNumbers)
        {
            Dictionary<Unit, int> army = new Dictionary<Unit, int>();

            for (int i = 0; i < 7; i++)
            {
                army.Add(unitList[i], armyNumbers[i]);
            }

            return army;
        }

        /// <summary>
        /// Wygenerowanie armii z jednostkami w kolejności losowej
        /// </summary>
        /// <param name="oldArmy"> Armia do przetasowania </param>
        /// <returns> Słownik <Jednostka, liczebność> armii </returns>
        public Dictionary<Unit, int> RandomizeArmy(Dictionary<Unit, int> oldArmy)
        {
            Dictionary<Unit, int> newArmy = new Dictionary<Unit, int>();
            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                int r = random.Next(0, oldArmy.Count());
                newArmy.Add(oldArmy.ElementAt(r).Key, oldArmy.ElementAt(r).Value);
                oldArmy.Remove(oldArmy.ElementAt(r).Key);
            }

            return newArmy;
        }
    }
}
