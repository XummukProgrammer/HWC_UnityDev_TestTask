using System.Collections.Generic;
using UnityEngine;

public static class EventsActions
{
    private static Dictionary<string, System.Action<string, List<IBaseEventListener>>> _actions = new();

    public static void Init()
    {
        AddAction<PlayerChatEvent>();
        AddAction<PlayerFullConnectedEvent>();
        AddAction<PlayerAcceptedEvent>();
        AddAction<PlayerButtonPressedEvent>();
        AddAction<PlayerCreateEntityEvent>();
        AddAction<CreateEntityEvent>();
        AddAction<UpdateEntityPositionEvent>();
        AddAction<PlayerPullEntitiesEvent>();
    }

    public static void AddAction<T>() where T : Event
    {
        _actions[typeof(T).Name] = (jsonEvent, listeners) =>
        {
            var castedEvent = JsonUtility.FromJson<T>(jsonEvent);

            foreach (var listener in listeners)
            {
                if (listener is EventListener<T> castedListener)
                {
                    castedListener.Accept(castedEvent);
                }
            }
        };
    }

    public static void RunAction(string jsonEvent, List<IBaseEventListener> listeners)
    {
        var @event = JsonUtility.FromJson<Event>(jsonEvent);

        if (_actions.TryGetValue(@event.Id, out var action))
        {
            action.Invoke(jsonEvent, listeners);
            return;
        }
    }
}

public class Events
{
    private List<IBaseEventListener> _listeners = new();
    private Dictionary<string, System.Action<string>> _actions = new();

    public void AddListener(IBaseEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(IBaseEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Fire(string jsonEvent)
    {
        EventsActions.RunAction(jsonEvent, _listeners);
    }
}
