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

        private void OnEnable()
        {
            Subscribe();
            Refresh();
        }

        private void OnDisable()
        {
            if (LevelManager.Instance == null)
            {
                return;
            }

            LevelManager.Instance.GoldChanged -= Refresh;
            LevelManager.Instance.HealthChanged -= Refresh;
        }

        private void Subscribe()
        {
            if (LevelManager.Instance == null)
            {
                return;
            }

            LevelManager.Instance.GoldChanged += Refresh;
            LevelManager.Instance.HealthChanged += Refresh;
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
