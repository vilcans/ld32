using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    public enum State {
        Child,
        Adult
    }

    public DirectionMap directions;
    public int col, row;
    public State state = State.Child;
    [Tooltip("Movement speed for cost=1")]
    public float
        moveSpeed = 3.0f;

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
                //Debug.Log("Reached goal!");
                GoalReached();
                break;
            }
            else {
                Direction dir = directions.GetDirection(col, row).GetOpposite();
                byte tileType = map.GetTile(col, row);
                float cost = map.GetCost(tileType);
                //Debug.Log("Walking " + dir + " from " + col + "," + row + " cost " + cost);
                map.Walk(ref col, ref row, dir);
                Vector3 oldPos = this.transform.position;
                Vector3 newPos = map.ColRowToWorld(col, row) + new Vector3(Random.Range(-.2f, .2f), 0, Random.Range(-.2f, .2f));
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
            if(!IsAcceptableGoal(m)) {
                continue;
            }
            float cost = m.GetCostToGoal(this.col, this.row);
            if(cost < lowestCost) {
                lowestCost = cost;
                this.directions = m;
            }
        }
        //Debug.Log(this.state + " found best goal: " + this.directions + " cost " + lowestCost);
    }

    public bool IsAcceptableGoal(DirectionMap goal) {
        if(!goal.acceptedByStates.Contains(this.state)) {
            return false;
        }
        Occupation school = goal.GetComponent<Occupation>();
        if(school == null) {
            return true;
        }
        return school.CanReceive();
    }

    private void GoalReached() {
        Occupation school = this.directions.GetComponent<Occupation>();
        Debug.Log(this + " reached goal; school is " + school);
        if(school != null) {
            if(school.CanReceive()) {
                school.ReceivePerson();
                Destroy(this.gameObject);
            }
            else {
                Debug.Log("This school is full");
            }
        }
    }
}
