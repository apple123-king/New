---
description: "Unity 架构决策指南。Use when designing system structure, module boundaries, choosing patterns, deciding between MonoBehaviour and plain C#."
---

# Unity 架构决策指南

## 薄 MonoBehaviour 原则

MonoBehaviour 只负责：
- Unity 生命周期回调（Awake, Start, Update, OnDestroy）
- Inspector 字段绑定
- 将事件转发给纯 C# 逻辑层

业务逻辑、数据处理、算法放在**纯 C# 类**中，方便测试和复用。

## ScriptableObject 用途

适用：
- 配置数据（数值、参数、曲线）
- 信号/事件通道（ScriptableObject Event 模式）
- 跨场景共享状态

不适用：
- 运行时频繁变化的状态
- 需要序列化到存档的数据

## 通信方式选择

| 方式 | 适用场景 | 耦合度 |
|------|---------|--------|
| 直接引用 | 父子关系、明确依赖 | 高 |
| 接口 | 可替换实现、测试注入 | 中 |
| 事件/委托 | 一对多通知、解耦 | 低 |
| 命令模式 | 可撤销操作、跨层操作 | 低 |

## 模块边界划分

- 按功能域划分文件夹（Combat/, UI/, Level/, Growth/）
- 使用 Assembly Definition (.asmdef) 控制编译依赖
- 模块间通过接口或事件通信，避免直接引用

## 性能热路径

- Update/FixedUpdate 中避免分配 GC（new、LINQ、string 拼接）
- 使用对象池管理频繁创建销毁的对象
- 大量实体考虑 DOTS/Jobs（但要权衡复杂度）
