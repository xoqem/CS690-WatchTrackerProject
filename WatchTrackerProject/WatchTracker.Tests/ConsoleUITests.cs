namespace WatchTracker.Tests;

public class ConsoleUITests
{
    [Fact]
    public void Test_ConsoleUI_Show()
    {
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
            "\b3", // backspace over the 1 and change it to 3
            "\b3", // backspace over the 1 and change it to 3
            "\b3", // backspace over the 1 and change it to 3
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

        result = AssertContainsAndReturnAfterMatch("========== Watch List ==========", result);
        result = AssertContainsAndReturnAfterMatch("No items in watch list.", result);
        result = AssertContainsAndReturnAfterMatch("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit", result);
        result = AssertContainsAndReturnAfterMatch("========== Add Item ==========", result);
        result = AssertContainsAndReturnAfterMatch("Enter title:", result);
        result = AssertContainsAndReturnAfterMatch("Enter genre:", result);
        result = AssertContainsAndReturnAfterMatch("Enter progress:", result);
        result = AssertContainsAndReturnAfterMatch("Enter item type:", result);
        result = AssertContainsAndReturnAfterMatch("========== Watch List ==========", result);
        result = AssertContainsAndReturnAfterMatch("1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie", result);
        result = AssertContainsAndReturnAfterMatch("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit", result);
        result = AssertContainsAndReturnAfterMatch("========== Add Item ==========", result);
        result = AssertContainsAndReturnAfterMatch("Enter title:", result);
        result = AssertContainsAndReturnAfterMatch("Enter genre:", result);
        result = AssertContainsAndReturnAfterMatch("Enter progress:", result);
        result = AssertContainsAndReturnAfterMatch("Enter item type:", result);
        result = AssertContainsAndReturnAfterMatch("========== Watch List ==========", result);
        result = AssertContainsAndReturnAfterMatch("1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie", result);
        result = AssertContainsAndReturnAfterMatch("2. Title: Test title2, Genre: Test genre2, Progress: Test progress2, Type: TVShow", result);
        result = AssertContainsAndReturnAfterMatch("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit", result);
        result = AssertContainsAndReturnAfterMatch("========== Edit Item ==========", result);
        result = AssertContainsAndReturnAfterMatch("Enter the id of the item you wish to edit (or press enter to return to the watch list):", result);
        result = AssertContainsAndReturnAfterMatch("Enter title:", result);
        result = AssertContainsAndReturnAfterMatch("Enter genre:", result);
        result = AssertContainsAndReturnAfterMatch("Enter progress:", result);
        result = AssertContainsAndReturnAfterMatch("Enter item type:", result);
        result = AssertContainsAndReturnAfterMatch("========== Watch List ==========", result);
        result = AssertContainsAndReturnAfterMatch("1. Title: Test title2, Genre: Test genre2, Progress: Test progress2, Type: TVShow", result);
        result = AssertContainsAndReturnAfterMatch("2. Title: Test title3, Genre: Test genre3, Progress: Test progress3, Type: Movie", result);
        result = AssertContainsAndReturnAfterMatch("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit", result);
        result = AssertContainsAndReturnAfterMatch("========== Delete Item ==========", result);
        result = AssertContainsAndReturnAfterMatch("Enter the id of the item you wish to delete (or press enter to return to the watch list):", result);
        result = AssertContainsAndReturnAfterMatch("Are you sure you want to delete: Test title2? [y/n]", result);
        result = AssertContainsAndReturnAfterMatch("========== Watch List ==========", result);
        result = AssertContainsAndReturnAfterMatch("1. Title: Test title3, Genre: Test genre3, Progress: Test progress3, Type: Movie", result);
        result = AssertContainsAndReturnAfterMatch("Please enter an option: [1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit", result);
        result = AssertContainsAndReturnAfterMatch("Exiting application...", result);
    }

    // util function to find a match in result string, Assert that its found, then return everything after the match
    private string AssertContainsAndReturnAfterMatch(string match, string result)
    {
        Assert.Contains(match, result);
        return result.Substring(result.IndexOf(match) + match.Length);
    }
}