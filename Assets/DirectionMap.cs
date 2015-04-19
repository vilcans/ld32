using UnityEngine;
using System.Collections.Generic;

public class DirectionMap : MonoBehaviour {

    public TileMap map;
    public int targetColumn;
    public int targetRow;
    public List<Follower.State> acceptedByStates;
    public int updatesPerFrame = 50;

    private struct PathItem {
        public int col;
        public int row;
        public Direction fromDirection;
        public float cost;
        public override string ToString() {
            return "col " + col + " row " + row + " from " + fromDirection + " cost " + cost;
        }
        public PathItem(int col, int row, Direction fromDirection, float cost) {
            this.col = col;
            this.row = row;
            this.fromDirection = fromDirection;
            this.cost = cost;
        }
    };

    private Queue<PathItem> path;

    // Costs to move to this object from every position on the map
    private float[] costs;
    private Direction[] directions;

    void Start() {
        Debug.Log("Starting " + this.name);
        path = new Queue<PathItem>();
        costs = new float[map.width * map.height];
        directions = new Direction[map.width * map.height];
        Clear();
        UpdateMap();
        //Print();
    }

    void Clear() {
        for(var i = 0; i < map.NumberOfIndices; ++i) {
            costs[i] = Mathf.Infinity;
            directions[i] = Direction.Left;
        }
    }

    void Update() {
        if(path == null) {
            Debug.LogWarning("path is null in " + this);
            path = new Queue<PathItem>();
        }
        if(path.Count != 0) {
            //Debug.Log(this + " Path queue " + path.Count);
            for(int i = 0; i < updatesPerFrame && path.Count != 0; ++i) {
                UpdateOneStep();
            }
        }
        if(Input.GetKeyDown(KeyCode.P)) {
            Print();
        }
    }

	public bool IsAtGoal(int col, int row) {
		return col == this.targetColumn && row == this.targetRow;
	}

    public Direction GetDirection(int col, int row) {
        return directions[map.GetIndex(col, row)];
    }

    public float GetCostToGoal(int col, int row) {
        return costs[map.GetIndex(col, row)];
    }

    public void TileChanged(int col, int row) {
        int index = map.GetIndex(col, row);
        //costs[index] = Mathf.Infinity;
        //Clear();
        //UpdateMap();
        float bestCost = Mathf.Infinity;
        Direction bestDirection = Direction.Left;
        foreach(Direction dir in System.Enum.GetValues(typeof(Direction))) {
            int newCol = col;
            int newRow = row;
            map.Walk(ref newCol, ref newRow, dir);
            if(!map.IsInBounds(newCol, newRow)) {
                //Debug.Log("Out of bounds: " + newCol + " " + newRow);
                continue;
            }
            float costInDirection = costs[map.GetIndex(newCol, newRow)];
            if(costInDirection < bestCost) {
                bestCost = costInDirection;
                bestDirection = dir;
            }
        }
        Debug.Log("Best cost from " + col + "," + row + " is " + bestCost + " in direction " + bestDirection);
        costs[index] = bestCost + map.GetCost(map.GetTile(col, row));
        directions[index] = bestDirection.GetOpposite();
        UpdateAroundTile(col, row, costs[index]);
    }

    public void UpdateMap() {
        path.Enqueue(new PathItem(targetColumn, targetRow, Direction.Left, 0));

        /*int iterations = 0;
        while(path.Count != 0) {
            UpdateOneStep();
            ++iterations;
        }
        Debug.Log("Direction map updated in " + iterations + " iterations");*/
    }

    private void UpdateOneStep() {
        if(path.Count == 0) {
            return;
        }

        PathItem item = path.Dequeue();
        int index = map.GetIndex(item.col, item.row);
        float oldCost = costs[index];
        if(oldCost <= item.cost) {
            //Debug.Log("didn't beat cost " + oldCost + " with " + item);
            return;
        }
        costs[index] = item.cost;
        directions[index] = item.fromDirection;
        //Debug.Log("Update map " + item);
        UpdateAroundTile(item.col, item.row, item.cost);

        return;
    }

    private void UpdateAroundTile(int col, int row, float tileCost) {
        foreach(Direction dir in System.Enum.GetValues(typeof(Direction))) {
            int newCol = col;
            int newRow = row;
            map.Walk(ref newCol, ref newRow, dir);
            if(!map.IsInBounds(newCol, newRow)) {
                //Debug.Log("Out of bounds: " + newCol + " " + newRow);
                continue;
            }
            float newCost = tileCost + map.GetCost(map.GetTile(newCol, newRow));
            if(costs[map.GetIndex(newCol, newRow)] > newCost) {
                PathItem newItem = new PathItem(newCol, newRow, dir, newCost);
                path.Enqueue(newItem);
            }
        }
    }

    public void Print() {
        Debug.Log("Direction map " + this);
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
                case Direction.Left:
                    s.Append("< ");
                    break;
                case Direction.Right:
                    s.Append("> ");
                    break;
                case Direction.Up:
                    s.Append("^ ");
                    break;
                case Direction.Down:
                    s.Append("v ");
                    break;
                }
            }
            Debug.Log(s.ToString());
        }
    }
}
