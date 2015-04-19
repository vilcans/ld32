using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Transform target;
    
    void Update() {
        Vector3 my = this.gameObject.transform.position;
        this.transform.position = new Vector3(target.position.x, my.y, my.z);

        /* = new Vector3(
            target.x,
            my.x,
            my.y
        );*/
    }
}
