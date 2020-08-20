namespace ShoppingBasket.Tests
{
    public class CyberTruck
    {
        private const string ForwardCommand = "F";
        private const string BackwardCommand = "B";
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }

        public void Drop(int x, int y, Direction direction = Direction.North)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Move(string command)
        {
            if (command == "R")
            {
                if (Direction == Direction.East)
                {
                    Direction = Direction.South;
                }
                else
                {
                    Direction = Direction.East;
                }
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