using UnityEngine;
using System.Collections;

public class DirectionMap : MonoBehaviour {

    public TileMap map;
    public int targetColumn;
    public int targetRow;

    // Costs to move to this object from every position on the map
    private float[] costs;

    void Start() {
        Debug.Log("Starting " + this.name);
        costs = new float[map.width * map.height];
        for(var i = 0; i < map.NumberOfIndices; ++i) {
            costs[i] = Mathf.Infinity;
        }
        UpdateMap();
        Print();
    }

    public void UpdateMap() {
        UpdateRecursively(targetColumn, targetRow);
    }

    private void UpdateRecursively(int col, int row, float totalCost=0, int recursionDepth=0) {
        Debug.Log("Update map at col " + col + " row " + row + " recursion depth " + recursionDepth);
        if(recursionDepth > 40 /*System.Math.Max(map.width, map.height*/) {
            Debug.LogWarning("Max recursion reached!");
            Print();
            throw new System.ApplicationException("Max recursion");
        }
        costs[map.GetIndex(col, row)] = totalCost;
        foreach(TileMap.Direction dir in System.Enum.GetValues(typeof(TileMap.Direction))) {
            int newCol = col;
            int newRow = row;
            map.Walk(ref newCol, ref newRow, dir);
            if(!map.IsInBounds(newCol, newRow)) {
                Debug.Log("Out of bounds: " + newCol + " " + newRow);
                continue;
            }
            int index = map.GetIndex(newCol, newRow);
            float newCost = totalCost + map.GetCost(map.GetTile(newCol, newRow));
            if(newCost < costs[index]) {
                Debug.Log("Cost for " + newCol + " " + newRow + " is " + newCost + " - better than " + costs[index]);
                UpdateRecursively(newCol, newRow, newCost, recursionDepth + 1);
            }
            else {
                Debug.Log("Cost for " + newCol + " " + newRow + " is " + newCost + " - not better than " + costs[index]);
            }
        }
    }

    public void Print() {
        for(int row = 0; row < map.height; ++row) {
            string s = string.Format("{0,-3}", row) + ": ";
            for(int col = 0; col < map.width; ++col) {
                int index = map.GetIndex(col, row);
                float w = costs[index];
                string f = (w == Mathf.Infinity) ? "-    " : string.Format("{0,-5:F1}", w);
                s += f;
                s += " ";
            }
            Debug.Log(s);
        }
    }
}
