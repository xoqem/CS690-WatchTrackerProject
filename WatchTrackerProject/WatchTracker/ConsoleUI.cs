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
            if (error != null)
            {
                AnsiConsole.WriteLine("");
                string idInput = AnsiConsole.Prompt(
                    new TextPrompt<string>($"{error} Press enter to continue.")
                        .AllowEmpty()
                );

                error = null;
            }

            AnsiConsole.Clear();
            DisplayWatchList();
            AnsiConsole.WriteLine("");
            AnsiConsole.WriteLine("========== Main Menu ==========");

            AnsiConsole.WriteLine("[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit");
            string input = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter an option:")
            );

            if (int.TryParse(input, out var option))
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
                        CreateFilter();
                        break;
                    case 5:
                        ShowExitMessage();
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
        return $"{i + 1}. {watchList.Items[i].GetDisplayString()}";
    }

    void DisplayWatchList()
    {
        AnsiConsole.WriteLine("========== Watch List ==========");
        if (watchList.Items.Count == 0)
        {
            AnsiConsole.WriteLine("No items in watch list.");
        }
        else
        {
            if (filter != null)
            {
                AnsiConsole.WriteLine($"Filtering by: {filter.GetDisplayString()}");
                AnsiConsole.WriteLine("");
            }

            for (int i = 0; i < watchList.Items.Count; i++)
            {
                // intentionally filtering inside the for, so the indices stay the same with or without filters
                if (watchList.Items[i].MatchesFilter(filter))
                {
                    AnsiConsole.WriteLine(GetWatchListItemDisplayString(i));
                }
            }
        }
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
                $"Enter item type: {WatchItemTypeUtils.GetOptions()}:",
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

        if (int.TryParse(idInput, out var id) && id > 0 && id <= watchList.Items.Count)
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

    void CreateFilter()
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

    void ShowExitMessage()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Exiting application...");
    }

    public void SaveWatchList()
    {
        watchList.SortByTitle();
        watchListFileIO.SaveWatchList(watchList);
    }
}