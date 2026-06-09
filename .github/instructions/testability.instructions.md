---
description: "可测试性指南。Use when writing testable code, isolating logic from MonoBehaviour, planning test strategy, choosing EditMode vs PlayMode."
---

# 可测试性指南

## 逻辑是否需要 MonoBehaviour？

| 需要 MonoBehaviour | 不需要 MonoBehaviour |
|-------------------|---------------------|
| 依赖 Unity 生命周期 (Update, OnCollision) | 纯计算、数据处理 |
| 需要 Inspector 绑定 | 状态机逻辑 |
| 需要协程 | 数值公式 |
| 需要物理/渲染交互 | 业务规则判断 |

**原则**：如果逻辑不依赖 Unity API，放纯 C# 类。

## 接口注入 vs 静态全局

- 优先使用接口注入（构造函数或方法参数）
- 需要测试的逻辑不应该依赖单例或静态类
- 如果框架提供服务定位器（如 GetSystem），抽象为接口方便 Mock

```csharp
// 好：可测试
public class DamageCalculator
{
    private readonly IDamageConfig config;
    public DamageCalculator(IDamageConfig config) { this.config = config; }
    public int Calculate(int baseDamage, float multiplier) => ...;
}

// 差：不可测试
public class DamageCalculator
{
    public int Calculate(int baseDamage, float multiplier)
    {
        var config = GameManager.Instance.DamageConfig; // 静态依赖
        return ...;
    }
}
```

## EditMode vs PlayMode

| EditMode | PlayMode |
|----------|----------|
| 纯逻辑、数据处理 | MonoBehaviour 生命周期 |
| 运行快（无需 Play） | 需要场景加载 |
| 无 GC 开销 | 物理/碰撞/协程 |
| 首选 | 仅在需要时使用 |

## 测试缝隙（Seam）设计

在需要测试的边界处预留接口：

- MonoBehaviour 引用接口而非具体实现
- 时间依赖使用 `ITimeProvider` 而非 `Time.deltaTime`
- 随机数使用 `IRandom` 而非 `UnityEngine.Random`
- 输入使用 `IInputProvider` 而非直接读取 Input
