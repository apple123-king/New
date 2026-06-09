# /fleet Prompt: 迭代 1 实现批次

## 概览

实现《双人彩弹躲藏对抗》的本地双人 3D 灰盒垂直切片，验证完整比赛循环、攻击方枪线博弈、砖墙破坏穿透、躲避方持续移动与双视角调试 UI 是否成立。

本文件只描述 Developer 实现批次。Reviewer 不放入同一个 `/fleet` 批次；实现完成后通过 `/review` 或 `/auto-iterate` 下一轮单独验收。

## 模型分配

| 角色 | 模型系列 | 说明 |
|------|---------|------|
| Developer | Claude | 实现代码 |
| Reviewer | GPT | 后续独立验收，不在本批次执行 |

Developer 和 Reviewer 禁止使用同一模型系列。

## 批次顺序

1. 第一批串行执行 CoreLoop，产出共享接口 `GameSliceContracts.cs`。
2. CoreLoop 编译和 EditMode 测试通过后，第二批并行执行 Shooter、WallDestruction、DodgerAndUI。
3. 所有实现任务完成后，单独运行 `/review docs/harness/contracts/iteration-1-*.md` 或继续 `/auto-iterate 迭代 1`。

## Track 1: CoreLoop - 比赛状态机与回合循环

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责本地垂直切片的规则核心。目标是把整局比赛、大回合、单次进攻做成可测试的纯 C# 状态机，并提供其他 track 只读所需的状态接口和事件合同。

**任务**:
1. 建立整局、大回合、单次进攻三层状态机及数据模型。
2. 实现得分、回合切换、先手交替、部署开始/结束、反僵局换手规则。
3. 产出共享接口文件 `Assets/Scripts/Game/GameSliceContracts.cs`。
4. 编写比赛规则与反僵局规则的 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/CoreLoop/`, `Assets/Tests/EditMode/CoreLoopTests.cs`, `Assets/Scripts/Game/GameSliceContracts.cs`
- 禁止: `Assets/Scripts/Game/Shooter/`, `Assets/Scripts/Game/WallDestruction/`, `Assets/Scripts/Game/DodgerAndUI/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 能管理完整比赛状态、得分与胜负结算。
2. 能管理换手、部署与反僵局计时。
3. 共享接口足以驱动后续 track，不要求后续 track 修改 CoreLoop 内部实现。

**依赖**: 无

## Track 2: Shooter - 攻击方瞄准、开镜与射击

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责攻击方玩法。攻击方位于固定狙击位，未开镜时只能观察，开镜后进入窄视野瞄准态并向躲避方显示 100% 准确枪线。开镜与开火是两个分离输入，开火使用镜头中心权威射线。

**任务**:
1. 建立固定狙击位下的视野切换与开镜/开火输入流程。
2. 实现开镜后持续更新的权威轨迹。
3. 实现镜头中心射线开火判定、子弹消耗和命中结果上报。
4. 编写瞄准、轨迹与射击规则相关 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/Shooter/`, `Assets/Tests/EditMode/ShooterTests.cs`
- 禁止: `Assets/Scripts/Game/WallDestruction/`, `Assets/Scripts/Game/DodgerAndUI/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 开镜与开火分离，未开镜不能开火。
2. 轨迹与真实命中方向严格一致。
3. 能向 CoreLoop 与 WallDestruction 正确交互。

**依赖**: Track 1 的 `Assets/Scripts/Game/GameSliceContracts.cs`

## Track 3: WallDestruction - 砖墙网格、破坏与穿透

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责主砖墙。墙体使用固定砖块网格表达。单发命中墙体时按命中点周围圆形半径破坏砖块，随后子弹沿原轨迹继续检测躲避方命中，不偏移、不减速、不变线。

**任务**:
1. 定义砖墙网格与三条同构主墙初始化。
2. 实现圆形半径破坏与状态重置。
3. 实现穿墙后继续检测命中的顺序逻辑。
4. 编写墙体破坏、继承与重置规则相关 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/WallDestruction/`, `Assets/Tests/EditMode/WallDestructionTests.cs`
- 禁止: `Assets/Scripts/Game/Shooter/`, `Assets/Scripts/Game/DodgerAndUI/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 墙体采用确定性网格结构。
2. 圆形半径破坏结果稳定可复现。
3. 同一大回合继承、跨大回合重置成立。

**依赖**: Track 1 的 `Assets/Scripts/Game/GameSliceContracts.cs`

## Track 4: DodgerAndUI - 躲避方控制、双视角与最低限 UI

**Agent**: @developer  
**Model**: Claude

**上下文**:
负责躲避方第三人称玩法和双视角调试界面。躲避方在部署阶段和被进攻阶段始终可移动，可即时切换站立、下蹲、趴下。UI 显示比分、大回合、当前进攻方、攻击方剩余子弹和部署倒计时。

**任务**:
1. 实现躲避方第三人称移动和三态姿势切换。
2. 实现部署阶段自由移动与 4 秒结束锁定逻辑。
3. 实现双视角调试布局、攻击方部署遮罩和最低限比赛 UI。
4. 编写姿势状态、部署锁定和 UI 数据绑定相关 EditMode 测试。

**文件分区**:
- 独占: `Assets/Scripts/Game/DodgerAndUI/`, `Assets/Tests/EditMode/DodgerAndUITests.cs`
- 禁止: `Assets/Scripts/Game/Shooter/`, `Assets/Scripts/Game/WallDestruction/`, `Assets/ThirdParty/`, `Assets/Scripts/LubanGenerated/`

**验收标准**:
1. 姿势切换即时生效，躲避方全程可移动。
2. 部署阶段 4 秒锁定当前位置与姿势。
3. 双视角调试 UI 显示最低限关键状态且权限正确。

**依赖**: Track 1 的 `Assets/Scripts/Game/GameSliceContracts.cs`

## 后续验收

实现批次完成后，单独执行：

```text
/review docs/harness/contracts/iteration-1-core-loop.md
/review docs/harness/contracts/iteration-1-shooter.md
/review docs/harness/contracts/iteration-1-wall-destruction.md
/review docs/harness/contracts/iteration-1-dodger-and-ui.md
```

或继续自动闭环：

```text
/auto-iterate 迭代 1，完成本地双人 3D 灰盒垂直切片
```
