using System.Text.Json;
using System.Text.Json.Serialization;

namespace WatchTracker;

class WatchListFileIO
{
    private string filePath;

    public WatchListFileIO(string filePath)
    {
        this.filePath = filePath;
    }

    public List<WatchItem> LoadWatchList()
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<WatchItem>>(jsonString) ?? new List<WatchItem>();
        }
        return new List<WatchItem>();
    }

    public void SaveWatchList(List<WatchItem> watchList)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(watchList, options);
        File.WriteAllText(filePath, jsonString);
    }
}