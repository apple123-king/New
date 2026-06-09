# 任务合同: DodgerAndUI - 躲避方控制、双视角与最低限 UI

## 基本信息

- **迭代**: 迭代 1
- **关联需求**: `docs/harness/specs/iteration-1-requirements.md`
- **状态**: 通过

## 完成定义

### 验收标准

1. [x] 躲避方具备可移动状态，以及站立/下蹲/趴下三种姿态与对应受击体积。
2. [x] 部署阶段允许自由移动，并能锁定当前部署位置与姿态作为起始状态。
3. [x] 双视角调试状态能区分攻击方与躲避方 HUD。
4. [x] 最低限 UI 状态包含比分、大回合编号、当前进攻方、攻击方剩余子弹和部署倒计时；剩余子弹不显示给躲避方。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 新增 `Assets/Tests/EditMode/DodgerAndUITests.cs`
- [x] EditMode 测试全量通过: `152/152`
- [x] 覆盖移动、姿态受击体积、部署锁定、攻击方/躲避方 HUD 可见性

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/DodgerAndUI/
Assets/Tests/EditMode/DodgerAndUITests.cs
```

### 共享依赖

```text
Assets/Scripts/Game/GameSliceContracts.cs
Assets/Scripts/Game/CoreLoop/
```

## 实现记录

- 新增 `DodgerController` 和 `DodgerPose`。
- 新增 `DebugHudPresenter` 与 `DebugHudState`，为后续 Unity UI 接入提供纯逻辑输出。
