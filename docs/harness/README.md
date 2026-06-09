# Harness 文档导航

本目录承载本地双人 3D 灰盒垂直切片的规划、执行、验收和问题记录。当前文档体系面向 `planner`、`developer`、`reviewer`、`improver`、`doc-cleaner` 等角色协作使用。

## 当前状态

- **当前迭代**: 迭代 1，本地双人 3D 灰盒垂直切片
- **规划状态**: 玩法规格、PRD、需求拆分、任务合同和执行计划已建立
- **实现状态**: CoreLoop、Shooter、WallDestruction、DodgerAndUI 的纯逻辑层已完成
- **验证状态**: Unity 编译通过；EditMode 测试 `152/152` 通过；Unity Console error 为 `0`
- **下一步**: 执行统一 Review 报告，然后进入 Unity 场景灰盒接入

## 文档地图

| 文档 | 用途 |
|------|------|
| `progress.md` | 项目进度、当前迭代状态、下一步 |
| `auto-iterate-checklist.md` | 自动迭代启动与每轮结束时的文档齐全性检查 |
| `specs/iteration-1-requirements.md` | 迭代 1 可验证需求拆分 |
| `specs/iteration-1-architecture-plan.md` | 模块边界、通信方式、架构约束 |
| `specs/iteration-1-execution-plan.md` | 实现顺序、并行策略、文件分区与验收链路 |
| `contracts/iteration-1-*.md` | 每个实现 Track 的任务合同 |
| `contracts/iteration-1-fleet-prompt.md` | `/fleet` 并行执行提示词 |
| `reports/` | 验收、文档一致性、试玩观察报告 |
| `error-log.md` | 规划、实现、测试、文件分区等问题记录 |
| `improvements.md` | 高频问题触发后的流程改进记录 |

## 推荐阅读顺序

1. Planner: PRD -> 需求拆分 -> 架构规划 -> 执行计划
2. Developer: 对应任务合同 -> 执行计划 -> 需求拆分 -> 编码规范
3. Reviewer: 需求拆分 -> 四份任务合同 -> 执行计划 -> 测试结果 -> 验收报告
4. Improver: error-log -> improvements -> progress

## 当前缺口

- Unity 场景灰盒接入尚未完成，当前完成的是可测试规则核心。
- `Library/`、`Logs/`、`Temp/` 等 Unity 生成目录仍出现在工作区状态中；提交前需要确认这些目录不进入索引。
- 统一 Review 报告尚未生成。
