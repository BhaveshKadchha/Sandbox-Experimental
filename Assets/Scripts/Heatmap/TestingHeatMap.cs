using UnityEngine;

namespace CustomClasses
{
    public class TestingHeatMap : MonoBehaviour
    {
        Grid<HeatMap> _grid;

        [SerializeField] int _width;
        [SerializeField] int _height;
        [SerializeField] float _cellSize;

        [Space(10)]
        [SerializeField] Material _mat;
        [SerializeField] Transform _textMeshParent;
        [SerializeField] Transform _meshParent;

        void Start()
        {
            _grid = new Grid<HeatMap>(_width, _height, _cellSize, Vector3.zero, (Grid<HeatMap> g, int x, int y) => new HeatMap(g, x, y), _textMeshParent);

            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                    _grid.GetGridObject(x, y).CreateMesh(_mat, _meshParent);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _grid.GetXY(ExperimentalHelper.GetMouseWorlPosition(), out int x, out int y);
                HeatMap heatMap = _grid.GetGridObject(x, y);

                if (heatMap != null)
                {
                    //_grid.GetGridObject(x, y).ChangeValue(5, true);
                    _grid.GetGridObject(x, y).ChangeValue(100, true, true);
                }
            }
        }
    }
}