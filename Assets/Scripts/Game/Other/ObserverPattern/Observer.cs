
public enum EventEnum
{

}

public static class Observer
{
    public delegate void EventHandler(EventEnum eventEnum);
    public static event EventHandler OnEvent;

    public static void OnHandleEvent(EventEnum eventEnum) => OnEvent?.Invoke(eventEnum);
}
