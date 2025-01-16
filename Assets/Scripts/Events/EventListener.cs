public interface IBaseEventListener
{
}

public class EventListener<T> : IBaseEventListener where T : Event
{
    public System.Action<T> Accepted;

    public void Accept(T @event)
    {
        Accepted?.Invoke(@event);
    }
}
