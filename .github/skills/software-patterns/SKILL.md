---
name: software-patterns
description: "软件工程模式知识库路由。Use when making architecture decisions, choosing design patterns, selecting data structures, reviewing code quality, applying SOLID principles, modeling domains. Routes to 7 sub-skills: gof-patterns, clrs-algorithms, clean-code, ddia, pragmatic-programmer, ddd, sicp."
---

# Software Patterns — 软件工程模式知识库

147 篇文档，涵盖 7 个经典软件工程知识领域。本 skill 作为统一路由器，根据查询类型自动分发到适当的子 skill。

## 自动激活条件

- 架构决策、模块划分、通信方式选择
- 设计模式选型（创建、结构、行为）
- 数据结构 / 算法选择、复杂度分析
- 代码质量审查、SOLID 原则、重构
- 领域建模（实体、值对象、聚合）
- 分布式系统设计（复制、分区、一致性）

## 子 Skill 速查

| Skill | 文件数 | 覆盖范围 | 路径 |
|-------|--------|---------|------|
| **gof-patterns** | 25 | 23 GoF 设计模式 + 选型指南 | `references/gof-patterns/` |
| **clrs-algorithms** | 40 | 数据结构 & 算法 | `references/clrs-algorithms/` |
| **clean-code** | 14 | SOLID 原则 + 8 项实践 | `references/clean-code/` |
| **ddia** | 21 | 分布式系统 | `references/ddia/` |
| **pragmatic-programmer** | 19 | 软件匠艺 | `references/pragmatic-programmer/` |
| **ddd** | 15 | 领域驱动设计 | `references/ddd/` |
| **sicp** | 13 | 计算机科学基础 | `references/sicp/` |

**总计：147 篇文档**

## 路由规则

| 查询类型 | 主 Skill | 辅助 Skill |
|----------|---------|-----------|
| 设计模式选择 | `gof-patterns` | `clean-code` (SOLID), `ddd` (战术模式) |
| 数据结构选择 | `clrs-algorithms` | `ddia` (存储引擎) |
| 代码质量审查 | `clean-code` | `pragmatic-programmer` |
| 分布式系统 | `ddia` | `clrs-algorithms` (图), `ddd` (上下文) |
| 领域建模 | `ddd` | `gof-patterns` (战术模式) |
| 基础概念 | `sicp` | `pragmatic-programmer` |
| 架构综合 | **全部** | 跨 skill 编排 |

## 与 Unity 游戏开发的结合

### 架构决策时

参考 `architecture.instructions.md` + 本知识库：

| 场景 | 推荐查阅 |
|------|---------|
| 薄 MonoBehaviour 分离 | `clean-code/solid/single-responsibility.md` + `clean-code/solid/dependency-inversion.md` |
| 事件系统选型 | `gof-patterns/gof-behavioral/observer.md` + `gof-patterns/gof-behavioral/mediator.md` |
| 状态机设计 | `gof-patterns/gof-behavioral/state.md` + `gof-patterns/gof-behavioral/strategy.md` |
| 对象池 / 工厂 | `gof-patterns/gof-creational/factory-method.md` + `gof-patterns/gof-creational/prototype.md` |
| 命令 / 撤销系统 | `gof-patterns/gof-behavioral/command.md` + `gof-patterns/gof-behavioral/memento.md` |
| 配置数据管理 | `ddd/tactical/value-objects.md` + `gof-patterns/gof-structural/flyweight.md` |
| 模块化 asmdef 划分 | `ddd/strategic/bounded-contexts.md` + `clean-code/solid/interface-segregation.md` |

### 代码审查时

Reviewer 对照以下子 skill 进行审查：

1. **命名与函数** → `clean-code/practices/meaningful-names.md`, `clean-code/practices/functions.md`
2. **SOLID 违规** → `clean-code/solid/` (5 项原则)
3. **代码异味** → `clean-code/practices/code-smells.md`
4. **模式使用正确性** → `gof-patterns/pattern-selection.md`
5. **性能数据结构** → `clrs-algorithms/data-structure-selection.md`

### TDD 与测试时

- 测试策略 → `clean-code/practices/unit-testing.md`
- 可测试性设计 → `clean-code/solid/dependency-inversion.md`
- 契约式设计 → `pragmatic-programmer/practices/design-by-contract.md`

## 查询示例

### 模式选型
```
问题: "我需要在不知道具体类型的情况下创建对象"
→ 路由到: gof-patterns/gof-creational/factory-method.md
→ 参考: gof-patterns/pattern-selection.md
```

### 数据结构选型
```
问题: "需要快速查找 + 保持排序"
→ 路由到: clrs-algorithms/data-structures/trees/red-black-tree.md
→ 参考: clrs-algorithms/data-structure-selection.md
```

### 架构综合
```
问题: "设计一个支持撤销的操作系统"
→ 路由到:
  1. gof-patterns: Command (操作封装) + Memento (状态快照)
  2. clrs-algorithms: Stack (撤销栈)
  3. clean-code: SRP (分离关注点) + DIP (抽象依赖)
```

## 详细文档

每个子 skill 的 SKILL.md 位于 `references/<skill-name>/SKILL.md`，包含完整的速查表、决策指南和使用说明。所有模式文档使用语言无关伪代码，可直接翻译为 C#/TypeScript/Python 等。
