---
description: "规划一个迭代的任务合同和 /fleet prompt"
agent: "coordinator"
argument-hint: "迭代号（如：迭代 1）"
---

请为指定迭代规划任务：

1. 通过 Unity Skills `workflow_task_start` 创建任务快照
2. 读取 `docs/harness/specs/` 中该迭代的需求文档
3. 为每个任务生成合同（使用 `.github/templates/task-contract.md` 模板）
4. 验证所有并行任务的文件分区无冲突
5. 生成 /fleet prompt（使用 `.github/templates/fleet-prompt.md` 模板）
6. 更新 `docs/harness/progress.md`

合同输出到 `docs/harness/contracts/`。
