// 职责：计算敌人受到伤害后的生命变化与击杀奖励。
namespace TowerDefense.Enemy
{
    public static class DamageCalculator
    {
        public static int ApplyDamage(EnemyState enemy, int damage)
        {
            if (enemy.IsDead)
            {
                return 0;
            }

            enemy.ReduceHealth(damage);
            if (!enemy.IsDead)
            {
                return 0;
            }

            return enemy.GoldReward;
        }
    }
}
