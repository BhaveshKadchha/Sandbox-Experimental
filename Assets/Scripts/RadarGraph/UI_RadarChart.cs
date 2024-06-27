using UnityEngine;
using UnityEngine.UI;

namespace CustomClasses
{
    public class UI_RadarChart : MonoBehaviour
    {
        private Stats _stats;
        int _radarChartHeight = 210;
        float _incrementAngle = 360f / 5;

        [SerializeField] Material _mat;
        [SerializeField] CanvasRenderer radarMeshRenderer;

        [Space(10)]
        [SerializeField] Button attackPlus;
        [SerializeField] Button attackMinus;
        [SerializeField] Button defencePlus;
        [SerializeField] Button defenceMinus;
        [SerializeField] Button speedPlus;
        [SerializeField] Button speedMinus;
        [SerializeField] Button manaPlus;
        [SerializeField] Button manaMinus;
        [SerializeField] Button healthPlus;
        [SerializeField] Button healthMinus;

        public void SetStats(Stats stats)
        {
            _stats = stats;
            _stats.UpdateStats += Stat_OnStatsChanged;

            attackPlus.onClick.AddListener(() => _stats.IncreaseStatAmount(Stats.StatType.Attack));
            attackMinus.onClick.AddListener(() => _stats.DecreaseStatAmount(Stats.StatType.Attack));

            defencePlus.onClick.AddListener(() => _stats.IncreaseStatAmount(Stats.StatType.Defence));
            defenceMinus.onClick.AddListener(() => _stats.DecreaseStatAmount(Stats.StatType.Defence));

            speedPlus.onClick.AddListener(() => _stats.IncreaseStatAmount(Stats.StatType.Speed));
            speedMinus.onClick.AddListener(() => _stats.DecreaseStatAmount(Stats.StatType.Speed));

            manaPlus.onClick.AddListener(() => _stats.IncreaseStatAmount(Stats.StatType.Mana));
            manaMinus.onClick.AddListener(() => _stats.DecreaseStatAmount(Stats.StatType.Mana));

            healthPlus.onClick.AddListener(() => _stats.IncreaseStatAmount(Stats.StatType.Health));
            healthMinus.onClick.AddListener(() => _stats.DecreaseStatAmount(Stats.StatType.Health));

            UpdateStatsBar();
        }

        void Stat_OnStatsChanged(object sender, System.EventArgs args) => UpdateStatsBar();

        void UpdateStatsBar()
        {
            radarMeshRenderer.SetMesh(CreateMesh());
            radarMeshRenderer.SetMaterial(_mat, null);
        }

        Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[6];
            Vector2[] uv = new Vector2[6];
            int[] triangles = new int[15];

            vertices[0] = Vector3.zero;
            vertices[1] = Quaternion.Euler(0, 0, -_incrementAngle * 0) * Vector3.up * _radarChartHeight * _stats.GetStatAmountNormalize(Stats.StatType.Attack);
            vertices[2] = Quaternion.Euler(0, 0, -_incrementAngle * 1) * Vector3.up * _radarChartHeight * _stats.GetStatAmountNormalize(Stats.StatType.Defence);
            vertices[3] = Quaternion.Euler(0, 0, -_incrementAngle * 2) * Vector3.up * _radarChartHeight * _stats.GetStatAmountNormalize(Stats.StatType.Speed);
            vertices[4] = Quaternion.Euler(0, 0, -_incrementAngle * 3) * Vector3.up * _radarChartHeight * _stats.GetStatAmountNormalize(Stats.StatType.Mana);
            vertices[5] = Quaternion.Euler(0, 0, -_incrementAngle * 4) * Vector3.up * _radarChartHeight * _stats.GetStatAmountNormalize(Stats.StatType.Health);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            triangles[6] = 0;
            triangles[7] = 3;
            triangles[8] = 4;
            triangles[9] = 0;
            triangles[10] = 4;
            triangles[11] = 5;
            triangles[12] = 0;
            triangles[13] = 5;
            triangles[14] = 1;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }
    }
}