using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses
{
    public class TestingGrid : MonoBehaviour
    {
        Grid<HeatMapGridObject> grid;

        public int gridWidth;
        public int gridHeight;
        public float cellSize;
        public Vector3 originPosition;

        void Start()
        {
            grid = new Grid<HeatMapGridObject>(gridWidth, gridHeight, cellSize, originPosition, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HeatMapGridObject mapGridObject = grid.GetGridObject(ExperimentalHelper.GetMouseWorlPosition());
                if (mapGridObject != null)
                    mapGridObject.AddValue(5);
            }
            //    grid.SetGridObject(Helper.GetMouseWorlPosition(), );

            //if (Input.GetMouseButtonDown(1))
            //    Debug.Log(grid.GetGridObject(Helper.GetMouseWorlPosition()));
        }
    }






    public class HeatMapGridObject
    {
        const int min = 1;
        const int max = 10;

        public int value;

        public Grid<HeatMapGridObject> grid;
        public int x;
        public int y;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }


        public void AddValue(int addValue)
        {
            value += addValue;
            Mathf.Clamp(value, min, max);
            grid.TriggerGridObjectChanged(x, y);
        }

        public float GetNormalizedValue()
        {
            return (float)value / max;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}