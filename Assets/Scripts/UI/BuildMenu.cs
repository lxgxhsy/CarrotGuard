// 职责：处理空白格点击后的炮塔选择菜单和建造按钮。
using UnityEngine;
using TowerDefense.Core;
using TowerDefense.Tower;

namespace TowerDefense.UI
{
    public sealed class BuildMenu : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private GameObject towerMenu;
        [SerializeField] private GameObject towerPanel;
        [SerializeField] private RectTransform towerMenuRect;
        [SerializeField] private RectTransform towerPanelRect;
        [SerializeField] private Tower rapidTowerPrefab;
        [SerializeField] private Tower heavyTowerPrefab;

        private readonly TowerUpgrade towerUpgrade = new TowerUpgrade();
        private bool hasBuildSelection;
        private Vector3 selectedWorldPosition;
        private Tower selectedTower;

        private void Update()
        {
            if (!PointerPressed() || PointerInputGuard.IsPointerOverUi() || LevelManager.Instance == null)
            {
                return;
            }

            Vector3 world = ScreenToWorld(PointerPosition());
            Tower tower = FindTowerAt(world);
            if (tower != null)
            {
                selectedTower = tower;
                ShowTowerPanel(PointerPosition());
                return;
            }

            GridCell cell = LevelManager.Instance.GridSystem.Align(world.x, world.y);
            if (LevelManager.Instance.GridSystem.IsOccupied(cell))
            {
                return;
            }

            selectedWorldPosition = new Vector3(cell.X, cell.Y, 0f);
            hasBuildSelection = true;
            ShowTowerMenu(PointerPosition());
        }

        public void BuildRapidTower()
        {
            BuildTower(TowerType.Rapid, rapidTowerPrefab);
        }

        public void BuildHeavyTower()
        {
            BuildTower(TowerType.Heavy, heavyTowerPrefab);
        }

        public void UpgradeSelectedTower()
        {
            if (selectedTower == null || selectedTower.Instance == null || LevelManager.Instance == null)
            {
                return;
            }

            bool upgraded = towerUpgrade.Upgrade(
                selectedTower.Instance,
                LevelManager.Instance.GoldSystem);
            if (upgraded)
            {
                LevelManager.Instance.AwardGold(0);
            }

            HideMenus();
        }

        public void SellSelectedTower()
        {
            if (selectedTower == null || LevelManager.Instance == null)
            {
                return;
            }

            int refund = towerUpgrade.SellRefund(selectedTower.Instance);
            Vector3 position = selectedTower.transform.position;
            GridCell cell = LevelManager.Instance.GridSystem.Align(position.x, position.y);
            LevelManager.Instance.GridSystem.SetOccupied(cell, false);
            LevelManager.Instance.AwardGold(refund);
            Destroy(selectedTower.gameObject);
            HideMenus();
        }

        private void BuildTower(TowerType type, Tower prefab)
        {
            if (!hasBuildSelection || prefab == null || LevelManager.Instance == null)
            {
                return;
            }

            TowerData data = FindData(type);
            bool built = LevelManager.Instance.TryBuild(
                selectedWorldPosition.x,
                selectedWorldPosition.y,
                data);
            if (built)
            {
                Instantiate(prefab, selectedWorldPosition, Quaternion.identity);
            }

            HideMenus();
        }

        private TowerData FindData(TowerType type)
        {
            foreach (TowerData data in TowerData.CreateDefaults())
            {
                if (data.Type == type)
                {
                    return data;
                }
            }

            return TowerData.CreateDefaults()[0];
        }

        private void ShowTowerMenu(Vector3 screenPosition)
        {
            if (towerMenu != null)
            {
                towerMenu.SetActive(true);
            }

            if (towerMenuRect != null)
            {
                towerMenuRect.position = screenPosition;
            }
        }

        private void ShowTowerPanel(Vector3 screenPosition)
        {
            if (towerPanel != null)
            {
                towerPanel.SetActive(true);
            }

            if (towerPanelRect != null)
            {
                towerPanelRect.position = screenPosition;
            }
        }

        private void HideMenus()
        {
            hasBuildSelection = false;
            selectedTower = null;
            if (towerMenu != null)
            {
                towerMenu.SetActive(false);
            }

            if (towerPanel != null)
            {
                towerPanel.SetActive(false);
            }
        }

        private static Tower FindTowerAt(Vector3 worldPosition)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);
            return hit == null ? null : hit.GetComponent<Tower>();
        }

        private Vector3 ScreenToWorld(Vector3 screenPosition)
        {
            Camera cameraToUse = worldCamera == null ? Camera.main : worldCamera;
            screenPosition.z = -cameraToUse.transform.position.z;
            return cameraToUse.ScreenToWorldPoint(screenPosition);
        }

        private static bool PointerPressed()
        {
            return Input.touchCount > 0
                ? Input.GetTouch(0).phase == TouchPhase.Began
                : Input.GetMouseButtonDown(0);
        }

        private static Vector3 PointerPosition()
        {
            return Input.touchCount > 0
                ? (Vector3)Input.GetTouch(0).position
                : Input.mousePosition;
        }
    }
}
