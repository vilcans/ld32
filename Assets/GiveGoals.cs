using UnityEngine;
using System.Collections;

public class GiveGoals : MonoBehaviour {

    IEnumerator Start() {
        while(true) {
            DirectionMap directions = GetComponentInChildren<DirectionMap>();
            Debug.Log("Giving new directions: " + directions);
            foreach(FollowDirections follower in GetComponentsInChildren<FollowDirections>()) {
                follower.directions = directions;
            }
            yield return new WaitForSeconds(3);
        }
    }
}
