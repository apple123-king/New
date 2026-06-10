# /fleet Prompt: 迭代 2 实现批次

## 概览

在迭代 1 已归档的本地双人 3D 灰盒垂直切片基础上，推进“真实信息保密、联网前置权威边界、PlayMode 场景级自动化验证”三项前置目标。

本文件只描述 Developer 实现批次。Reviewer 不放入同一个 `/fleet` 批次；实现完成后通过 `/review` 或 `/auto-iterate` 下一轮单独验收。

## 模型分配

| 角色 | 模型系列 | 说明 |
|------|---------|------|
| Developer | Claude | 实现代码 |
| Reviewer | GPT | 后续独立验收，不在本批次执行 |

Developer 和 Reviewer 禁止使用同一模型系列。

## 批次顺序

1. 第一批串行执行 Shared Boundaries，先固定共享接口与角色视图边界。
2. Shared Boundaries 编译通过后，第二批并行执行 Role Visibility、Command Boundary、PlayMode Validation。
3. 并行 Track 完成后，第三批串行执行 Scene Integration 收口。
4. 所有实现任务完成后，单独运行 `/review docs/harness/contracts/iteration-2-*.md` 或继续 `/auto-iterate 迭代 2`。

## Track 1: Shared Boundaries - 共享合同与角色视图边界

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责迭代 2 的串行入口。目标是在不破坏迭代 1 核心规则的前提下，先把共享合同补成“输入命令 -> 权威规则 -> 角色视图”的最小边界。

**任务**:
1. 扩展 `Assets/Scripts/Game/GameSliceContracts.cs`，建立攻击方、躲避方、调试模式可消费的最小只读视图边界。
2. 为后续 Track 提供清晰共享合同，避免它们直接读取控制器内部实现。
3. 为新增共享边界补齐必要 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/GameSliceContracts.cs`, `Assets/Tests/EditMode/SharedBoundaryTests.cs`
- 禁止: `Assets/Scripts/Game/SceneIntegration/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 能表达攻击方、躲避方与调试模式的最小可见信息边界。
2. 不破坏既有编译与 EditMode 基线。
3. 下游 Track 可以只依赖共享合同实现。

**依赖**: 无

## Track 2: Role Visibility - 信息保密模式与 HUD 权限

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责“谁能看到什么”。目标是把现有调试型 HUD 从默认全信息状态，整理成正式信息保密模式和显式调试模式。

**任务**:
1. 基于共享合同整理攻击方、躲避方与调试模式的 HUD / 状态显示权限。
2. 保留调试全信息模式，但必须和正式信息保密模式显式区分。
3. 更新 `DodgerAndUI` 相关 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/DodgerAndUI/`, `Assets/Tests/EditMode/DodgerAndUITests.cs`
- 禁止: `Assets/Scripts/Game/Shooter/`, `Assets/Scripts/Game/WallDestruction/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 正式模式下不再默认泄露攻击方剩余子弹等信息。
2. 调试模式仍可服务玩法观察。
3. HUD 依赖共享只读视图，不直接窥视控制器内部。

**依赖**: Track 1 的 `Assets/Scripts/Game/GameSliceContracts.cs`

## Track 3: Command Boundary - 输入命令与权威适配层

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责“输入怎么进入规则”。目标是把当前 SceneIntegration 中混在一起的输入采集和权威调用，整理成更适合未来联网接入的适配边界。

**任务**:
1. 在 `SceneIntegration/` 中抽出清晰的命令入口或适配层。
2. 表达攻击方观察、开镜、开火与躲避方移动、姿态切换等核心命令。
3. 为适配层补齐必要 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/SceneIntegration/`, `Assets/Tests/EditMode/SceneIntegrationCommandBoundaryTests.cs`
- 禁止: `Assets/Scripts/Game/DodgerAndUI/`, `Assets/Scripts/Game/WallDestruction/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 输入采集与权威调用之间至少存在一层清晰边界。
2. 不引入完整 NGO，只做前置架构整理。
3. 既有规则与场景行为基线不被破坏。

**依赖**: Track 1 的 `Assets/Scripts/Game/GameSliceContracts.cs`

## Track 4: PlayMode Validation - 场景级自动化验证

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责把场景级验证从“只靠 Unity Skills 临时取证”升级为“至少有一条可复用的 PlayMode 自动化验证链路”。

**任务**:
1. 新建 `Assets/Tests/PlayMode/` 与必要测试入口。
2. 编写至少一条 PlayMode 测试，覆盖运行时自举、双相机、角色视图或模式切换中的关键行为。
3. 保证既有 EditMode 测试无回退。

**文件分区**:
- 独占: `Assets/Tests/PlayMode/`
- 禁止: `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 至少一条 PlayMode 测试稳定可执行。
2. PlayMode 结果可作为 reviewer 的正式证据之一。
3. 不影响迭代 1 已通过的规则核心。

**依赖**: Track 1 的共享合同；运行时代码只读

## Track 5: Scene Integration - 场景桥接收口与模式切换

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责迭代 2 的串行收口。目标是把共享合同、角色可见性、命令适配层和场景验证链路接回现有运行时桥接，并形成稳定的正式模式 / 调试模式切换。

**任务**:
1. 收口 `SceneIntegration/` 与 `SampleScene.unity` 的运行时装配。
2. 确认双相机、自举对象、基础 HUD 和关键桥接流程保持可用。
3. 让正式信息保密模式与调试全信息模式在运行时可区分。

**文件分区**:
- 独占: `Assets/Scripts/Game/SceneIntegration/`, `Assets/Scenes/SampleScene.unity`
- 禁止: `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 新的边界与模式切换能够在运行时成立。
2. 不破坏迭代 1 已验收的核心对局流程。
3. 能配合 PlayMode 测试和后续 reviewer 取证。

**依赖**: Track 2、Track 3、Track 4

## 后续验收

实现批次完成后，单独执行：

```text
/review docs/harness/contracts/iteration-2-shared-boundaries.md
/review docs/harness/contracts/iteration-2-role-visibility.md
/review docs/harness/contracts/iteration-2-command-boundary.md
/review docs/harness/contracts/iteration-2-playmode-validation.md
/review docs/harness/contracts/iteration-2-scene-integration.md
```

或继续自动闭环：

```text
/auto-iterate 迭代 2，完成信息保密验证与联网前置架构
```
