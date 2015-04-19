using UnityEngine;
using System.Collections;

public class BuildHouse : MonoBehaviour {
    public byte tileType = 17;

    public void Execute(int col, int row) {
        TileMap map = TileMap.instance;
        DestroyObject(this.gameObject);
        GameObject obj = map.CreateObject(tileType, col, row);
        Debug.Log(this + " built house " + obj);
    }
}
