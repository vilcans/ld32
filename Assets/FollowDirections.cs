using UnityEngine;
using System.Collections;

public class FollowDirections : MonoBehaviour {

    public DirectionMap directions;
    public float moveSpeed = 3.0f;

    IEnumerator Start() {
        Vector3 pos = this.transform.position;
        int col, row;
        TileMap map = TileMap.instance;
        map.WorldToColRow(pos, out col, out row);

        do {
            while(directions == null) {
                yield return null;
            }
            if(!map.IsInBounds(col, row)) {
                Debug.LogError("Out of bounds at " + col + "," + row);
                break;
            }
            if(directions.IsAtGoal(col, row)) {
                Debug.Log("Reached goal!");
                Destroy(this.gameObject);
                break;
            }
            else {
                Direction dir = directions.GetDirection(col, row).GetOpposite();
                Debug.Log("Walking " + dir + " from " + col + "," + row);
                map.Walk(ref col, ref row, dir);
                Vector3 oldPos = this.transform.position;
                Vector3 newPos = map.ColRowToWorld(col, row);
                float t = 0;
                float transitionLength = Vector3.Distance(oldPos, newPos) / moveSpeed;
                while(t < transitionLength) {
                    this.transform.position = Vector3.Lerp(oldPos, newPos, t / transitionLength);
                    t += Time.deltaTime;
                    yield return null;
                }
                this.transform.position = newPos;
            }
        } while(true);
    }
}
