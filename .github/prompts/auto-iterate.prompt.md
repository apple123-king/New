---
description: "自动推进一个迭代，直到通过验收或进入阻塞状态"
agent: "coordinator"
argument-hint: "迭代号和目标，例如：迭代 1，完成本地双人 3D 灰盒垂直切片"
---

请作为总协调者自动推进指定迭代，直到满足停止条件。你的职责不是只给建议，而是根据当前状态选择下一个可执行动作，并把状态写回 `docs/harness/progress.md`。

## 停止条件

只有满足以下任一条件时才停止并询问用户：

1. 所有任务通过 `@reviewer` 验收，迭代完成。
2. 同一任务连续失败 2 轮，且 `@improver` 介入后仍无法推进。
3. 缺少用户必须决策的信息，且无法从需求、合同、进度或报告中合理推断。
4. Unity、Unity Skills、测试工具链或必要依赖不可用，导致无法完成验证。

## 自动循环

每一轮按以下顺序执行：

1. 读取当前状态：
   - `docs/harness/auto-iterate-checklist.md`
   - `docs/harness/progress.md`
   - `docs/harness/specs/`
   - `docs/harness/contracts/`
   - `docs/harness/reports/`
   - `docs/harness/error-log.md`
   - `docs/harness/improvements.md`
2. 按 `docs/harness/auto-iterate-checklist.md` 检查文档齐全性：
   - 缺少必备运行文档时，先创建或补齐。
   - `docs/harness/reports/` 目录必须存在，但验收前允许没有具体报告。
   - 缺少当前迭代需求、架构计划、执行计划或任务合同时，先补规划。
3. 判断迭代所处阶段：
   - 缺少需求拆分或架构计划：交给 `@planner` 补齐规划文档。
   - 缺少任务合同：生成任务合同、文件分区和依赖顺序。
   - 有待实现任务：生成 `/fleet` 实现批次。
   - 有待验收任务：启动 `@reviewer` 独立验收。
   - 验收失败：生成返工任务合同并进入下一轮实现。
   - 验收通过：更新进度并推进下一个任务或完成迭代。
4. 每轮结束必须更新 `docs/harness/progress.md`：
   - 当前任务状态
   - 验收结果
   - 下一步动作
   - 失败次数或维护触发记录

## 实现批次规则

生成 `/fleet` 时遵守以下规则：

1. 只把 `@developer` 放入实现批次。
2. 不要把 `@reviewer` 与 `@developer` 放在同一个 `/fleet` 批次中。
3. 共享接口、基础合同、公共数据结构必须先串行完成。
4. 无文件冲突且依赖已满足的任务可以并行。
5. 每个 track 必须自包含，包含：
   - 任务目标
   - 相关需求和验收标准
   - 独占文件列表
   - 禁止修改文件列表
   - 依赖关系
   - 测试和编译要求
6. 每个 track 必须声明 `**Model**` 字段。
7. Developer 与 Reviewer 必须使用不同模型系列；如果无法满足，停止并提示用户配置第二模型。

## 验收规则

实现批次结束后，单独启动 `@reviewer`：

1. 对照任务合同、需求文档和验收标准逐项检查。
2. 运行必要的编译、测试、日志和场景验证。
3. 输出报告到 `docs/harness/reports/`。
4. 将结论写回 `docs/harness/progress.md`。
5. 发现问题必须追加到 `docs/harness/error-log.md`。

## 返工规则

如果 reviewer 不通过：

1. 从验收报告提取阻塞问题和非阻塞风险。
2. 为阻塞问题生成返工任务合同。
3. 保持文件分区安全，不扩大修改范围。
4. 同一任务失败计数 +1。
5. 如果同一任务连续失败 2 轮，触发 `@improver` 分析根因。
6. `@improver` 后仍无法推进时，标记为阻塞并询问用户。

## 维护触发

满足以下条件时，在常规循环间隙触发维护角色：

1. `docs/harness/error-log.md` 中同类错误累计 >= 3 次：触发 `@improver`。
2. 同一任务连续 FAIL >= 2 次：触发 `@improver`。
3. reviewer 多次报告文档与实现不一致：触发 `@doc-cleaner`。
4. 完成 3 个迭代或重大重构后：触发 `@doc-cleaner`。

维护角色不参与常规实现批次。维护结果必须记录到 `docs/harness/progress.md` 的维护记录中。

## 输出要求

你的输出应包含：

1. 当前判断出的阶段。
2. 本轮要执行的动作。
3. 如果需要用户复制执行 `/fleet`，给出完整可执行 prompt。
4. 如果已停止，明确说明停止条件和需要用户提供的信息。
