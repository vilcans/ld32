using UnityEngine;
using System.Collections;

public class FollowDirections : MonoBehaviour {

    public DirectionMap directions;
    public int col, row;

    [Tooltip("Movement speed for cost=1")]
    public float moveSpeed = 3.0f;

    IEnumerator Start() {
        TileMap map = TileMap.instance;
        this.transform.position = map.ColRowToWorld(col, row);

        float timeInStep = 0;

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
                byte tileType = map.GetTile(col, row);
                float cost = map.GetCost(tileType);
                //Debug.Log("Walking " + dir + " from " + col + "," + row + " cost " + cost);
                map.Walk(ref col, ref row, dir);
                Vector3 oldPos = this.transform.position;
                Vector3 newPos = map.ColRowToWorld(col, row);
                float transitionLength = Vector3.Distance(oldPos, newPos) / moveSpeed * cost;
                while(timeInStep < transitionLength) {
                    this.transform.position = Vector3.Lerp(oldPos, newPos, timeInStep / transitionLength);
                    timeInStep += Time.deltaTime;
                    yield return null;
                }
                timeInStep -= transitionLength;
                this.transform.position = newPos;
            }
        } while(true);
    }

    public void ChooseDirection(DirectionMap[] directions) {
        float lowestCost = Mathf.Infinity;
        foreach(DirectionMap m in directions) {
            float cost = m.GetCostToGoal(this.col, this.row);
            if(cost < lowestCost) {
                lowestCost = cost;
                this.directions = m;
            }
        }
        Debug.Log("Found best goal: " + this.directions + " cost " + lowestCost);
    }
}
