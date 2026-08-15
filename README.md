# GCRuntime 游戏集成框架

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

**GCRuntime** 是一套基于Unity引擎的轻量级游戏开发集成框架，旨在为中小型游戏项目提供一套**开箱即用**的核心系统解决方案。它涵盖了游戏开发中最常见的基础设施，包括事件通信、对话管理、任务系统、AI行为树、状态机和基础算法。

> 框架设计遵循**高内聚、低耦合**的原则，各系统可独立使用，也可无缝协作。

---

## 🎯 框架目标

- **加速原型开发**：提供通用系统，减少重复造轮子。
- **模块化设计**：各系统独立，可插拔，便于按需集成。
- **可视化的编辑器支持**：通过自定义编辑器（如UIToolkit）降低设计和配置门槛。
- **高效的运行时性能**：底层采用对象池、缓存和高效数据结构。

---

## 📦 框架内容

| 系统模块 | 说明 |
| :--- | :--- |
| **EventSystem 事件系统** | 基于观察者模式的高效事件管理，支持全局和局部事件的注册、派发与回调。 |
| **DialogueSystem 对话系统** | 灵活的对话管理框架，支持对话树编辑器的可视化配置和回调机制。 |
| **QuestSystem 任务系统** | 完整的任务生命周期管理，与事件系统深度集成，实现动态任务触发和追踪。 |
| **BehaviourTree 行为树编辑器** | 可视化AI行为树编辑工具，支持拖拽式构建和实时调试。 |
| **StateMachine 有限状态机** | 轻量级状态机框架，支持状态切换、进入/退出回调，适用于角色AI和UI状态控制。 |
| **Algorithm 算法库** | 基础算法工具库，当前已集成A*寻路算法。 |

---

## 🧩 模块详解

### 1. EventSystem 事件系统

基于观察者模式设计，提供高效的事件注册与派发机制，支持全局/局部事件、泛型事件类型及一次性回调，实现模块间解耦。

---

### 2. DialogueSystem 对话系统

提供可视化的对话树编辑工具（基于UIToolkit），支持多分支对话、条件节点和自定义回调，可与EventSystem联动触发事件或任务。

---

### 3. QuestSystem 任务系统

管理任务的完整生命周期（未接取 → 进行中 → 已完成/已失败），与EventSystem深度集成，支持任务链、并行/串行任务及进度追踪。

---

### 4. BehaviourTree 行为树编辑器

基于UIToolkit的可视化行为树编辑工具，支持拖拽创建节点、连线，内置Sequence、Selector、Parallel等组合节点，支持自定义条件/动作节点及运行时调试。

---

### 5. StateMachine 有限状态机

轻量级状态机框架，支持状态注册、切换、回退，提供OnEnter/OnUpdate/OnExit生命周期回调，支持层级状态机，适用于角色AI及UI状态控制。

---

### 6. Algorithm 算法库

通用算法工具集，当前已集成A*寻路算法，支持网格地图、自定义代价函数和启发函数。

---

## 🚀 快速开始

### 环境要求

- Unity 2021.3 LTS 或更高版本
- .NET Standard 2.0

### 导入框架

将 `GCRuntime` 文件夹拷贝到Unity项目的 `Assets` 目录下即可。

### 初始化

框架无需全局初始化，各系统按需使用。

---

## 🛠️ 编辑器工具

| 菜单路径 | 编辑器 |
| :--- | :--- |
| `GCRuntime → Dialogue Editor` | 对话树可视化编辑器 |
| `GCRuntime → BehaviourTree Editor` | 行为树可视化编辑器 |

---

## 📁 目录结构

```
GCRuntime/
├── Runtime/
│   ├── EventSystem/          # 事件系统
│   ├── DialogueSystem/       # 对话系统
│   ├── QuestSystem/          # 任务系统
│   ├── StateMachine/         # 有限状态机
│   └── Algorithm/            # 算法库
│       └── AStar/            # A*寻路算法
├── Editor/
│   ├── DialogueEditor/       # 对话树编辑器
│   └── BehaviourTreeEditor/  # 行为树编辑器
└── Examples/                 # 示例场景和代码
```

---

## 📄 许可证

[MIT License](https://opensource.org/licenses/MIT)

Copyright (c) 2026 [你的名字]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

---

## 🤝 贡献

欢迎提交Issue和Pull Request。

---

## 📧 联系方式

- 作者：[GameCrafter]
- 邮箱：[minekuat@163.com]
