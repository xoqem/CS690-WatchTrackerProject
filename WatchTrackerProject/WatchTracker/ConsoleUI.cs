using System.Runtime.CompilerServices;
using System.Text;
using Spectre.Console;

namespace WatchTracker;

public class ConsoleUI
{
    WatchList watchList;
    WatchListFileIO watchListFileIO;
    WatchItem? filter;

    public ConsoleUI()
    {
        watchListFileIO = new WatchListFileIO("watchlist.json");
        watchList = watchListFileIO.LoadWatchList();
        watchList.SortByTitle();
    }

    public void Show()
    {
        string? error = null;

        while (true)
        {
            DisplayWatchList();
            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine("========== Main Menu ==========");

            if (error != null)
            {
                AnsiConsole.WriteLine(error);
                error = null;
            }

            AnsiConsole.WriteLine("[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit");
            string input = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter an option:")
            );

            int option;
            if (int.TryParse(input, out option))
            {
                switch (option)
                {
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        EditItem();
                        break;
                    case 3:
                        DeleteItem();
                        break;
                    case 4:
                        FilterList();
                        break;
                    case 5:
                        Exit();
                        return;
                    default:
                        error = "Invalid option. Please try again.";
                        break;
                }
            }
            else
            {
                error = "Invalid input. Please try again.";
            }
        }
    }

    string GetWatchItemDisplayString(WatchItem? item)
    {
        var displayStringItems = new List<string>();
        
        if (!string.IsNullOrEmpty(item?.Title)) displayStringItems.Add($"Title: {item?.Title}");
        if (!string.IsNullOrEmpty(item?.Genre)) displayStringItems.Add($"Genre: {item?.Genre}");
        if (!string.IsNullOrEmpty(item?.Progress)) displayStringItems.Add($"Progress: {item?.Progress}");
        if (!string.IsNullOrEmpty(item?.ItemType?.ToString())) displayStringItems.Add($"Type: {item?.ItemType}");

        return string.Join(", ", displayStringItems);
    }

    string GetWatchListItemDisplayString(int i)
    {
        return $"{i + 1}. {GetWatchItemDisplayString(watchList.Items[i])}";
    }

    void DisplayWatchList()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("========== Watch List ==========");
        if (watchList.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items in watch list.");
        }
        else
        {
            if (filter != null)
            {
                AnsiConsole.WriteLine($"Filtering by: {GetWatchItemDisplayString(filter)}");
                AnsiConsole.WriteLine("");
            }

            for (int i = 0; i < watchList.Items.Count; i++)
            {
                var item = watchList.Items[i];
                    
                if (filter != null) {
                    if (
                        (!string.IsNullOrEmpty(filter?.Title) && !item.Title?.Contains(filter.Title, StringComparison.OrdinalIgnoreCase) == true) ||
                        (!string.IsNullOrEmpty(filter?.Genre) && !item.Genre?.Contains(filter.Genre, StringComparison.OrdinalIgnoreCase) == true) ||
                        (!string.IsNullOrEmpty(filter?.Progress) && !item.Progress?.Contains(filter.Progress, StringComparison.OrdinalIgnoreCase) == true) ||
                        (filter?.ItemType != null && item.ItemType != filter.ItemType)
                    ) {
                        continue;
                    }
                }

                AnsiConsole.WriteLine(GetWatchListItemDisplayString(i));
            }
        }
    }

    string GenerateItemTypeOptions()
    {
        var itemTypeOptions = new StringBuilder();
        foreach (WatchItemType itemType in Enum.GetValues(typeof(WatchItemType)))
        {
            itemTypeOptions.Append($"[[{(int)itemType}]] {itemType}, ");
        }
        return itemTypeOptions.ToString().TrimEnd(',', ' ');
    }

    string? PromptWithOptionalDefault(string question, string? defaultValue = null)
    {
        var textPrompt = new TextPrompt<string?>(question)
            .DefaultValue(defaultValue)
            .AllowEmpty();

        if (string.IsNullOrEmpty(defaultValue))
        {
            textPrompt.HideDefaultValue();
        }

        return AnsiConsole.Prompt(textPrompt);
    }

    WatchItem BuildWatchItem(WatchItem item)
    {
        item.Title = PromptWithOptionalDefault("Enter title:", item.Title);
        item.Genre = PromptWithOptionalDefault("Enter genre:", item.Genre);
        item.Progress = PromptWithOptionalDefault("Enter progress:", item.Progress);
        item.ItemType = WatchItemTypeUtils.GetItemTypeFromId(
            PromptWithOptionalDefault(
                $"Enter item type: {GenerateItemTypeOptions()}:",
                WatchItemTypeUtils.GetIdFromItemType(item.ItemType)
            )
        );

        return item;
    }

    void AddItem()
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.WriteLine("========== Add Item ==========");
        var newItem = BuildWatchItem(new WatchItem());
        watchList.Items.Add(newItem);
        SaveWatchList();
    }

    void EditItem()
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.WriteLine("========== Edit Item ==========");
        string idInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the id of the item you wish to edit (or press enter to return to the main menu):")
                .AllowEmpty()
        );

        if (string.IsNullOrEmpty(idInput))
        {
            return;
        }

        if (int.TryParse(idInput, out var id) && id > 0 && id <= watchList.Items.Count)
        {
            var item = watchList.Items[id - 1];
            AnsiConsole.WriteLine($"Editing item: {GetWatchListItemDisplayString(id - 1)}");
            var updatedItem = BuildWatchItem(item);
            watchList.Items[id - 1] = updatedItem;
            SaveWatchList();
        }
        else
        {
            AnsiConsole.WriteLine("Invalid id. Please try again.");
        }
    }

    void DeleteItem()
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.WriteLine("========== Delete Item ==========");
        string idInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the id of the item you wish to delete (or press enter to return to the main menu):")
                .AllowEmpty()
        );

        if (string.IsNullOrEmpty(idInput))
        {
            return;
        }

        int id;
        if (int.TryParse(idInput, out id) && id > 0 && id <= watchList.Items.Count)
        {
            var item = watchList.Items[id - 1];
            bool confirmed = AnsiConsole.Confirm($"Are you sure you want to delete: {item.Title}?");
            if (confirmed)
            {
                watchList.Items.RemoveAt(id - 1);
                SaveWatchList();
            }
        }
    }

    void FilterList()
    {
        AnsiConsole.WriteLine("");
        AnsiConsole.WriteLine("========== Filter List ==========");
        AnsiConsole.WriteLine("Enter the values to filter by below. Leave a field blank to not filter by that field.");

        filter = BuildWatchItem(new WatchItem());

        if (
            string.IsNullOrEmpty(filter.Title) &&
            string.IsNullOrEmpty(filter.Genre) &&
            string.IsNullOrEmpty(filter.Progress) &&
            filter.ItemType == null)
        {
            filter = null;
        }
    }

    void Exit()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Exiting application...");
    }

    public void SaveWatchList()
    {
        watchList.SortByTitle();
        watchListFileIO.SaveWatchList(watchList);
    }

    public void ClearWatchList()
    {
        watchList.Items.Clear();
        SaveWatchList();
    }
}