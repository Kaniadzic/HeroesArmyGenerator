using Heroes;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        ArmyGenerator armyGenerator = new ArmyGenerator();

        int[] armyNumbers = armyGenerator.GenerateArmyNumbers();
        List<Unit> unitList = new List<Unit>();

        unitList.Add(new Unit(1, "Troglodyta", 4, 3, false, 5, 1, 3, 4));
        unitList.Add(new Unit(2, "Harpia", 6, 5, false, 14, 1, 4, 6));
        unitList.Add(new Unit(3, "Złe Oko", 9, 7, true, 22, 3, 5, 5));
        unitList.Add(new Unit(4, "Meduza", 9, 9, true, 25, 6, 8, 5));
        unitList.Add(new Unit(5, "Minotaur", 14, 12, false, 50, 12, 20, 6));
        unitList.Add(new Unit(6, "Mantykora", 15, 13, false, 80, 14, 20, 7));
        unitList.Add(new Unit(7, "Smok", 19, 19, false, 180, 40, 50, 11));

        Dictionary<Unit, int> normalArmy = armyGenerator.GenerateArmy(unitList, armyNumbers);
        Dictionary<Unit, int> randomizedArmy = armyGenerator.RandomizeArmy(armyGenerator.GenerateArmy(unitList, armyNumbers));
    }
}
