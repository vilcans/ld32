using UnityEngine;
using System.Collections;

public class FollowDirections : MonoBehaviour {

    public DirectionMap directions;

    IEnumerator Start() {
        Vector3 pos = this.transform.position;
        int row, col;
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
	            this.transform.position = map.ColRowToWorld(col, row);
	            yield return new WaitForSeconds(.5f);
			}
        } while(true);
    }
}
