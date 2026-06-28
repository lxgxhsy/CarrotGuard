// 职责：将关卡金币和生命值实时显示到 Unity UI 文本。
using UnityEngine;
using UnityEngine.UI;
using TowerDefense.Core;

namespace TowerDefense.UI
{
    public sealed class Hud : MonoBehaviour
    {
        [SerializeField] private Text goldText;
        [SerializeField] private Text healthText;

        private bool subscribed;

        private void OnEnable()
        {
            Subscribe();
            Refresh();
        }

        private void Update()
        {
            if (!subscribed)
            {
                Subscribe();
                Refresh();
            }
        }

        private void OnDisable()
        {
            if (!subscribed || LevelManager.Instance == null)
            {
                return;
            }

            LevelManager.Instance.GoldChanged -= Refresh;
            LevelManager.Instance.HealthChanged -= Refresh;
            subscribed = false;
        }

        private void Subscribe()
        {
            if (subscribed || LevelManager.Instance == null)
            {
                return;
            }

            LevelManager.Instance.GoldChanged += Refresh;
            LevelManager.Instance.HealthChanged += Refresh;
            subscribed = true;
        }

        public void Refresh()
        {
            if (LevelManager.Instance == null)
            {
                return;
            }

            if (goldText != null)
            {
                goldText.text = LevelManager.Instance.Gold.ToString();
            }

            if (healthText != null)
            {
                healthText.text = LevelManager.Instance.Health.ToString();
            }
        }
    }
}
