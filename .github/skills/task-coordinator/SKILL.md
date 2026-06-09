---
name: task-coordinator
description: "任务协调与 /fleet 编排。Use when generating task contracts, planning /fleet parallel execution, tracking iteration progress, handling review results."
---

# 任务协调

管理任务生命周期、编排 /fleet 并行执行、处理验收结果。

## 使用时机

- 需要从需求文档生成任务合同
- 需要生成 /fleet prompt 进行并行实现
- 需要处理验收结果（通过/回滚）
- 需要更新迭代进度

## 流程

### 1. 创建任务快照

```python
import unity_skills
unity_skills.call_skill("workflow_task_start", taskName="iteration-N", description="迭代N")
```

### 2. 生成任务合同

读取 `docs/harness/specs/` 中的需求文档，使用 `.github/templates/task-contract.md` 模板生成合同：
- 每个任务的验收标准
- 独占文件和禁止文件列表
- 依赖关系

### 3. 验证文件分区

检查所有并行 track 的文件分区无冲突。参考 `file-partition.instructions.md`。

### 4. 生成 /fleet Prompt

参考 [/fleet 最佳实践](./references/fleet-best-practices.md) 生成 prompt。
使用 `.github/templates/fleet-prompt.md` 模板。

### 5. 更新进度

使用 `.github/templates/progress.md` 模板更新 `docs/harness/progress.md`。

### 6. 处理验收结果

- **通过**：
  ```python
  unity_skills.call_skill("workflow_task_end", taskName="iteration-N")
  ```
- **不通过**：
  ```python
  unity_skills.call_skill("workflow_undo_task", taskName="iteration-N")
  ```
  根据验收报告调整合同，重新分发。
