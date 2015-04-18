using UnityEngine;
using System.Collections.Generic;

public class DirectionMap : MonoBehaviour {

    public TileMap map;
    public int targetColumn;
    public int targetRow;
    
    private struct PathItem {
        public int col;
        public int row;
        public TileMap.Direction fromDirection;
        public float cost;
        public override string ToString() {
            return "col " + col + " row " + row + " from " + fromDirection + " cost " + cost;
        }
        public PathItem(int col, int row, TileMap.Direction fromDirection, float cost) {
            this.col = col;
            this.row = row;
            this.fromDirection = fromDirection;
            this.cost = cost;
        }
    };
    
    // Costs to move to this object from every position on the map
    private float[] costs;
    private TileMap.Direction[] directions;

    void Start() {
        Debug.Log("Starting " + this.name);
        costs = new float[map.width * map.height];
        directions = new TileMap.Direction[map.width * map.height];
        for(var i = 0; i < map.NumberOfIndices; ++i) {
            costs[i] = Mathf.Infinity;
        }
        UpdateMap();
        Print();
    }

    public void UpdateMap() {
        Queue<PathItem> path = new Queue<PathItem>();
        path.Enqueue(new PathItem(targetColumn, targetRow, TileMap.Direction.Left, 0));

        int iterations = 0;
        while(path.Count != 0) {
            if(++iterations > 20000) {
                Print();
                throw new System.ApplicationException ("Too many iterations");
            }

            PathItem item = path.Dequeue();
            int index = map.GetIndex(item.col, item.row);
            float oldCost = costs[index];
            if(oldCost <= item.cost) {
                //Debug.Log("didn't beat cost " + oldCost + " with " + item);
                continue;
            }
            costs[index] = item.cost;
            directions[index] = item.fromDirection;
            //Debug.Log("Update map " + item);
            foreach (TileMap.Direction dir in System.Enum.GetValues(typeof(TileMap.Direction))) {
                int newCol = item.col;
                int newRow = item.row;
                map.Walk(ref newCol, ref newRow, dir);
                if(!map.IsInBounds(newCol, newRow)) {
                    //Debug.Log("Out of bounds: " + newCol + " " + newRow);
                    continue;
                }
                float newCost = item.cost + map.GetCost(map.GetTile(newCol, newRow));
                PathItem newItem = new PathItem(newCol, newRow, dir, newCost);
                path.Enqueue(newItem);
            }
        }
        Debug.Log("Direction map updated in " + iterations + " iterations");
    }

    public void Print() {
        for(int row = 0; row < map.height; ++row) {
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            s.AppendFormat("{0,-3}: ", row);
            for(int col = 0; col < map.width; ++col) {
                int index = map.GetIndex(col, row);
                float w = costs[index];
                if(w == Mathf.Infinity) {
                    s.Append("  99.9");
                }
                else {
                    s.AppendFormat("{0,5:F1}", w);
                }
                switch(directions[index]) {
                case TileMap.Direction.Left:
                    s.Append("< ");
                    break;
                case TileMap.Direction.Right:
                    s.Append("> ");
                    break;
                case TileMap.Direction.Up:
                    s.Append("^ ");
                    break;
                case TileMap.Direction.Down:
                    s.Append("v ");
                    break;
                }
            }
            Debug.Log(s.ToString());
        }
    }
}
