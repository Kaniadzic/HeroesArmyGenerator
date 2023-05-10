namespace Heroes
{
    public class Unit
    {
        public Unit()
        {

        }

        public Unit(byte level, string name, byte attack, byte defense, bool ranged, byte hp, byte minDamage, byte maxDamage, byte speed)
        {
            this.level = level;
            this.name = name;
            this.attack = attack;
            this.defense = defense;
            this.ranged = ranged;
            this.hp = hp;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.speed = speed;
        }

        public byte level { get; }
        public string name { get; }
        public byte attack { get; }
        public byte defense { get; }
        public bool ranged { get; }
        public byte hp { get; set; }
        public byte minDamage { get; }
        public byte maxDamage { get; }
        public byte speed { get; }
        public bool player { get; set; }
        public int total_hp {get; set; }
    }
}
