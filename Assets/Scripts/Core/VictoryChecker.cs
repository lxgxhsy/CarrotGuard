// 职责：判断全波完成且场上无敌人时的胜利条件。
using System;

namespace TowerDefense.Core
{
    public sealed class VictoryChecker
    {
        public event Action OnVictory;

        public bool HasVictory { get; private set; }

        public void Evaluate(bool allWavesComplete, int aliveEnemyCount)
        {
            if (HasVictory || !allWavesComplete || aliveEnemyCount > 0)
            {
                return;
            }

            HasVictory = true;
            if (OnVictory != null)
            {
                OnVictory.Invoke();
            }
        }
    }
}
