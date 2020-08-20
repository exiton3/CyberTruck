using NUnit.Framework;

namespace ShoppingBasket.Tests
{

    //1.	Drop should position Truck on surface in provided coordinates
    //2.	Drop truck in specified direction North etc.
    //3.	Passed wrong symbol it should skip it
    //4.	Pass R should move From North to East
    //5.	Pass R second Time should move from East to South.
    //6.	Pass L should move from North to West
    //7.	F should move one square in same direction 
    //8.	B should move one square in opposite direction
    //9.	Can accept multiple commands

    [TestFixture]
    public class CyberTruckTests
    {
        private CyberTruck _truck;

        [SetUp]
        public void SetUp()
        {
            _truck = new CyberTruck();
        }

        [Test]
        public void DropAndPassCoordinates_ShouldSetCoordinates()
        {

            _truck.Drop(4, 7);

            Assert.That(4, Is.EqualTo(_truck.X));
            Assert.That(7, Is.EqualTo(_truck.Y));
        }

        [Test]
        public void DropSecondTimeAndPassCoordinates_ShouldSetCoordinates()
        {

            _truck.Drop(4, 7);
            _truck.Drop(6, 8);

            Assert.That(6, Is.EqualTo(_truck.X));
            Assert.That(8, Is.EqualTo(_truck.Y));
        }

        [Test]
        public void DropAndPassDirection_ShouldSetDirection()
        {

            _truck.Drop(4, 7, Direction.South);

            Assert.That(Direction.South, Is.EqualTo(_truck.Direction));
        }

        [Test]
        public void MovePassedWrongCommand_TruckSkipThisCommand()
        {
            _truck.Drop(6, 8, Direction.East);

            var wrongCommand = "W";

            _truck.Move(wrongCommand);

            Assert.That(6, Is.EqualTo(_truck.X));
            Assert.That(8, Is.EqualTo(_truck.Y));
            Assert.That(Direction.East, Is.EqualTo(_truck.Direction));
        }

        [Test]
        public void MovePassedFCommandAndNorthDirection_ShouldChangeYCoordinate()
        {
            _truck.Drop(6, 8);

            var forwardCommand = "F";

            _truck.Move(forwardCommand);

            Assert.That(6, Is.EqualTo(_truck.X));
            Assert.That(9, Is.EqualTo(_truck.Y));
            Assert.That(Direction.North, Is.EqualTo(_truck.Direction));
        }

        [Test]
        public void MovePassedBCommandAndNorthDirection_ShouldDecreseYCoordinate()
        {
            _truck.Drop(6, 8);

            var backWardCommand = "B";

            _truck.Move(backWardCommand);

            Assert.That(6, Is.EqualTo(_truck.X));
            Assert.That(7, Is.EqualTo(_truck.Y));
            Assert.That(Direction.North, Is.EqualTo(_truck.Direction));
        }


        [Test]
        [TestCase(Direction.North,Direction.East)]
        [TestCase(Direction.East,Direction.South)]
        [TestCase(Direction.South,Direction.West)]
        [TestCase(Direction.West,Direction.North)]
        public void MovePassedRCommand_TurnTruckClockwise(Direction from, Direction to)
        {
            _truck.Drop(6, 8,from);

            var RightCommand = "R";

            _truck.Move(RightCommand);

            Assert.That(6, Is.EqualTo(_truck.X));
            Assert.That(8, Is.EqualTo(_truck.Y));

            Assert.That(to, Is.EqualTo(_truck.Direction));
        }
    }
}
