---
description: "将需求访谈和方案整理为正式 PRD"
agent: "planner"
argument-hint: "功能想法、设计方案或需求描述"
---

请使用 `write-a-prd` skill，把输入整理成正式 PRD。

执行要求：

1. 先确认问题陈述、目标用户、核心收益和不可做范围。
2. 必要时探索代码库，验证当前能力和约束。
3. 追问关键设计分支，直到验收标准、边界和依赖关系清晰。
4. 用领域建模方式识别主要模块、职责和接口边界。
5. 输出 PRD，包含：
   - Problem Statement
   - Solution
   - User Stories
   - Implementation Decisions
   - Testing Decisions
   - Out of Scope
   - Further Notes
6. PRD 完成后，提示下一步使用 `/prd-to-issues` 拆分垂直切片任务。

如果当前信息不足，不要编造需求；一次只问最关键的 1-3 个问题。
