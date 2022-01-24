namespace T6
{
    public class Configuration
    {
        Army army = new Army(10);
        int coinsCount = 10;
        string name = "Vladislav";
    }

    public class Army
    {
        private int _unitsAmount;

        public Army(int unitsAmount)
        {
            _unitsAmount = unitsAmount;
        }

        public int UnitsAmount => _unitsAmount;
    }
}
