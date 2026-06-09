---
description: "启动独立验收审查"
agent: "reviewer"
argument-hint: "任务名称或合同路径"
---

请对指定任务执行完整验收：

1. 读取任务合同中的完成定义和验收标准
2. 通过 Unity Skills 执行验收链路：
   - `debug_check_compilation` + `debug_get_errors` 编译检查
   - `test_run` + `test_get_result` 测试运行
   - `console_get_logs` 日志检查
   - `validate_scene` + `validate_find_missing_scripts` 场景验证
3. 代码审查（规范、注释、冗余、文件分区）
4. **设计质量审查**（参考 `.github/skills/software-patterns/`）：
   - SOLID 原则违规检查 → `clean-code/solid/`
   - 代码异味识别 → `clean-code/practices/code-smells.md`
   - 设计模式使用正确性 → `gof-patterns/pattern-selection.md`
   - 数据结构合理性 → `clrs-algorithms/data-structure-selection.md`
5. 对照验收标准逐项评分
6. 生成验收报告（使用 `.github/templates/review-report.md` 模板）
7. 输出到 `docs/harness/reports/`

**你没有编辑权限，只能读取、搜索和执行。**
**你必须使用与开发者不同系列的 AI 模型进行审查。**
