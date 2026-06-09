---
name: codebase-scout
description: "项目探查。Use when analyzing an existing project, discovering tech stack, understanding module structure, assessing test coverage, exploring architecture."
---

# 项目探查

分析已有项目的结构、技术栈、模块边界和测试覆盖情况，为后续需求拆分提供依据。

## 使用时机

- 对已有项目添加新功能前，先了解项目现状
- 需要分析模块边界和依赖关系
- 评估已有测试覆盖情况

## 流程

### 1. 目录结构

扫描项目根目录，识别关键文件夹：
- `Assets/Scripts/` — 代码组织
- `Assets/Tests/` — 测试覆盖
- `Assets/ThirdParty/` — 第三方依赖
- `Packages/manifest.json` — Unity 包依赖
- `ProjectSettings/ProjectVersion.txt` — Unity 版本

### 2. 技术栈识别

- Unity 版本
- 已安装 Unity 包（Addressables、Localization 等）
- 第三方框架（QFramework、TopDownEngine 等）
- 配置表方案（Luban、ScriptableObject 等）
- 测试框架

### 3. 模块分析

通过 Unity Skills 分析：
```python
import unity_skills
unity_skills.call_skill("scene_summarize")
unity_skills.call_skill("asset_find", searchPattern="*.asmdef", folder="Assets")
```

识别模块边界：
- Assembly Definition 文件划分
- 文件夹/命名空间组织
- 核心模块间的依赖方向

### 4. 架构模式识别

扫描代码识别使用的模式：
- MVC / MVP / MVVM
- ECS (Entity Component System)
- 单例模式
- 事件系统
- 命令模式
- 服务定位器

### 5. 测试覆盖评估

- EditMode 测试数量和覆盖范围
- PlayMode 测试数量
- 哪些模块有测试，哪些没有
- 测试质量评估（命名规范、断言充分性）

### 6. 输出

生成项目概览文档，包含：
- 技术栈总结
- 模块列表和简述
- 架构模式
- 测试覆盖概况
- 需要注意的技术债
- 新功能开发建议的切入点
