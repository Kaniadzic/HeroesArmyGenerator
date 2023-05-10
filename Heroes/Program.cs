using Heroes;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        int turnCounter = 0;
        GameHelper helper = new GameHelper();

        #region Generowanie armii

        ArmyGenerator armyGenerator = new ArmyGenerator();

        // generowanie armii 1
        int[] armyNumbers1 = armyGenerator.GenerateArmyNumbers();
        List<KeyValuePair<Unit, int>> army1 = armyGenerator.GenerateArmy(armyNumbers1);
        foreach(var unit in army1)
        {
            unit.Key.player = true;
            unit.Key.total_hp = unit.Key.hp * unit.Value;
        }

        // generowanie armii 2
        int[] armyNumbers2 = armyGenerator.GenerateArmyNumbers();
        List<KeyValuePair<Unit, int>> army2 = armyGenerator.GenerateArmy(armyNumbers2);
        foreach (var unit in army2)
        {
            unit.Key.player = false;
            unit.Key.total_hp = unit.Key.hp * unit.Value;
        }

        #endregion

        #region Generowanie planszy, pozycji startowych i sortowanie armii (według szybkości jednostek)

        // generowanie planszy
        List<Field> board = new List<Field>();

        for(int i=0; i < 15; i++)
        {
            for(int j=0; j < 11; j++)
            {
                board.Add(new Field(i, j, new KeyValuePair<Unit, int>(null, 0)));
            }
        }

        // generowanie pozycji startowych
        var startingPositionsArmy1 = armyGenerator.generateStartingPositions();
        var startingPositionsArmy2 = armyGenerator.generateStartingPositions();

        for(int i = 0; i < army1.Count(); i++)
        {
            board.Where(f => f.y == startingPositionsArmy1[i] & f.x == 0).FirstOrDefault().unit = army1.ElementAt(i);
            board.Where(f => f.y == startingPositionsArmy2[i] & f.x == 14).FirstOrDefault().unit = army2.ElementAt(i);
        }

        // wyświetlenie początkowych armii
        helper.logStartingArmies(board.Where(x => x.unit.Key != null));

        #endregion

        #region Przygotowania do bitwy

        var sortedUnits = board
           .Where(x => x.unit.Key != null)
           .OrderByDescending(x => x.unit.Key.speed);

        int existingArmies = board
            .Where(x => x.unit.Key != null)
            .Select(x => x.unit.Key.player)
            .Distinct()
            .Count();

        #endregion

        #region Bitwa

        // Rozgrywka toczy się dopóki na planszy są dwie armie
        while (existingArmies == 2)
        {
            turnCounter += 1; // licznik tur

            existingArmies = sortedUnits
                .Where(x => x.unit.Key != null)
                .Where(x => x.unit.Key.total_hp > 0)
                .Select(x => x.unit.Key.player)
                .Distinct()
                .Count();

            // sprawdzenie czy istnieją jeszcze żywe jednostki
            foreach (var u in sortedUnits)
            {
                if (u.unit.Key.total_hp <= 0) // jeśli jednostka jest martwa to ją pomijamy
                {
                    continue;
                }
                else // walka
                {
                    // walka dystansowa
                    if (u.unit.Key.ranged == true)
                    {
                        // wybranie celu
                        var target = helper.getSlowestEnemy(u.unit.Key.player, board);

                        if (target == null)
                        {
                            continue;
                        }

                        // obliczenie obrażeń
                        double damage = helper.calculateDamage(
                            u.unit.Key.attack,
                            target.unit.Key.defense,
                            u.unit.Key.minDamage,
                            u.unit.Key.maxDamage,
                            u.unit.Key.total_hp / u.unit.Key.hp
                        );

                        // zadanie obrażeń
                        target.unit.Key.total_hp -= (int)damage;
                    }
                    else // walka w zwarciu
                    {
                        //helper.getEnemiesInRange(u.unit.Key.player, u.x, u.y, u.unit.Key.speed, board);

                        helper.getEnemiesInRange(u.unit.Key.player, 0, 0, 2, board);

                    }


                }
            }
        }

        #endregion
    }
}
