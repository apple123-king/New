# Reports 目录说明

本目录用于存放验收、文档一致性、试玩观察和专项审查报告。

## 计划报告

| 报告 | 触发时机 | 负责人 |
|------|----------|--------|
| `iteration-1-review.md` | 四个实现 Track 完成后 | @reviewer |
| `doc-consistency-*.md` | 文档清理或一致性审查后 | @doc-cleaner |
| `playtest-observation-*.md` | 本地试玩验证后 | Planner 或 Reviewer |

## 报告要求

- 必须写明检查对象、检查时间、结论、阻塞问题和非阻塞风险。
- 需要能回链到需求拆分或任务合同中的具体验收标准。
- 如果发现架构、测试、文件分区或流程问题，需要同步追加到 `../error-log.md`。
