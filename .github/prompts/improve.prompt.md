---
description: "触发流程改进分析"
agent: "improver"
argument-hint: "可选：重点分析的错误类型"
---

请分析错误日志并执行流程改进：

1. 读取 `docs/harness/error-log.md`，按错误类型聚类统计
2. 识别累计 ≥ 3 次的高频错误模式
3. 对每个高频错误执行 5-Why 根因分析
4. 生成改进方案（目标文件、具体修改、验证方式）
5. 应用改进到 agent 指令 / instructions / 检查清单 / 模板
6. 将改进结果追加到 `docs/harness/improvements.md`

**你只修改框架文件，不修改业务代码。每次只修复一类错误。**
