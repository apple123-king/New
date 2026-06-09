---
name: expand-requirements
description: "将设计文档或灵感展开为结构化需求。Use when expanding design documents, decomposing features into requirements, planning iterations, analyzing dependencies."
---

# 需求展开

将设计文档或灵感拆分为可独立验证的功能需求，按迭代排序。

## 使用时机

- 有设计文档需要拆分为开发任务
- 有几句话的灵感需要展开为完整设计
- 需要分析功能间的依赖关系和并行性

## 流程

### 1. 确定起点

- **新项目**（从灵感开始）→ 先使用 brainstorming skill 探索设计
- **已有项目**（添加功能）→ 先使用 codebase-scout skill 分析现有结构

### 2. 分析设计文档

- 读取设计文档，识别所有系统和模块
- 使用 Unity Skills 分析已有场景：
  ```python
  import unity_skills
  unity_skills.call_skill("scene_summarize")
  unity_skills.call_skill("asset_find", searchPattern="*.cs", folder="Assets/Scripts")
  ```

### 3. 功能拆分

按 [分解模式参考](./references/decomposition-patterns.md) 拆分：
- 每个功能必须可独立验证
- 每个功能有明确的输入/输出/验收标准
- 粒度：一个功能 = 一个 @developer 能在一个 session 内完成

### 4. 迭代排序

- 按依赖关系排序到迭代
- 同一迭代内的功能应该可以并行
- 标注文件分区（哪些文件归哪个功能独占）

### 5. 输出

为每个功能生成需求文档，使用 `.github/templates/requirement.md` 模板。
输出到 `docs/harness/specs/` 目录。

### 6. 迭代总览

生成迭代总览：
- 每个迭代包含哪些功能
- 并行性分析
- 预估文件分区
- /fleet prompt 草稿
