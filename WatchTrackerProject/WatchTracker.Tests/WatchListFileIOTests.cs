namespace WatchTracker.Tests;

public class WatchListFileIOTests
{
    [Fact]
    public void Test_WatchListFileIO_SaveAndLoadWatchList()
    {
        var watchList = new List<WatchItem>
        {
            new WatchItem { Title = "Movie 1", Genre = "Action", Progress = "Not Started", ItemType = "Movie" },
            new WatchItem { Title = "TV Show 1", Genre = "Drama", Progress = "Season 1, Episode 2", ItemType = "TV Show" },
        };

        var watchListFileIO = new WatchListFileIO("test.json");
        watchListFileIO.SaveWatchList(watchList);

        Assert.True(File.Exists("test.json"));

        var loadedWatchList = watchListFileIO.LoadWatchList();
        
        Assert.Equal(watchList.Count, loadedWatchList.Count);
        
        for (int i = 0; i < watchList.Count; i++)
        {
            Assert.Equal(watchList[i].Title, loadedWatchList[i].Title);
            Assert.Equal(watchList[i].Genre, loadedWatchList[i].Genre);
            Assert.Equal(watchList[i].Progress, loadedWatchList[i].Progress);
            Assert.Equal(watchList[i].ItemType, loadedWatchList[i].ItemType);
        }

        File.Delete("test.json");
    }
}