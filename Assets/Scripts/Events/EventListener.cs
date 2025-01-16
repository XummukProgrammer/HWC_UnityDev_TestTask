public interface IBaseEventListener
{
}

public interface IEventListener<T> : IBaseEventListener where T : Event
{
    public void Accept(T @event);
}
