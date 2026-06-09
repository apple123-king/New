---
description: "并行文件分区规则。Use when planning parallel tasks, generating /fleet prompts, resolving file conflicts between concurrent tracks."
---

# 文件分区规则

## 背景

Copilot CLI `/fleet` 命令的子 agent 共享文件系统，但**没有文件锁**。最后写入者会静默覆盖其他 agent 的修改。安全完全依赖 prompt 中的文件分区声明。

## 分区协议

### 独占文件

每个并行 track 必须声明独占修改的文件列表。规则：

- 同一文件**不能**出现在两个 track 的独占列表中
- track 只能修改自己独占列表中的文件
- 新创建的文件自动归入创建者的独占列表

### 禁止修改

每个 track 必须声明禁止修改的文件列表：

- 其他 track 的独占文件
- 公共配置文件（除非该 track 被指定为唯一修改者）
- 自动生成的文件（Luban 生成代码等）

### 共享文件

某些文件需要多个 track 读取但不能并行修改：

- **接口/抽象类文件**：必须由一个 track 先创建完成，其他 track depends on 它
- **全局注册文件**（如 DI 容器注册）：安排为最后一个串行 track 处理
- **配置表文件**：串行处理

## /fleet Prompt 模板

```
Track 1: [功能名]
@developer
独占: Assets/Scripts/Game/FeatureA/, Assets/Tests/EditMode/FeatureATests.cs
禁止: Assets/Scripts/Game/FeatureB/, Assets/Scripts/Game/Shared/
任务: [具体任务描述]
依赖: 无

Track 2: [功能名]
@developer
独占: Assets/Scripts/Game/FeatureB/, Assets/Tests/EditMode/FeatureBTests.cs
禁止: Assets/Scripts/Game/FeatureA/
任务: [具体任务描述]
依赖: Track 1 的 ISharedInterface.cs
```

## 常见冲突场景

| 场景 | 解决方案 |
|------|---------|
| 两个功能依赖同一接口 | 接口文件由单独 track 先完成 |
| 多个功能注册到同一容器 | 注册步骤安排为串行最后 track |
| 共享 ScriptableObject | 只由一个 track 修改，其他只读 |
| .asmdef 修改 | 串行处理，不并行 |
