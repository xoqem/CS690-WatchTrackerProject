namespace WatchTracker;

public enum WatchItemType
{
    Movie = 1,
    TVShow = 2
}

public class WatchItem
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Progress { get; set; }
    public WatchItemType? ItemType { get; set; }

    // set the item type given the id in the enum (e.g. pass 2 to set it to TVShow)
    public void SetItemTypeFromId(string itemTypeId)
    {
        ItemType = Enum.TryParse<WatchItemType>(itemTypeId, out var itemType) ? itemType : null;
    }

    // get the item type as an id (e.g. returns the value 2 if the item type is TVShow)
    public string GetItemTypeAsId()
    {
        return ItemType?.ToString() ?? "";
    }
}
