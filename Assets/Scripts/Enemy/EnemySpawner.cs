// 职责：把刷怪信号绑定到 Unity 敌人实例创建。
using UnityEngine;
using TowerDefense.Core;

namespace TowerDefense.Enemy
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform spawnRoot;

        private bool subscribed;

        private void Update()
        {
            TrySubscribe();
        }

        private void OnDisable()
        {
            if (!subscribed || LevelManager.Instance == null || LevelManager.Instance.WaveSpawner == null)
            {
                return;
            }

            LevelManager.Instance.WaveSpawner.OnSpawn -= SpawnEnemy;
            subscribed = false;
        }

        private void TrySubscribe()
        {
            if (subscribed || LevelManager.Instance == null || LevelManager.Instance.WaveSpawner == null)
            {
                return;
            }

            LevelManager.Instance.WaveSpawner.OnSpawn += SpawnEnemy;
            subscribed = true;
        }

        private void SpawnEnemy(int waveIndex)
        {
            if (enemyPrefab == null || waypoints.Length < 2)
            {
                return;
            }

            Transform parent = spawnRoot == null ? transform : spawnRoot;
            Enemy enemy = Instantiate(
                enemyPrefab,
                waypoints[0].position,
                Quaternion.identity,
                parent);
            enemy.ConfigureWaypoints(waypoints);
            LevelManager.Instance.RegisterEnemy(enemy);
        }
    }
}
