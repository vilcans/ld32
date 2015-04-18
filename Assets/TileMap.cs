using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

    public GameObject homePrefab;
    public GameObject schoolPrefab;
    public GameObject workPrefab;
    public GameObject terrorPrefab;

    public float groundWeight = 1.0f;
    public float dirtWeight = 1.0f;

    private MapData mapData = new MapData();
    public int width = 30;
    public int height = 20;
    private byte[] tileData;

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
                DirectionMap directionMap = obj.GetComponent<DirectionMap>();
                if(directionMap != null) {
                    directionMap.map = this;
                    directionMap.targetColumn = col;
                    directionMap.targetRow = row;
                }
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
        return new Vector3(col - width * .5f, 0, height * .5f - row);
    }

    public int NumberOfIndices {
        get { return this.width * this.height; }
    }

    public bool IsInBounds(int col, int row) {
        return col >= 0 && col < width && row >= 0 && row < height;
    }

    public byte GetTile(int col, int row) {
        return tileData[GetIndex(col, row)];
    }

    public float GetCost(byte tileType) {
        switch(tileType) {
        case 48:
        case 49:
        case 56:
        case 57:
            return groundWeight;
        default:
			return Mathf.Infinity;
        }
    }

    public int GetIndex(int col, int row) {
        if(col < 0 || col >= width) {
            throw new System.ApplicationException("Column out of range: " + col);
        }
        if(row < 0 || row >= height) {
            throw new System.ApplicationException("Row out of range: " + row);
        }
        return col + row * width;
    }

    public void Walk(ref int col, ref int row, Direction dir) {
        switch(dir) {
        case Direction.Right:
            ++col;
            break;
        case Direction.Left:
            --col;
            break;
        case Direction.Up:
            --row;
            break;
        case Direction.Down:
            ++row;
            break;
        default:
            throw new System.ApplicationException("Unknown Direction");
        }
    }
}
