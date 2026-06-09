# 错误日志

各角色在工作中发现的错误和问题记录。当同一类错误累计 `>= 3` 次时，由 `@improver` 分析根因并生成改进方案。

## 记录格式

```text
### [日期时间] [角色] [迭代/任务]

- **错误类型**: 编译 / 测试 / 规范 / 架构 / 文件分区 / 其他
- **描述**: 一句话描述错误
- **上下文**: 发生在哪个文件或步骤
- **影响**: 导致了什么后果
- **当时的修复**: 临时怎么修的
```

## 错误类型统计

| 错误类型 | 累计次数 | 最后触发 Improver |
|----------|----------|-------------------|
| 编译 | 0 | - |
| 测试 | 2 | - |
| 规范 | 0 | - |
| 架构 | 1 | - |
| 文件分区 | 1 | - |
| 文档编码 | 1 | - |
| 其他 | 1 | - |

---

### 2026-06-09 20:38 [@planner] [迭代 1/规划补充]

- **错误类型**: 文件分区
- **描述**: Unity 生成目录进入 git 状态列表，且根目录缺少 `.gitignore`。
- **上下文**: 项目侦察阶段检查 git 状态时发现 `Library/`、`Logs/`、`Temp/` 等目录产生大量变更。
- **影响**: 后续提交、diff、审查和任务分区容易被生成文件干扰。
- **当时的修复**: 已补齐根级 `.gitignore`；提交前仍需确认生成目录未进入索引。

### 2026-06-09 20:38 [@planner] [迭代 1/规划补充]

- **错误类型**: 架构
- **描述**: 实现目录与测试目录尚为空，CoreLoop 共享接口未建立。
- **上下文**: `Assets/Scripts/` 与 `Assets/Tests/` 当时没有玩法实现文件。
- **影响**: Shooter、WallDestruction、DodgerAndUI 无法稳定依赖共享合同并行开发。
- **当时的修复**: 执行计划要求 CoreLoop 串行先行创建 `GameSliceContracts.cs`。

### 2026-06-09 20:38 [@planner] [迭代 1/规划补充]

- **错误类型**: 其他
- **描述**: `docs/harness/reports/` 被进度文档引用但缺少目录说明。
- **上下文**: 统一验收计划要求输出报告到 `docs/harness/reports/`。
- **影响**: Reviewer 和 Doc Cleaner 缺少报告落点与格式约束。
- **当时的修复**: 已创建 `docs/harness/reports/README.md`。

### 2026-06-09 21:35 [@developer] [迭代 1/CoreLoop]

- **错误类型**: 测试
- **描述**: Unity Skills 曾处于非 Bypass 判定，`test_run` 被拒绝启动。
- **上下文**: CoreLoop 编译通过后，显式测试运行返回 only allowed in Bypass mode。
- **影响**: CoreLoop 不能立即标记通过。
- **当时的修复**: 确认 Unity Skills 已回到 Bypass 后重跑测试，最终 EditMode 通过。

### 2026-06-09 22:10 [@developer] [迭代 1/Shooter-Wall-Dodger]

- **错误类型**: 测试
- **描述**: Unity 脚本刷新后首次 `test_run` 短暂返回模式判定错误。
- **上下文**: 编译通过后启动 EditMode 测试时返回 only allowed in Bypass mode；随后检查状态显示 `currentMode=bypass`。
- **影响**: 测试启动延迟一次。
- **当时的修复**: 重试后测试正常启动并完成 `152/152` 通过。

### 2026-06-09 22:20 [@doc-cleaner] [迭代 1/统一验收准备]

- **错误类型**: 文档编码
- **描述**: 多个 harness 文档出现中文乱码，不利于 agents 闭环读取。
- **上下文**: `README.md`、`iteration-1-requirements.md`、`iteration-1-architecture-plan.md`、`iteration-1-execution-plan.md`、`reports/README.md`、`error-log.md`。
- **影响**: Planner/Reviewer/Developer 读取合同和验收标准时可能误判。
- **当时的修复**: 重写核心 harness 文档为可读中文，并同步当前验证状态。
