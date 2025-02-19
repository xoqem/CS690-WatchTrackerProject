using System.Text;
using Spectre.Console;

namespace WatchTracker;

public class ConsoleUI
{
    WatchList watchList;
    WatchListFileIO watchListFileIO;

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

    string GetWatchListItemDisplayString(int i)
    {
        var item = watchList.Items[i];
        return $"{i + 1}. Title: {item.Title}, Genre: {item.Genre}, Progress: {item.Progress}, Type: {item.ItemType}";
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
            for (int i = 0; i < watchList.Items.Count; i++)
            {
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
        item.SetItemTypeFromId(
            PromptWithOptionalDefault($"Enter item type: {GenerateItemTypeOptions()}:", item.GetItemTypeAsId())
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

        int id;
        if (int.TryParse(idInput, out id) && id > 0 && id <= watchList.Items.Count)
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
        AnsiConsole.Prompt(new TextPrompt<string>("Filtering is not implemented yet. Press enter to return to the main menu:")
            .AllowEmpty()
        );
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