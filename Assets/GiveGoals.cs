using UnityEngine;
using System.Collections;

public class GiveGoals : MonoBehaviour {

    public float delayBetweenUpdates = .02f;

    IEnumerator Start() {
        while(true) {
            yield return new WaitForSeconds(delayBetweenUpdates);
            DirectionMap[] candidates = GetComponentsInChildren<DirectionMap>();
            if(candidates.Length == 0) {
                Debug.Log("No candidates yet; hold on");
                continue;
            }
            //Debug.Log ("Found candidates: " + candidates.Length);
            foreach(FollowDirections follower in GetComponentsInChildren<FollowDirections>()) {
                follower.ChooseDirection(candidates);
                yield return new WaitForSeconds(delayBetweenUpdates);
            }
        }
    }
}
