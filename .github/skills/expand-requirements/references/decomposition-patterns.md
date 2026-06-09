# 游戏系统分解模式

常见游戏系统的分解参考，帮助需求拆分时识别子模块和边界。

## 战斗系统

| 子模块 | 职责 | 典型文件 |
|--------|------|---------|
| 伤害计算 | 公式、暴击、元素克制 | DamageCalculator.cs |
| 弹幕/射击 | 发射器、弹道、弹幕模式 | BulletEmitter.cs, BulletPattern.cs |
| 碰撞检测 | 命中判定、碰撞响应 | HitDetection.cs |
| 状态效果 | Buff/Debuff、持续伤害、减速 | StatusEffect.cs, StatusManager.cs |
| 技能系统 | 技能定义、冷却、释放 | Skill.cs, SkillManager.cs |

## UI 系统

| 子模块 | 职责 | 典型文件 |
|--------|------|---------|
| HUD | 血条、能量条、小地图 | HudController.cs |
| 商店 | 购买、出售、库存 | ShopController.cs, ShopModel.cs |
| 背包 | 物品管理、装备 | InventoryController.cs |
| 对话框 | 标题、确认、输入 | DialogManager.cs |
| 设置 | 音量、画质、控制 | SettingsController.cs |

## 关卡系统

| 子模块 | 职责 | 典型文件 |
|--------|------|---------|
| 房间生成 | 地图布局、房间模板 | RoomGenerator.cs |
| 敌人刷新 | 波次、刷新点、难度 | EnemySpawner.cs |
| Boss 逻辑 | Boss 行为、多阶段 | BossController.cs |
| 道具掉落 | 掉落表、拾取 | DropTable.cs, Loot.cs |
| 进度存档 | 关卡进度、解锁 | ProgressSaver.cs |

## 成长系统

| 子模块 | 职责 | 典型文件 |
|--------|------|---------|
| 经验系统 | 经验获取、等级计算 | ExpSystem.cs |
| 技能树 | 解锁条件、升级效果 | SkillTree.cs |
| 装备系统 | 穿戴、属性加成 | EquipmentSystem.cs |
| 货币 | 多种货币、消费、获取 | CurrencyManager.cs |

## 分解原则

1. **单一职责**：每个子模块只做一件事
2. **可独立测试**：子模块可以在 EditMode 测试中单独验证
3. **最小接口**：模块间通过最小接口通信
4. **数据驱动**：配置数据用 ScriptableObject 或配置表，不硬编码
