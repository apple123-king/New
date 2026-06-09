---
description: "生成可执行的 /fleet 并行实现命令"
agent: "coordinator"
argument-hint: "迭代号，例如：迭代 1"
---

请为指定迭代生成可直接执行的 `/fleet` 实现命令。

## 输入来源

1. 读取 `docs/harness/contracts/` 中该迭代的任务合同。
2. 读取 `docs/harness/specs/` 中相关需求、架构计划和执行计划。
3. 读取 `docs/harness/progress.md`，只选择当前可实现的任务。

## 生成规则

1. 每个实现 track 指定 `@developer` agent。
2. 不要在本 `/fleet` 批次中添加 `@reviewer` track。
3. reviewer 必须在实现批次完成后，通过 `/review` 或 `/auto-iterate` 的下一轮单独启动。
4. 将每个任务转换为自包含 track prompt。
5. 每个 track 必须包含完整上下文、文件分区、TDD 流程、验收标准和测试要求。
6. 确保 track 之间没有文件冲突。
7. 共享接口、基础合同、公共数据结构必须先串行完成；不要和依赖它们的实现任务放在同一批次。
8. Developer 与 Reviewer 必须使用不同模型系列；本批次只声明 Developer 模型，并说明后续 Reviewer 应使用不同模型系列。
9. 每个 track 必须明确标注 `**Model**` 字段。
10. 输出可直接粘贴执行的 `/fleet` prompt。

## 批次限制

- 每批控制在 1-5 个实现 track。
- 如果依赖关系要求串行，只输出当前第一个可执行 track。
- 如果多个任务可并行，仍必须严格遵守文件分区。

参考 `.github/skills/task-coordinator/references/fleet-best-practices.md`。
