using UnityEngine;
using System;

namespace CustomClasses
{
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangeEventArgs> OnGridChanged;

        public class OnGridObjectChangeEventArgs : EventArgs
        {
            public int xVal;
            public int yVal;
        }

        public int GetWidth { get { return _width; } private set { } }
        public int GetHeight { get { return _height; } private set { } }
        public float CellSize { get { return _cellsize; } private set { } }

        private int _height;
        private int _width;
        private float _cellsize;

        private Vector3 _originPositon;
        private TGridObject[,] _gridArray;
        private TextMesh[,] _debugTextArray;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, Transform parent = null)
        {
            _height = height;
            _width = width;
            _cellsize = cellSize;
            _originPositon = originPosition;

            _gridArray = new TGridObject[width, height];

            bool _showDebug = true;

            if (_showDebug)
            {
                _debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < _gridArray.GetLength(0); x++)
                    for (int y = 0; y < _gridArray.GetLength(1); y++)
                    {
                        _gridArray[x, y] = createGridObject(this, x, y);
                        _debugTextArray[x, y] = ExperimentalHelper.CreateWorldText(_gridArray[x, y].ToString(), parent, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.black, TextAlignment.Center, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                    }

                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

                OnGridChanged += (object sender, OnGridObjectChangeEventArgs eventArgs) => _debugTextArray[eventArgs.xVal, eventArgs.yVal].text = _gridArray[eventArgs.xVal, eventArgs.yVal].ToString();
            }
            else
            {
                for (int x = 0; x < _gridArray.GetLength(0); x++)
                    for (int y = 0; y < _gridArray.GetLength(1); y++)
                        _gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        /// <summary>
        /// Convert grid points to Vector3.
        /// </summary>
        Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellsize + _originPositon;
        }

        /// <summary>
        /// Convert Vector3 to Grid Points.
        /// </summary>
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPositon).x / _cellsize);
            y = Mathf.FloorToInt((worldPosition - _originPositon).y / _cellsize);
        }

        /// <summary>
        /// Set Value in Target Grid.
        /// </summary>
        public void SetGridObject(int x, int y, TGridObject val)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = val;
                _debugTextArray[x, y].text = val.ToString();
                OnGridChanged?.Invoke(this, new OnGridObjectChangeEventArgs { xVal = x, yVal = y });
            }
        }

        /// <summary>
        /// Trigger event to change value in grid.
        /// </summary>
        public void TriggerGridObjectChanged(int x, int y) => OnGridChanged?.Invoke(this, new OnGridObjectChangeEventArgs { xVal = x, yVal = y });

        /// <summary>
        /// Set Value in Target Grid.(using Vector3 point)
        /// </summary>
        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        /// <summary>
        /// Get Value of target Grid.
        /// </summary>
        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
                return _gridArray[x, y];
            else
                return default;
        }

        /// <summary>
        /// Get Value of target Grid.(Using Vector2)
        /// </summary>
        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
    }
}