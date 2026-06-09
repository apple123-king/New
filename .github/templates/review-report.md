# 验收报告: [任务名称]

## 基本信息

- **任务合同**: [合同路径]
- **审查时间**: [日期时间]
- **总体结果**: **PASS** / **FAIL**

## 编译检查

- **结果**: PASS / FAIL
- **Error 数**: [N]
- **Warning 数**: [N]
- **详情**: [Unity Skills debug_check_compilation 返回]

## 测试运行

### EditMode

- **结果**: PASS / FAIL
- **通过**: [N] / **失败**: [N] / **跳过**: [N]
- **失败测试**:
  - `[TestClass.TestMethod]`: [失败原因]

### PlayMode

- **结果**: PASS / FAIL / N/A
- **通过**: [N] / **失败**: [N] / **跳过**: [N]

## 日志检查

- **结果**: PASS / FAIL
- **Error 数**: [N]
- **Warning 数**: [N]
- **关键日志**:
  ```
  [相关日志条目]
  ```

## 场景验证

- **validate_scene**: PASS / FAIL / N/A
- **validate_find_missing_scripts**: PASS / FAIL / N/A
- **详情**: [Unity Skills 返回结果]

## 代码审查

| 检查项 | 结果 | 备注 |
|--------|------|------|
| 编码规范 | PASS/FAIL | |
| 中文注释 | PASS/FAIL | |
| 无冗余代码 | PASS/FAIL | |
| 文件分区 | PASS/FAIL | |

## 设计质量审查（software-patterns）

| 检查项 | 结果 | 备注 |
|--------|------|------|
| SOLID 原则 | PASS/FAIL | [违反了哪条，具体位置] |
| 代码异味 | PASS/FAIL | [识别到的异味类型] |
| 模式使用 | PASS/FAIL | [使用的模式是否正确/合理] |
| 数据结构 | PASS/FAIL | [集合类型是否匹配访问模式] |
| 命名与函数 | PASS/FAIL | [命名意图性、函数单一职责] |

## 审查模型信息

- **Developer 模型**: [模型系列，如 Claude]
- **Reviewer 模型**: [模型系列，如 GPT — 必须与 Developer 不同]

## 验收标准逐项评分

| 标准 | 结果 | 证据 |
|------|------|------|
| [标准1] | PASS/FAIL | [证据] |
| [标准2] | PASS/FAIL | [证据] |

## 修改建议（FAIL 时）

1. **[文件路径:行号]**: [问题描述] → [修改建议]
2. **[文件路径:行号]**: [问题描述] → [修改建议]
