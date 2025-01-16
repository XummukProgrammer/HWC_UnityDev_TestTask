using System.Collections.Generic;
using UnityEngine;

public class Events
{
    private List<IBaseEventListener> _listeners = new();
    private Dictionary<string, System.Action<string>> _actions = new();

    public Events()
    {
        AddAction<PlayerConnectEvent>("PlayerConnectEvent");
    }

    public void AddListener(IBaseEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void RemoveListener(IBaseEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Send(string jsonEvent)
    {
        var @event = JsonUtility.FromJson<Event>(jsonEvent);
        
        if (_actions.TryGetValue(@event.Id, out var action))
        {
            action.Invoke(jsonEvent);
            return;
        }
    }

    private void AddAction<T>(string id) where T : Event
    {
        _actions[id] = (jsonEvent) =>
        {
            var castedEvent = JsonUtility.FromJson<T>(jsonEvent);

            foreach (var listener in _listeners)
            {
                if (listener is IEventListener<T> castedListener)
                {
                    castedListener.Accept(castedEvent);
                }
            }
        };
    }
}
