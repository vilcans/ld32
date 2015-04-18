using UnityEngine;
using System.Collections;

public class Birth : MonoBehaviour {

    public GameObject personPrefab;

    void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            for(int row = 0; row < 20; ++row) {
                for(int col = 0; col < 30; ++col) {
                    EduGame game = GetComponentInParent<EduGame>();
                    GameObject obj = (GameObject)Object.Instantiate(personPrefab);
                    obj.transform.parent = game.transform;
                    FollowDirections follower = obj.GetComponent<FollowDirections>();
                    follower.col = col;
                    follower.row = row;
                    //obj.transform.localPosition = TileMap.instance.ColRowToWorld(col, row);
                    /*obj.transform.localPosition = new Vector3(
                        Random.Range(-10, 10), 0, Random.Range(-5, 5)
                    );*/
                }
            }
        }
    }
}
