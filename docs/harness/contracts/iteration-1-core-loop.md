# 任务合同: CoreLoop - 比赛状态机与回合循环

## 基本信息

- **迭代**: 迭代 1
- **关联需求**: `docs/harness/specs/iteration-1-requirements.md`
- **状态**: 通过

## 完成定义

### 验收标准

1. [x] 能管理整局 `9` 个大回合上限、先到 `5` 分获胜、到上限按比分结算的比赛流程。
2. [x] 能管理单个大回合中的先手交替、攻防换手、命中得分、跨大回合墙体重置时机。
3. [x] 能管理单次进攻的 `3` 发子弹消耗和 `8` 秒警告 + `3` 秒强制换手反僵局规则。
4. [x] 公开只读状态接口，足以驱动 Shooter、WallDestruction、DodgerAndUI 三个 Track。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 新增 `Assets/Tests/EditMode/CoreLoopTests.cs`
- [x] EditMode 测试全量通过: `152/152`
- [x] 覆盖默认规则、部署结束、换手、命中得分、胜负结算、反僵局警告和强制换手

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/CoreLoop/
Assets/Tests/EditMode/CoreLoopTests.cs
```

### 共享文件

```text
Assets/Scripts/Game/GameSliceContracts.cs
```

## 实现记录

- 新增 `CoreLoopSettings` 管理迭代 1 默认规则。
- 新增 `CoreLoopController` 作为纯 C# 状态机。
- 新增 `CoreLoopSnapshot` 和 `ICoreLoopReadOnly`，为其他模块提供只读状态。
