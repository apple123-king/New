---
description: "将设计文档展开为结构化需求"
agent: "planner"
argument-hint: "设计文档路径或功能描述"
---

请将以下设计文档/描述展开为结构化需求：

1. 分析所有系统和模块
2. 拆分为可独立验证的功能特性
3. 按依赖关系排序到迭代
4. 为每个功能生成需求文档（使用 `.github/templates/requirement.md` 模板）
5. 分析文件分区和并行性
6. 输出到 `docs/harness/specs/`

如果是新项目，先使用 brainstorming skill 探索设计方向。
如果是已有项目，先使用 codebase-scout skill 分析现有结构。
