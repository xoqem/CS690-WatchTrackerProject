using Spectre.Console.Rendering;

namespace WatchTracker;

public enum WatchItemType
{
    Movie = 1,
    TVShow = 2
}

public class WatchItemTypeUtils {
    public static WatchItemType? GetItemTypeFromId(string? itemTypeId)
    {
        return Enum.TryParse<WatchItemType>(itemTypeId, out var itemType) ? itemType : null;
    }

    public static string? GetIdFromItemType(WatchItemType? itemType)
    {
        return itemType?.ToString("D") ?? null;
    }
}

public class WatchItem
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Progress { get; set; }
    public WatchItemType? ItemType { get; set; }
}
