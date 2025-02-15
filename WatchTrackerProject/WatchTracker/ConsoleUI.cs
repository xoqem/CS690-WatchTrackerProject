using System.Text;

namespace WatchTracker;

enum ItemType
{
    Movie = 1,
    TVShow = 2
}

public class ConsoleUI
{
    List<WatchItem> watchList;
    WatchListFileIO watchListFileIO;

    public ConsoleUI()
    {
        watchListFileIO = new WatchListFileIO("watchlist.json");
        watchList = watchListFileIO.LoadWatchList();
        SortWatchListByTitle();
    }

    public void Show()
    {
        string? error = null;

        while (true)
        {
            DisplayWatchList();
            Console.WriteLine("");
            Console.WriteLine("========== Main Menu ==========");

            if (error != null)
            {
                Console.WriteLine(error);
                error = null;
            }

            Console.WriteLine("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit");

            string? input = Console.ReadLine();
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
        var item = watchList[i];
        return $"{i + 1}. Title: {item.Title}, Genre: {item.Genre}, Progress: {item.Progress}, Type: {item.ItemType}";
    }

    void DisplayWatchList()
    {
        Console.Clear();
        Console.WriteLine("========== Watch List ==========");
        if (watchList.Count == 0)
        {
            Console.WriteLine("No items in watch list.");
        }
        else
        {
            for (int i = 0; i < watchList.Count; i++)
            {
                Console.WriteLine(GetWatchListItemDisplayString(i));
            }
        }
    }

    string ItemTypeIdToName(string? itemTypeId)
    {
        if (int.TryParse(itemTypeId, out var itemType))
        {
            if (Enum.IsDefined(typeof(ItemType), itemType))
            {
                return ((ItemType)itemType).ToString();
            }
        }
        return "";
    }

    string ItemTypeNameToId(string? itemTypeName)
    {
        if (Enum.TryParse(typeof(ItemType), itemTypeName, out var itemType))
        {
            return ((int)itemType).ToString();
        }

        return "";
    }

    string GenerateItemTypeOptions()
    {
        var itemTypeOptions = new StringBuilder();
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
        {
            itemTypeOptions.Append($"[{(int)itemType}] {itemType}, ");
        }
        return itemTypeOptions.ToString().TrimEnd(',', ' ');
    }

    WatchItem BuildWatchItem(WatchItem item)
    {
        item.Title = ReadLineWithDefault("Enter title:", item.Title);
        item.Genre = ReadLineWithDefault("Enter genre:", item.Genre);
        item.Progress = ReadLineWithDefault("Enter progress:", item.Progress);

        string itemType;
        while (true)
        {
            Console.WriteLine($"Select item type: {GenerateItemTypeOptions()}");
            string? itemTypeId = ReadLineWithDefault("Enter item type:", ItemTypeNameToId(item.ItemType));
            itemType = ItemTypeIdToName(itemTypeId);
            if (!string.IsNullOrEmpty(itemType))
            {
                item.ItemType = itemType;
                break;
            }
            else
            {
                Console.WriteLine("Invalid item type. Please try again.");
            }
        }

        return item;
    }

    void AddItem()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Add Item ==========");
        var newItem = BuildWatchItem(new WatchItem());
        watchList.Add(newItem);
        SaveWatchList();
    }

    void EditItem()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Edit Item ==========");
        Console.WriteLine("Enter the id of the item you wish to edit (or press enter to return to the watch list):");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrEmpty(idInput))
        {
            return;
        }

        int id;
        if (int.TryParse(idInput, out id) && id > 0 && id <= watchList.Count)
        {
            var item = watchList[id - 1];
            Console.WriteLine($"Editing item: {GetWatchListItemDisplayString(id - 1)}");
            var updatedItem = BuildWatchItem(item);
            watchList[id - 1] = updatedItem;
            SaveWatchList();
        }
        else
        {
            Console.WriteLine("Invalid id. Please try again.");
        }
    }

    string ReadLineWithDefault(string prompt, string? defaultValue)
    {
        defaultValue ??= string.Empty;
        Console.Write($"{prompt} {defaultValue}");
        var input = new StringBuilder(defaultValue);
        
        int key;
        while ((key = Console.Read()) != (int)ConsoleKey.Enter)
        {
            if (key == (int)ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
            }
            // if not control character add to input
            else if (!char.IsControl((char)key))
            {
                input.Append((char)key);
            }
        }

        return input.Length > 0 ? input.ToString() : defaultValue;
    }

    void DeleteItem()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Delete Item ==========");
        Console.WriteLine("Enter the id of the item you wish to delete (or press enter to return to the watch list):");
        string? idInput = Console.ReadLine();
        if (string.IsNullOrEmpty(idInput))
        {
            return;
        }

        int id;

        if (int.TryParse(idInput, out id) && id > 0 && id <= watchList.Count)
        {
            var item = watchList[id - 1];
            Console.WriteLine($"Are you sure you want to delete: {item.Title}? [y/n]");
            string? confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                watchList.RemoveAt(id - 1);
                SaveWatchList();
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
        else
        {
            Console.WriteLine("Invalid id. Please try again.");
        }
    }

    void FilterList()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Filter List ==========");
        Console.WriteLine("Filtering is not implemented yet. Press enter to return to the main menu.");
        Console.ReadLine();
    }

    void Exit()
    {
        Console.Clear();
        Console.WriteLine("Exiting application...");
    }

    public void SaveWatchList()
    {
        SortWatchListByTitle();
        watchListFileIO.SaveWatchList(watchList);
    }

    void SortWatchListByTitle()
    {
        watchList.Sort((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase));
    }

    public void ClearWatchList()
    {
        watchList.Clear();
        SaveWatchList();
    }
}