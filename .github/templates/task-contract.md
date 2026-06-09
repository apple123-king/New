# 任务合同: [任务名称]

## 基本信息

- **迭代**: [迭代号]
- **关联需求**: [需求文档路径]
- **状态**: 待开始 | 进行中 | 待验收 | 通过 | 不通过 | 阻塞

## 完成定义

### 验收标准

1. [ ] [标准1 — 必须可量化验证]
2. [ ] [标准2]
3. [ ] [标准3]

### 编译要求

- [ ] `debug_check_compilation` 返回 0 error
- [ ] 无新增 Warning（或仅已知 Warning）

### 测试要求

- [ ] 新增 EditMode 测试全部通过
- [ ] 既有测试套件无回退
- [ ] 每个公开方法至少一个测试

### 代码规范

- [ ] 符合 unity-csharp.instructions.md
- [ ] 中文注释完整
- [ ] 无冗余代码

## 文件分区

### 独占文件（只有本任务可修改）

```
Assets/Scripts/Game/[模块]/
Assets/Tests/EditMode/[模块]Tests.cs
```

### 禁止修改

```
Assets/Scripts/Game/[其他模块]/
Assets/ThirdParty/
Assets/Scripts/LubanGenerated/
```

### 共享文件（需串行处理）

```
[共享接口文件等]
```

## 依赖

- **前置任务**: [无 / 任务ID]
- **被依赖**: [无 / 任务ID]

## 实现指引

[可选：建议的实现步骤、架构参考]
