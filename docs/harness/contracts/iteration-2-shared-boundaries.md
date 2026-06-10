# 任务合同: Shared Boundaries - 共享合同与角色视图边界

## 基本信息

- **迭代**: 迭代 2
- **关联需求**: `docs/harness/specs/iteration-2-requirements.md`
- **状态**: 待验收

## 完成定义

### 验收标准

1. [x] 在 `GameSliceContracts.cs` 中补齐攻击方、躲避方、调试模式可消费的最小只读视图边界。
2. [x] 明确至少一层“输入命令 -> 权威规则 -> 角色视图”的共享合同，供后续 Track 依赖。
3. [x] 新合同不破坏迭代 1 已验收的 CoreLoop、Shooter、WallDestruction、DodgerAndUI 既有编译与测试基线。
4. [x] 下游 Track 不需要通过读取控制器内部细节来判断角色可见信息。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 既有 EditMode 测试无回退
- [x] 至少补充一组共享合同相关 EditMode 测试，或扩展既有测试覆盖新增只读边界
- [x] 共享接口变更后，下游 Track 可以只依赖合同而不是直接修改规则核心

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/GameSliceContracts.cs
Assets/Tests/EditMode/SharedBoundaryTests.cs
```

### 禁止修改

```text
Assets/Scripts/Game/SceneIntegration/
Assets/ThirdParty/
Assets/Scripts/LubanGenerated/
Library/
Logs/
Temp/
UserSettings/
```

### 共享依赖

```text
Assets/Scripts/Game/CoreLoop/
Assets/Scripts/Game/Shooter/
Assets/Scripts/Game/WallDestruction/
Assets/Scripts/Game/DodgerAndUI/
```

## 依赖

- **前置任务**: 无
- **被依赖**: `iteration-2-role-visibility.md`, `iteration-2-command-boundary.md`, `iteration-2-scene-integration.md`

## 实现记录

- 本 Track 是迭代 2 的串行入口。
- 目标是先固定共享接口，再让 Role Visibility、Command Boundary 和 Scene Integration 各自实现。
- 已新增 `GamePresentationMode`、`PlayerViewRole` 和 `RoleHudView` 作为共享边界。
