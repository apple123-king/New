# 任务合同: Command Boundary - 输入命令与权威适配层

## 基本信息

- **迭代**: 迭代 2
- **关联需求**: `docs/harness/specs/iteration-2-requirements.md`
- **状态**: 待验收

## 完成定义

### 验收标准

1. [x] 场景桥接层中的输入采集与权威规则调用被拆出至少一层清晰适配边界。
2. [x] 新边界能表达攻击方观察、开镜、开火与躲避方移动、姿态切换等核心命令。
3. [x] SceneIntegration 不再把多个玩法控制器的内部调用直接混在同一个巨大更新流程里。
4. [x] 新结构不破坏迭代 1 已验收的规则与运行时行为基线。

### 编译要求

- [x] `debug_check_compilation` 返回 0 error
- [x] Unity Console 无新增 error

### 测试要求

- [x] 新增或更新至少一组 EditMode 测试，覆盖命令边界或适配层的关键行为
- [x] 既有 EditMode 测试无回退
- [x] 需要能从测试中验证命令入口与权威调用的基本映射关系

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/SceneIntegration/
Assets/Tests/EditMode/SceneIntegrationCommandBoundaryTests.cs
```

### 禁止修改

```text
Assets/Scripts/Game/DodgerAndUI/
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
Assets/Scripts/Game/CoreLoop/
Assets/Scripts/Game/Shooter/
```

## 依赖

- **前置任务**: `iteration-2-shared-boundaries.md`
- **被依赖**: `iteration-2-scene-integration.md`

## 实现记录

- 本 Track 是“未来联网改造”的前置清理，不在本轮引入完整 NGO。
- 如果需要新增适配器、命令对象或桥接辅助类，应优先放在 `SceneIntegration/` 下，避免污染规则核心。
- 已新增 `SceneCommandBoundary`、`AttackerCommand` 与 `DodgerCommand`，把输入采集与权威调用分开。
