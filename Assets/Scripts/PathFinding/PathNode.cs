using System.Collections.Generic;

namespace CustomClasses
{
    public class PathNode
    {
        #region VARIABLES
        private Grid<PathNode> _grid;
        public int x;
        public int y;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;
        public PathNode cameFromNode;
        public List<PathNode> neighbourNodes;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            _grid = grid;
            this.x = x;
            this.y = y;

            cameFromNode = null;
            gCost = int.MaxValue;
            CalculateFCost();

            isWalkable = true;
        }

        /// <summary>
        /// Change value to non walkable.
        /// </summary>
        public void ChangeWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
            _grid.TriggerGridObjectChanged(x, y);
        }

        public void CalculateFCost() => fCost = gCost + hCost;

        public override string ToString()
        {
            if (isWalkable)
                return x + " ; " + y;
            else
                return "No";
        }
    }
}