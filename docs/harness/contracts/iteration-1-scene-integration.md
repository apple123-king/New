# 任务合同: Scene Integration - 灰盒场景装配、双视角桥接与运行时验证

## 基本信息

- **迭代**: 迭代 1
- **关联需求**: `docs/harness/specs/iteration-1-requirements.md`
- **状态**: 通过

## 完成定义

### 验收标准

1. [x] 运行时存在本地双人 3D 灰盒场景桥接，能够把 `CoreLoop`、`Shooter`、`WallDestruction`、`DodgerAndUI` 四个纯逻辑模块接入 Unity 生命周期。
2. [x] 运行时存在攻击方与躲避方双视角，并能显示最小调试 HUD。
3. [x] 躲避方输入、姿态切换、部署锁定、攻击方开镜与开火流程在场景桥接层中已接通。
4. [x] 砖墙运行时可见、可破坏、可随大回合重置。
5. [x] 已通过独立验收确认当前桥接满足“迭代 1 本地双人 3D 灰盒垂直切片”的正式完成口径。

### 编译要求

- [x] 运行时代码已存在于仓库中
- [x] 已在独立验收时重新确认编译、日志与场景运行结果

### 测试与验证要求

- [x] 已由独立 reviewer 对场景桥接进行运行时验收
- [x] 已形成至少一种场景级验证记录:
  - PlayMode / Unity Test Runner 结果，或
  - Unity Skills 场景查询 / 编译 / Console 验证，或
  - 等价的正式运行记录
- [x] 已把结论写入正式报告，避免仅凭“代码已存在”认定完成

## 文件分区

### 独占文件

```text
Assets/Scripts/Game/SceneIntegration/
Assets/Scenes/SampleScene.unity
```

### 共享依赖

```text
Assets/Scripts/Game/GameSliceContracts.cs
Assets/Scripts/Game/CoreLoop/
Assets/Scripts/Game/Shooter/
Assets/Scripts/Game/WallDestruction/
Assets/Scripts/Game/DodgerAndUI/
```

## 实现记录

- 已存在 `GameSliceSceneBootstrap`，通过 `RuntimeInitializeOnLoadMethod` 在场景加载后自动创建桥接对象。
- 已桥接攻击方输入、躲避方输入、双相机、调试 HUD、砖墙显示与回合重置逻辑。
- 2026-06-10 已通过 Unity Skills 的编译、Console 与运行时场景层查询形成独立正式验收记录。
