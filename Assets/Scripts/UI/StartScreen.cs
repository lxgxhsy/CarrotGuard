// 职责：控制开始界面到关卡运行状态的切换。
using UnityEngine;

namespace TowerDefense.UI
{
    public sealed class StartScreen : MonoBehaviour
    {
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject levelRoot;

        private void Awake()
        {
            Time.timeScale = 0f;
            if (startPanel != null)
            {
                startPanel.SetActive(true);
            }

            if (levelRoot != null)
            {
                levelRoot.SetActive(false);
            }
        }

        public void EnterLevel()
        {
            if (startPanel != null)
            {
                startPanel.SetActive(false);
            }

            if (levelRoot != null)
            {
                levelRoot.SetActive(true);
            }

            Time.timeScale = 1f;
        }
    }
}
