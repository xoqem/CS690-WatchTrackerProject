namespace WatchTracker.Tests;

public class WatchListFileIOTests
{
    [Fact]
    public void Test_WatchListFileIO_SaveAndLoadWatchList()
    {
        File.Delete("test.json");

        var watchList = new WatchList();
        watchList.Items = new List<WatchItem>
        {
            new WatchItem { Title = "Movie 1", Genre = "Action", Progress = "Not Started", ItemType = WatchItemType.Movie },
            new WatchItem { Title = "TV Show 1", Genre = "Drama", Progress = "Season 1, Episode 2", ItemType = WatchItemType.TVShow },
        };

        var watchListFileIO = new WatchListFileIO("test.json");
        watchListFileIO.SaveWatchList(watchList);

        Assert.True(File.Exists("test.json"));

        var loadedWatchList = watchListFileIO.LoadWatchList();
        
        Assert.Equal(watchList.Items.Count, loadedWatchList.Items.Count);
        
        for (int i = 0; i < watchList.Items.Count; i++)
        {
            Assert.Equal(watchList.Items[i].Title, loadedWatchList.Items[i].Title);
            Assert.Equal(watchList.Items[i].Genre, loadedWatchList.Items[i].Genre);
            Assert.Equal(watchList.Items[i].Progress, loadedWatchList.Items[i].Progress);
            Assert.Equal(watchList.Items[i].ItemType, loadedWatchList.Items[i].ItemType);
        }

        File.Delete("test.json");
    }
}