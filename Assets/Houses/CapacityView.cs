using UnityEngine;
using System.Collections;

public class CapacityView : MonoBehaviour {

    private int lastCapacity;
    private Occupation occupation;
    private TextMesh textMesh;

    void Start () {
        occupation = GetComponentInParent<Occupation>();
        textMesh = GetComponent<TextMesh>();
	}

    void Update() {
        int capacity = occupation.GetCapacityLeft();
        if(capacity != lastCapacity) {
            lastCapacity = capacity;
            this.textMesh.text = capacity.ToString();
        }
    }
}
