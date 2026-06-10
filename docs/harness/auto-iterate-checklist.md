# Auto Iterate Checklist

本清单定义 `/auto-iterate` 启动前和每轮循环结束时应检查的文档齐全性与阶段一致性。

## 必备运行文档

| 文档 | 状态要求 | 用途 |
|------|----------|------|
| `docs/harness/progress.md` | 必须存在 | 正式阶段、阶段依据、代码现状、偏差、下一步 |
| `docs/harness/error-log.md` | 必须存在 | 失败、返工、异常触发记录 |
| `docs/harness/improvements.md` | 必须存在 | `@improver` 输出历史 |
| `docs/harness/specs/` | 必须存在且包含当前迭代所需文档 | 需求、架构、执行计划 |
| `docs/harness/contracts/` | 必须存在且包含当前迭代任务合同 | 文件分区、验收标准、依赖 |
| `docs/harness/reports/` | 必须存在 | reviewer 与 doc-cleaner 报告输出 |

## 当前迭代所需文档

| 文档 | 何时需要 |
|------|----------|
| `specs/iteration-N-requirements.md` | 进入正式规划前必须有 |
| `specs/iteration-N-architecture-plan.md` | 进入实现前必须有 |
| `specs/iteration-N-execution-plan.md` | 生成 `/fleet` 前必须有 |
| `contracts/iteration-N-*.md` | 实现前必须有 |
| `contracts/iteration-N-fleet-prompt.md` | 生成实现批次前必须有 |
| `reports/iteration-N-review.md` | 独立验收通过后必须有 |

## 阶段一致性检查

每轮都检查以下问题：

1. `progress.md` 是否明确写出正式阶段。
2. `progress.md` 是否写出阶段依据。
3. `progress.md` 是否单独记录当前代码现状。
4. `progress.md` 是否明确说明正式状态与代码现状之间的偏差。
5. `reports/` 中的正式报告是否与 `progress.md` 的阶段结论一致。
6. `/fleet` 实现批次是否不包含 reviewer track。
7. Developer 与 Reviewer 是否使用不同模型系列。

## 每轮结束检查

1. `progress.md` 已写入当前阶段、阶段依据、偏差和下一步。
2. 失败或返工问题已追加到 `error-log.md`。
3. 新的流程改进已追加到 `improvements.md`。
4. reviewer 或 doc-cleaner 产物已写入 `reports/`。
5. 如果发现“代码已存在但文档未登记”，本轮是否优先做了文档对齐。
6. 如果 reviewer 已通过但进度未同步，本轮是否优先更新了 `progress.md`。

## 使用说明

- 本清单是通用规则，不预设某一迭代一定处于哪一阶段。
- `reports/` 目录必须存在，但在独立验收前允许没有具体验收报告。
- 代码仓库中的实现只能作为参考信号，不能替代正式阶段记录。
