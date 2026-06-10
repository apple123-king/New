# 任务合同: Role Visibility - 信息保密模式与 HUD 权限

## 基本信息

- **迭代**: 迭代 2
- **关联需求**: `docs/harness/specs/iteration-2-requirements.md`
- **状态**: 待验收

## 完成定义

### 验收标准

1. [x] 攻击方、躲避方与调试模式的 HUD / 状态显示边界被明确区分。
2. [x] 正式信息保密模式下，躲避方不再默认看到攻击方剩余子弹等不应直视的数据。
3. [x] 调试全信息模式被显式保留，并能与正式信息保密模式清晰区分。
4. [x] 角色视图层优先消费共享合同中的只读视图，而不是读取控制器内部实现细节。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 更新或新增 `Assets/Tests/EditMode/DodgerAndUITests.cs`
- [x] 既有 EditMode 测试无回退
- [x] 至少覆盖一种“攻击方可见 / 躲避方不可见 / 调试模式可见”的权限差异

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/DodgerAndUI/
Assets/Tests/EditMode/DodgerAndUITests.cs
```

### 禁止修改

```text
Assets/Scripts/Game/Shooter/
Assets/Scripts/Game/WallDestruction/
Assets/ThirdParty/
Assets/Scripts/LubanGenerated/
Library/
Logs/
Temp/
UserSettings/
```

### 共享依赖

```text
Assets/Scripts/Game/GameSliceContracts.cs
Assets/Scripts/Game/SceneIntegration/
```

## 依赖

- **前置任务**: `iteration-2-shared-boundaries.md`
- **被依赖**: `iteration-2-scene-integration.md`

## 实现记录

- 本 Track 只负责“看见什么”，不负责改写比赛规则。
- 如果需要兼容调试全信息模式，应通过显式模式切换而不是临时条件分支散落在多个调用点。
- `DebugHudPresenter` 已按 `Secure / Debug` 与 `Attacker / Dodger` 两个维度输出权限不同的 HUD 视图。
