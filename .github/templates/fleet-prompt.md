# /fleet Prompt: 迭代 [N] 实现批次

## 概览

[用一句话描述本批次目标。只包含实现任务，不包含验收任务。]

## 模型分配

| 角色 | 模型系列 | 说明 |
|------|---------|------|
| Developer | [如 Claude / GPT / Gemini] | 实现代码 |
| Reviewer | [必须与 Developer 不同系列] | 后续独立验收，不在本批次执行 |

> Developer 和 Reviewer 禁止使用同一模型系列。本 `/fleet` 批次只运行 Developer track；Reviewer 在实现完成后通过 `/review` 或 `/auto-iterate` 下一轮单独启动。

## Track 列表

### Track 1: [功能名称]

**Agent**: @developer
**Model**: [模型系列，例如 Claude]

**上下文**:
[完整的功能背景、需求来源、合同路径和相关约束。不要依赖外部对话历史。]

**任务**:
1. [步骤 1]
2. [步骤 2]
3. [步骤 3]

**TDD 流程**:
- 先写测试，运行 `debug_check_compilation` 和 `test_run`，确认红灯。
- 编写实现，运行 `debug_check_compilation` 和 `test_run`，确认绿灯。
- 运行 `console_get_logs`，确认没有新增错误。

**文件分区**:
- 独占: `[文件列表]`
- 禁止: `[文件列表]`

**验收标准**:
1. [标准 1]
2. [标准 2]

**依赖**: 无

---

### Track 2: [功能名称]

**Agent**: @developer
**Model**: [模型系列，例如 Claude]

**上下文**:
[完整的功能背景、需求来源、合同路径和相关约束。]

**任务**:
1. [步骤 1]
2. [步骤 2]

**TDD 流程**:
- 先写测试，运行 `debug_check_compilation` 和 `test_run`，确认红灯。
- 编写实现，运行 `debug_check_compilation` 和 `test_run`，确认绿灯。
- 运行 `console_get_logs`，确认没有新增错误。

**文件分区**:
- 独占: `[文件列表]`
- 禁止: `[文件列表]`

**验收标准**:
1. [标准 1]

**依赖**: Track 1 的 `[具体文件或接口]`

---

## 后续验收

本批次完成后，不要在同一个 `/fleet` 中执行验收。下一步应单独运行：

```text
/review [任务名称或合同路径]
```

或继续运行：

```text
/auto-iterate [迭代号和目标]
```

## 注意事项

- 每个 track 的 prompt 必须自包含，因为子 agent 看不到协调者的对话历史。
- 实际执行时，将每个 track 的内容作为独立 prompt。
- 每批控制在 1-5 个实现 track。
- 如果共享接口、基础合同或公共数据结构尚未完成，只输出当前第一个串行 track。
- 如果多个任务并行，必须先确认文件分区无冲突。
