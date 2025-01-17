[System.Serializable]
[EventAttribute("PlayerChatEvent")]
public class PlayerChatEvent : Event
{
    public string Message;
    public int[] Slots;
}
