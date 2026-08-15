using System.Collections.Generic;
using UnityEngine;

namespace GCRuntime.QuestSystem
{
    [CreateAssetMenu(fileName = "QuestContainer", menuName = "GCRuntime/QuestSystem/QuestContainer")]
    public class QuestContainer : ScriptableObject
    {
        /// <summary>
        /// 任务管理列表
        /// </summary> 
        [Header("任物管理列表")]
        public List<QuestInfo> QuestInfos = new List<QuestInfo>();

        /// <summary>
        /// 通过名称得到任务信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns> 
        public QuestInfo GetQuestByName(string Name)
        {
            foreach (var item in QuestInfos)
            {
                if (item.QuestName == Name)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 通过ID得到任务信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns> 
        public QuestInfo GetQuestByID(string ID)
        {
            foreach (var item in QuestInfos)
            {
                if (item.ID == ID)
                    return item;
            }
            return null;
        }
        
        /// <summary>
        /// 添加任务信息
        /// </summary>
        /// <param name="questInfo"></param>
        public void AddQuestInfo(QuestInfo questInfo)
        {
            QuestInfos.Add(questInfo);
        }

        /// <summary>
        /// 移除人物信息
        /// </summary>
        /// <param name="questInfo"></param>
        public void RemoveQuest(QuestInfo questInfo)
        {
            QuestInfos.Remove(questInfo);
        }
        
        /// <summary>
        /// 通过名称移除任务信息
        /// </summary>
        /// <param name="Name"></param>
        public void RemoveQuestByName(string Name)
        {
            foreach (var item in QuestInfos)
            {
                if (item.QuestName == Name)
                    QuestInfos.Remove(item);
            }
            return;
        }
        
        /// <summary>
        /// 通过ID移除任务信息
        /// </summary>
        /// <param name="ID"></param> 
        public void RemoveQuestByID(string ID)
        {
            foreach (var item in QuestInfos)
            {
                if (item.ID == ID)
                    QuestInfos.Remove(item);
            }
            return;
        }
    }
}
