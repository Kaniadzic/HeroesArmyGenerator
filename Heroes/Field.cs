namespace Heroes
{
    public class Field
    {
        public Field()
        {

        }

        public Field(int x, int y, KeyValuePair<Unit, int> unit)
        {
            this.x = x;
            this.y = y;
            this.unit = unit;
        }

        public KeyValuePair<Unit, int> unit { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
