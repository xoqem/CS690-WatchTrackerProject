using Spectre.Console;
using Spectre.Console.Testing;

namespace WatchTracker.Tests;

public class ConsoleUITests
{
    [Fact]
    public void Test_ConsoleUI_Show()
    {
        File.Delete("watchlist.json");

        var consoleUI = new ConsoleUI();

        var console = new TestConsole();
        console.Input.PushTextWithEnter("1"); // Add item
        console.Input.PushTextWithEnter("Test title1");
        console.Input.PushTextWithEnter("Test genre1");
        console.Input.PushTextWithEnter("Test progress1");
        console.Input.PushTextWithEnter("1"); // Movie type
        console.Input.PushTextWithEnter("1"); // Add item
        console.Input.PushTextWithEnter("Test title2");
        console.Input.PushTextWithEnter("Test genre2");
        console.Input.PushTextWithEnter("Test progress2");
        console.Input.PushTextWithEnter("2"); // TVShow Type
        console.Input.PushTextWithEnter("2"); // Edit item
        console.Input.PushTextWithEnter("1"); // Select first item
        console.Input.PushTextWithEnter("Test title3");
        console.Input.PushTextWithEnter(""); // press enter to keep the existing genre
        console.Input.PushTextWithEnter(""); // press enter to keep the existing progress
        console.Input.PushTextWithEnter(""); // Press enter to keep the existing item type
        console.Input.PushTextWithEnter("3"); // Delete item
        console.Input.PushTextWithEnter("1"); // Select first item
        console.Input.PushTextWithEnter("y"); // Confirm deletion
        console.Input.PushTextWithEnter("5"); // Exit
        
        AnsiConsole.Console = console;

        // just setting the test console to something really large, so the test output doesn't
        // have extra line breaks in the middle of the output
        console.Profile.Width = 500;

        consoleUI.Show();

        string[] expectedOutput = [
            // console is cleared here
            "========== Watch List ==========\n",
            "No items in watch list.\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option: 1\n",
            "\n",
            "========== Add Item ==========\n",
            "Enter title: Test title1\n",
            "Enter genre: Test genre1\n",
            "Enter progress: Test progress1\n",
            "Enter item type: [1] Movie, [2] TVShow: 1\n",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "\n",
            "========== Main Menu ==========",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option: 1\n",
            "\n",
            "========== Add Item ==========\n",
            "Enter title: Test title2\n",
            "Enter genre: Test genre2\n",
            "Enter progress: Test progress2\n",
            "Enter item type: [1] Movie, [2] TVShow: 2\n",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "2. Title: Test title2, Genre: Test genre2, Progress: Test progress2, Type: TVShow\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option: 2\n",
            "\n",
            "========== Edit Item ==========\n",
            "Enter the id of the item you wish to edit (or press enter to return to the main menu): 1\n",
            "Editing item: 1. Title: Test title1, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "Enter title: (Test title1): Test title3\n",
            "Enter genre: (Test genre1): Test genre1\n",
            "Enter progress: (Test progress1): Test progress1\n",
            "Enter item type: [1] Movie, [2] TVShow: (1): 1\n",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title2, Genre: Test genre2, Progress: Test progress2, Type: TVShow\n",
            "2. Title: Test title3, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option: 3\n",
            "\n",
            "========== Delete Item ==========\n",
            "Enter the id of the item you wish to delete (or press enter to return to the main menu): 1\n",
            "Are you sure you want to delete: Test title2? [y/n] (y): y\n",

            // console is cleared here
            "========== Watch List ==========\n",
            "1. Title: Test title3, Genre: Test genre1, Progress: Test progress1, Type: Movie\n",
            "\n",
            "========== Main Menu ==========\n",
            "[1] Add item, [2] Edit item, [3] Delete item, [4] Filter list, [5] Exit\n",
            "Please enter an option: 5\n",
            "Exiting application...\n"
        ];

        var result = console.Output;
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
