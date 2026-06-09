# Unity Skills REST API 配置指南

## 前置条件

Unity Skills 是本框架的**强制依赖**。所有 Agent 通过 Unity Skills REST API 与 Unity Editor 交互。

## 安装步骤

### 1. 安装 Unity Skills 包

按照官方文档完成安装：**https://github.com/Besty0728/Unity-Skills**

### 2. 验证 REST 服务器

Unity Skills 包安装后，REST 服务器默认运行在 `localhost:8090`。在浏览器中打开确认：

```
http://localhost:8090/status
```

### 3. 配置 Python Helper

确保 `.agents/skills/unity-skills/scripts/unity_skills.py` 在 Python path 中可用，并使用 Python 3：

```bash
# 验证
python -c "import sys; sys.path.insert(0, r'.agents/skills/unity-skills/scripts'); import unity_skills; print(unity_skills.get_server_status())"
```

### 4. 连接测试

在项目中测试完整连接：

```python
import sys
sys.path.insert(0, r".agents/skills/unity-skills/scripts")
import unity_skills

# 基础状态检查
status = unity_skills.get_server_status()
print(f"Server running: {status.get('running')}")

# 编译检查
result = unity_skills.call_skill("debug_check_compilation")
print(f"Compilation: {'OK' if result.get('success') else 'FAILED'}")

# 场景信息
summary = unity_skills.call_skill("scene_summarize")
print(f"Current scene: {summary}")
```

## 模式说明

### Semi-Auto（默认）

安全模式，仅以下模块可用：
- `script` — 脚本创建和编辑
- `perception` — 场景分析
- `scene` — 场景管理
- `editor` — 编辑器上下文
- `asset` — 资源查找
- `workflow` — 任务追踪
- `debug` / `console` — 编译检查和日志
- Advisory 模块 — 架构/设计指导

### Full-Auto

所有 513 个 REST skill 可用。在本框架中，仅 Reviewer 在验收阶段使用 Full-Auto 的 `validation` 模块：
- `validate_scene` — 场景完整性验证
- `validate_find_missing_scripts` — 缺失脚本检查

## 常见问题

| 问题 | 排查方法 |
|------|---------|
| 连接失败 | 确认 Unity Editor 运行且 Skills 包已安装 |
| 端口占用 | 检查 8090 端口，或在 Skills 包设置中修改端口 |
| Python import 失败 | 确认使用 Python 3，且 `.agents/skills/unity-skills/scripts` 已加入 PYTHONPATH |
| 编译检查超时 | Domain Reload 期间服务器暂时不可用，等待重试 |
| test_run 无返回 | 使用 jobId 轮询 `test_get_result`，测试是异步的 |
