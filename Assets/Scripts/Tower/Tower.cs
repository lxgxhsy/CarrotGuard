// 职责：把炮塔目标选择和开火冷却绑定到 Unity 对象。
using System;
using UnityEngine;
using EnemyBehaviour = TowerDefense.Enemy.Enemy;

namespace TowerDefense.Tower
{
    public sealed class Tower : MonoBehaviour
    {
        [SerializeField] private TowerType towerType = TowerType.Rapid;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        private TargetSelector targetSelector;
        private FireController fireController;
        private EnemyBehaviour currentTarget;
        private TowerInstance towerInstance;

        public event Action<Tower, EnemyBehaviour> Fired;

        public TowerInstance Instance
        {
            get { return towerInstance; }
        }

        private void Start()
        {
            towerInstance = new TowerInstance(FindData());
            targetSelector = new TargetSelector();
            fireController = new FireController(towerInstance.FireInterval);
            fireController.OnFire += HandleFire;
        }

        private void Update()
        {
            currentTarget = SelectTarget();
            fireController.Tick(Time.deltaTime, currentTarget != null);
        }

        private EnemyBehaviour SelectTarget()
        {
            EnemyBehaviour[] enemies = FindObjectsOfType<EnemyBehaviour>();
            TargetPoint[] points = BuildTargetPoints(enemies);
            Vector3 position = transform.position;
            TargetPoint selected = targetSelector.SelectNearest(
                points,
                position.x,
                position.y,
                towerInstance.Range);
            return MatchEnemy(enemies, points, selected);
        }

        private TargetPoint[] BuildTargetPoints(EnemyBehaviour[] enemies)
        {
            TargetPoint[] points = new TargetPoint[enemies.Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 position = enemies[i].transform.position;
                points[i] = new TargetPoint(position.x, position.y);
                if (enemies[i].IsDead)
                {
                    points[i].MarkDead();
                }
            }

            return points;
        }

        private static EnemyBehaviour MatchEnemy(
            EnemyBehaviour[] enemies,
            TargetPoint[] points,
            TargetPoint selected)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (object.ReferenceEquals(points[i], selected))
                {
                    return enemies[i];
                }
            }

            return null;
        }

        private TowerData FindData()
        {
            foreach (TowerData data in TowerData.CreateDefaults())
            {
                if (data.Type == towerType)
                {
                    return data;
                }
            }

            return TowerData.CreateDefaults()[0];
        }

        private void HandleFire()
        {
            SpawnBullet();
            if (Fired != null)
            {
                Fired.Invoke(this, currentTarget);
            }
        }

        private void SpawnBullet()
        {
            if (bulletPrefab == null || currentTarget == null)
            {
                return;
            }

            Transform spawnPoint = firePoint == null ? transform : firePoint;
            Bullet bullet = Instantiate(
                bulletPrefab,
                spawnPoint.position,
                Quaternion.identity);
            bullet.Initialize(currentTarget, towerInstance.Damage);
        }
    }
}
