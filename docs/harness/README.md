# Harness 文档导航

本目录承载《双人彩弹躲藏对抗》的规划、执行、验收和问题记录。当前文档体系面向 `@planner`、`@developer`、`@reviewer`、`@improver`、`@doc-cleaner` 等角色协作使用。

## 当前状态

- 当前迭代：迭代 2，信息保密验证与联网前置架构
- 阶段口径：文档优先。正式阶段以 `progress.md` 与 `reports/` 为准，代码仓库中的实现只记录为“当前代码现状”
- 当前正式状态：迭代 1 已归档；迭代 2 已完成首轮实现，等待独立验收
- 当前代码现状：仓库中已存在迭代 2 的共享视图边界、信息保密模式、命令适配层和 PlayMode 测试脚本
- 当前重点：完成独立验收，并处理 Unity Skills 在 PlayMode 结果回收上的工具阻塞

## 文档地图

| 文档 | 用途 |
|------|------|
| `progress.md` | 正式阶段入口，记录阶段依据、代码现状、偏差与下一步 |
| `auto-iterate-checklist.md` | 自动迭代启动前与每轮结束时的文档齐全性检查 |
| `specs/iteration-1-requirements.md` | 迭代 1 需求拆分 |
| `specs/iteration-1-architecture-plan.md` | 迭代 1 模块边界、通信方式、架构约束 |
| `specs/iteration-1-execution-plan.md` | 迭代 1 实现顺序、并行策略、文件分区与验收链路 |
| `specs/iteration-2-requirements.md` | 迭代 2 需求拆分 |
| `specs/iteration-2-architecture-plan.md` | 迭代 2 架构边界与前置联网约束 |
| `specs/iteration-2-execution-plan.md` | 迭代 2 实现顺序与验证链路 |
| `contracts/iteration-1-*.md` | 迭代 1 每个实现 Track 的任务合同 |
| `reports/` | 验收、文档一致性、试玩观察等正式报告 |
| `error-log.md` | 规划、实现、测试、文件分区等问题记录 |
| `improvements.md` | 高频问题触发后的流程改进记录 |

## 推荐阅读顺序

1. Planner：PRD -> 当前迭代需求拆分 -> 架构计划 -> 执行计划
2. Developer：任务合同 -> 执行计划 -> 需求拆分 -> 编码规范
3. Reviewer：需求拆分 -> 任务合同 -> 执行计划 -> 测试结果 -> 验收报告
4. Coordinator：`progress.md` -> `reports/` -> `contracts/` -> `error-log.md`
5. Improver / Doc-Cleaner：`error-log.md` -> `improvements.md` -> `progress.md`

## 当前缺口

- `progress.md` 需要持续维护正式阶段、代码现状与偏差说明，否则 `auto-iterate` 会误判下一步
- 验收报告、进度状态与代码现状之间必须保持用词一致，尤其是“已实现 / 已验收 / 已完成”
- 迭代 2 当前仍缺独立验收结论；PlayMode 自动化结果回收还存在工具阻塞
- Unity 生成目录是否进入索引仍需在提交前确认

## 使用约定

- `@coordinator` 只根据正式文档状态推进阶段，不直接把代码现状当作阶段完成
- `@reviewer` 负责独立验收和出报告，不负责改写阶段定义
- 当代码领先于文档时，优先进行文档对齐、补录验收或阶段重判
