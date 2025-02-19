using Spectre.Console.Rendering;

namespace WatchTracker;

public class WatchItem
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Progress { get; set; }
    public WatchItemType? ItemType { get; set; }

    public string GetDisplayString() {
        var displayStringItems = new List<string>();
        
        if (!string.IsNullOrEmpty(Title)) displayStringItems.Add($"Title: {Title}");
        if (!string.IsNullOrEmpty(Genre)) displayStringItems.Add($"Genre: {Genre}");
        if (!string.IsNullOrEmpty(Progress)) displayStringItems.Add($"Progress: {Progress}");
        if (!string.IsNullOrEmpty(ItemType?.ToString())) displayStringItems.Add($"Type: {ItemType}");

        return string.Join(", ", displayStringItems);
    }

    public bool MatchesFilter(WatchItem? filter) {
        if (filter == null) {
            return true;
        }

        if (
            (!string.IsNullOrEmpty(filter?.Title) && !Title?.Contains(filter.Title, StringComparison.OrdinalIgnoreCase) == true) ||
            (!string.IsNullOrEmpty(filter?.Genre) && !Genre?.Contains(filter.Genre, StringComparison.OrdinalIgnoreCase) == true) ||
            (!string.IsNullOrEmpty(filter?.Progress) && !Progress?.Contains(filter.Progress, StringComparison.OrdinalIgnoreCase) == true) ||
            (filter?.ItemType != null && ItemType != filter.ItemType)
        ) {
            return false;
        }

        return true;
    }
}
