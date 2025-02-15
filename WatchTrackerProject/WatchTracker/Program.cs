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

    static void Main(string[] args)
    {
        LoadWatchList();

        while (true)
        {
            DisplayWatchList();
            Console.WriteLine("");
            Console.WriteLine("========== Main Menu ==========");
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
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }

    static void DisplayWatchList()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Watch List ==========");
        if (watchList.Count == 0)
        {
            Console.WriteLine("No items in watch list.");
        }
        else
        {
            for (int i = 0; i < watchList.Count; i++)
            {
                var item = watchList[i];
                Console.WriteLine($"{i + 1}. Title: {item.Title}, Genre: {item.Genre}, Progress: {item.Progress}, Type: {item.ItemType}");
            }
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

    static void AddItem()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Add Item ==========");
        Console.WriteLine("Enter title:");
        string? title = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter genre:");
        string? genre = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter progress:");
        string? progress = Console.ReadLine() ?? string.Empty;

        string itemType;
        while (true)
        {
            Console.WriteLine($"Select item type: {GenerateItemTypeOptions()}");
            string? itemTypeId = Console.ReadLine();
            itemType = ItemTypeIdToName(itemTypeId);
            if (!string.IsNullOrEmpty(itemType))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid item type. Please try again.");
            }
        }

        watchList.Add(new WatchItem { Title = title, Genre = genre, Progress = progress, ItemType = itemType });
        SaveWatchList();
        Console.WriteLine($"Added {itemType}: {title}, Genre: {genre}, Progress: {progress}");
    }

    static void EditItem()
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

            Console.WriteLine($"Editing item: {item.Title}");
            item.Title = ReadLineWithDefault("Enter new title:", item.Title);
            item.Genre = ReadLineWithDefault("Enter new genre:", item.Genre);
            item.Progress = ReadLineWithDefault("Enter new progress:", item.Progress);

            string? typeInput = ReadLineWithDefault("Select new item type: [1] Movie, [2] TV Show:", ItemTypeNameToId(item.ItemType));
            int itemType;
            if (int.TryParse(typeInput, out itemType) && (itemType == 1 || itemType == 2))
            {
                item.ItemType = itemType == 1 ? "Movie" : "TV Show";
            }

            SaveWatchList();
            Console.WriteLine("Item updated successfully.");
        }
        else
        {
            Console.WriteLine("Invalid id. Please try again.");
        }
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
                Console.WriteLine("Item deleted successfully.");
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

    static void FilterList()
    {
        Console.WriteLine("");
        Console.WriteLine("========== Filter List ==========");
        Console.WriteLine("Filtering is not implemented yet.");
    }

    static void Exit()
    {
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
