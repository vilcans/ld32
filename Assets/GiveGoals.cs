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
            FollowDirections[] followers = GetComponentsInChildren<FollowDirections>();
            //Debug.Log("Giving " + candidates.Length + " goals to chose from to " + followers.Length + " people");
            foreach(FollowDirections follower in followers) {
                follower.ChooseDirection(candidates);
                yield return new WaitForSeconds(delayBetweenUpdates);
            }
        }
    }
}
