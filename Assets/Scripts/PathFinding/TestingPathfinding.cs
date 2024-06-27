using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses
{
    public class TestingPathfinding : MonoBehaviour
    {
        Pathfinding _pathfinding;

        void Start()
        {
            _pathfinding = new Pathfinding(10, 10);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pathfinding.grid.GetXY(ExperimentalHelper.GetMouseWorlPosition(), out int x, out int y);
                List<PathNode> path = _pathfinding.FindPath(0, 0, x, y);

                if (path != null)
                    for (int i = 0; i < path.Count - 1; i++)
                        Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5, Color.red, 100f);
            }

            if (Input.GetMouseButtonDown(1))
            {
                PathNode node = _pathfinding.grid.GetGridObject(ExperimentalHelper.GetMouseWorlPosition());
                node.ChangeWalkable(false);
            }
        }
    }
}