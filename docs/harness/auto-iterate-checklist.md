# Auto Iterate Checklist

本清单定义 `/auto-iterate` 启动前和每轮循环结束时应检查的文档齐全性。

## 必备运行文档

| 文档 | 状态要求 | 用途 |
|------|----------|------|
| `docs/harness/progress.md` | 必须存在 | 当前迭代、任务状态、下一步 |
| `docs/harness/error-log.md` | 必须存在 | 失败、返工、异常触发记录 |
| `docs/harness/improvements.md` | 必须存在 | `@improver` 输出历史 |
| `docs/harness/specs/` | 必须存在且包含当前迭代需求 | 需求、架构、执行计划 |
| `docs/harness/contracts/` | 必须存在且包含当前迭代任务合同 | 文件分区、验收标准、依赖 |
| `docs/harness/reports/` | 必须存在 | reviewer 和 doc-cleaner 报告输出 |

## 当前迭代建议文档

| 文档 | 何时需要 |
|------|----------|
| `specs/iteration-N-requirements.md` | 规划完成前必须有 |
| `specs/iteration-N-architecture-plan.md` | 开始实现前必须有 |
| `specs/iteration-N-execution-plan.md` | 生成 `/fleet` 前必须有 |
| `contracts/iteration-N-*.md` | 实现前必须有 |
| `contracts/iteration-N-fleet-prompt.md` | 生成实现批次前必须有 |
| `reports/iteration-N-review.md` | 独立验收后生成，不要求预先存在 |

## 每轮结束检查

1. `progress.md` 已写入当前阶段、任务状态和下一步。
2. 失败或返工问题已追加到 `error-log.md`。
3. 新的流程改进已追加到 `improvements.md`。
4. reviewer 或 doc-cleaner 产物已写入 `reports/`。
5. `/fleet` 实现批次没有包含 reviewer track。
6. Developer 和 Reviewer 模型系列不同。

## 当前迭代 1 状态

- 必备运行文档：已齐全。
- 规划文档：已齐全。
- 任务合同：已齐全。
- 验收报告：尚未生成，等待实现完成后由 `@reviewer` 创建。
- 下一步：创建 Unity Skills 快照，然后串行执行 CoreLoop。
