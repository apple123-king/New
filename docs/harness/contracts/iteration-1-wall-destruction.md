# 任务合同: WallDestruction - 砖墙网格、破坏与穿透

## 基本信息

- **迭代**: 迭代 1
- **关联需求**: `docs/harness/specs/iteration-1-requirements.md`
- **状态**: 通过

## 完成定义

### 验收标准

1. [x] 以固定砖块网格表达一面主砖墙，并支持确定性初始化。
2. [x] 单发命中墙体时，能按命中点周围圆形半径稳定破坏砖块。
3. [x] 子弹命中墙体后的射线结果保留原始方向，供 Shooter/后续穿透判定使用。
4. [x] 墙体状态能在同一大回合跨换手继承，并在 CoreLoop 进入下一大回合时按版本号整体重置。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 新增 `Assets/Tests/EditMode/WallDestructionTests.cs`
- [x] EditMode 测试全量通过: `152/152`
- [x] 覆盖网格初始化、圆形破坏、重复命中不重复计数、版本重置

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/WallDestruction/
Assets/Tests/EditMode/WallDestructionTests.cs
```

### 共享依赖

```text
Assets/Scripts/Game/GameSliceContracts.cs
Assets/Scripts/Game/CoreLoop/
```

## 实现记录

- 新增 `BrickGridWall` 作为确定性砖墙状态模型。
- 新增 `WallShotResolution` 和 `IWallShotResolver` 作为 Shooter 与墙体判定之间的合同。
