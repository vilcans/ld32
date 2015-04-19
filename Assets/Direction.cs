using UnityEngine;

public enum Direction {
	Left, Right, Up, Down
};

public static class DirectionExtension {
    private static Quaternion[] rotations = {
        Quaternion.AngleAxis(-90, Vector3.up),
        Quaternion.AngleAxis(90, Vector3.up),
        Quaternion.AngleAxis(0, Vector3.up),
        Quaternion.AngleAxis(180, Vector3.up),
    };

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
    public static Quaternion GetRotation(this Direction dir) {
        return rotations[(int)dir];
    }
}
