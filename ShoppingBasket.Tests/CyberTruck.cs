namespace ShoppingBasket.Tests
{
    public class CyberTruck
    {
        private const string ForwardCommand = "F";
        private const string BackwardCommand = "B";
        private const string RightCommand = "R";
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction => _predefinedDirections[_currentDirectionIndex];

        private readonly Direction[] _predefinedDirections = {Direction.North, Direction.East, Direction.South, Direction.West};

        private int _currentDirectionIndex;
        public void Drop(int x, int y, Direction direction = Direction.North)
        {
            X = x;
            Y = y;
            _currentDirectionIndex = (int)direction;
        }

        public void Move(string command)
        {
            if (command == RightCommand)
            {
                _currentDirectionIndex = _currentDirectionIndex >= 3 ? 0 : ++_currentDirectionIndex;
            }
            if (command == ForwardCommand)
            {
                Y++;
            }

            if (command == BackwardCommand)
            {
                Y--;
            }
        }
    }
}