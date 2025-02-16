using System.Text.Json;
using System.Text.Json.Serialization;

namespace WatchTracker;

public class WatchListFileIO
{
    private string filePath;

    public WatchListFileIO(string filePath)
    {
        this.filePath = filePath;
    }

    public WatchList LoadWatchList()
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<WatchList>(jsonString) ?? new WatchList();
        }
        return new WatchList();
    }

    public void SaveWatchList(WatchList watchList)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(watchList, options);
        File.WriteAllText(filePath, jsonString);
    }
}