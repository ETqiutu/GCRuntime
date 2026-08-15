using UnityEngine;

namespace GCRuntime.QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "GCRuntime/QuestSystem/Quest")]

    public class QuestInfo : ScriptableObject
    {
        [HideInInspector] public string QuestName;
        public string ID;        
        [TextArea(3, 10)] public string Description;
        public Difficulty QuestDifficulty;
        [HideInInspector] public QuestState QuestState;
        public bool SingleTime;
    }
}
