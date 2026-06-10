# 迭代 2 执行计划

## 执行目标

以“边界先行、验证增强、联网后置”为原则，完成信息保密验证、联网前置架构整理和 PlayMode 场景级自动化验证。

## 阶段顺序

### 阶段 0: 迭代基线确认

1. 以迭代 1 已归档结果作为稳定基线。
2. 确认 `progress.md`、`reports/` 与现有代码状态一致。
3. 确认 Unity 生成目录未误纳入索引。

### 阶段 1: 共享边界串行先行

1. 先定义迭代 2 的共享合同与角色视图边界。
2. 明确哪些信息属于攻击方可见、躲避方可见、调试模式可见。
3. 明确命令入口、权威结果和只读视图之间的最小接口。

### 阶段 2: 三个 Track 并行

1. Role Visibility Track：整理 HUD 与状态显示，建立真实信息保密模式。
2. Command Boundary Track：把输入命令与权威规则调用拆出更清晰的适配层。
3. PlayMode Validation Track：补充至少一条场景级自动化验证，覆盖运行时桥接核心行为。

### 阶段 3: 场景装配串行收口

1. 将角色视图、命令边界和现有 Scene Integration 重新装配。
2. 确认双相机、HUD、自举对象和基础输入链路保持可用。
3. 检查调试模式与正式信息保密模式的切换是否明确。

### 阶段 4: 独立验收

1. Reviewer 单独启动，不与 Developer 混批。
2. 对照迭代 2 合同逐项检查信息边界、架构边界和自动化验证结果。
3. 输出 `docs/harness/reports/iteration-2-review.md`。
4. 同步 `progress.md`、`error-log.md` 与必要的 `improvements.md`。

## 并行策略

| Track | 并行性 | 原因 |
|------|--------|------|
| Shared Contracts / View Models | 必须先串行 | 所有实现都依赖统一边界 |
| Role Visibility | 可并行 | 重点修改 HUD、状态显示与角色视图 |
| Command Boundary | 可并行 | 重点修改输入到权威调用的中间层 |
| PlayMode Validation | 可并行 | 重点补测试，不应阻塞核心边界设计 |
| Scene Integration | 必须串行 | 需要集中收口运行时装配 |
| Review | 必须最后单独执行 | 依赖所有实现完成 |

## 文件分区

| Track | 独占文件建议 | 共享或只读 |
|------|---------------|------------|
| Shared Contracts | `Assets/Scripts/Game/GameSliceContracts.cs` | 迭代 1 既有模块 |
| Role Visibility | `Assets/Scripts/Game/DodgerAndUI/`, 部分 `SceneIntegration/` HUD 相关文件 | `GameSliceContracts.cs` |
| Command Boundary | `Assets/Scripts/Game/SceneIntegration/` 中输入与桥接相关文件 | 规则核心模块 |
| PlayMode Validation | `Assets/Tests/PlayMode/` | 只读所有运行时代码 |
| Integration | `Assets/Scripts/Game/SceneIntegration/`, `Assets/Scenes/SampleScene.unity` | 所有模块输出 |

## 验证链路

每个 Track 完成时必须执行:

1. `debug_check_compilation` 返回 0 error。
2. 对应 EditMode 或 PlayMode 测试通过。
3. 既有验收基线无回退。
4. Unity Console 无新增 error。
5. 更新对应任务合同状态。

统一验收时额外执行:

1. 全量编译检查。
2. 迭代 1 核心规则回归检查。
3. 至少一条 PlayMode 场景级自动化验证。
4. 必要的 Unity Skills 运行态补充取证。
5. 生成 `docs/harness/reports/iteration-2-review.md`。

## 当前阶段

- 阶段 0、1、2、3 已完成到“首轮实现落地”层面。
- 当前正式状态是“已实现”。
- 下一步不是继续扩展实现，而是进入独立验收，并处理 PlayMode 结果回收阻塞。
