using System;

[AttributeUsage(AttributeTargets.Class)]
public class EventAttribute : Attribute
{
    public string Id { get; }

    public EventAttribute(string id)
    {
        Id = id;
    }
}
