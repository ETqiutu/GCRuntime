using GCRuntime.Event;

namespace GCRuntime.QuestSystem
{
    public enum QuestState
    {
        Locked,     
        Available,  
        Active,    
        Completed,  
        Failed    
    }

    public struct QuestStateChangedEvent : IEvent
    {
        public string QuestId;
        public QuestState PreviousState;
        public QuestState CurrentState;
    }
}
