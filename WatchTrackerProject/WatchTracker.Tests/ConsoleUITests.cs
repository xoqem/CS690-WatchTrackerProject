namespace WatchTracker.Tests;

public class ConsoleUITests
{
    [Fact]
    public void Test_ConsoleUI_Show()
    {
        File.Delete("watchlist.json");

        var consoleUI = new ConsoleUI();
        var sw = new StringWriter();

        Console.SetOut(sw);

        string[] inputs = {
            "1",// Add item
            "Test title1",
            "Test genre1",
            "Test progress1",
            "1", // Movie type
            "1", // Add item
            "Test title2",
            "Test genre2",
            "Test progress2",
            "2", // TVShow Type
            "2", // Edit item
            "1", // Select first item
            "Test title3",
            "", // press enter to keep the existing genre
            "", // press enter to keep the existing progress
            "", // Press enter to keep the existing item type
            "3", // Delete item
            "1", // Select first item
            "y", // Confirm deletion
            "5" // Exit
        };
        Console.SetIn(new StringReader(String.Join("\r", inputs)));
        consoleUI.ClearWatchList();
        consoleUI.Show();
        var result = sw.ToString();

        string[] expectedOutput = [
            // console is cleared here
            "========== Watch List ==========\n",
            "No items in watch list.\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option ❯ ",
            "\n",
            "========== Add Item ==========\n",
            "Enter title ❯ ",
            "Enter genre ❯ ",
            "Enter progress ❯ ",
            "Enter item type: [1] Movie, [2] TVShow ❯ ",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "\n",
            "========== Main Menu ==========",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option ❯ ",
            "\n",
            "========== Add Item ==========\n",
            "Enter title ❯ ",
            "Enter genre ❯ ",
            "Enter progress ❯ ",
            "Enter item type: [1] Movie, [2] TVShow ❯ ",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "2. Title: Test title2, Genre: Test genre2, Progress: Test progress2, Type: TVShow\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option ❯ ",
            "\n",
            "========== Edit Item ==========\n",
            "Enter the id of the item you wish to edit (or press enter to return to the main menu) ❯ ",
            "Editing item: 1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "Enter title (or press enter to keep value \"Test title1\") ❯ ",
            "Enter genre (or press enter to keep value \"Test genre1\") ❯ ",
            "Enter progress (or press enter to keep value \"Test progress1\") ❯ ",
            "Enter item type: [1] Movie, [2] TVShow (or press enter to keep value \"Movie\") ❯ ",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title2, Genre: Test genre2, Progress: Test progress2, Type: TVShow\n",
            "2. Title: Test title3, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option ❯ ",
            "\n",
            "========== Delete Item ==========\n",
            "Enter the id of the item you wish to delete (or press enter to return to the main menu) ❯ ",
            "Are you sure you want to delete: Test title2? [y/n]",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title3, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option ❯ ",
            "Exiting application...\n"
        ];

        for (int i = 0; i < expectedOutput.Length; i++)
        {
            result = AssertContainsAndReturnAfterMatch(expectedOutput[i], result);
        }

        File.Delete("watchlist.json");
    }

    // util function to find a match in result string, assert that its found, then return everything after the match
    private string AssertContainsAndReturnAfterMatch(string match, string result)
    {
        Assert.Contains(match, result);
        return result.Substring(result.IndexOf(match) + match.Length);
    }
}