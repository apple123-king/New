# 迭代 1 架构规划

## 目标

为本地双人 3D 灰盒垂直切片建立可测试、可并行、可复盘的模块边界。首版目标是验证玩法成立，不提前承担联网同步、正式美术、复杂物理破坏或平台接入成本。

## 技术基线

- Unity: 2022.3.62f3c1
- UI: UGUI + TextMeshPro
- 测试: Unity Test Runner EditMode
- 代码风格: 薄 MonoBehaviour + 纯 C# 逻辑层
- 当前实现状态: 纯逻辑层已完成；场景桥接待实现

## 模块边界

| 模块 | 职责 | 当前实现 |
|------|------|----------|
| CoreLoop | 整局、大回合、单次进攻、比分、换手、反僵局 | `CoreLoopController` + `CoreLoopSettings` |
| Shooter | 固定狙击位、开镜、前摇、权威射线、开火上报 | `ShooterController` + `ShooterShotResult` |
| WallDestruction | 砖块网格、圆形破坏、穿墙结果、继承与重置 | `BrickGridWall` + `WallShotResolution` |
| DodgerAndUI | 躲避方移动、姿态、部署锁定、双视角 UI 状态 | `DodgerController` + `DebugHudPresenter` |
| GameSliceContracts | 跨模块只读状态、枚举、快照接口 | `GameSliceContracts.cs` |

## 通信方式

1. CoreLoop 暴露只读状态快照，供 Shooter、WallDestruction、DodgerAndUI 查询。
2. Shooter 通过委托向 CoreLoop 上报命中或落空结果。
3. WallDestruction 接收射线/命中点输入，返回破坏与穿透检测结果，不直接修改 Shooter 内部状态。
4. DodgerAndUI 读取 CoreLoop 状态并生成 UI 状态，不反向驱动比赛规则。
5. 场景桥接阶段可使用 MonoBehaviour 将 Unity 输入、相机、碰撞和 UI 转发到纯逻辑层。

## 方案选择

### 采用: 纯 C# 规则核心 + 薄 MonoBehaviour 桥接

- 优点: 容易写 EditMode 测试，模块边界清晰，后续迁移到联网权威更低成本。
- 风险: 前期需要先定义共享接口。
- 当前状态: 已采用，纯逻辑层已通过测试。

### 不采用: MonoBehaviour 直接驱动所有规则

- 问题: 状态散落在场景对象上，测试困难，后续联网和复盘成本高。

### 暂不采用: ScriptableObject 运行时状态主导

- 问题: 高频变化运行时状态不宜放在 ScriptableObject 中，容易造成测试和重置边界混乱。

## 风险与约束

- `GameSliceContracts.cs` 是共享文件，后续修改需要同步检查所有模块测试。
- `Assets/Scenes/SampleScene.unity` 属于集成装配文件，不适合多个 Track 并行修改。
- Unity 生成目录必须保持在提交之外。
- 所有复杂规则必须先有纯 C# 测试，再接入场景表现。
