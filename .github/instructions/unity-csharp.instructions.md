---
description: "Unity C# 编码规范。Use when writing or reviewing C# scripts for Unity projects."
applyTo: "**/*.cs"
---

# Unity/C# 编码规范

## 命名

- 类、方法、属性：`PascalCase`
- 私有字段：`camelCase`
- 常量：`UPPER_SNAKE_CASE` 或 `PascalCase`
- 文件名与类名一致

## 格式

- 4 空格缩进，花括号独占新行
- 每个文件一个类

## 字段声明

- Inspector 绑定优先使用 `[SerializeField] private`
- 避免直接公开字段
- 字段按 `[SerializeField]` → `private` → `property` 顺序排列

## 注释

- 变量和内部函数使用中文单行注释 `//`
- 暴露给外部的函数使用 XML 文档注释 `///`
- 每个测试方法添加中文注释说明目的

## 架构

- **薄 MonoBehaviour**：MonoBehaviour 负责 Unity 生命周期和 Inspector 绑定，业务逻辑放纯 C# 类
- **ScriptableObject**：用于配置数据和共享状态
- 逻辑层与表现层分离，方便测试

## 精简原则

- 字段和接口越少越好
- 不需要的代码要删除（特别是冗余变量和函数）
- 新功能不需要兼容旧版本
