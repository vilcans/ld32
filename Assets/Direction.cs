
public enum Direction {
	Left, Right, Up, Down
};

public static class DirectionExtension {
    public static Direction GetOpposite(this Direction dir) {
        switch(dir) {
        case Direction.Left:
            return Direction.Right;
        case Direction.Right:
            return Direction.Left;
        case Direction.Up:
            return Direction.Down;
        case Direction.Down:
            return Direction.Up;
        default:
            throw new System.ApplicationException("Can't get opposite of direction: " + dir);
        }
    }
}
