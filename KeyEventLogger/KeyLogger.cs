using System.Text;

namespace KeyEventLogger;

public class KeyLogger
{
    private readonly List<KeyLogEntry> entries = new List<KeyLogEntry>();

    public bool IsRecording { get; private set; }
    public IReadOnlyList<KeyLogEntry> Entries => entries;
    public int KeyDownCount => entries.Count(e => e.EventType == "KeyDown");

    public void Start() => IsRecording = true;
    public void Stop() => IsRecording = false;
    public void Clear() => entries.Clear();

    // Kayıt kapalıysa null döner, açıksa oluşturulan kaydı geri verir
    public KeyLogEntry? Record(string keyName, string eventType)
    {
        if (!IsRecording) return null;

        var entry = new KeyLogEntry(keyName, eventType);
        entries.Add(entry);
        return entry;
    }

    public void SaveToFile(string path)
    {
        using var writer = new StreamWriter(path, false, Encoding.UTF8);
        foreach (var entry in entries)
            writer.WriteLine(entry.ToString());
    }

    public IEnumerable<KeyValuePair<string, int>> TopKeys(int count)
    {
        return entries
            .Where(e => e.EventType == "KeyDown")
            .GroupBy(e => e.KeyName)
            .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
            .OrderByDescending(p => p.Value)
            .Take(count);
    }
}