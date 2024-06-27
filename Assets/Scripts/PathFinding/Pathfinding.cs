using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses
{
    public class Pathfinding
    {
        #region VARIABLES
        private const int MOVE_STRAIGTH_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public Grid<PathNode> grid;
        private List<PathNode> _openList;
        private List<PathNode> _closeList;
        #endregion


        /// <summary>
        /// Constructor.
        /// </summary>
        public Pathfinding(int width, int height)
        {
            grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid.GetGridObject(x, y).neighbourNodes = NeighbourNodes(x, y);
        }

        /// <summary>
        /// Find Path using Pathfinding.(Using worldpositions)
        /// </summary>
        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            grid.GetXY(startWorldPosition, out int startX, out int startY);
            grid.GetXY(endWorldPosition, out int endX, out int endY);

            List<PathNode> path = FindPath(startX, startY, endX, endY);

            if (path == null)
                return null;
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (PathNode node in path)
                    vectorPath.Add(new Vector3(node.x, node.y) * grid.CellSize + Vector3.one * grid.CellSize * 0.5f);

                return vectorPath;
            }
        }

        /// <summary>
        /// Find Path using Pathfinding.(Using Vector positions.)
        /// </summary>
        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            if (startX < 0 || startX > grid.GetWidth || startY < 0 || startY > grid.GetHeight
                || endX < 0 || endX > grid.GetWidth || endY < 0 || endY > grid.GetHeight)
                return null;

            PathNode startNode = grid.GetGridObject(startX, startY);
            PathNode endNode = grid.GetGridObject(endX, endY);

            _openList = new List<PathNode> { startNode };
            _closeList = new List<PathNode>();

            startNode.gCost = 0;
            startNode.hCost = CalculateDistance(startNode, endNode);
            startNode.CalculateFCost();

            while (_openList.Count > 0)
            {
                PathNode currentNode = GiveLowestFCostNode(_openList);
                if (currentNode == endNode)
                    return CalculatePath(currentNode);

                _openList.Remove(currentNode);
                _closeList.Add(currentNode);

                //foreach (PathNode neighbourNode in NeighbourNodes(currentNode))
                foreach (PathNode neighbourNode in currentNode.neighbourNodes)
                {
                    if (_closeList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        _closeList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!_openList.Contains(neighbourNode))
                            _openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Calculate final Path.
        /// </summary>
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();

            path.Add(endNode);
            PathNode currentNode = endNode;

            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Return list of neighbours of a node.
        /// </summary>
        private List<PathNode> NeighbourNodes(int x, int y)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            if (x - 1 >= 0)
            {
                neighbourList.Add(GetNode(x - 1, y));

                if (y - 1 >= 0) neighbourList.Add(GetNode(x - 1, y - 1));
                if (y + 1 < grid.GetHeight) neighbourList.Add(GetNode(x - 1, y + 1));
            }
            if (x + 1 < grid.GetWidth)
            {
                neighbourList.Add(GetNode(x + 1, y));

                if (y - 1 >= 0) neighbourList.Add(GetNode(x + 1, y - 1));
                if (y + 1 < grid.GetHeight) neighbourList.Add(GetNode(x + 1, y + 1));
            }

            if (y - 1 >= 0) neighbourList.Add(GetNode(x, y - 1));
            if (y + 1 < grid.GetHeight) neighbourList.Add(GetNode(x, y + 1));

            return neighbourList;
        }

        /// <summary>
        /// Return the pathnode using x and y value.
        /// </summary>
        private PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        /// <summary>
        /// Calculate average distance between start and end node.
        /// </summary>
        /// <returns></returns>
        private int CalculateDistance(PathNode startNode, PathNode endNode)
        {
            int xDistance = Mathf.Abs(startNode.x - endNode.x);
            int yDistance = Mathf.Abs(startNode.y - endNode.y);
            int remaining = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGTH_COST * (remaining);

        }

        /// <summary>
        /// Give LowestFcost among the nodes passed as Param.
        /// </summary>
        private PathNode GiveLowestFCostNode(List<PathNode> pathNodes)
        {
            PathNode lowestPathNode = pathNodes[0];
            for (int i = 0; i < pathNodes.Count; i++)
                if (pathNodes[i].fCost < lowestPathNode.fCost)
                    lowestPathNode = pathNodes[i];

            return lowestPathNode;
        }
    }
}