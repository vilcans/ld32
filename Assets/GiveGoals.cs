using UnityEngine;
using System.Collections;

public class GiveGoals : MonoBehaviour {

    IEnumerator Start() {
        while(true) {
            DirectionMap[] candidates = GetComponentsInChildren<DirectionMap>();
            if(candidates.Length != 0) {
                foreach(FollowDirections follower in GetComponentsInChildren<FollowDirections>()) {
                    follower.ChooseDirection(candidates);
                }
                yield return new WaitForSeconds(.02f);
            }
            yield return new WaitForSeconds(3);
        }
    }
}
