# CarrotGuard — 从零构建 Unity 2D 塔防手游原型

## 0. 项目宪法（任何阶段都不可违反）

### 0.1 Git 工作流（Worktree 纯净原则）
- master 分支永远保持可编译、全测试通过的状态，禁止直接 push
- 每个 Phase 创建独立开发分支：`phase/0-skeleton`、`phase/1-data-model`、`phase/2-movement` ...
- 在开发分支上按 Step 逐步 commit（Red → Green → Refactor）
- 一个 Phase 全部 Step 完成后，squash merge 回 master，merge commit message 为 `milestone: Phase N — <描述>`
- merge 前确认：全量测试通过 + 无编译警告 + git status 干净
- merge 后删除开发分支，再从最新 master 切出下一个 Phase 分支
- 禁止跨 Phase 分支并行开发

### 0.2 Executable Boundary（可执行边界）
- 每个 commit 必须是可编译状态，禁止提交编译不过的代码
- Phase 1-5 的每个 Green commit 必须附带通过的测试作为证据
- Phase 6-7 无自动化测试的 commit，必须在 commit message 中写明手动验证步骤

### 0.3 Contract Regression（契约回归）
- 任何 commit 不得破坏已有测试。如果新功能需要修改旧接口，先修改测试（Red），再改实现（Green），分两个 commit
- 禁止删除或 skip 已有测试

### 0.4 Prompt Surface Contract（需求锁定）
- 本文档即全部需求，Codex 不得自行扩展功能范围
- 禁止添加：联网功能、存档系统、音效系统、粒子特效、多关卡、成就系统、广告 SDK
- 如果实现过程中发现需要本文档未提及的辅助类，可以创建，但必须在 commit message 中标注 `[辅助] 原因：xxx`

### 0.5 Cache Stability（架构稳定）
- 目录结构一旦在 Phase 0 建立，后续 Phase 禁止移动文件路径
- 命名空间 TowerDefense.{Enemy, Tower, Core, UI, Tests} 锁定，不得新增或修改
- 公开接口（public 方法签名）一旦在 Green commit 中确立，后续只能通过 Red → Green 流程修改

### 0.6 Owner Transfer（边界职责）
- 每个类只做一件事，职责边界在第一次创建时通过文件头部中文注释锁定
- 禁止出现上帝类：单个类不超过 200 行，单个方法不超过 30 行
- 违反时必须先 `refactor:` 拆分，再继续功能开发

### 0.7 Change Ledger（变更台账）
- 每个 commit message 严格遵循格式：`<type>(<scope>): <描述>`
  - type: test | feat | refactor | fix | docs
  - scope: core | enemy | tower | ui | build
  - 示例：`test(core): GoldSystem 扣款不足时返回 false`
- 禁止一个 commit 跨两个 scope（除非是 refactor 提取公共模块）

---

## 1. 最终交付物

一个可在 Unity Editor 中运行的 2D 塔防原型，包含：
- 怪物沿路径移动，到终点扣血
- 2 种炮塔（快速低伤 / 慢速高伤），自动锁定射击
- 3 波递增刷怪
- 金币 + 生命值系统
- 手机触控建造 / 升级 / 出售
- 胜利 / 失败 / 重开 UI
- README 场景搭建指南

不多，不少。

---

## 2. TDD 迭代计划

严格按顺序执行。每个 Phase 在独立分支上开发，完成后 squash merge 回 master。

### Phase 0: 骨架
**分支：`phase/0-skeleton`**

commit: `feat(build): 初始化目录结构和 asmdef`

创建：
- Assets/Scripts/{Enemy,Tower,Core,UI}/
- Assets/Tests/EditMode/
- Assembly Definition: TowerDefense.asmdef, TowerDefense.Tests.asmdef（引用 TowerDefense + NUnit）

→ squash merge master: `milestone: Phase 0 — 项目骨架`

### Phase 1: 数据模型（纯 C#，零 MonoBehaviour）
**分支：`phase/1-data-model`**（从最新 master 切出）

| Step | Red | Green |
|------|-----|-------|
| 1.1 | `test(core): GoldSystem — 初始余额正确；扣款成功扣除；余额不足返回 false 不扣` | `feat(core): GoldSystem` |
| 1.2 | `test(core): HealthSystem — 扣血正确；归零触发 OnDead 事件；归零后不再重复触发` | `feat(core): HealthSystem` |
| 1.3 | `test(core): WaveConfig — 3 波数据解析正确` | `feat(core): WaveConfig` |
| 1.4 | `test(tower): TowerData — 两种塔属性读取正确` | `feat(tower): TowerData` |
| 1.5 | `test(enemy): DamageCalculator — 扣血正确；击杀时标记死亡并返回金币奖励` | `feat(enemy): DamageCalculator` |

→ squash merge master: `milestone: Phase 1 — 数据模型层`

### Phase 2: 移动系统
**分支：`phase/2-movement`**

| Step | Red | Green |
|------|-----|-------|
| 2.1 | `test(enemy): PathFollower — 沿路径点序列移动；到终点触发 OnReachedEnd` | `feat(enemy): Waypoint + PathFollower` |
| 2.2 | `test(enemy): PathFollower — 速度与 deltaTime 成正比` | `refactor(enemy): PathFollower 提取速度参数` |

→ squash merge master: `milestone: Phase 2 — 移动系统`

### Phase 3: 瞄准与射击
**分支：`phase/3-combat`**

| Step | Red | Green |
|------|-----|-------|
| 3.1 | `test(tower): TargetSelector — 选射程内最近；无目标返回 null；目标出射程后丢失` | `feat(tower): TargetSelector` |
| 3.2 | `test(tower): FireController — 冷却结束触发 OnFire；冷却中不触发` | `feat(tower): FireController` |
| 3.3 | `test(tower): BulletMover — 追踪目标；目标消失时标记自毁` | `feat(tower): BulletMover` |

→ squash merge master: `milestone: Phase 3 — 瞄准与射击系统`

### Phase 4: 刷怪系统
**分支：`phase/4-spawner`**

| Step | Red | Green |
|------|-----|-------|
| 4.1 | `test(enemy): WaveSpawner — 按配置依次产出；波间有间隔` | `feat(enemy): WaveSpawner` |
| 4.2 | `test(core): VictoryChecker — 全波次完成+存活为零→触发 OnVictory` | `feat(core): VictoryChecker` |

→ squash merge master: `milestone: Phase 4 — 刷怪系统`

### Phase 5: 建造系统
**分支：`phase/5-build`**

| Step | Red | Green |
|------|-----|-------|
| 5.1 | `test(core): GridSystem — 坐标对齐整数；查询占用状态正确` | `feat(core): GridSystem` |
| 5.2 | `test(core): BuildService — 金币够+格子空→成功扣款；否则失败` | `feat(core): BuildService` |
| 5.3 | `test(tower): TowerUpgrade — 升级扣款提升属性；出售返还 50%` | `feat(tower): TowerUpgrade` |

→ squash merge master: `milestone: Phase 5 — 建造系统`

### Phase 6: Unity 集成
**分支：`phase/6-integration`**

逐步将纯逻辑绑定到 Unity 组件，每步一个 commit：
- `feat(enemy): Enemy MonoBehaviour — 绑定 PathFollower + DamageCalculator`
- `feat(tower): Tower MonoBehaviour — 绑定 TargetSelector + FireController`
- `feat(tower): Bullet MonoBehaviour — 绑定 BulletMover`
- `feat(core): LevelManager 单例 — 持有 GoldSystem + HealthSystem + WaveSpawner + VictoryChecker`
- `feat(enemy): EnemySpawner MonoBehaviour — 绑定 WaveSpawner`

→ squash merge master: `milestone: Phase 6 — Unity 集成`

### Phase 7: UI 与交互
**分支：`phase/7-ui`**

每步一个 commit：
- `feat(ui): HUD — 实时显示金币和生命值`
- `feat(ui): 点击空白格子弹出炮塔选择菜单`
- `feat(ui): 点击已有炮塔弹出升级/出售面板`
- `feat(ui): 防误触 EventSystem.IsPointerOverGameObject`
- `feat(ui): Game Over / Victory 界面 + 重新开始`
- `feat(ui): 开始界面 → 点击进入关卡`

→ squash merge master: `milestone: Phase 7 — UI 与交互`

### Phase 8: 收尾
**分支：`phase/8-docs`**
- `docs(build): README.md — 场景搭建指南`
- `refactor(build): 全量测试通过，最终清理`

→ squash merge master: `milestone: Phase 8 — 收尾`

---

## 3. 工程约束

- Unity 2D，C#，竖屏 1080x1920
- 不用第三方插件
- 每个文件顶部中文注释说明职责
- 不确定的地方注释 `// 假设：xxx`
- 不要问任何问题，严格按 Phase/Step 顺序执行
- 每个 Phase 开始前：`git checkout master && git pull && git checkout -b phase/N-xxx`
- 每个 Phase 结束后：全量测试通过 → `git checkout master && git merge --squash phase/N-xxx && git commit -m "milestone: Phase N — 描述"` → `git branch -d phase/N-xxx`
- 如果某个 Step 的测试写不出来（比如纯 UI），在 commit message 中写明 `[无测试] 手动验证：xxx`
- 禁止在任何 Step 中预埋"将来可能用到"的代码
