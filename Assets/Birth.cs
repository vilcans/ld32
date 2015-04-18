using UnityEngine;
using System.Collections;

public class Birth : MonoBehaviour {

    public float timeBetweenBirths;

    public GameObject personPrefab;

    IEnumerator Start() {
        while(true) {
            yield return new WaitForSeconds(timeBetweenBirths);
            Spawn();
        }
    }

	GameObject Spawn() {
        EduGame game = gameObject.GetComponentInParent<EduGame>();
        GameObject obj = (GameObject)Object.Instantiate(personPrefab);
        obj.transform.parent = game.transform;
        FollowDirections follower = obj.GetComponent<FollowDirections>();

        // The DirectionMap's goal happens to be this house's position
        DirectionMap dirs = gameObject.GetComponent<DirectionMap>();
        follower.col = dirs.targetColumn;
        follower.row = dirs.targetRow;
		return obj;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            for(int row = 0; row < 20; ++row) {
                for(int col = 0; col < 30; ++col) {
					GameObject obj = Spawn();
                    FollowDirections follower = obj.GetComponent<FollowDirections>();
                    follower.col = col;
                    follower.row = row;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            GameObject obj = Spawn();
            FollowDirections follower = obj.GetComponent<FollowDirections>();
            follower.col = Random.Range(0, 30);
            follower.row = Random.Range(0, 19);
        }
    }
}
