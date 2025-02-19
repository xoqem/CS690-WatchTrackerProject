namespace WatchTracker.Tests;

public class WatchItemTests
{
    [Fact]
    public void Test_WatchItemTests_GetDisplayString_AllValues()
    {
        var watchItem = new WatchItem
        {
            Title = "Test Title",
            Genre = "Test Genre",
            Progress = "Test Progress",
            ItemType = WatchItemType.Movie
        };

        var displayString = watchItem.GetDisplayString();

        Assert.Equal("Title: Test Title, Genre: Test Genre, Progress: Test Progress, Type: Movie", displayString);
    }

    [Fact]
    public void Test_WatchItemTests_GetDisplayString_SomeMissingValues()
    {
        var watchItem = new WatchItem
        {
            Title = "Test Title",
            Genre = "",
            Progress = "Test Progress",
            ItemType = null
        };

        var displayString = watchItem.GetDisplayString();

        Assert.Equal("Title: Test Title, Progress: Test Progress", displayString);
    }

    [Fact]
    public void Test_WatchItemTests_GetDisplayString_AllMissingValues()
    {
        var watchItem = new WatchItem {};

        var displayString = watchItem.GetDisplayString();

        Assert.Equal("", displayString);
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_Title_Matches()
    {
        var watchItem = new WatchItem
        {
            Title = "Test Title",
        };

        var filter = new WatchItem
        {
            Title = "title",
        };

        Assert.True(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_Title_DoesNotMatch()
    {
        var watchItem = new WatchItem
        {
            Title = "unmatched",
        };

        var filter = new WatchItem
        {
            Title = "Test Title",
        };

        Assert.False(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_Genre_Matches()
    {
        var watchItem = new WatchItem
        {
            Genre = "Test Genre",
        };

        var filter = new WatchItem
        {
            Genre = "genre",
        };

        Assert.True(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_Genre_DoesNotMatch()
    {
        var watchItem = new WatchItem
        {
            Genre = "Test Genre",
        };

        var filter = new WatchItem
        {
            Genre = "unmatched",
        };

        Assert.False(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_Progress_Matches()
    {
        var watchItem = new WatchItem
        {
            Progress = "Test Progress",
        };

        var filter = new WatchItem
        {
            Progress = "progress",            
        };

        Assert.True(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_Progress_DoesNotMatch()
    {
        var watchItem = new WatchItem
        {
            Progress = "Test Progress",
        };

        var filter = new WatchItem
        {
            Progress = "unmatched",
        };

        Assert.False(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_ItemType_Matches()
    {
        var watchItem = new WatchItem
        {
            ItemType = WatchItemType.Movie,
        };

        var filter = new WatchItem
        {
            ItemType = WatchItemType.Movie,
        };

        Assert.True(watchItem.MatchesFilter(filter));
    }

    [Fact]
    public void Test_WatchItemTests_MatchesFilter_ItemType_DoesNotMatch()
    {
        var watchItem = new WatchItem
        {
            ItemType = WatchItemType.Movie,
        };

        var filter = new WatchItem
        {
            ItemType = WatchItemType.TVShow,
        };

        Assert.False(watchItem.MatchesFilter(filter));
    }
}