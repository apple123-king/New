# 任务合同: Scene Integration - 场景桥接收口与模式切换

## 基本信息

- **迭代**: 迭代 2
- **关联需求**: `docs/harness/specs/iteration-2-requirements.md`
- **状态**: 待验收

## 完成定义

### 验收标准

1. [x] SceneIntegration 能同时接入共享合同、角色视图边界和命令适配层。
2. [x] 运行时存在可区分的正式信息保密模式与调试全信息模式。
3. [x] 双相机、自举对象、基础 HUD 和关键桥接流程在新结构下保持可用。
4. [x] 该收口不会破坏迭代 1 已验收的核心对局流程。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 既有 EditMode 测试无回退
- [x] 新增或复用至少一种场景级验证：PlayMode 自动化、Unity Skills 场景查询或等价正式记录
- [ ] reviewer 能根据运行时证据确认模式切换与桥接收口成立

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/SceneIntegration/
Assets/Scenes/SampleScene.unity
```

### 禁止修改

```text
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
Assets/Scripts/Game/DodgerAndUI/
Assets/Tests/PlayMode/
```

## 依赖

- **前置任务**: `iteration-2-role-visibility.md`, `iteration-2-command-boundary.md`, `iteration-2-playmode-validation.md`
- **被依赖**: `docs/harness/reports/iteration-2-review.md`

## 实现记录

- 本 Track 是迭代 2 的串行收口阶段。
- 允许消费前面几个 Track 的产物，但不应重新发散为新的大范围规则重写。
- 已通过 Unity Skills 运行态查询确认 `GameSliceSceneBootstrap`、`GameSlice_Greybox`、`AttackCamera`、`DodgerCamera` 在 Play Mode 下存在，且 Console error 为 0。
