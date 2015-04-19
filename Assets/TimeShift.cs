using UnityEngine;
using System.Collections;

public class TimeShift : MonoBehaviour {

    public bool fast = false;

    void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha0)) {
            this.fast = !this.fast;
            Time.timeScale = fast ? 5 : 1;
        }
	}
}
