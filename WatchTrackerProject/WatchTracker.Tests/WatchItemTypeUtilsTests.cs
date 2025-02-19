namespace WatchTracker.Tests;

public class WatchItemTypeUtilsTests
{
    [Fact]
    public void Test_WatchItemTypeUtilsTests_GetDisplayString_GetIdFromItemType()
    {        
        Assert.Equal(
            "1",
            WatchItemTypeUtils.GetIdFromItemType(WatchItemType.Movie)
        );

        Assert.Equal(
            "2",
            WatchItemTypeUtils.GetIdFromItemType(WatchItemType.TVShow)
        );
    }

    [Fact]
    public void Test_WatchItemTypeUtilsTests_GetDisplayString_GetItemTypeFromId()
    {        
        Assert.Equal(
            WatchItemType.Movie,
            WatchItemTypeUtils.GetItemTypeFromId("1")
        );

        Assert.Equal(
            WatchItemType.TVShow,
            WatchItemTypeUtils.GetItemTypeFromId("2")
        );
    }

    [Fact]
    public void Test_WatchItemTypeUtilsTests_GetDisplayString_GetOptions()
    {        
        Assert.Equal(
            "[[1]] Movie, [[2]] TVShow",
            WatchItemTypeUtils.GetOptions()
        );
    }
}