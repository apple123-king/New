# 任务合同: Shooter - 攻击方瞄准、开镜与射击

## 基本信息

- **迭代**: 迭代 1
- **关联需求**: `docs/harness/specs/iteration-1-requirements.md`
- **状态**: 通过

## 完成定义

### 验收标准

1. [x] 攻击方具备固定狙击位上的开镜状态；未开镜或开镜前摇未完成时不能开火。
2. [x] 开镜与开火为分离输入；开镜后存在 `0.25` 秒前摇，权威轨迹在开镜瞬间出现并持续更新。
3. [x] 射击采用镜头中心权威射线；后坐力只作为表现偏移，不改变真实判定方向。
4. [x] 能向 CoreLoop 上报命中结果，并从 WallDestruction 获取穿透与破坏反馈。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 新增 `Assets/Tests/EditMode/ShooterTests.cs`
- [x] EditMode 测试全量通过: `152/152`
- [x] 覆盖开镜前摇、权威射线归一化、后坐力不影响判定、命中上报

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/Shooter/
Assets/Tests/EditMode/ShooterTests.cs
```

### 共享依赖

```text
Assets/Scripts/Game/GameSliceContracts.cs
Assets/Scripts/Game/CoreLoop/
Assets/Scripts/Game/WallDestruction/
```

## 实现记录

- 新增 `ShooterController` 作为纯 C# 控制器。
- 新增 `ShooterShotResult` 记录单次射击的权威判定结果。
- 通过 `IWallShotResolver` 与墙体/躲避方命中判定解耦。
