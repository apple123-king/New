---
name: task-reviewer
description: "任务验收审查。Use when reviewing completed tasks, running acceptance tests, checking compilation, validating scenes, generating review reports."
---

# 任务验收

对照任务合同，通过 Unity Skills 执行完整验收链路。

## 使用时机

- 一个任务或迭代实现完成，需要验收
- 需要运行编译检查、测试、场景验证
- 需要生成验收报告

## 验收链路

按顺序执行以下步骤，每步记录结果：

### Step 1: 编译检查

```python
import unity_skills
compilation = unity_skills.call_skill("debug_check_compilation")
errors = unity_skills.call_skill("debug_get_errors")
# 必须 0 error
```

### Step 2: 测试运行

```python
# EditMode 测试
job = unity_skills.call_skill("test_run", testMode="EditMode")
edit_result = unity_skills.call_skill("test_get_result", jobId=job["jobId"])

# PlayMode 测试（如有）
job = unity_skills.call_skill("test_run", testMode="PlayMode")
play_result = unity_skills.call_skill("test_get_result", jobId=job["jobId"])
```

### Step 3: 日志检查

```python
logs = unity_skills.call_skill("console_get_logs")
# 关注 Error 和 Warning 级别条目
```

### Step 4: 场景验证

```python
unity_skills.call_skill("validate_scene")
unity_skills.call_skill("validate_find_missing_scripts")
```

### Step 5: 代码审查

对照 [验收检查清单](./references/review-checklist.md) 逐项检查。

### Step 6: 打分与报告

- 对照合同验收标准逐项评 PASS/FAIL
- 使用 `.github/templates/review-report.md` 模板生成报告
- 输出到 `docs/harness/reports/`

## 核心约束

- **绝对不能编辑文件**
- 不能使用 `script` 模块
- 结论基于客观证据（编译结果、测试结果、日志）
- 不通过时给出具体修改建议 + Unity Skills 返回的错误信息
