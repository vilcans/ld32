using UnityEngine;
using System.Collections;

public class Birth : MonoBehaviour {

    public GameObject personPrefab;

    void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
			EduGame game = GetComponentInParent<EduGame>();
			GameObject obj = (GameObject)Object.Instantiate(personPrefab);
            obj.transform.parent = game.transform;
            obj.transform.localPosition = new Vector3(
                Random.Range(-10, 10), 0, Random.Range(-5, 5)
            );
        }
    }
}
