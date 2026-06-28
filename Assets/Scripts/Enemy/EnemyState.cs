// 职责：保存单个敌人的生命值、死亡状态与击杀奖励。
namespace TowerDefense.Enemy
{
    public sealed class EnemyState
    {
        public EnemyState(int health, int goldReward)
        {
            CurrentHealth = health;
            GoldReward = goldReward;
        }

        public int CurrentHealth { get; private set; }

        public int GoldReward { get; private set; }

        public bool IsDead { get; private set; }

        public void ReduceHealth(int amount)
        {
            if (IsDead)
            {
                return;
            }

            CurrentHealth -= amount;
            if (CurrentHealth > 0)
            {
                return;
            }

            CurrentHealth = 0;
            IsDead = true;
        }
    }
}
