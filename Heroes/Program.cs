using Heroes;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        int turnCounter = 0;
        GameLogic helper = new GameLogic();
        ArmyLogic armyGenerator = new ArmyLogic();
        BoardLogic boardLogic = new BoardLogic();

        #region Generowanie armii

        // generowanie armii 1
        int[] armyNumbers1 = armyGenerator.GenerateArmyNumbers();
        List<KeyValuePair<Unit, int>> army1 = armyGenerator.GenerateArmy(armyNumbers1, true);

        // generowanie armii 2
        int[] armyNumbers2 = armyGenerator.GenerateArmyNumbers();
        List<KeyValuePair<Unit, int>> army2 = armyGenerator.GenerateArmy(armyNumbers2, false);

        #endregion

        #region Generowanie planszy, pozycji startowych i sortowanie armii (według szybkości jednostek)

        // generowanie planszy
        List<Field> board = boardLogic.generateGameBoard();

        // generowanie pozycji startowych
        var startingPositionsArmy1 = armyGenerator.generateStartingPositions();
        var startingPositionsArmy2 = armyGenerator.generateStartingPositions();

        // wypełnienie planszy jednostkami
        for(int i = 0; i < army1.Count(); i++)
        {
            board.Where(f => f.y == startingPositionsArmy1[i] & f.x == 0).FirstOrDefault().unit = army1.ElementAt(i);
            board.Where(f => f.y == startingPositionsArmy2[i] & f.x == 14).FirstOrDefault().unit = army2.ElementAt(i);
        }

        // wyświetlenie początkowych armii
        Console.WriteLine("Stan przed bitwą: ");
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
            existingArmies = sortedUnits
                .Where(x => x.unit.Key != null)
                .Where(x => x.unit.Key.total_hp > 0)
                .Select(x => x.unit.Key.player)
                .Distinct()
                .Count();

            // sprawdzenie czy istnieją jeszcze żywe jednostki
            foreach (var u in sortedUnits)
            {
                turnCounter += 1; // licznik tur

                // jeśli jednostka jest martwa to ją pomijamy
                if (u.unit.Key.total_hp <= 0) 
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

                        helper.logAttack(u, target, (int)damage);
                    }
                    // walka w zwarciu
                    else
                    {
                        // sprawdzenie czy w zasięgu istoty są przeciwnicy
                        List<Field> fieldsInRange = new List<Field>();
                        fieldsInRange.AddRange(helper.getFieldsInRange(0, 0, board));
                        List<Field> temp = new List<Field>();
                        temp.AddRange(fieldsInRange);

                        for (int i = 0; i < u.unit.Key.speed-1; i++)
                        {
                            foreach (var f in temp)
                            {
                                fieldsInRange.AddRange(helper.getFieldsInRange(f.x, f.y, board));
                            }

                            temp.AddRange(fieldsInRange.Distinct().ToList());
                        }

                        fieldsInRange = fieldsInRange.Distinct().ToList();

                        // przeciwnicy w zasięgu
                        List<Field> enemiesInRange = fieldsInRange
                            .Where(x => x.unit.Key != null)
                            .Where(x => x.unit.Key.player == !u.unit.Key.player)
                            .ToList();

                        // brak przeciwników w zasięgu
                        if (enemiesInRange.Count() == 0)
                        {
                            var closestEnemy = board
                                .Where(e => e.unit.Key != null)
                                .Where(e => e.unit.Key.player == !u.unit.Key.player)
                                .OrderBy(e => Math.Abs(e.x - u.x))
                                .ThenBy(e => Math.Abs(e.y - u.y))
                                .FirstOrDefault();

                            // ruch po osi X
                            if (closestEnemy.x > u.x)
                            {
                                var targetField = board
                                   .Where(n => n.x == u.x + 1)
                                   .Where(n => n.y == u.y)
                                   .FirstOrDefault();

                                if (targetField.unit.Key == null) // ruszamy jednostkę tylko jeśli pole docelowe jest puste
                                {
                                    targetField.unit = u.unit;
                                    u.unit = new KeyValuePair<Unit, int>(null, 0);
                                }
                            }
                            else
                            {
                                var targetField = board
                                   .Where(n => n.x == u.x - 1)
                                   .Where(n => n.y == u.y)
                                   .FirstOrDefault();

                                if (targetField != null && targetField.unit.Key == null) // ruszamy jednostkę tylko jeśli pole docelowe jest puste
                                {
                                    targetField.unit = u.unit;
                                    u.unit = new KeyValuePair<Unit, int>(null, 0);
                                }
                            }

                            // ruch po osi Y
                            if (closestEnemy.y > u.y)
                            {
                                var targetField = board
                                   .Where(n => n.x == u.x)
                                   .Where(n => n.y == u.y + 1)
                                   .FirstOrDefault();

                                if (targetField.unit.Key == null) // ruszamy jednostkę tylko jeśli pole docelowe jest puste
                                {
                                    targetField.unit = u.unit;
                                    u.unit = new KeyValuePair<Unit, int>(null, 0);
                                }
                            }
                            else
                            {
                                var targetField = board
                                   .Where(n => n.x == u.x)
                                   .Where(n => n.y == u.y + 1)
                                   .FirstOrDefault();

                                if (targetField != null && targetField.unit.Key == null) // ruszamy jednostkę tylko jeśli pole docelowe jest puste
                                {
                                    targetField.unit = u.unit;
                                    u.unit = new KeyValuePair<Unit, int>(null, 0);
                                }
                            }
                        }
                        // przeciwnik w zasięgu
                        else
                        {
                            if (u.unit.Key.attack >= 10) // silne jednostki atakują słabe jednostki
                            {
                                var target = helper.getLowestDefenseEnemy(u.unit.Key.player, enemiesInRange);

                                if (target != null)
                                {
                                    // sprawdzenie czy można podejść do celu
                                    var space = helper
                                        .getFieldsInRange(target.x, target.y, board)
                                        .Where(x => x.unit.Key == null)
                                        .OrderBy(e => Math.Abs(e.x - u.x))
                                        .ThenBy(e => Math.Abs(e.y - u.y))
                                        .FirstOrDefault();

                                    if (space != null)
                                    {
                                        space.unit = u.unit;
                                        u.unit = new KeyValuePair<Unit, int>(null, 0);

                                        // atak
                                        // obliczenie obrażeń
                                        double damage = helper.calculateDamage(
                                            space.unit.Key.attack,
                                            target.unit.Key.defense,
                                            space.unit.Key.minDamage,
                                            space.unit.Key.maxDamage,
                                            space.unit.Key.total_hp / space.unit.Key.hp
                                        );

                                        // zadanie obrażeń
                                        target.unit.Key.total_hp -= (int)damage;
                                    }
                                }                                   
                            }
                            else // słabe jednostki atakują ranne jednostki
                            {
                                var target = helper.getLowestHealthEnemy(u.unit.Key.player, enemiesInRange);

                                if (target != null)
                                {
                                    // sprawdzenie czy można podejść do celu
                                    var space = helper
                                        .getFieldsInRange(target.x, target.y, board)
                                        .Where(x => x.unit.Key == null)
                                        .OrderBy(e => Math.Abs(e.x - u.x))
                                        .ThenBy(e => Math.Abs(e.y - u.y))
                                        .FirstOrDefault();

                                    if (space != null)
                                    {
                                        space.unit = u.unit;
                                        u.unit = new KeyValuePair<Unit, int>(null, 0);

                                        // atak
                                        // obliczenie obrażeń
                                        double damage = helper.calculateDamage(
                                            space.unit.Key.attack,
                                            target.unit.Key.defense,
                                            space.unit.Key.minDamage,
                                            space.unit.Key.maxDamage,
                                            space.unit.Key.total_hp / space.unit.Key.hp
                                        );

                                        // zadanie obrażeń
                                        target.unit.Key.total_hp -= (int)damage;
                                    }
                                }         
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Po bitwie

        helper.logGameResult(board, turnCounter);

        #endregion
    }
}
