using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    public enum State {
        Child,
        Adult,
        Parent,
    }

    public DirectionMap goal;
    public int col, row;
    public State state = State.Child;
    [Tooltip("Movement speed for cost=1")]
    public float
        moveSpeed = 3.0f;

    public float contemplatingTime = 2.0f;

    private int stepsTaken;

    IEnumerator Start() {
        TileMap map = TileMap.instance;
        this.transform.position = map.ColRowToWorld(col, row);

        float timeInStep = 0;

        do {
            if(stepsTaken == 1) {
                // Step out and stop to think about next move
                yield return new WaitForSeconds(contemplatingTime);
            }
            while(goal == null) {
                yield return null;
            }
            if(!map.IsInBounds(col, row)) {
                Debug.LogError("Out of bounds at " + col + "," + row);
                break;
            }
            if(goal.IsAtGoal(col, row)) {
                //Debug.Log("Reached goal!");
                GoalReached();
                break;
            }
            else {
                Direction dir = goal.GetDirection(col, row).GetOpposite();
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
                ++stepsTaken;
            }
        } while(true);
    }

    public void ChooseGoal(DirectionMap[] goals) {
        float lowestCost = Mathf.Infinity;
        foreach(DirectionMap m in goals) {
            if(!IsAcceptableGoal(m)) {
                continue;
            }
            float cost = m.GetCostToGoal(this.col, this.row);
            if(cost < lowestCost) {
                lowestCost = cost;
                this.goal = m;
            }
        }
        //Debug.Log(this.state + " found best goal: " + this.goal + " cost " + lowestCost);
    }

    public bool IsAcceptableGoal(DirectionMap goal) {
        if(!goal.acceptedByStates.Contains(this.state)) {
            return false;
        }
        Occupation occupation = goal.GetComponent<Occupation>();
        if(occupation == null) {
            return true;
        }
        return occupation.CanReceive();
    }

    private void GoalReached() {
        Occupation occupation = this.goal.GetComponent<Occupation>();
        Debug.Log(this + " reached goal; occupation is " + occupation);
        if(occupation != null) {
            if(occupation.CanReceive()) {
                occupation.ReceivePerson();
                Destroy(this.gameObject);
            }
            else {
                Debug.Log("This place is full");
            }
        }
    }
}
