namespace WatchTracker;

public class WatchList
{
    public List<WatchItem> Items { get; set; }

    public WatchList()
    {
        Items = new List<WatchItem>();
    }

    public void SortByTitle()
    {
        Items = Items.OrderBy(i => i.Title).ToList();
    }
}
