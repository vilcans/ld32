using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Occupation : MonoBehaviour {

    public float stageTime = 5.0f;
    public int capacity = 10;
    public int minOccupancyForNextStage = 0;

    public Follower.State nextState;
    public GameObject nextStagePrefab;
    private Queue<float> releaseTimes;

    void Start() {
        releaseTimes = new Queue<float>();
    }

    void Update() {
        float now = Time.timeSinceLevelLoad;

        if(releaseTimes.Count < minOccupancyForNextStage) {
            return;
        }
        while(releaseTimes.Count != 0 && releaseTimes.Peek() <= now) {
            releaseTimes.Dequeue();
            DirectionMap place = GetComponent<DirectionMap>();
            BuildHouse buildHouse = GetComponent<BuildHouse>();
            if(buildHouse != null) {
                buildHouse.Execute(place.targetColumn, place.targetRow);
            }
            if(nextStagePrefab != null) {
                SpawnNextStage();
            }
        }
    }

    public bool CanReceive() {
        return releaseTimes.Count < capacity;
    }

    public int GetNumberOfOccupants() {
        return releaseTimes.Count;
    }

    public void ReceivePerson() {
        releaseTimes.Enqueue(Time.timeSinceLevelLoad + stageTime);
    }

    private void SpawnNextStage() {
        DirectionMap dirs = GetComponent<DirectionMap>();
        GameObject person = (GameObject)Object.Instantiate(nextStagePrefab);
        person.transform.parent = GetComponentInParent<EduGame>().transform;
        //person.transform.parent = this.gameObject.transform;
        Follower follower = person.GetComponent<Follower>();
        follower.state = this.nextState;
        follower.col = dirs.targetColumn;
        follower.row = dirs.targetRow;
        //person.transform.position = dirs.map.ColRowToWorld(dirs.targetColumn, dirs.targetRow);
        Debug.Log("Spawned " + person);
    }
}
