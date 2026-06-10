# 项目进度

## 总体状态

- 当前迭代：迭代 2
- 总迭代数：2
- 最后更新：2026-06-10
- 阶段口径：文档优先
- 正式阶段权威来源：`docs/harness/progress.md` 与 `docs/harness/reports/`

## 正式阶段定义

| 阶段 | 含义 |
|------|------|
| 已规划 | 正式规划文档齐全，可生成或已生成任务合同 |
| 已实现 | 实现已落地，但尚未形成通过的独立验收记录 |
| 已验收 | 已存在通过的 reviewer 报告，并可回链到需求或合同 |
| 已归档 | 阶段状态已同步完成，无待执行动作 |

## 当前正式状态

### 迭代 1：本地双人 3D 灰盒垂直切片

- 正式阶段：已归档
- 阶段依据：
  - `docs/harness/specs/iteration-1-requirements.md`
  - `docs/harness/specs/iteration-1-architecture-plan.md`
  - `docs/harness/specs/iteration-1-execution-plan.md`
  - `docs/harness/contracts/iteration-1-*.md`
  - `docs/harness/reports/iteration-1-review.md`
- 正式结论：
  - 规划文档已存在
  - 五份实现合同均已形成正式验收结论
  - `iteration-1-review.md` 已覆盖纯逻辑模块与场景桥接
  - 迭代 1 的阶段同步、验收与历史文档收口已完成，可作为后续迭代基线

### 迭代 2：信息保密验证与联网前置架构

- 正式阶段：已实现
- 阶段依据：
  - `docs/harness/specs/iteration-2-requirements.md`
  - `docs/harness/specs/iteration-2-architecture-plan.md`
  - `docs/harness/specs/iteration-2-execution-plan.md`
- 正式结论：
  - 已基于现有 PRD 确立下一轮目标为“真实信息保密、联网前置权威边界、PlayMode 场景验证”
  - 共享边界、角色可见性、命令适配层、场景收口和 PlayMode 测试脚本均已落地
  - 编译通过，EditMode 回归通过 `157/157`
  - PlayMode 测试脚本已被 Unity 发现，但 Unity Skills 在执行后丢失 job 句柄，正式结果回收暂时受阻
  - 迭代 2 已达到“已实现”口径，但尚未形成独立验收通过记录

## 当前代码现状

- 仓库中已存在以下实现：
  - `CoreLoop`、`Shooter`、`WallDestruction`、`DodgerAndUI` 纯逻辑模块
  - `Assets/Scripts/Game/SceneIntegration/GameSliceSceneBootstrap.cs` 场景集成入口
- 当前尚不存在专门属于迭代 2 的新实现：
  - 尚未抽出真实信息保密的角色视图层
  - 已补齐 PlayMode 自动化测试脚本，但正式结果回收仍受工具链限制
  - 尚未建立更接近联网版的命令/权威边界
- 代码现状说明：
  - 代码现状可作为参考信号
  - 代码现状不会自动提升正式阶段
  - 如果后续代码继续领先于文档，必须先补录状态，再推进流程

## 正式状态与代码现状的偏差

| 项目 | 正式状态 | 代码现状 | 需要动作 |
|------|----------|----------|----------|
| 迭代 1 基线 | 已归档 | 仓库中已存在完整本地灰盒垂直切片实现 | 后续作为迭代 2 的复用基线 |
| 迭代 2 规划 | 已实现 | 已有迭代 2 专属代码与测试脚本 | 下一步进入独立验收 |
| PlayMode 自动化 | 已实现 | 已新增测试脚本并被 Unity 发现 | 正式结果回收受 Unity Skills job 句柄问题阻塞 |

## 迭代任务状态

| 任务 | 正式状态 | 验收结果 | 备注 |
|------|----------|----------|------|
| Iteration 1 Archive - 垂直切片归档 | 已归档 | 进度、报告、规格状态已同步 | 作为后续规划基线 |
| CoreLoop - 比赛状态机与回合循环 | 已验收 | 统一验收报告记录通过 | 作为已验收模块记录，不单独推进阶段 |
| Shooter - 攻击方瞄准、开镜与射击 | 已验收 | 统一验收报告记录通过 | 同上 |
| WallDestruction - 砖墙网格、破坏与穿透 | 已验收 | 统一验收报告记录通过 | 同上 |
| DodgerAndUI - 躲避方控制、双视角与最低限 UI | 已验收 | 统一验收报告记录通过 | 同上 |
| Review - 统一验收 | 已验收 | `docs/harness/reports/iteration-1-review.md` | 作为正式阶段依据之一 |
| Scene Integration - 灰盒场景装配、双视角桥接与运行时验证 | 已验收 | 统一验收报告记录通过 | 已完成运行态独立验收 |
| Iteration 2 Planning - 信息保密验证与联网前置架构 | 已实现 | 规格文档、合同和首轮实现均已建立 | 等待独立验收 |
| Iteration 2 Shared Boundaries - 共享合同与角色视图边界 | 已实现 | 编译通过，EditMode 回归通过 | 合同状态待验收 |
| Iteration 2 Role Visibility - 信息保密模式与 HUD 权限 | 已实现 | 编译通过，EditMode 回归通过 | 合同状态待验收 |
| Iteration 2 Command Boundary - 输入命令与权威适配层 | 已实现 | 编译通过，EditMode 回归通过 | 合同状态待验收 |
| Iteration 2 PlayMode Validation - 场景级自动化验证 | 已实现 | PlayMode 测试脚本已新增并被发现 | 正式结果回收暂时阻塞 |
| Iteration 2 Scene Integration - 场景桥接收口与模式切换 | 已实现 | 运行态场景查询通过，Console error 0 | 合同状态待验收 |

## 已知问题

- Unity 生成目录是否进入索引仍需在提交前确认
- 历史文档中仍可能残留少量旧口径，需要在后续清理中继续收口
- Unity Skills 在 PlayMode 测试执行后丢失 job 句柄，导致正式结果未能稳定回收

## 维护记录

| 日期 | 类型 | 角色 | 摘要 | 路径 |
|------|------|------|------|------|
| 2026-06-09 | 规划补充 | @planner | 补齐文档导航、需求拆分、架构规划、执行计划与报告目录说明 | `docs/harness/README.md` |
| 2026-06-09 | 实现 | @developer | 完成 CoreLoop、Shooter、WallDestruction、DodgerAndUI 逻辑与测试 | `docs/harness/contracts/` |
| 2026-06-09 | 统一验收 | @reviewer | 产出迭代 1 统一验收报告 | `docs/harness/reports/iteration-1-review.md` |
| 2026-06-10 | 流程整理 | @coordinator | 统一 auto-iterate、coordinator 与 harness 文档的“文档优先”口径 | `.github/prompts/auto-iterate.prompt.md` |
| 2026-06-10 | 文档对齐 | @coordinator | 补齐 Scene Integration 合同，并把迭代整体正式阶段回调为“已实现，待场景独立验收” | `docs/harness/contracts/iteration-1-scene-integration.md` |
| 2026-06-10 | 独立验收 | @reviewer | 基于 Unity Skills 运行态场景查询、编译与 Console 检查，通过 Scene Integration 并把迭代 1 提升为“已验收” | `docs/harness/reports/iteration-1-review.md` |
| 2026-06-10 | 归档与新迭代规划 | @coordinator | 将迭代 1 收口为“已归档”，并建立迭代 2 规划文档骨架 | `docs/harness/specs/iteration-2-*.md` |
| 2026-06-10 | 任务拆分 | @coordinator | 补齐迭代 2 的任务合同与 `/fleet` prompt | `docs/harness/contracts/iteration-2-*.md` |
| 2026-06-10 | 实现推进 | @developer | 完成共享视图边界、信息保密模式、命令边界、PlayMode 测试脚本和场景收口 | `Assets/Scripts/Game/`, `Assets/Tests/` |

## 下一步

1. 运行独立验收，重点确认迭代 2 的角色可见性边界、命令适配层和场景收口。
2. 处理 Unity Skills 的 PlayMode job 句柄回收问题，或在 reviewer 阶段采用手工补充取证。
3. 在提交前确认 Unity 生成目录未被误纳入索引。
