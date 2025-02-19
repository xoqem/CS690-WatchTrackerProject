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

    public static string GetOptions()
    {
        var itemTypeOptions = new List<string>();
        foreach (WatchItemType itemType in Enum.GetValues(typeof(WatchItemType)))
        {
            itemTypeOptions.Add($"[[{(int)itemType}]] {itemType}");
        }

        return string.Join(", ", itemTypeOptions);
    }
}
