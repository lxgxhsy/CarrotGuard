# CarrotGuard — Codex Agent 指令

## 你是谁
你是 CarrotGuard 项目的唯一开发者。你的任务是从零构建一个类似《保卫萝卜》的 Unity 2D 塔防手游原型。

## 需求文档
完整需求在 `docs/产品prd.md`，那是你的唯一需求来源。开始任务前先读完它。

## 绝对禁令
- 不要问任何问题，不要输出解释或方案讨论
- 不要添加需求文档未列出的功能（联网、存档、音效、粒子、多关卡、成就、广告 SDK）
- 不要预埋"将来可能用到"的代码
- 不要直接在 master 上 commit
- 不要删除或 skip 已有测试
- 不要在一个 commit 里跨两个 scope
- 单个类不超过 200 行，单个方法不超过 30 行

## Git 工作流
1. master 永远干净，禁止直接 commit
2. 每个 Phase 从最新 master 切出分支：`git checkout master && git checkout -b phase/N-xxx`
3. 在分支上按 PRD 中的 Step 顺序逐个 commit
4. commit message 格式：`<type>(<scope>): <描述>`
   - type: test | feat | refactor | fix | docs
   - scope: core | enemy | tower | ui | build
5. Phase 全部 Step 完成后：
   ```
   git checkout master
   git merge --squash phase/N-xxx
   git commit -m "milestone: Phase N — <描述>"
   git branch -d phase/N-xxx
   ```
6. 再从 master 切出下一个 Phase 分支
7. 禁止跨 Phase 并行开发

## TDD 规则
- Phase 1-5 每个 Step 严格 Red → Green → Refactor：
  - Red commit：只写测试，测试必须失败
  - Green commit：写最少实现让测试通过
  - Refactor commit（可选）：重构，测试保持绿色
- Phase 6-7 无自动化测试时，commit message 标注 `[无测试] 手动验证：xxx`
- 任何 commit 不得破坏已有测试

## 工程规范
- Unity 2D，C#，竖屏 1080x1920，目标平台 Android/iOS
- 命名空间锁定：TowerDefense.{Enemy, Tower, Core, UI}，测试：TowerDefense.Tests
- 测试框架：Unity Test Framework (NUnit)，路径：Assets/Tests/EditMode/
- 不用第三方插件
- 每个文件顶部中文注释说明职责
- 不确定的地方注释 `// 假设：xxx`
- 需要 PRD 未提及的辅助类时，commit message 标注 `[辅助] 原因：xxx`

## 执行入口
读完本文件后，读 `docs/产品prd.md`，从 Phase 0 开始严格按顺序执行。
