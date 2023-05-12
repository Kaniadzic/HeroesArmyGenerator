namespace Heroes
{
    public class GameLogic
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
        /// Wybranie przeciwnika z najmniejszą obroną z wrogiej armii
        /// </summary>
        /// <param name="player"> Obecny "gracz" </param>
        /// <param name="units">  Lista jednostek </param>
        /// <returns> Najsłabiej broniąca się jednostka </returns>
        public Field getLowestDefenseEnemy(bool player, List<Field> units)
        {
            var result = units
                .Where(u => u.unit.Key != null)
                .Where(u => u.unit.Key.player == !player)
                .Where(u => u.unit.Key.total_hp > 0)
                .OrderBy(u => u.unit.Key.defense)
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Wybranie przeciwnika z najmniejszym zdrowiem
        /// </summary>
        /// <param name="player"> Obecny "gracz" </param>
        /// <param name="units">  Lista jednostek </param>
        /// <returns> Najsłabsza jednostka </returns>
        public Field getLowestHealthEnemy(bool player, List<Field> units)
        {
            var result = units
                .Where(u => u.unit.Key != null)
                .Where(u => u.unit.Key.player == !player)
                .Where(u => u.unit.Key.total_hp > 0)
                .OrderBy(u => u.unit.Key.total_hp)
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

        /// <summary>
        /// Wyświetlenie początkowych armii
        /// </summary>
        /// <param name="units"> Jednostki do wyświetlenia </param>
        public void logStartingArmies(IEnumerable<Field> units)
        {
            foreach (var u in units)
            {
                if (u.unit.Key.player)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.WriteLine($"Gracz {(u.unit.Key.player ? "czerwony" : "niebieski")} jednostka {u.unit.Key.name} w liczbie: {u.unit.Value} HP: {u.unit.Key.total_hp} jest na pozycji X: {u.x} Y: {u.y}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turnCounter"></param>
        public void logGameResult(List<Field> board, int turnCounter)
        {
            Console.WriteLine("\nStan po bitwie: ");
            logStartingArmies(board.Where(x => x.unit.Key != null));

            Console.WriteLine($"\nBitwa trwała {turnCounter} tur!");

            bool winner = board
                .Where(u => u.unit.Key != null)
                .Where(u => u.unit.Key.total_hp > 0)
                .Select(u => u.unit.Key.player)
                .FirstOrDefault();

            if (winner)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wygrywa gracz czerwony!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Wygrywa gracz niebieski!");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void logAttack(Field attacker, Field target, int damage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"{(attacker.unit.Key.player ? "czerwony" : "niebieski")} {attacker.unit.Key.name} atakuje {(!attacker.unit.Key.player ? "czerwony" : "niebieski")} {target.unit.Key.name} za {damage}!");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Wyszukanie wszystkich pól w zasięgu
        /// </summary>
        /// <param name="x"> Pozycja x jednostki </param>
        /// <param name="y"> Pozycja y jednostki </param>
        /// <param name="board"> Plansza </param>
        /// <returns> Lista pól w zasięgu jednostki </returns>
        public List<Field> getFieldsInRange(int x, int y, List<Field> board)
        {
            List<Field> result = new List<Field>();

            if (y % 2 == 0)
            {
                var n1 = board
                    .Where(n => n.x == x - 1)
                    .Where(n => n.y == y - 1)
                    .FirstOrDefault();

                var n2 = board
                    .Where(n => n.x == x)
                    .Where(n => n.y == y - 1)
                    .FirstOrDefault();

                var n3 = board
                    .Where(n => n.x == x + 1)
                    .Where(n => n.y == y)
                    .FirstOrDefault();

                var n4 = board
                    .Where(n => n.x == x)
                    .Where(n => n.y == y + 1)
                    .FirstOrDefault();

                var n5 = board
                    .Where(n => n.x == x - 1)
                    .Where(n => n.y == y + 1)
                    .FirstOrDefault();

                var n6 = board
                    .Where(n => n.x == x - 1)
                    .Where(n => n.y == y)
                    .FirstOrDefault();

                if (!result.Contains(n1) && n1 != null)
                {
                    result.Add(n1);
                }
                if (!result.Contains(n2) && n2 != null)
                {
                    result.Add(n2);
                }
                if (!result.Contains(n3) && n3 != null)
                {
                    result.Add(n3);
                }
                if (!result.Contains(n4) && n4 != null)
                {
                    result.Add(n4);
                }
                if (!result.Contains(n5) && n5 != null)
                {
                    result.Add(n5);
                }
                if (!result.Contains(n6) && n6 != null)
                {
                    result.Add(n6);
                }
            }
            else
            {
                var n1 = board
                    .Where(n => n.x == x)
                    .Where(n => n.y == y - 1)
                    .FirstOrDefault();

                var n2 = board
                    .Where(n => n.x == x + 1)
                    .Where(n => n.y == y - 1)
                    .FirstOrDefault();

                var n3 = board
                    .Where(n => n.x == x + 1)
                    .Where(n => n.y == y)
                    .FirstOrDefault();

                var n4 = board
                    .Where(n => n.x == x + 1)
                    .Where(n => n.y == y + 1)
                    .FirstOrDefault();

                var n5 = board
                    .Where(n => n.x == x)
                    .Where(n => n.y == y + 1)
                    .FirstOrDefault();

                var n6 = board
                    .Where(n => n.x == x - 1)
                    .Where(n => n.y == y)
                    .FirstOrDefault();

                if (!result.Contains(n1) && n1 != null)
                {
                    result.Add(n1);
                }
                if (!result.Contains(n2) && n2 != null)
                {
                    result.Add(n2);
                }
                if (!result.Contains(n3) && n3 != null)
                {
                    result.Add(n3);
                }
                if (!result.Contains(n4) && n4 != null)
                {
                    result.Add(n4);
                }
                if (!result.Contains(n5) && n5 != null)
                {
                    result.Add(n5);
                }
                if (!result.Contains(n6) && n6 != null)
                {
                    result.Add(n6);
                }
            }

            return result;
        }
    }
}
