# Harness 工作流

本项目使用 AI Harness 进行迭代开发，核心目标是让多个 agent 在一个可恢复的状态机中协作，而不是依赖用户逐个派发任务。

## 总入口

优先使用：

```text
/auto-iterate 迭代 1，完成本地双人 3D 灰盒垂直切片
```

`/auto-iterate` 由 `@coordinator` 执行，循环推进：

```text
读取进度 -> 判断阶段 -> 规划 -> 合同 -> /fleet 实现 -> 独立验收 -> 返工或完成 -> 维护 -> 更新进度
```

只有在迭代完成、连续失败后仍无法推进、缺少用户必须决策的信息、或 Unity/工具链不可用时才停止。

## 完整工作流

### 发现阶段

```text
/brainstorming -> /grill-me -> /write-a-prd -> /prd-to-issues
```

- `/brainstorming`：从早期想法发散并收敛到可验证方向。
- `/grill-me`：压力测试需求，消除设计歧义。
- `/write-a-prd`：整理为正式 PRD。
- `/prd-to-issues`：拆为可并行的垂直切片任务。

### 开发阶段

```text
/plan-iteration -> /fleet-implement -> /review -> /auto-iterate 继续闭环
```

- `/plan-iteration`：生成任务合同、文件分区和实现批次。
- `/fleet-implement`：只生成 Developer 实现批次。
- `/review`：实现完成后单独验收。
- `/auto-iterate`：自动判断下一步，处理返工、维护和完成状态。

## Agent 角色

| Agent | 职责 | 关键约束 |
|------|------|----------|
| `@planner` | 需求拆分、领域建模、架构规划 | 只读分析和文档输出 |
| `@coordinator` | 状态机调度、进度管理、`/fleet` 编排 | 不直接写实现代码 |
| `@developer` | TDD 实现 | 遵守文件分区和任务合同 |
| `@reviewer` | 独立验收 | 不编辑实现代码 |
| `@improver` | 高频错误分析和流程改进 | 异常或维护窗口触发 |
| `@doc-cleaner` | 文档与实现一致性清理 | 维护窗口或文档偏差触发 |

## 运行时文档

所有运行时产物放在 `docs/harness/`：

| 路径 | 用途 |
|------|------|
| `docs/harness/README.md` | 文档导航 |
| `docs/harness/auto-iterate-checklist.md` | 自动迭代齐全性检查 |
| `docs/harness/progress.md` | 当前进度、任务状态、下一步 |
| `docs/harness/specs/` | PRD、需求拆分、架构计划、执行计划 |
| `docs/harness/contracts/` | 任务合同和 `/fleet` 实现批次 |
| `docs/harness/reports/` | 验收、文档一致性和试玩观察报告 |
| `docs/harness/error-log.md` | 错误、返工和异常记录 |
| `docs/harness/improvements.md` | 流程改进历史 |

`/auto-iterate` 每轮必须读取 `progress.md`、`specs/`、`contracts/`、`reports/`、`error-log.md` 和 `improvements.md`。

## 文件分区规则

`/fleet` 子 agent 共享文件系统且没有文件锁。安全依赖任务合同中的文件分区：

- 每个 track 声明独占文件。
- 每个 track 声明禁止修改文件。
- 共享接口和公共数据结构必须先串行完成。
- Reviewer 不放进 Developer 实现批次。
- 实现批次完成后，单独运行 `/review`。

详见 `.github/instructions/file-partition.instructions.md` 和 `.github/skills/task-coordinator/references/fleet-best-practices.md`。

## 模型多样性

Developer 和 Reviewer 必须使用不同系列模型。

允许示例：

- Developer=Claude + Reviewer=GPT
- Developer=GPT + Reviewer=Claude
- Developer=Claude + Reviewer=Gemini
- Developer=Gemini + Reviewer=GPT

如果当前环境只有单一模型，Coordinator 必须停止并提醒用户配置第二模型后再启动验收。

## Unity Skills

本项目通过 Unity Skills REST API 与 Unity Editor 交互。

前置条件：

1. Unity Editor 正在运行。
2. 已安装 Unity Skills 包。
3. Python helper `unity_skills.py` 可用。

常用能力：

```python
unity_skills.call_skill("debug_check_compilation")
unity_skills.call_skill("test_run", testMode="EditMode")
unity_skills.call_skill("console_get_logs")
unity_skills.call_skill("scene_summarize")
unity_skills.call_skill("workflow_task_start", taskName="feature-x", description="...")
```

workflow 相关参数必须以 `.agents/skills/unity-skills/skills/workflow/SKILL.md` 的当前签名为准，不要在 agent 指令中硬编码。

## 维护触发

| 条件 | 动作 |
|------|------|
| 同类错误累计 >= 3 次 | 触发 `@improver` |
| 同一任务连续 FAIL >= 2 次 | 触发 `@improver` |
| Reviewer 多次报告文档不符 | 触发 `@doc-cleaner` |
| 每完成 3 个迭代 | 触发维护窗口 |
| 重大重构完成后 | 触发 `@doc-cleaner` |

维护结果必须记录到 `docs/harness/progress.md` 的维护记录中。

## 快速入口

| Prompt | 用途 |
|--------|------|
| `/brainstorming` | 早期想法发散与收敛 |
| `/grill-me` | 压力测试需求，消除设计歧义 |
| `/write-a-prd` | 整理正式 PRD |
| `/prd-to-issues` | PRD 拆分为垂直切片任务 |
| `/scout` | 项目结构探查 |
| `/plan-iteration` | 迭代规划和任务合同 |
| `/fleet-implement` | 生成 Developer `/fleet` 实现批次 |
| `/review` | 独立验收 |
| `/auto-iterate` | 自动推进闭环直到通过或阻塞 |
| `/tdd` | 查看并遵循 TDD 红绿循环 |
| `/improve` | 流程改进 |
| `/improve-codebase-architecture` | 架构改进分析和 RFC |
| `/doc-clean` | 文档一致性清理 |
