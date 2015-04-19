using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Transform target;
    
    void LateUpdate() {
        Vector3 my = this.gameObject.transform.position;
        this.transform.position = new Vector3(target.position.x, my.y, my.z);
    }
}
