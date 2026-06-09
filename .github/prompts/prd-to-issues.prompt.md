---
description: "将 PRD 拆解为可并行的垂直切片任务（GitHub Issues）"
agent: "planner"
argument-hint: "PRD GitHub Issue 编号"
---

请将 PRD 拆解为可独立执行的垂直切片任务，使用 `prd-to-issues` skill：

1. 获取 PRD（Issue 编号或 URL）
2. 探索代码库了解当前状态（如尚未探索）
3. 将 PRD 拆解为垂直切片（Tracer Bullet）：每个 Issue 横切所有层（数据、逻辑、测试）
4. 与用户确认粒度和依赖关系
5. 按依赖顺序创建 GitHub Issues（blockers 先创建）

每个 Issue 包含：父 PRD 引用、需要构建的内容、验收标准、依赖关系、涉及的用户故事。

Issues 创建完成后，使用 `/plan-iteration` 生成任务合同。
