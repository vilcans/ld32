using UnityEngine;
using System.Collections;

public class CapacityView : MonoBehaviour {

    private int lastValue = int.MinValue;
    private Occupation occupation;
    private TextMesh textMesh;

    void Start () {
        occupation = GetComponentInParent<Occupation>();
        textMesh = GetComponent<TextMesh>();
	}

    void Update() {
        int number = occupation.GetNumberOfOccupants();
        if(number != lastValue) {
            lastValue = number;
            this.textMesh.text = number.ToString();
        }
    }
}
