using UnityEngine;

namespace CustomClasses
{
    public class HeatMap
    {
        const int HEAT_MAP_MIN_VALUE = 0;
        const int HEAT_MAP_MAX_VALUE = 100;
        const int DISTANCE_FOR_EXPONENTIAL = 10;

        public Grid<HeatMap> grid;

        public int x;
        public int y;
        public int Value { get { return _value; } private set { } }

        private int _value;
        private Mesh _mesh;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HeatMap(Grid<HeatMap> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Create mesh for cell.
        /// </summary>
        public void CreateMesh(Material mat, Transform parent = null)
        {
            _mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            int[] triangles = new int[6];

            vertices[0] = new Vector3(-grid.CellSize * 0.5f, -grid.CellSize * 0.5f, 0.1f);
            vertices[1] = new Vector3(-grid.CellSize * 0.5f, grid.CellSize * 0.5f, 0.1f);
            vertices[2] = new Vector3(grid.CellSize * 0.5f, grid.CellSize * 0.5f, 0.1f);
            vertices[3] = new Vector3(grid.CellSize * 0.5f, -grid.CellSize * 0.5f, 0.1f);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;

            GameObject row = new GameObject();

            row.transform.position = new Vector2(x * grid.CellSize, y * grid.CellSize) + Vector2.one * grid.CellSize * 0.5f;
            row.AddComponent<MeshFilter>().mesh = _mesh;
            row.AddComponent<MeshRenderer>().material = mat;

            ChangeMeshColor();
            if (parent)
                row.transform.SetParent(parent);
        }

        /// <summary>
        /// Change Heatmap value.
        /// </summary>
        public void ChangeValue(int addVal, bool originalNode, bool changeExponentially = false)
        {
            _value = Mathf.Clamp(_value + addVal, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            ChangeMeshColor();
            grid.TriggerGridObjectChanged(x, y);

            if (originalNode)
            {
                if (!changeExponentially)
                    ChangeNeightbourLinear(addVal);
                else
                    ChangeNeightbourExponentially(addVal);
            }
        }

        /// <summary>
        /// Change neighbour values linearly.
        /// </summary>
        public void ChangeNeightbourLinear(int val)
        {
            for (int xVal = 0; xVal < grid.GetWidth; xVal++)
                for (int yVal = 0; yVal < grid.GetHeight; yVal++)
                    if (Mathf.Abs(xVal - x) + Mathf.Abs(yVal - y) < 3 && Mathf.Abs(xVal - x) + Mathf.Abs(yVal - y) != 0)
                        grid.GetGridObject(xVal, yVal).ChangeValue(val, false);
        }

        /// <summary>
        /// Change neighbour values Exponentially.
        /// </summary>
        public void ChangeNeightbourExponentially(int val)
        {
            for (int xVal = 0; xVal < grid.GetWidth; xVal++)
                for (int yVal = 0; yVal < grid.GetHeight; yVal++)
                {
                    int distance = Mathf.Abs(xVal - x) + Mathf.Abs(yVal - y);

                    int multiplier = val / DISTANCE_FOR_EXPONENTIAL;

                    if (distance < DISTANCE_FOR_EXPONENTIAL && distance != 0)
                    {
                        grid.GetGridObject(xVal, yVal).ChangeValue(val - (distance * multiplier), false);
                    }
                }
        }

        /// <summary>
        /// Change UV for changing color.
        /// </summary>
        public void ChangeMeshColor()
        {
            Vector2[] uv = new Vector2[4];
            float val = (1 - (float)_value / HEAT_MAP_MAX_VALUE);

            uv[0] = new Vector2(val, 0);
            uv[1] = new Vector2(val, 0);
            uv[2] = new Vector2(val, 0);
            uv[3] = new Vector2(val, 0);

            _mesh.uv = uv;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}