using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

	public GameObject homePrefab;
	public GameObject schoolPrefab;
	public GameObject workPrefab;
	public GameObject terrorPrefab;

	private MapData mapData = new MapData();
	private int width = 30;
	private int height = 20;
	private byte[] tileData = {48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,56,57,24,25,26,57,24,25,26,57,24,25,26,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,48,49,32,33,34,49,32,33,34,49,32,33,34,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,56,57,40,41,42,57,40,41,42,57,40,41,42,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,3,4,5,57,56,57,56,57,56,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,11,12,13,49,48,49,48,49,48,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,19,20,21,57,56,57,56,57,56,48,49,48,49,48,49,48,49,0,1,2,49,48,49,48,49,48,49,48,49,48,49,50,49,49,48,49,48,49,48,56,57,56,57,56,57,56,57,8,9,10,50,50,50,50,50,50,50,50,50,50,50,50,57,57,56,57,56,57,56,48,49,48,49,48,49,48,49,16,17,18,49,48,49,48,49,48,49,48,49,48,49,49,48,49,48,49,48,49,48,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,57,56,57,56,57,56,57,56,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,49,48,49,48,49,48,49,48,56,57,27,28,29,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,57,56,57,56,57,56,57,56,48,49,35,36,37,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,49,48,49,48,49,48,49,48,56,57,43,44,45,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,57,56,57,56,57,56,57,56,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,48,49,49,48,49,48,49,48,49,48,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,56,57,57,56,57,56,57,56,57,56};

	void Start() {
		this.width = mapData.width;
		this.height = mapData.height;
		this.tileData = mapData.tileData;
        CreateObjects();
    }

    void CreateObjects() {
        for(int row = 0; row < height; ++row) {
            for(int col = 0; col < width; ++col) {
                int index = row * width + col;
                byte tileCode = tileData[index];
                GameObject obj = CreateObject(tileCode);
                if(obj == null) {
                    continue;
                }
                obj.transform.position = RowColToWorld(row, col);
				Debug.Log("Instantiated " + obj.name + " at col " + col + " row " + row + " " + obj.transform.position);
            }
        }
    }

    GameObject CreateObject(byte tileCode) {
		if(tileCode == 0) {
			GameObject obj = (GameObject)Object.Instantiate(homePrefab);
			obj.name = "Home";
			return obj;
		}
		if(tileCode == 3) {
			GameObject obj = (GameObject)Object.Instantiate(schoolPrefab);
			obj.name = "School";
			return obj;
		}
		if(tileCode == 24) {
			GameObject obj = (GameObject)Object.Instantiate(workPrefab);
			obj.name = "Work";
			return obj;
		}
		if(tileCode == 27) {
			GameObject obj = (GameObject)Object.Instantiate(terrorPrefab);
			return obj;
		}
		return null;
    }

    public Vector3 RowColToWorld(int row, int col) {
        return new Vector3(col - width * .5f, 0, height - row);
    }
}
