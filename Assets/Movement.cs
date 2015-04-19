using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public int column = 14;
    public int row = 6;
    public Direction direction;
    public float speed = 50.0f;
    private static Vector3 pivotPoint = new Vector3(.5f, 0, .5f);
    private TileMap map;

    IEnumerator Start() {
        do {
            yield return null;
            map = TileMap.instance;
        } while(map == null);

        while(true) {
            this.gameObject.transform.position = map.ColRowToWorld(column, row, pivotPoint);
            this.gameObject.transform.rotation = this.direction.GetRotation();
            Direction newDirection;
            if(GetNewDirection(out newDirection)) {
                foreach(var x in TryToMove(newDirection)) {
                    yield return x;
                }
            }
            else {
                yield return null;
            }
        }
    }

    private IEnumerable TryToMove(Direction newDirection) {
        this.gameObject.transform.rotation = newDirection.GetRotation();

        this.direction = newDirection;
        int newCol = column;
        int newRow = row;
        map.Walk(ref newCol, ref newRow, newDirection);
        if(!map.IsInBounds(newCol, newRow)) {
            yield return null;
        }
        else {
            byte oldTileType = map.GetTile(column, row);
            byte tileType = map.GetTile(newCol, newRow);
            float cost = map.GetCost(tileType);
            if(cost == Mathf.Infinity) {
                Debug.Log("Can't go " + newDirection + " into " + tileType);
                yield return null;
            }
            else {
                Debug.Log("Moving " + newDirection + " to " + newCol + "," + newRow);
                if(map.IsModifiable(tileType)) {
                    CreateRoad(newCol, newRow);
                }
                Vector3 newPos = map.ColRowToWorld(newCol, newRow, pivotPoint);
                Vector3 oldPos = this.transform.position;
                float oldCost = map.GetCost(oldTileType);
                float mixedCost = cost * .5f + oldCost * .5f;
                float transitionTime = Vector3.Distance(oldPos, newPos) * mixedCost / this.speed;
                float t = 0;
                while(t < transitionTime) {
                    this.transform.position = Vector3.Lerp(oldPos, newPos, t / transitionTime);
                    yield return null;
                    t += Time.deltaTime;
                }
                this.transform.position = newPos;
                row = newRow;
                column = newCol;
            }
        }
    }

    public static bool GetNewDirection(out Direction dir) {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float absH = Mathf.Abs(horizontal);
        float absV = Mathf.Abs(vertical);
        if(absH > absV) {
            if(horizontal < 0) {
                dir = Direction.Left;
                return true;
            }
            if(horizontal > 0) {
                dir = Direction.Right;
                return true;
            }
        }
        else {
            if(vertical < 0) {
                dir = Direction.Down;
                return true;
            }
            if(vertical > 0) {
                dir = Direction.Up;
                return true;
            }
        }
        dir = Direction.Left;
        return false;
    }

    void CreateRoad(int col, int row) {
        GameObject obj = map.CreateObject(50);
        obj.transform.position = map.ColRowToWorld(col, row);
        map.SetTile(col, row, 50);
    }
}
