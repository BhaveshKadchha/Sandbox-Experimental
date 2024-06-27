using UnityEngine;

namespace CustomClasses
{
    public class TestingRadarGraph : MonoBehaviour
    {
        Stats stats;
        [SerializeField] UI_RadarChart uiRadarChart;

        void Start()
        {
            stats = new Stats(14, 18, 11, 15, 9);
            uiRadarChart.SetStats(stats);
        }
    }
}