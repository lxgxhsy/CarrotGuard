// 职责：维护玩家生命值，并在生命首次归零时发出死亡事件。
using System;

namespace TowerDefense.Core
{
    public sealed class HealthSystem
    {
        private bool isDead;

        public event Action OnDead;

        public int CurrentHealth { get; private set; }

        public HealthSystem(int initialHealth)
        {
            CurrentHealth = initialHealth;
        }

        public void ApplyDamage(int amount)
        {
            if (isDead)
            {
                return;
            }

            CurrentHealth -= amount;
            if (CurrentHealth > 0)
            {
                return;
            }

            CurrentHealth = 0;
            isDead = true;
            if (OnDead != null)
            {
                OnDead.Invoke();
            }
        }
    }
}
