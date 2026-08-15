using System.Collections.Generic;
using GCRuntime.Event;
using UnityEngine;

namespace GCRuntime.QuestSystem
{
    public static class QuestManager
    {
        /// <summary>
        /// 人物容器
        /// </summary> 
        public static QuestContainer QuestContainer;

        /// <summary>
        /// 只读任务容器
        /// </summary> 
        public static IReadOnlyList<QuestInfo> AllQuests => QuestContainer.QuestInfos.AsReadOnly();

        /// <summary>
        /// 初始化
        /// </summary> 
        public static void Initialize()
        {
            QuestContainer = Resources.Load<QuestContainer>("QuestContainer");
        }

        public static bool AcceptQuest(string questId)
        {
            var quest = QuestContainer.GetQuestByID(questId);
            if (quest == null)
            {
                Debug.LogWarning($"[QuestManager] 未找到任务: {questId}");
                return false;
            }

            if (quest.QuestState != QuestState.Available)
            {
                Debug.LogWarning($"[QuestManager] 任务无法接取，当前状态: {quest.QuestState}");
                return false;
            }

            var previous = quest.QuestState;
            quest.QuestState = QuestState.Active;

            EventSystem.Publish(new QuestStateChangedEvent
            {
                QuestId = questId,
                PreviousState = previous,
                CurrentState = QuestState.Active
            });

            Debug.Log($"[QuestManager] 接取任务: {quest.QuestName}");
            return true;
        }

        /// <summary>
        /// 完成任务（Active → Completed）
        /// </summary>
        public static bool CompleteQuest(string questId)
        {
            var quest = QuestContainer.GetQuestByID(questId);
            if (quest == null)
            {
                Debug.LogWarning($"[QuestManager] 未找到任务: {questId}");
                return false;
            }

            if (quest.QuestState != QuestState.Active)
            {
                Debug.LogWarning($"[QuestManager] 任务无法完成，当前状态: {quest.QuestState}");
                return false;
            }

            var previous = quest.QuestState;
            quest.QuestState = QuestState.Completed;

            EventSystem.Publish(new QuestStateChangedEvent
            {
                QuestId = questId,
                PreviousState = previous,
                CurrentState = QuestState.Completed
            });
            if (!quest.SingleTime) ResetQuest(questId);
            Debug.Log($"[QuestManager] 完成任务: {quest.QuestName}");
            return true;
        }

        /// <summary>
        /// 任务失败（Active → Failed）
        /// </summary>
        public static bool FailQuest(string questId)
        {
            var quest = QuestContainer.GetQuestByID(questId);
            if (quest == null)
            {
                Debug.LogWarning($"[QuestManager] 未找到任务: {questId}");
                return false;
            }

            if (quest.QuestState != QuestState.Active)
            {
                Debug.LogWarning($"[QuestManager] 任务无法失败，当前状态: {quest.QuestState}");
                return false;
            }

            var previous = quest.QuestState;
            quest.QuestState = QuestState.Failed;

            EventSystem.Publish(new QuestStateChangedEvent
            {
                QuestId = questId,
                PreviousState = previous,
                CurrentState = QuestState.Failed
            });
            ResetQuest(questId);
            Debug.Log($"[QuestManager] 任务失败: {quest.QuestName}");
            return true;
        }

        /// <summary>
        /// 解锁任务（Locked → Available）
        /// </summary>
        public static bool UnlockQuest(string questId)
        {
            var quest = QuestContainer.GetQuestByID(questId);
            if (quest == null) return false;

            if (quest.QuestState != QuestState.Locked) return false;

            var previous = quest.QuestState;
            quest.QuestState = QuestState.Available;

            EventSystem.Publish(new QuestStateChangedEvent
            {
                QuestId = questId,
                PreviousState = previous,
                CurrentState = QuestState.Available
            });

            Debug.Log($"[QuestManager] 解锁任务: {quest.QuestName}");
            return true;
        }

        /// <summary>
        /// 重置任务到初始状态（Completed/Failed → Available）
        /// </summary>
        public static bool ResetQuest(string questId)
        {
            var quest = QuestContainer.GetQuestByID(questId);
            if (quest == null) return false;

            if (quest.QuestState != QuestState.Completed && quest.QuestState != QuestState.Failed)
                return false;

            var previous = quest.QuestState;
            quest.QuestState = QuestState.Available;

            EventSystem.Publish(new QuestStateChangedEvent
            {
                QuestId = questId,
                PreviousState = previous,
                CurrentState = QuestState.Available
            });

           Debug.Log($"[QuestManager] 重置任务: {quest.QuestName}");
            return true;
        }
    }
}
