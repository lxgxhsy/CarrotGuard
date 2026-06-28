// 职责：显示胜利或失败界面，并提供重新开始当前关卡的方法。
using UnityEngine;
using UnityEngine.SceneManagement;
using TowerDefense.Core;

namespace TowerDefense.UI
{
    public sealed class GameResultView : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;

        private bool subscribed;

        private void Start()
        {
            HidePanels();
            Subscribe();
        }

        private void OnDisable()
        {
            if (!subscribed || LevelManager.Instance == null)
            {
                return;
            }

            LevelManager.Instance.GameOver -= ShowGameOver;
            LevelManager.Instance.Victory -= ShowVictory;
            subscribed = false;
        }

        public void Restart()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name);
        }

        private void Subscribe()
        {
            if (subscribed || LevelManager.Instance == null)
            {
                return;
            }

            LevelManager.Instance.GameOver += ShowGameOver;
            LevelManager.Instance.Victory += ShowVictory;
            subscribed = true;
        }

        private void HidePanels()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }

            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false);
            }
        }

        private void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }

        private void ShowVictory()
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }
        }
    }
}
