# 迭代 1 执行计划

## 执行目标

按“可测试规则优先、表现装配后置”的顺序完成本地双人 3D 灰盒垂直切片。执行时以任务合同为硬约束，以需求拆分为行为标准。

## 阶段顺序

### 阶段 0: 仓库与快照准备

1. 确认 Unity `.gitignore` 生效，避免 `Library/`、`Logs/`、`Temp/`、`UserSettings/` 进入索引或提交。
2. 创建 Unity Skills 快照 `iteration-1-local-vertical-slice`。
3. 确认 `Assets/Scripts/Game/` 与 `Assets/Tests/EditMode/` 目录结构。

### 阶段 1: CoreLoop 串行先行

1. 创建 `Assets/Scripts/Game/GameSliceContracts.cs`。
2. 实现纯 C# 比赛状态机。
3. 覆盖比分、大回合、先手交替、换手、部署、子弹、反僵局测试。
4. 编译和 EditMode 测试通过后，其他 Track 才能依赖共享接口。

### 阶段 2: 三个功能 Track 并行

1. Shooter 读取 CoreLoop 合同，实现开镜、前摇、轨迹和开火流程。
2. WallDestruction 读取 CoreLoop 合同，实现墙体网格、破坏、穿透和重置。
3. DodgerAndUI 读取 CoreLoop 合同，实现躲避方姿态、部署锁定和最低限 UI。

### 阶段 3: 场景装配串行收口

1. 在 `SampleScene.unity` 中装配固定狙击位、主墙、躲避区域、相机和 UI。
2. 连接 CoreLoop、Shooter、WallDestruction、DodgerAndUI 的运行时桥接组件。
3. 执行编译、测试、日志检查和必要场景验证。

### 阶段 4: 独立验收

1. Reviewer 在实现批次完成后单独启动。
2. Reviewer 对照四份任务合同逐项验收。
3. 生成报告到 `docs/harness/reports/iteration-1-review.md`。
4. 更新 `progress.md` 和 `error-log.md`。

## 并行策略

| Track | 并行性 | 原因 |
|------|--------|------|
| CoreLoop | 必须先串行 | 创建共享接口与规则核心 |
| Shooter | 可与 WallDestruction、DodgerAndUI 并行 | 独占目录不同，只读取 CoreLoop 合同 |
| WallDestruction | 可与 Shooter、DodgerAndUI 并行 | 独占目录不同，只读取 CoreLoop 合同 |
| DodgerAndUI | 可与 Shooter、WallDestruction 并行 | 独占目录不同，只读取 CoreLoop 合同 |
| Scene Integration | 必须串行 | `SampleScene.unity` 不适合多 Track 同时修改 |
| Review | 必须最后单独执行 | 依赖全部实现完成 |

## 文件分区

| Track | 独占文件 | 共享或只读 |
|------|----------|------------|
| CoreLoop | `Assets/Scripts/Game/CoreLoop/`, `Assets/Tests/EditMode/CoreLoopTests.cs`, `Assets/Scripts/Game/GameSliceContracts.cs` | PRD、需求拆分、编码规范 |
| Shooter | `Assets/Scripts/Game/Shooter/`, `Assets/Tests/EditMode/ShooterTests.cs` | `GameSliceContracts.cs`, WallDestruction 接口 |
| WallDestruction | `Assets/Scripts/Game/WallDestruction/`, `Assets/Tests/EditMode/WallDestructionTests.cs` | `GameSliceContracts.cs` |
| DodgerAndUI | `Assets/Scripts/Game/DodgerAndUI/`, `Assets/Tests/EditMode/DodgerAndUITests.cs` | `GameSliceContracts.cs`, CoreLoop 状态 |
| Integration | `Assets/Scenes/SampleScene.unity` | 所有模块输出 |

## 验证链路

每个 Track 完成时必须执行:

1. `debug_check_compilation` 返回 0 error。
2. 对应 EditMode 测试通过。
3. 既有测试无回退。
4. Unity Console 无新增 error。
5. 更新对应任务合同状态。

统一验收时额外执行:

1. 全量编译检查。
2. 全量 EditMode 测试。
3. 必要的场景手动验证或 Unity Skills 场景查询。
4. 生成 `docs/harness/reports/iteration-1-review.md`。

## 当前阶段

- 阶段 0、1、2、3、4 已完成。
- Scene Integration 已补齐专门任务合同，并已通过独立正式验收。
- `iteration-1-review.md` 现已覆盖纯逻辑层与场景桥接两部分结论。
- 如果后续继续维护本迭代，重点应转为归档与历史文档措辞清理，而不是重新进入实现阶段。
