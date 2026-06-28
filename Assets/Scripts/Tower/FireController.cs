// 职责：维护炮塔开火冷却，并在有目标且冷却结束时发出开火事件。
using System;

namespace TowerDefense.Tower
{
    public sealed class FireController
    {
        private readonly float fireInterval;
        private float elapsedTime;

        public event Action OnFire;

        public FireController(float fireInterval)
        {
            this.fireInterval = fireInterval;
        }

        public void Tick(float deltaTime, bool hasTarget)
        {
            if (!hasTarget)
            {
                return;
            }

            elapsedTime += deltaTime;
            if (elapsedTime < fireInterval)
            {
                return;
            }

            elapsedTime = 0f;
            if (OnFire != null)
            {
                OnFire.Invoke();
            }
        }
    }
}
