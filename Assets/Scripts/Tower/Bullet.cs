// 职责：把子弹追踪逻辑绑定到 Unity 对象，并在命中时造成伤害。
using UnityEngine;
using EnemyBehaviour = TowerDefense.Enemy.Enemy;

namespace TowerDefense.Tower
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 6f;
        [SerializeField] private int damage = 5;

        private EnemyBehaviour target;
        private TargetPoint targetPoint;
        private BulletMover mover;

        public void Initialize(EnemyBehaviour target, int damage)
        {
            this.target = target;
            this.damage = damage;
            CreateMover();
        }

        private void Start()
        {
            if (mover == null && target != null)
            {
                CreateMover();
            }
        }

        private void Update()
        {
            if (mover == null)
            {
                return;
            }

            SyncTargetPoint();
            mover.Tick(Time.deltaTime);
            transform.position = new Vector3(mover.X, mover.Y, 0f);
            ResolveHitOrDestroy();
        }

        private void CreateMover()
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = target.transform.position;
            targetPoint = new TargetPoint(targetPosition.x, targetPosition.y);
            mover = new BulletMover(position.x, position.y, speed, targetPoint);
        }

        private void SyncTargetPoint()
        {
            if (target == null || target.IsDead)
            {
                targetPoint.MarkDead();
                return;
            }

            Vector3 targetPosition = target.transform.position;
            targetPoint.MoveTo(targetPosition.x, targetPosition.y);
        }

        private void ResolveHitOrDestroy()
        {
            if (mover.IsDestroyed)
            {
                Destroy(gameObject);
                return;
            }

            if (target != null && Vector3.Distance(transform.position, target.transform.position) <= 0.05f)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
