using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

    public static TileMap instance;

    public GameObject homePrefab;
    public GameObject schoolPrefab;
    public GameObject workPrefab;
    public GameObject terrorPrefab;
    public GameObject dirtPrefab;

    public float groundCost = 1.0f;
    public float dirtCost = 1.0f;
    public float insideCost = 1.0f;

    public bool shrinkObjects = false;

    private MapData mapData = new MapData();
    public int width = 30;
    public int height = 20;
    private byte[] tileData;

    void Start() {
        instance = this;
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
                obj.transform.parent = this.gameObject.transform;
                obj.transform.position = ColRowToWorld(col, row);
                if(shrinkObjects) {
                    obj.transform.localScale = new Vector3(1, .1f, 1);
                }
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

    public GameObject CreateObject(byte tileCode) {
        if(tileCode == 17) {
            GameObject obj = (GameObject)Object.Instantiate(homePrefab);
            obj.name = "Home";
            return obj;
        }
        if(tileCode == 20) {
            GameObject obj = (GameObject)Object.Instantiate(schoolPrefab);
            obj.name = "School";
            return obj;
        }
        if(tileCode == 41) {
            GameObject obj = (GameObject)Object.Instantiate(workPrefab);
            obj.name = "Work";
            return obj;
        }
        if(tileCode == 44) {
            GameObject obj = (GameObject)Object.Instantiate(terrorPrefab);
            return obj;
        }
        if(tileCode == 50) {
            GameObject obj = (GameObject)Object.Instantiate(dirtPrefab);
            return obj;
        }
        /*if(tileCode == 48 || tileCode == 49 || tileCode == 56 || tileCode == 57) {
            GameObject obj = (GameObject)Object.Instantiate(dirtPrefab);
            return obj;
        }*/
        return null;
    }

    public Vector3 ColRowToWorld(int col, int row) {
        return new Vector3(col - width * .5f, 0, height * .5f - row);
    }
    public Vector3 ColRowToWorld(int col, int row, Vector3 pivotPoint) {
        return ColRowToWorld(col, row) + pivotPoint;
    }

    public void WorldToColRow(Vector3 pos, out int col, out int row) {
        col = Mathf.FloorToInt(pos.x + width * .5f);
        row = Mathf.FloorToInt(height * .5f - pos.z);
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

    public void SetTile(int col, int row, byte tile) {
        int index = GetIndex(col, row);
        if(tileData[index] == tile) {
            return;
        }
        tileData[index] = tile;
        foreach(DirectionMap dirMap in GetComponentsInChildren<DirectionMap>()) {
            dirMap.TileChanged(col, row);
        }
    }

    public float GetCost(byte tileType) {
        switch(tileType) {
        case 48:
            return groundCost;
        case 50:
            return dirtCost;
        case 17:
        case 20:
        case 41:
        case 44:
            return insideCost;
        default:
			return Mathf.Infinity;
        }
    }

    public bool IsModifiable(byte tileType) {
        return tileType == 48;
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
