using UnityEngine;
using System.Collections.Generic;

public class School : MonoBehaviour {

    public float stageTime = 5.0f;
    public int capacity = 10;
    public GameObject adultPrefab;
    private Queue<float> releaseTimes;

    void Start() {
        releaseTimes = new Queue<float>();    
    }

    void Update() {
        float now = Time.timeSinceLevelLoad;
        /*if(releaseTimes.Count != 0) {
            Debug.Log("School has " + releaseTimes.Count + " pupils, now is " + now + ", first release time is " + releaseTimes.Peek());
        }
        else {
            //Debug.Log ("School is empty");
        }*/
        while(releaseTimes.Count != 0 && releaseTimes.Peek() <= now) {
            releaseTimes.Dequeue();
            SpawnAdult();
        }
    }

    public bool CanReceive() {
        return releaseTimes.Count < capacity;
    }

    public void ReceivePerson() {
        releaseTimes.Enqueue(Time.timeSinceLevelLoad + stageTime);
    }

    private void SpawnAdult() {
        DirectionMap dirs = GetComponent<DirectionMap>();
        GameObject person = (GameObject)Object.Instantiate(adultPrefab);
        person.transform.parent = GetComponentInParent<EduGame>().transform;
        //person.transform.parent = this.gameObject.transform;
        Follower follower = person.GetComponent<Follower>();
        follower.state = Follower.State.Adult;
        follower.col = dirs.targetColumn;
        follower.row = dirs.targetRow;
        //person.transform.position = dirs.map.ColRowToWorld(dirs.targetColumn, dirs.targetRow);
        Debug.Log("Spawned adult " + person);
    }
}
