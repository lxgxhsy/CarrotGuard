# CarrotGuard

Unity 2D 塔防原型。场景目标分辨率为竖屏 1080x1920，核心逻辑位于 `Assets/Scripts`，EditMode 测试位于 `Assets/Tests/EditMode`。

## 场景搭建

1. 创建一个 2D Scene，主相机设为正交相机，画面按竖屏构图摆放路径、建造格和 UI Canvas。
2. 创建空物体 `LevelManager`，挂载 `TowerDefense.Core.LevelManager`。
3. 创建敌人路径点，按移动顺序摆放多个 `Transform`。
4. 创建敌人预制体，挂载 `TowerDefense.Enemy.Enemy`，绑定生命、速度、奖励参数。
5. 创建空物体 `EnemySpawner`，挂载 `TowerDefense.Enemy.EnemySpawner`，绑定敌人预制体和路径点数组。
6. 创建快速塔和重炮塔预制体，挂载 `TowerDefense.Tower.Tower`，选择 `Rapid` 或 `Heavy`，绑定子弹预制体。
7. 创建子弹预制体，挂载 `TowerDefense.Tower.Bullet`，设置速度和伤害。
8. 创建 Canvas，添加金币文本、生命文本，并挂载 `TowerDefense.UI.Hud`。
9. 在 Canvas 中创建炮塔选择菜单，按钮分别绑定 `BuildMenu.BuildRapidTower` 和 `BuildMenu.BuildHeavyTower`。
10. 在 Canvas 中创建升级/出售面板，按钮分别绑定 `BuildMenu.UpgradeSelectedTower` 和 `BuildMenu.SellSelectedTower`。
11. 在场景中添加 `EventSystem`，确保 UI 点击不会穿透到场景建造。
12. 创建胜利、失败面板，挂载 `TowerDefense.UI.GameResultView`，重开按钮绑定 `Restart`。
13. 创建开始面板，挂载 `TowerDefense.UI.StartScreen`，开始按钮绑定 `EnterLevel`。

## 运行检查

- 进入 Play 后，开始界面显示，点击开始进入关卡。
- 敌人按 3 波配置沿路径移动，到达终点扣生命值。
- 点击空白建造格显示炮塔选择菜单，金币足够时能建造炮塔。
- 炮塔自动锁定射程内敌人，按冷却生成子弹并造成伤害。
- 点击已有炮塔可升级或出售。
- 全波次完成且敌人清空时显示胜利；生命归零时显示失败；重开按钮重载当前场景。
