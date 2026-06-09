# 验收检查清单

验收时按以下清单逐项检查，记录 PASS/FAIL 和原因。

## 必须项

- [ ] **编译通过**：`debug_check_compilation` 返回 0 error
- [ ] **所有新增测试通过**：`test_run` + `test_get_result` 全部 PASS
- [ ] **未破坏既有测试**：既有测试套件全部 PASS
- [ ] **代码规范**：符合 `unity-csharp.instructions.md`（命名、注释、格式）
- [ ] **中文注释完整**：所有变量和函数都有中文注释
- [ ] **无冗余代码**：没有未使用的变量、方法、类
- [ ] **文件分区未违反**：未修改禁止列表中的文件
- [ ] **验收标准满足**：任务合同中的每条验收标准都达成

## 设计质量项（参考 software-patterns 知识库）

- [ ] **SOLID 原则**：无 SRP / OCP / LSP / ISP / DIP 违规（参考 `software-patterns/references/clean-code/solid/`）
- [ ] **代码异味**：无 Long Method、Feature Envy、God Class 等异味（参考 `software-patterns/references/clean-code/practices/code-smells.md`）
- [ ] **模式使用正确**：设计模式用法符合 GoF 定义（参考 `software-patterns/references/gof-patterns/pattern-selection.md`）
- [ ] **数据结构合理**：集合类型选择匹配访问模式（参考 `software-patterns/references/clrs-algorithms/data-structure-selection.md`）
- [ ] **命名与函数**：命名意图清晰、函数短小单一职责（参考 `software-patterns/references/clean-code/practices/meaningful-names.md`、`functions.md`）

## 可选项（根据任务类型）

- [ ] **场景完整性**：`validate_scene` 无错误
- [ ] **无缺失脚本**：`validate_find_missing_scripts` 返回空
- [ ] **运行时无报错**：`console_get_logs` 无 Error 级别条目
- [ ] **无新增性能热点**：无明显的 GC 分配、无 Update 中的频繁 new
- [ ] **可测试性**：新增逻辑有对应测试，测试覆盖关键路径

## 评分标准

| 等级 | 标准 |
|------|------|
| **PASS** | 必须项全部通过 + 可选项无重大问题 |
| **PASS (with notes)** | 必须项全部通过 + 可选项有小问题但不阻塞 |
| **FAIL** | 任何必须项未通过 |

## 报告格式

```markdown
## 验收结果: [PASS/FAIL]

### 编译: [PASS/FAIL]
[detail]

### 测试: [PASS/FAIL]
- EditMode: X passed, Y failed
- PlayMode: X passed, Y failed

### 日志: [PASS/FAIL]
[error/warning count]

### 代码审查: [PASS/FAIL]
[findings]

### 修改建议（FAIL 时）
1. [具体文件 + 行号 + 问题 + 建议]
```
