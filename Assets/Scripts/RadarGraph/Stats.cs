using UnityEngine;

namespace CustomClasses
{
    public class Stats
    {
        const int STAT_MIN = 0;
        const int STAT_MAX = 20;
        const int STAT_AMOUNT_CHANGE = 1;

        public event System.EventHandler UpdateStats;

        public enum StatType { Attack, Defence, Speed, Mana, Health }

        public SingleStat attackStat;
        public SingleStat defenceStat;
        public SingleStat speedStat;
        public SingleStat manaStat;
        public SingleStat healthStat;

        public Stats(int attackStatAmount, int defenceStatAmount, int speedStatAmount, int manaStatAmount, int healthStatAmount)
        {
            attackStat = new SingleStat(attackStatAmount);
            defenceStat = new SingleStat(defenceStatAmount);
            speedStat = new SingleStat(speedStatAmount);
            manaStat = new SingleStat(manaStatAmount);
            healthStat = new SingleStat(healthStatAmount);
        }

        public void SetStatAmount(StatType type, int statAmount)
        {
            GetSingleStat(type).SetStatAmount(statAmount);
            UpdateStats?.Invoke(this, System.EventArgs.Empty);
        }

        public void IncreaseStatAmount(StatType type) => SetStatAmount(type, STAT_AMOUNT_CHANGE);

        public void DecreaseStatAmount(StatType type) => SetStatAmount(type, -STAT_AMOUNT_CHANGE);

        public float GetStatAmountNormalize(StatType type)
        {
            return GetSingleStat(type).GetStatAmountNormalize();
        }

        public int GetStatAmount(StatType type)
        {
            return GetSingleStat(type).stat;
        }

        private SingleStat GetSingleStat(StatType statType)
        {
            switch (statType)
            {
                default: return attackStat;
                case StatType.Defence: return defenceStat;
                case StatType.Speed: return speedStat;
                case StatType.Mana: return manaStat;
                case StatType.Health: return healthStat;
            }
        }




        public class SingleStat
        {
            public int stat { get; private set; }

            public SingleStat(int stat)
            {
                SetStatAmount(stat);
            }

            public void SetStatAmount(int statAmount)
            {
                stat = Mathf.Clamp(stat + statAmount, STAT_MIN, STAT_MAX);
            }

            public float GetStatAmountNormalize()
            {
                return (float)stat / STAT_MAX;
            }
        }
    }
}