public class DifficultyEvent
{
    public enum Type
    {
        Unknown,
        AddChasers,
        AddPushers
    }

    public float Time { get; private set; }
    public string Message { get; private set; }

    public Type EventType { get; private set; }

    public bool Fired { get; set; }
    

    public DifficultyEvent(float time, string message, Type eventType)
    {
        Time = time;
        Message = message;
        EventType = eventType;
    }
}