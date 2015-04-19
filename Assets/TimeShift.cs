using UnityEngine;
using System.Collections;

public class TimeShift : MonoBehaviour {

    void Update () {
        if(Input.GetKey(KeyCode.Alpha0)) {
            Time.timeScale = 5;
        }
        else {
            Time.timeScale = 1;
        }
	}
}
