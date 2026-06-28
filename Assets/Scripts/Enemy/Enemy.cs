// 职责：把敌人的路径移动和受伤计算绑定到 Unity 对象。
using System;
using UnityEngine;
using TowerDefense.Core;

namespace TowerDefense.Enemy
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float speed = 1f;
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private int goldReward = 3;

        private PathFollower pathFollower;
        private EnemyState state;
        private bool hasLeftLevel;

        public event Action<Enemy> ReachedEnd;
        public event Action<Enemy, int> Killed;

        public bool IsDead
        {
            get { return state != null && state.IsDead; }
        }

        public void ConfigureWaypoints(Transform[] path)
        {
            waypoints = path;
        }

        private void Start()
        {
            state = new EnemyState(maxHealth, goldReward);
            pathFollower = new PathFollower(CreateWaypoints(), speed);
            pathFollower.OnReachedEnd += HandleReachedEnd;
        }

        private void Update()
        {
            if (pathFollower == null || IsDead || hasLeftLevel || IsLevelEnded())
            {
                return;
            }

            pathFollower.Tick(Time.deltaTime);
            transform.position = new Vector3(pathFollower.X, pathFollower.Y, 0f);
        }

        public void TakeDamage(int damage)
        {
            if (hasLeftLevel || state == null || IsDead || IsLevelEnded())
            {
                return;
            }

            int reward = DamageCalculator.ApplyDamage(state, damage);
            if (reward <= 0)
            {
                return;
            }

            if (Killed != null)
            {
                Killed.Invoke(this, reward);
            }

            Destroy(gameObject);
        }

        private Waypoint[] CreateWaypoints()
        {
            Waypoint[] points = new Waypoint[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                Vector3 position = waypoints[i].position;
                points[i] = new Waypoint(position.x, position.y);
            }

            return points;
        }

        private static bool IsLevelEnded()
        {
            return LevelManager.Instance != null && LevelManager.Instance.IsGameOver;
        }

        private void HandleReachedEnd()
        {
            hasLeftLevel = true;
            if (ReachedEnd != null)
            {
                ReachedEnd.Invoke(this);
            }
        }
    }
}
