# 任务合同: PlayMode Validation - 场景级自动化验证

## 基本信息

- **迭代**: 迭代 2
- **关联需求**: `docs/harness/specs/iteration-2-requirements.md`
- **状态**: 待验收

## 完成定义

### 验收标准

1. [x] 至少新增一条 PlayMode 自动化验证，覆盖场景桥接核心行为。
2. [x] PlayMode 验证内容至少包含以下之一：运行时自举、双相机创建、角色视图存在、基础输入链路、模式切换。
3. [ ] PlayMode 测试能作为 reviewer 的正式证据之一，而不再只依赖 Unity Skills 临时取证。
4. [x] 新测试不破坏既有 EditMode 测试基线。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 新增 `Assets/Tests/PlayMode/`
- [x] 补齐 PlayMode `asmdef` 或等价测试入口
- [ ] 至少一条 PlayMode 测试可稳定执行并给出通过/失败结果

## 文件分区

### 独占文件

```text
Assets/Tests/PlayMode/
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
Assets/Scripts/Game/
Assets/Scenes/SampleScene.unity
```

## 依赖

- **前置任务**: `iteration-2-shared-boundaries.md`
- **被依赖**: `iteration-2-scene-integration.md`, `docs/harness/reports/iteration-2-review.md`

## 实现记录

- 本 Track 重点是验证链路增强，不要求本身改写玩法规则。
- 如果测试必须依赖运行时自举行为，应优先通过现有场景入口和正式视图边界取证。
- 已新增 `SceneBootstrapPlayModeTests.RuntimeBootstrap_CreatesSplitSceneObjectsInSecureMode`，并确认 Unity Test Runner 能发现该测试。
- 当前阻塞点是 Unity Skills 在 PlayMode 运行后丢失 job 句柄，导致正式通过/失败结果未能稳定回收。
