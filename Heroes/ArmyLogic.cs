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
        public List<KeyValuePair<Unit, int>> GenerateArmy(int[] armyNumbers)
        {
            List<Unit> unitList = new List<Unit>();

            unitList.Add(new Unit(1, "Troglodyta", 4, 3, false, 5, 1, 3, 4));
            unitList.Add(new Unit(2, "Harpia", 6, 5, false, 14, 1, 4, 6));
            unitList.Add(new Unit(3, "Złe Oko", 9, 7, true, 22, 3, 5, 5));
            unitList.Add(new Unit(4, "Meduza", 9, 9, true, 25, 6, 8, 5));
            unitList.Add(new Unit(5, "Minotaur", 14, 12, false, 50, 12, 20, 6));
            unitList.Add(new Unit(6, "Mantykora", 15, 13, false, 80, 14, 20, 7));
            unitList.Add(new Unit(7, "Smok", 19, 19, false, 180, 40, 50, 11));

            List<KeyValuePair<Unit, int>> army = new List<KeyValuePair<Unit, int>>();

            for (int i = 0; i < 7; i++)
            {
                army.Add(new KeyValuePair<Unit, int>(unitList[i], armyNumbers[i]));
            }

            return army;
        }

        /// <summary>
        /// Wygenerowanie armii z jednostkami w kolejności losowej
        /// </summary>
        /// <param name="oldArmy"> Armia do przetasowania </param>
        /// <returns> Słownik <Jednostka, liczebność> armii </returns>
        public List<KeyValuePair<Unit, int>> RandomizeArmy(List<KeyValuePair<Unit, int>> oldArmy)
        {
            List<KeyValuePair<Unit, int>> newArmy = new List<KeyValuePair<Unit, int>>();
            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                int r = random.Next(0, oldArmy.Count());
                newArmy.Add(new KeyValuePair<Unit, int>(oldArmy.ElementAt(r).Key, oldArmy.ElementAt(r).Value));
                oldArmy.Remove(oldArmy.ElementAt(r));
            }

            return newArmy;
        }

        /// <summary>
        /// Wygenerowanie pozycji startowych dla jednostek (koordynat x)
        /// </summary>
        /// <returns> Lista startowych pozycji (w przedziale od 0 do 10) </returns>
        public List<int> generateStartingPositions()
        {
            Random random = new Random();
            List<int> positions = new List<int>();

            while(positions.Count() < 7)
            {
                int r = random.Next(0, 11);

                if (!positions.Contains(r))
                {
                    positions.Add(r);
                }
            }

            return positions;
        }
    }

    public class GameHelper
    {
        /// <summary>
        /// Wybranie najwolniejszego przeciwnika z wrogiej armii
        /// </summary>
        /// <param name="player"> Obecny "gracz" </param>
        /// <param name="units"> Lista jednostek </param>
        /// <returns> Najwolniejsza jednostka </returns>
        public Field getSlowestEnemy(bool player, List<Field> units)
        {
            var result = units
                .Where(u => u.unit.Key != null)
                .Where(u => u.unit.Key.player == !player)
                .Where(u => u.unit.Key.total_hp > 0)
                .OrderBy(u => u.unit.Key.speed)
                .FirstOrDefault();

            return result;
        }


        /// <summary>
        /// Obliczenie obrażeń
        /// </summary>
        /// <param name="attack"> Premia do ataku napastnika </param>
        /// <param name="defense"> Premia do obrony celu </param>
        /// <param name="minDamage"> Minimalne obrażenia napastnika </param>
        /// <param name="maxDamage"> Maksymalne obrażenia napastnika </param>
        /// <param name="units"> Ilość atakujących jednostek </param>
        /// <returns> Wartość obrażeń </returns>
        public double calculateDamage(byte attack, byte defense, byte minDamage, byte maxDamage, int units)
        {
            Random random = new Random();            

            double damageModifier = ((double)attack - (double)defense) / 100;
            double randomizedDamage = random.Next(minDamage, maxDamage);
            double additionalDamage = randomizedDamage * damageModifier;
            double damage = (randomizedDamage + additionalDamage) * units;

            return damage;
        }

        public void logger(string message, ConsoleColor color)
        {

        }
    }
}
