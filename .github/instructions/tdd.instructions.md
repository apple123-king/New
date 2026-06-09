---
description: "TDD 红绿循环。Use when writing tests, implementing features with test-driven development, running Unity Test Framework."
applyTo: "**/Tests/**"
---

# TDD 红绿循环

## 流程

严格按顺序执行，每步通过 Unity Skills 验证：

1. **Red** — 写一个会失败的测试
   - `debug_check_compilation` 确认编译通过
   - `test_run` + `test_get_result` 确认测试失败

2. **Green** — 写最小实现让测试通过
   - `debug_check_compilation` 确认编译通过
   - `test_run` + `test_get_result` 确认测试通过

3. **Refactor** — 改善代码质量，保持绿灯
   - `debug_check_compilation` + `test_run` + `test_get_result` 确认无回退

## Unity Test Framework

- **EditMode 优先**：纯逻辑测试放 EditMode（不需要场景、不需要 Play）
- **PlayMode 仅场景交互**：需要 MonoBehaviour 生命周期、物理、协程等才用 PlayMode
- 测试文件放 `Assets/Tests/EditMode/` 或 `Assets/Tests/PlayMode/`

## 命名

- 测试文件：`FeatureNameTests.cs`
- 测试方法：`Method_State_ExpectedResult`
- 每个测试方法必须有中文注释说明意图

```csharp
// 测试：当伤害值超过最大生命值时，生命值归零
[Test]
public void TakeDamage_ExceedsMaxHp_HpBecomesZero()
{
    // ...
}
```

## 测试设计

- 一个测试只验证一件事
- 用 Arrange-Act-Assert 结构
- 避免测试间共享可变状态
- Mock 外部依赖（Unity API、文件系统、网络）
