# 需求文档: 迭代 1 本地双人 3D 灰盒垂直切片

## 基本信息

- **迭代**: 迭代 1
- **优先级**: 高
- **复杂度**: 复杂
- **依赖**: Unity 2022.3.62f3c1、UGUI/TextMeshPro、Unity Test Runner、Unity Skills

## 功能描述

实现一个本地双人同时操作的 3D 灰盒垂直切片，用于验证攻击方枪线博弈、砖墙破坏穿透、躲避方持续移动与完整比赛循环是否成立。本切片不追求正式美术、联网、房间、平台接入或商业化流程。

## 输入

- 攻击方观察、开镜、开火输入
- 躲避方移动与站立、下蹲、趴下姿态切换输入
- CoreLoop 的部署开始、部署结束、开火、命中、换手、进入下一大回合事件
- Shooter 的权威射线与墙体命中点
- WallDestruction 的墙体破坏状态与穿透检测结果

## 输出

- 当前比分、大回合编号、当前进攻方、剩余子弹、部署倒计时
- 开镜后对躲避方可见的权威枪线
- 单发射击的命中、落空、破墙与穿透结果
- 同一大回合内继承的墙体破坏状态
- 跨大回合重置后的完整墙体状态
- 比赛结束与胜负结算结果

## 行为规则

1. 整局最多 `9` 个大回合，先到 `5` 分获胜；达到上限仍无人先到 `5` 分时按当前比分结算。
2. 每个大回合只有任意一方成功命中时才结束并产生 `1` 分。
3. 单次进攻只有 `3` 发子弹；`3` 发耗尽未命中则攻防换手并进入新部署阶段。
4. 单次进攻无常规时间上限，但攻击方在可博弈状态下 `8` 秒未开枪时警告，再过 `3` 秒仍未开枪则强制换手。
5. 攻击方固定狙击位，未开镜只能观察；开镜后切换窄视野并允许开火。
6. 开镜与开火是两个分离输入，开镜到可开火存在 `0.2` 到 `0.3` 秒前摇。
7. 命中权威由开镜镜头中心射线决定，后坐力和枪口表现不得改变真实判定方向。
8. 墙体采用固定砖块网格；单发命中墙体后先按圆形半径破坏砖块，再沿原轨迹继续检测躲避方命中。
9. 子弹穿墙前后不偏移、不减速、不变线。
10. 墙体破坏状态同一大回合内跨换手继承，进入下一大回合整体重置。
11. 躲避方在部署阶段和被进攻阶段始终可移动。
12. 躲避方姿态仅包含站立、下蹲、趴下，切换即时影响受击体积。
13. 部署阶段固定 `4` 秒，结束时当前位置和当前姿态成为本次进攻起始状态。
14. 双视角调试 UI 只验证玩法流程，不验证真实信息保密。
15. 剩余子弹数只显示给攻击方视角，不显示给躲避方视角。

## 验收标准

1. [x] CoreLoop 能独立管理比分、大回合、换手、部署、子弹消耗、反僵局与胜负结算。
2. [x] Shooter 能保证开镜轨迹与真实命中射线严格一致。
3. [x] WallDestruction 能以确定性网格稳定完成圆形半径破坏、继承与重置。
4. [x] DodgerAndUI 能支持部署和被进攻阶段的连续移动、姿态即时切换与最低限 UI。
5. [x] 所有核心规则均有 EditMode 测试覆盖。
6. [x] `debug_check_compilation` 返回 0 error，Unity Console error 为 `0`。
7. [x] 统一验收报告对照四份任务合同逐项给出通过、不通过或风险说明。

## 文件分区

### 新增文件

- `Assets/Scripts/Game/GameSliceContracts.cs`
- `Assets/Scripts/Game/CoreLoop/*.cs`
- `Assets/Scripts/Game/Shooter/*.cs`
- `Assets/Scripts/Game/WallDestruction/*.cs`
- `Assets/Scripts/Game/DodgerAndUI/*.cs`
- `Assets/Tests/EditMode/CoreLoopTests.cs`
- `Assets/Tests/EditMode/ShooterTests.cs`
- `Assets/Tests/EditMode/WallDestructionTests.cs`
- `Assets/Tests/EditMode/DodgerAndUITests.cs`

### 后续需要修改

- `Assets/Scenes/SampleScene.unity`: 组装灰盒场景、双视角、墙体、控制器与 UI
- `docs/harness/progress.md`: 更新 Track 状态、验收结果与维护记录
- `docs/harness/error-log.md`: 记录规划、编译、测试、文件分区等问题

### 禁止修改

- `Assets/ThirdParty/`
- `Assets/Scripts/LubanGenerated/`
- `Library/`、`Logs/`、`Temp/`、`UserSettings/`

## 技术备注

- 业务规则优先放入纯 C# 类；MonoBehaviour 只负责 Unity 生命周期、Inspector 绑定和事件转发。
- 模块之间优先通过共享接口、只读状态快照和事件交互，避免跨模块直接改内部状态。
- 首版不引入联网权威、复杂物理破坏、复杂动画状态机或正式美术。
