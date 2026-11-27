namespace KeyEventLogger;

public class KeyLogEntry
{
    public DateTime Time { get; }
    public string KeyName { get; }
    public string EventType { get; }

    public KeyLogEntry(string keyName, string eventType)
    {
        Time = DateTime.Now;
        KeyName = keyName;
        EventType = eventType;
    }

    public override string ToString()
        => $"[{Time:yyyy-MM-dd HH:mm:ss.fff}]  {EventType,-7}  {KeyName}";
}