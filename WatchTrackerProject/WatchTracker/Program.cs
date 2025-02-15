using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace WatchTracker;

class WatchItem
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Progress { get; set; }
    public string? ItemType { get; set; }
}

enum ItemType
{
    Movie = 1,
    TVShow = 2
}

class Program
{
    static List<WatchItem> watchList = new List<WatchItem>();
    static string filePath = "watchlist.json";
    static int selectedIndex = 0;

    static void Main(string[] args)
    {
        LoadWatchList();

        string? error = null;

        while (true)
        {
            DisplayWatchList();
            Console.WriteLine("");
            Console.WriteLine("========== Main Menu ==========");
            Console.WriteLine("Use up / down arrows to highlight an item in the list.");

            if (error != null)
            {
                Console.WriteLine(error);
                error = null;
            }

            Console.WriteLine("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit");

            var key = Console.ReadKey(true).Key;
            
            Console.Clear();
            DisplayWatchList();
            Console.WriteLine("");

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    AddItem();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    EditItem();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    DeleteItem();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    FilterList();
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    Exit();
                    return;
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : watchList.Count - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex < watchList.Count - 1) ? selectedIndex + 1 : 0;
                    break;
                default:
                    error = "Invalid option. Please try again.";
                    break;
            }
        }
    }

    static string GetWatchListItemDisplayString(int i)
    {
        var item = watchList[i];
        return $"{i + 1}. Title: {item.Title}, Genre: {item.Genre}, Progress: {item.Progress}, Type: {item.ItemType}";
    }

    static void DisplayWatchList()
    {
        Console.Clear();

        if (watchList.Count == 0)
        {
            Console.WriteLine("========== Watch List ==========");
            Console.WriteLine("No items in watch list.");
            return;
        }

        int start, end;
        if (selectedIndex < 4)
        {
            start = 0;
            end = Math.Min(10, watchList.Count);
        }
        else if (selectedIndex >= watchList.Count - 5)
        {
            start = Math.Max(0, watchList.Count - 10);
            end = watchList.Count;
        }
        else
        {
            start = selectedIndex - 4;
            end = Math.Min(watchList.Count, selectedIndex + 6);
        }

        Console.WriteLine($"========== Watch List ========== ({start + 1}-{end} of {watchList.Count})");

        for (int i = start; i < end; i++)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine(GetWatchListItemDisplayString(i));
            Console.ResetColor();
        }
    }


    static string ItemTypeIdToName(string? itemTypeId)
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

    static string ItemTypeNameToId(string? itemTypeName)
    {
        if (Enum.TryParse(typeof(ItemType), itemTypeName, out var itemType))
        {
            return ((int)itemType).ToString();
        }

        return "";
    }

    static string GenerateItemTypeOptions()
    {
        var itemTypeOptions = new StringBuilder();
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
        {
            itemTypeOptions.Append($"[{(int)itemType}] {itemType}, ");
        }
        return itemTypeOptions.ToString().TrimEnd(',', ' ');
    }

    static WatchItem BuildWatchItem(WatchItem item)
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

    static void AddItem()
    {
        Console.WriteLine("========== Add Item ==========");
        var newItem = BuildWatchItem(new WatchItem());
        watchList.Add(newItem);
        SaveWatchList();
        Console.WriteLine($"Added {newItem.ItemType}: {newItem.Title}, Genre: {newItem.Genre}, Progress: {newItem.Progress}");
    }

    static void EditItem()
    {
        if (watchList.Count == 0)
        {
            Console.WriteLine("No items to edit.");
            return;
        }

        Console.WriteLine("========== Edit Item ==========");
        var item = watchList[selectedIndex];
        Console.WriteLine($"Editing item: {GetWatchListItemDisplayString(selectedIndex)}");
        var updatedItem = BuildWatchItem(item);
        watchList[selectedIndex] = updatedItem;
        SaveWatchList();
        Console.WriteLine("Item updated successfully.");
    }

    static string ReadLineWithDefault(string prompt, string? defaultValue)
    {
        defaultValue ??= string.Empty;
        Console.Write($"{prompt} {defaultValue}");
        var input = new StringBuilder(defaultValue);
        ConsoleKeyInfo key;

        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }

        Console.WriteLine();
        return input.Length > 0 ? input.ToString() : defaultValue;
    }

    static void DeleteItem()
    {
        if (watchList.Count == 0)
        {
            Console.WriteLine("No items to delete.");
            return;
        }

        Console.WriteLine("========== Delete Item ==========");
        var item = watchList[selectedIndex];
        Console.WriteLine($"Are you sure you want to delete: {item.Title}? [y/n]");
        string? confirmation = Console.ReadLine();
        if (confirmation?.ToLower() == "y")
        {
            watchList.RemoveAt(selectedIndex);
            SaveWatchList();
            Console.WriteLine("Item deleted successfully.");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    static void FilterList()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Filter List ==========");
        Console.WriteLine("Filtering is not implemented yet.");
    }

    static void Exit()
    {
        Console.Clear();
        Console.WriteLine("Exiting application...");
    }

    static void SaveWatchList()
    {
        SortWatchListByTitle();

        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(watchList, options);
        File.WriteAllText(filePath, jsonString);
    }

    static void LoadWatchList()
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            watchList = JsonSerializer.Deserialize<List<WatchItem>>(jsonString) ?? new List<WatchItem>();

            SortWatchListByTitle();
        }
    }

    static void SortWatchListByTitle()
    {
        watchList.Sort((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase));
    }
}
